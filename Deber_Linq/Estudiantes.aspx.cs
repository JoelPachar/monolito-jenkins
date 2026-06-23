using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Capa_Datos;
using TuProyectoWeb.Negocio;

namespace TuProyectoWeb
{
    public partial class Estudiantes : System.Web.UI.Page
    {
        EstudianteNegocio estudianteNegocio = new EstudianteNegocio();
        MateriaNegocio materiaNegocio = new MateriaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMaterias();
                CargarEstudiantes();
            }
        }

        private void CargarMaterias()
        {
            ddlMaterias.DataSource = materiaNegocio.ObtenerTodas();
            ddlMaterias.DataTextField = "Mat_Nombre";
            ddlMaterias.DataValueField = "MateriaId";
            ddlMaterias.DataBind();
            ddlMaterias.Items.Insert(0, new ListItem("-- Seleccione Materia --", "0"));
        }

        private void CargarEstudiantes()
        {
            gvEstudiantes.DataSource = estudianteNegocio.ListarConMateria();
            gvEstudiantes.DataBind();
        }

        private string CalcularSignoZodiacal(DateTime fecha)
        {
            int dia = fecha.Day;
            int mes = fecha.Month;

            if ((mes == 1 && dia <= 20) || (mes == 12 && dia >= 22)) return "Capricornio";
            if ((mes == 1 && dia >= 21) || (mes == 2 && dia <= 19)) return "Acuario";
            if ((mes == 2 && dia >= 20) || (mes == 3 && dia <= 20)) return "Piscis";
            if ((mes == 3 && dia >= 21) || (mes == 4 && dia <= 20)) return "Aries";
            if ((mes == 4 && dia >= 21) || (mes == 5 && dia <= 21)) return "Tauro";
            if ((mes == 5 && dia >= 22) || (mes == 6 && dia <= 21)) return "Géminis";
            if ((mes == 6 && dia >= 22) || (mes == 7 && dia <= 23)) return "Cáncer";
            if ((mes == 7 && dia >= 24) || (mes == 8 && dia <= 23)) return "Leo";
            if ((mes == 8 && dia >= 24) || (mes == 9 && dia <= 23)) return "Virgo";
            if ((mes == 9 && dia >= 24) || (mes == 10 && dia <= 23)) return "Libra";
            if ((mes == 10 && dia >= 24) || (mes == 11 && dia <= 22)) return "Escorpio";
            if ((mes == 11 && dia >= 23) || (mes == 12 && dia <= 21)) return "Sagitario";
            return "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Estudiante estudiante;
                int idEstudiante = 0;
                int.TryParse(hfEstudianteId.Value, out idEstudiante);

                if (idEstudiante > 0)
                {
                    estudiante = estudianteNegocio.ObtenerPorId(idEstudiante);
                    if (estudiante == null)
                    {
                        lblMensaje.Text = "Estudiante no encontrado.";
                        return;
                    }
                }
                else
                {
                    estudiante = new Estudiante();
                }

                estudiante.Alu_Nombre = txtNombre.Text.Trim();

                if (DateTime.TryParse(txtFechaNac.Text, out DateTime fechaNac))
                {
                    estudiante.Alu_FechaNacimiento = fechaNac;
                    estudiante.Alu_SignoZodiacal = CalcularSignoZodiacal(fechaNac);
                }
                else
                {
                    lblMensaje.Text = "Fecha de nacimiento inválida.";
                    return;
                }

                if (int.TryParse(ddlMaterias.SelectedValue, out int materiaId) && materiaId > 0)
                {
                    estudiante.MateriaId = materiaId;
                }
                else
                {
                    estudiante.MateriaId = null;
                }

                // Validar y guardar la foto si se subió una
                if (fuFotoEstudiante.HasFile)
                {
                    int maxFileSize = 2 * 1024 * 1024; // 2 MB
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

                    string fileExt = Path.GetExtension(fuFotoEstudiante.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExt))
                    {
                        lblMensaje.Text = "Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif, .bmp).";
                        return;
                    }

                    if (fuFotoEstudiante.PostedFile.ContentLength > maxFileSize)
                    {
                        lblMensaje.Text = "El tamaño de la imagen no debe exceder 2 MB.";
                        return;
                    }

                    byte[] fotoBytes;
                    using (BinaryReader br = new BinaryReader(fuFotoEstudiante.PostedFile.InputStream))
                    {
                        fotoBytes = br.ReadBytes(fuFotoEstudiante.PostedFile.ContentLength);
                    }

                    // Guardar la imagen como cadena Base64 en la propiedad Alu_Foto (string)
                    estudiante.Alu_Foto = Convert.ToBase64String(fotoBytes);
                }
                else if (idEstudiante == 0) // Nuevo estudiante sin foto
                {
                    estudiante.Alu_Foto = null;
                }

                if (idEstudiante > 0)
                {
                    estudianteNegocio.Actualizar(estudiante);
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    lblMensaje.Text = "Estudiante actualizado correctamente.";
                }
                else
                {
                    estudianteNegocio.Insertar(estudiante);
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    lblMensaje.Text = "Estudiante registrado correctamente.";
                }

                LimpiarCampos();
                CargarEstudiantes();
            }
            catch (Exception ex)
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                lblMensaje.Text = "Error: " + ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            hfEstudianteId.Value = "0";
            txtNombre.Text = "";
            txtFechaNac.Text = "";
            txtSigno.Text = "";
            ddlMaterias.SelectedIndex = 0;
            lblMensaje.Text = "";

            imgActualEstudiante.ImageUrl = "~/Imagenes/default.png";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "limpiarImagen", "limpiarCamposImagen();", true);
        }

        protected void gvEstudiantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int estudianteId = Convert.ToInt32(gvEstudiantes.DataKeys[index].Value);

                var estudiante = estudianteNegocio.ObtenerPorId(estudianteId);
                if (estudiante != null)
                {
                    hfEstudianteId.Value = estudiante.EstudianteId.ToString();
                    txtNombre.Text = estudiante.Alu_Nombre;
                    txtFechaNac.Text = estudiante.Alu_FechaNacimiento?.ToString("yyyy-MM-dd") ?? "";
                    txtSigno.Text = estudiante.Alu_SignoZodiacal;

                    if (estudiante.MateriaId.HasValue)
                        ddlMaterias.SelectedValue = estudiante.MateriaId.Value.ToString();
                    else
                        ddlMaterias.SelectedIndex = 0;

                    if (!string.IsNullOrEmpty(estudiante.Alu_Foto))
                    {
                        imgActualEstudiante.ImageUrl = "data:image/jpeg;base64," + estudiante.Alu_Foto;
                        imgActualEstudiante.Visible = true;
                    }
                    else
                    {
                        imgActualEstudiante.ImageUrl = "";
                        imgActualEstudiante.Visible = false;
                    }
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int estudianteId = Convert.ToInt32(gvEstudiantes.DataKeys[index].Value);

                bool eliminado = estudianteNegocio.Eliminar(estudianteId);
                if (eliminado)
                {
                    lblMensaje.ForeColor = System.Drawing.Color.Green;
                    lblMensaje.Text = "Estudiante eliminado correctamente.";
                }
                else
                {
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    lblMensaje.Text = "No se pudo eliminar el estudiante.";
                }

                CargarEstudiantes();
            }
        }

        protected void gvEstudiantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 1. Controlar la foto del Estudiante
                Image imgFotoEstu = (Image)e.Row.FindControl("imgFotoEstu");
                string base64FotoEstu = DataBinder.Eval(e.Row.DataItem, "FotoEstu") as string;

                if (!string.IsNullOrEmpty(base64FotoEstu))
                {
                    imgFotoEstu.ImageUrl = "data:image/jpeg;base64," + base64FotoEstu;
                    imgFotoEstu.Visible = true;
                }
                else
                {
                    imgFotoEstu.Visible = false;
                }

                // 2. Controlar la foto del Profesor (Esto soluciona los cuadros blancos)
                Image imgFotoProfesor = (Image)e.Row.FindControl("imgFotoProfesor");
                string fotoProfesor = DataBinder.Eval(e.Row.DataItem, "FotoProfesor") as string;

                if (!string.IsNullOrEmpty(fotoProfesor))
                {
                    // Determinar si la foto del profesor es una ruta (ej. ~/ruta/foto.jpg) o Base64
                    if (fotoProfesor.StartsWith("~") || fotoProfesor.StartsWith("http"))
                    {
                        imgFotoProfesor.ImageUrl = fotoProfesor;
                    }
                    else
                    {
                        imgFotoProfesor.ImageUrl = "data:image/jpeg;base64," + fotoProfesor;
                    }
                    imgFotoProfesor.Visible = true;
                }
                else
                {
                    // Si no hay profesor asignado o no tiene foto, ocultamos el cuadro
                    imgFotoProfesor.Visible = false;
                }
            }
        }
    }
}