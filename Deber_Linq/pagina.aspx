<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pagina.aspx.cs" Inherits="Deber_Linq.pagina" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <title>Gestión Académica</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .hero-buttons a {
            min-width: 200px;
            font-size: 1.2rem;
            margin: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
 
        <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
            <div class="container-fluid">
                <a class="navbar-brand" href="#">Gestión Académica</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item"><a class="nav-link" href="estudiantes.aspx">Alumnos</a></li>
                        <li class="nav-item"><a class="nav-link" href="default.aspx">Profesores</a></li>
                        <li class="nav-item"><a class="nav-link" href="materias.aspx">Materias</a></li>
                    </ul>
                </div>
            </div>
        </nav>


        <div class="container d-flex flex-column justify-content-center align-items-center text-center" style="min-height: 80vh;">
            <h1 class="display-5 mb-4">Bienvenido al Sistema Académico</h1>
            <p class="mb-5 text-muted">Selecciona una opción para comenzar</p>
            <div class="hero-buttons d-flex flex-wrap justify-content-center">
                <a href="Estudiantes.aspx" class="btn btn-outline-primary btn-lg rounded-pill shadow">Gestionar Alumnos</a>
                <a href="default.aspx" class="btn btn-outline-success btn-lg rounded-pill shadow">Gestionar Profesores</a>
                <a href="materias.aspx" class="btn btn-outline-danger btn-lg rounded-pill shadow">Gestionar Materias</a>
            </div>
        </div>


        <footer class="bg-dark text-white text-center py-4">
            <div class="container">
                <p class="mb-0">© 2025 Gestión Académica | Desarrollado por TuNombre</p>
            </div>
        </footer>
    </form>

    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
