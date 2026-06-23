<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="Deber_Linq.Profesores"
    UnobtrusiveValidationMode="None" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Administración de Profesores</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <script type="text/javascript">
        function validarImagen() {
            var fileUpload = document.getElementById('<%= fuFoto.ClientID %>');
            var preview = document.getElementById('imgPreviewProf');
            var archivo = fileUpload.files[0];

            if (!archivo) return true;

            var extensiones = /(\.jpg|\.jpeg|\.png|\.gif)$/i;
            if (!extensiones.exec(archivo.name)) {
                alert("Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif).");
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

        window.onload = function () {
            document.getElementById('<%= fuFoto.ClientID %>').addEventListener('change', validarImagen);
        }
    </script>
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="container mt-4" onsubmit="return validarImagen();">
        <!-- NAVBAR -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
            <div class="container-fluid">
                <span class="navbar-brand">Gestión de Profesores</span>
                <a href="pagina.aspx" class="btn btn-outline-light">Volver al Inicio</a>
            </div>
        </nav>

        <!-- MENSAJE -->
        <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger fw-bold" />

        <!-- FORMULARIO -->
        <div class="card shadow-sm mb-4">
            <div class="card-body">
                <h5 class="card-title">Formulario de Profesores</h5>
                <asp:HiddenField ID="hfProfesorId" runat="server" />

                <div class="mb-3">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre:" CssClass="form-label" />
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                        ErrorMessage="* El nombre es obligatorio" CssClass="text-danger" ValidationGroup="grupo1" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="lblFechaNacimiento" runat="server" Text="Fecha Nacimiento:" CssClass="form-label" />
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date" />
                    <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" ControlToValidate="txtFechaNacimiento"
                        ErrorMessage="* La fecha de nacimiento es obligatoria" CssClass="text-danger" ValidationGroup="grupo1" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="lblEspecialidad" runat="server" Text="Especialidad:" CssClass="form-label" />
                    <asp:TextBox ID="txtEspecialidad" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvEspecialidad" runat="server" ControlToValidate="txtEspecialidad"
                        ErrorMessage="* La especialidad es obligatoria" CssClass="text-danger" ValidationGroup="grupo1" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <asp:Label ID="lblFoto" runat="server" Text="Foto:" CssClass="form-label" />
                    <asp:FileUpload ID="fuFoto" runat="server" CssClass="form-control" accept="image/*" />
                    <img id="imgPreviewProf" class="mt-2 rounded border" style="display: none; max-width: 100px; max-height: 100px;" />
                </div>

                <div class="mb-3">
                    <asp:Image ID="imgFoto" runat="server" Width="100px" Height="100px" Visible="false" CssClass="rounded border" />
                </div>

                <div class="d-flex gap-2">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary"
                        OnClick="btnGuardar_Click" ValidationGroup="grupo1" CausesValidation="true" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                        OnClick="btnCancelar_Click" CausesValidation="false" />
                </div>
            </div>
        </div>

        <!-- TABLA -->
        <div class="card shadow-sm">
            <div class="card-body">
                <h5 class="card-title">Listado de Profesores</h5>
                <asp:GridView ID="gvProfesores" runat="server" CssClass="table table-bordered table-hover"
                    AutoGenerateColumns="False" OnRowCommand="gvProfesores_RowCommand" DataKeyNames="ProfesorId">
                    <Columns>
                        <asp:BoundField DataField="ProfesorId" HeaderText="ID" />
                        <asp:BoundField DataField="Pro_Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Pro_FechaNacimiento" HeaderText="Fecha Nacimiento" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="Pro_SignoZodiacal" HeaderText="Signo Zodiacal" />
                        <asp:BoundField DataField="Pro_Especialidad" HeaderText="Especialidad" />
                        <asp:TemplateField HeaderText="Foto">
                            <ItemTemplate>
                                <asp:Image ID="imgGrid" runat="server" Width="60" Height="60" CssClass="rounded" ImageUrl='<%# Eval("Pro_FotoPath") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" CssClass="btn btn-sm btn-warning me-1"
                                    CommandName="Editar" CommandArgument='<%# Eval("ProfesorId") %>' Text="Editar" />
                                <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-danger"
                                    CommandName="Eliminar" CommandArgument='<%# Eval("ProfesorId") %>' Text="Eliminar"
                                    OnClientClick="return confirm('¿Eliminar este profesor?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
