using System;
using System.IO;
using Capa_Negocios;
using Capa_Datos;

namespace Deber_Linq
{
    public partial class Profesores : System.Web.UI.Page
    {
        ProfesorNegocio negocio = new ProfesorNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrid();
            }
        }

        private void CargarGrid()
        {
            gvProfesores.DataSource = negocio.Listar();
            gvProfesores.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string ruta = "";
            if (fuFoto.HasFile)
            {
                string extension = Path.GetExtension(fuFoto.FileName).ToLower();
                string[] extensionesPermitidas = { ".jpg", ".jpeg", ".png", ".gif" };

                if (Array.IndexOf(extensionesPermitidas, extension) < 0)
                {
                    lblMensaje.Text = "Solo se permiten imágenes con formato .jpg, .jpeg, .png o .gif.";
                    return;
                }

                string nombreArchivo = Guid.NewGuid().ToString() + extension;
                ruta = "~/fotos/" + nombreArchivo;
                string rutaFisica = Server.MapPath(ruta);
                fuFoto.SaveAs(rutaFisica);
            }

            Profesor profesor = new Profesor()
            {
                Pro_Nombre = txtNombre.Text,
                Pro_FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text),
                Pro_Especialidad = txtEspecialidad.Text,
                Pro_FotoPath = ruta
            };

            int id = string.IsNullOrEmpty(hfProfesorId.Value) ? 0 : int.Parse(hfProfesorId.Value);

            if (id == 0)
            {
                negocio.Insertar(profesor);
                lblMensaje.Text = "Profesor agregado exitosamente.";
            }
            else
            {
                profesor.ProfesorId = id;

                if (string.IsNullOrEmpty(ruta))
                {
                    profesor.Pro_FotoPath = negocio.ObtenerPorId(id)?.Pro_FotoPath;
                }

                negocio.Actualizar(profesor);
                lblMensaje.Text = "Profesor actualizado correctamente.";
            }

            Limpiar();
            CargarGrid();
        }


        protected void gvProfesores_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                var profesor = negocio.ObtenerPorId(id);
                if (profesor != null)
                {
                    hfProfesorId.Value = profesor.ProfesorId.ToString();
                    txtNombre.Text = profesor.Pro_Nombre;
                    txtFechaNacimiento.Text = profesor.Pro_FechaNacimiento?.ToString("yyyy-MM-dd");
                    txtEspecialidad.Text = profesor.Pro_Especialidad;
                    imgFoto.ImageUrl = profesor.Pro_FotoPath;
                    imgFoto.Visible = true;
                    lblMensaje.Text = "";
                }
            }

            else if (e.CommandName == "Eliminar")
            {
                bool eliminado = negocio.Eliminar(id);
                if (eliminado)
                {
                    lblMensaje.Text = "Profesor eliminado correctamente.";
                    CargarGrid();
                }
                else
                {
                    lblMensaje.Text = "No se puede eliminar el profesor porque tiene materias asignadas.";
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
            lblMensaje.Text = "";
        }

        private void Limpiar()
        {
            txtNombre.Text = "";
            txtFechaNacimiento.Text = "";
            txtEspecialidad.Text = "";
            hfProfesorId.Value = "";
            imgFoto.Visible = false;
        }
    }
}
