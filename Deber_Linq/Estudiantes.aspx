<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Estudiantes.aspx.cs" Inherits="TuProyectoWeb.Estudiantes" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Gestión de Estudiantes</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <script type="text/javascript">
        // 1. Validar tamaño y extensión de la imagen
        function validarImagenEstudiante() {
            var fileUpload = document.getElementById('<%= fuFotoEstudiante.ClientID %>');
            var preview = document.getElementById('imgPreview');
            var archivo = fileUpload.files[0];

            if (!archivo) return true;

            var extensiones = /(\.jpg|\.jpeg|\.png|\.gif|\.bmp)$/i;
            if (!extensiones.exec(archivo.name)) {
                alert("Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif, .bmp).");
                fileUpload.value = "";
                preview.style.display = 'none';
                return false;
            }

            var maxSize = 2 * 1024 * 1024;
            if (archivo.size > maxSize) {
                alert("La imagen no debe superar los 2 MB.");
                fileUpload.value = "";
                preview.style.display = 'none';
                return false;
            }

            var reader = new FileReader();
            reader.onload = function (e) {
                preview.src = e.target.result;
                preview.style.display = 'block';
            }
            reader.readAsDataURL(archivo);

            return true;
        }

        // 2. Calcular signo zodiacal sin recargar la página (Evita que se borre la foto)
        function calcularSigno() {
            var fechaVal = document.getElementById('<%= txtFechaNac.ClientID %>').value;
            if (!fechaVal) {
                document.getElementById('<%= txtSigno.ClientID %>').value = "";
                return;
            }
            
            var partes = fechaVal.split('-');
            var mes = parseInt(partes[1], 10);
            var dia = parseInt(partes[2], 10);
            var signo = "";

            if ((mes == 1 && dia <= 20) || (mes == 12 && dia >= 22)) signo = "Capricornio";
            else if ((mes == 1 && dia >= 21) || (mes == 2 && dia <= 19)) signo = "Acuario";
            else if ((mes == 2 && dia >= 20) || (mes == 3 && dia <= 20)) signo = "Piscis";
            else if ((mes == 3 && dia >= 21) || (mes == 4 && dia <= 20)) signo = "Aries";
            else if ((mes == 4 && dia >= 21) || (mes == 5 && dia <= 21)) signo = "Tauro";
            else if ((mes == 5 && dia >= 22) || (mes == 6 && dia <= 21)) signo = "Géminis";
            else if ((mes == 6 && dia >= 22) || (mes == 7 && dia <= 23)) signo = "Cáncer";
            else if ((mes == 7 && dia >= 24) || (mes == 8 && dia <= 23)) signo = "Leo";
            else if ((mes == 8 && dia >= 24) || (mes == 9 && dia <= 23)) signo = "Virgo";
            else if ((mes == 9 && dia >= 24) || (mes == 10 && dia <= 23)) signo = "Libra";
            else if ((mes == 10 && dia >= 24) || (mes == 11 && dia <= 22)) signo = "Escorpio";
            else if ((mes == 11 && dia >= 23) || (mes == 12 && dia <= 21)) signo = "Sagitario";

            document.getElementById('<%= txtSigno.ClientID %>').value = signo;
        }

        window.onload = function () {
            document.getElementById('<%= fuFotoEstudiante.ClientID %>').addEventListener('change', validarImagenEstudiante);
            document.getElementById('<%= txtFechaNac.ClientID %>').addEventListener('change', calcularSigno);
        }

        // Función llamada desde el C# para limpiar la imagen previa al cancelar/guardar
        function limpiarCamposImagen() {
            var preview = document.getElementById('imgPreview');
            if (preview) {
                preview.src = '';
                preview.style.display = 'none';
            }
        }
    </script>
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="container mt-4" onsubmit="return validarImagenEstudiante();">

        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <nav class="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
            <div class="container-fluid">
                <span class="navbar-brand">Gestión de Estudiantes</span>
                <a href="pagina.aspx" class="btn btn-outline-light">Volver al Inicio</a>
            </div>
        </nav>

        <asp:HiddenField ID="hfEstudianteId" runat="server" />

        <asp:UpdatePanel ID="UpdatePanelFormulario" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="card shadow-sm mb-4">
                    <div class="card-body">
                        <h5 class="card-title">Formulario de Estudiante</h5>

                        <div class="mb-3">
                            <label class="form-label">Nombre del Estudiante:</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Fecha Nacimiento:</label>
                            <asp:TextBox ID="txtFechaNac" runat="server" TextMode="Date" CssClass="form-control" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Signo Zodiacal:</label>
                            <asp:TextBox ID="txtSigno" runat="server" ReadOnly="true" CssClass="form-control bg-light" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Foto del Estudiante:</label>
                            <asp:FileUpload ID="fuFotoEstudiante" runat="server" CssClass="form-control" accept="image/*" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Imagen actual del estudiante:</label><br />
                            <asp:Image ID="imgActualEstudiante" runat="server" CssClass="border rounded mb-2" Style="max-width:150px; max-height:150px;" />
                            <img id="imgPreview" class="border rounded" style="display:none; max-width:150px; max-height:150px;" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Materia:</label>
                            <asp:DropDownList ID="ddlMaterias" runat="server" CssClass="form-select"
                                DataTextField="Mat_Nombre" DataValueField="MateriaId" />
                        </div>

                        <div class="d-flex gap-2">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
                        </div>

                        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" CssClass="mt-3 fw-bold d-block" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="card shadow-sm">
                    <div class="card-body">
                        <h5 class="card-title">Lista de Estudiantes</h5>
                        <asp:GridView ID="gvEstudiantes" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            OnRowCommand="gvEstudiantes_RowCommand" DataKeyNames="EstudianteId" OnRowDataBound="gvEstudiantes_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="EstudianteId" HeaderText="ID" />
                                <asp:BoundField DataField="NombreEstu" HeaderText="Nombre" />
                                <asp:BoundField DataField="FechaNac" HeaderText="Fecha Nacimiento" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="Signo" HeaderText="Signo Zodiacal" />
                                <asp:TemplateField HeaderText="Foto Estudiante">
                                    <ItemTemplate>
                                        <asp:Image ID="imgFotoEstu" runat="server" Width="70px" Height="70px" CssClass="rounded"
                                            ImageUrl='<%# Eval("FotoEstu") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Materia" HeaderText="Materia" />
                                <asp:BoundField DataField="Profesor" HeaderText="Profesor" />
                                <asp:TemplateField HeaderText="Foto Profesor">
                                    <ItemTemplate>
                                        <asp:Image ID="imgFotoProfesor" runat="server" Width="70px" Height="70px" CssClass="rounded"
                                            ImageUrl='<%# Eval("FotoProfesor") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField CommandName="Editar" Text="Editar" ButtonType="Button" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" Text="Eliminar"
                                            CssClass="btn btn-sm btn-danger" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('¿Eliminar estudiante?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>