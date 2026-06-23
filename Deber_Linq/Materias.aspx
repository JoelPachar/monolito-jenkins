<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Materias.aspx.cs" Inherits="TuProyectoWeb.Materias" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Gestión de Materias</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="container mt-4">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <nav class="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
            <div class="container-fluid">
                <span class="navbar-brand">Gestión de Materias</span>
                <a href="pagina.aspx" class="btn btn-outline-light">Volver al Inicio</a>
            </div>
        </nav>

        <asp:HiddenField ID="hfMateriaId" runat="server" />

        <asp:UpdatePanel ID="UpdatePanelFormulario" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="card shadow-sm mb-4">
                    <div class="card-body">
                        <h5 class="card-title">Formulario de Materia</h5>

                        <div class="mb-3">
                            <label class="form-label">Nombre de la Materia:</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Descripción:</label>
                            <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Profesor:</label>
                            <asp:DropDownList ID="ddlProfesores" runat="server" CssClass="form-select" />
                        </div>

                        <div class="d-flex gap-2">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
                        </div>

                        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" CssClass="mt-3 fw-bold d-block" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="card shadow-sm">
                    <div class="card-body">
                        <h5 class="card-title">Lista de Materias</h5>
                        <asp:GridView ID="gvMaterias" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover"
                            OnRowCommand="gvMaterias_RowCommand"
                            OnPageIndexChanging="gvMaterias_PageIndexChanging"
                            AllowPaging="True" PageSize="5"
                            DataKeyNames="MateriaId">
                            <Columns>
                                <asp:BoundField DataField="MateriaId" HeaderText="ID" />
                                <asp:BoundField DataField="Mat_Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Mat_Descripcion" HeaderText="Descripción" />
                                <asp:BoundField DataField="ProfesorNombre" HeaderText="Profesor" />
                                <asp:TemplateField HeaderText="Foto Profesor">
                                    <ItemTemplate>
                                        <asp:Image ID="imgProfesor" runat="server" Width="70px" Height="70px" CssClass="rounded"
                                            ImageUrl='<%# Eval("ProfesorFoto") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("MateriaId") %>'
                                            Text="Editar" CssClass="btn btn-sm btn-warning me-1" />
                                        <asp:Button ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("MateriaId") %>'
                                            Text="Eliminar" CssClass="btn btn-sm btn-danger"
                                            OnClientClick="return confirm('¿Está seguro de eliminar esta materia?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvMaterias" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
