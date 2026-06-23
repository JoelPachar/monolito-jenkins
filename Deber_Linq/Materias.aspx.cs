using Capa_Datos;
using Capa_Negocios; // <-- Corregido para usar tu capa de negocios real
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Deber_Linq // <-- Corregido: Este es el nombre real de tu proyecto
{
    public partial class Materias : Page
    {
        private readonly MateriaNegocio materiaNegocio = new MateriaNegocio();
        private readonly ProfesorNegocio profesorNegocio = new ProfesorNegocio();
        private readonly EstudianteNegocio estudianteNegocio = new EstudianteNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProfesores();
                CargarMaterias();
                LimpiarFormulario();
            }
        }

        private void CargarProfesores()
        {
            var profesores = profesorNegocio.Listar();
            ddlProfesores.DataSource = profesores;
            ddlProfesores.DataTextField = "Pro_Nombre";
            ddlProfesores.DataValueField = "ProfesorId";
            ddlProfesores.DataBind();

            // Opción por defecto para evitar el error con el ID 0
            ddlProfesores.Items.Insert(0, new ListItem("--Seleccione--", "-1"));
        }

        private void CargarMaterias()
        {
            // Aquí llamamos al método de tu clase Negocio y llenamos el GridView
            var materias = materiaNegocio.ObtenerTodasConProfesor();
            gvMaterias.DataSource = materias;
            gvMaterias.DataBind();
        }

        private void LimpiarFormulario()
        {
            hfMateriaId.Value = string.Empty;
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            ddlProfesores.SelectedIndex = 0;
            lblMensaje.Text = string.Empty;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;

            // Validación correcta del profesor usando "-1"
            if (!int.TryParse(ddlProfesores.SelectedValue, out int profesorId) || profesorId == -1)
            {
                lblMensaje.Text = "Seleccione un profesor válido.";
                return;
            }

            var materia = new Materia
            {
                Mat_Nombre = txtNombre.Text.Trim(),
                Mat_Descripcion = txtDescripcion.Text.Trim(),
                ProfesorId = profesorId
            };

            if (string.IsNullOrEmpty(hfMateriaId.Value))
            {
                materiaNegocio.Insertar(materia);
                lblMensaje.Text = "Materia guardada correctamente.";
            }
            else
            {
                materia.MateriaId = Convert.ToInt32(hfMateriaId.Value);
                materiaNegocio.Actualizar(materia);
                lblMensaje.Text = "Materia actualizada correctamente.";
            }

            CargarMaterias();
            LimpiarFormulario();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void gvMaterias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int materiaId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                var materia = materiaNegocio.ObtenerPorId(materiaId);
                if (materia != null)
                {
                    hfMateriaId.Value = materia.MateriaId.ToString();
                    txtNombre.Text = materia.Mat_Nombre;
                    txtDescripcion.Text = materia.Mat_Descripcion;
                    ddlProfesores.SelectedValue = materia.ProfesorId.ToString();
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                if (estudianteNegocio.TieneEstudiantesEnMateria(materiaId))
                {
                    lblMensaje.Text = "No se puede eliminar la materia porque tiene estudiantes asociados.";
                }
                else
                {
                    materiaNegocio.Eliminar(materiaId);
                    lblMensaje.Text = "Materia eliminada correctamente.";
                    CargarMaterias();
                }
            }
        }

        protected void gvMaterias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMaterias.PageIndex = e.NewPageIndex;
            CargarMaterias();
        }
    }
}