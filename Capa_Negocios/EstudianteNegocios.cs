using System;
using System.Collections.Generic;
using System.Linq;
using Capa_Datos;  // Tu DataContext y entidades Linq to SQL

namespace Negocio
{
    public class EstudianteNegocio
    {
        private DataClasses1DataContext contexto;

        public EstudianteNegocio()
        {
            contexto = new DataClasses1DataContext();
        }

        // Listar estudiantes junto con info de materia y profesor para mostrar en GridView
        public List<dynamic> ListarConMateria()
        {
            var consulta = (from e in contexto.Estudiante
                            join m in contexto.Materia on e.MateriaId equals m.MateriaId into matJoin
                            from m in matJoin.DefaultIfEmpty()
                            join p in contexto.Profesor on m.ProfesorId equals p.ProfesorId into profJoin
                            from p in profJoin.DefaultIfEmpty()
                            select new
                            {
                                EstudianteId = e.EstudianteId,
                                NombreEstu = e.Alu_Nombre,
                                FechaNac = e.Alu_FechaNacimiento,
                                Signo = e.Alu_SignoZodiacal,
                                FotoEstuPath = e.Alu_Foto,
                                Materia = m != null ? m.Mat_Nombre : "",
                                Profesor = p != null ? p.Pro_Nombre : "",
                                FotoProfesorPath = p != null ? p.Pro_FotoPath : null
                            }).ToList();

            List<dynamic> resultado = new List<dynamic>();
            foreach (var item in consulta)
            {
                string fotoUrl = item.FotoEstuPath != null ? item.FotoEstuPath : null;

                resultado.Add(new
                {
                    item.EstudianteId,
                    item.NombreEstu,
                    item.FechaNac,
                    item.Signo,
                    FotoEstu = fotoUrl,
                    item.Materia,
                    item.Profesor,
                    FotoProfesor = item.FotoProfesorPath
                });
            }

            return resultado;
        }

        public Estudiante ObtenerPorId(int id)
        {
            return (from e in contexto.Estudiante
                    where e.EstudianteId == id
                    select e).FirstOrDefault();
        }

        public void Insertar(Estudiante estudiante)
        {
            // CORRECCIÓN 1: Autogenerar el ID para evitar el error de Llave Primaria duplicada (0)
            if (estudiante.EstudianteId == 0)
            {
                int maxId = contexto.Estudiante.Any() ? contexto.Estudiante.Max(e => e.EstudianteId) : 0;
                estudiante.EstudianteId = maxId + 1;
            }

            // CORRECCIÓN 2: Proteger la asignación del signo zodiacal por si la fecha viene nula
            if (estudiante.Alu_FechaNacimiento.HasValue)
            {
                estudiante.Alu_SignoZodiacal = CalcularSignoZodiacal(estudiante.Alu_FechaNacimiento.Value);
            }

            contexto.Estudiante.InsertOnSubmit(estudiante);
            contexto.SubmitChanges();
        }

        public void Actualizar(Estudiante estudianteActualizado)
        {
            var estudiante = (from e in contexto.Estudiante
                              where e.EstudianteId == estudianteActualizado.EstudianteId
                              select e).FirstOrDefault();

            if (estudiante != null)
            {
                estudiante.Alu_Nombre = estudianteActualizado.Alu_Nombre;
                estudiante.Alu_FechaNacimiento = estudianteActualizado.Alu_FechaNacimiento;
                estudiante.MateriaId = estudianteActualizado.MateriaId;
                estudiante.Alu_Foto = estudianteActualizado.Alu_Foto;

                // CORRECCIÓN 2 (Continuación): Proteger también en la actualización
                if (estudianteActualizado.Alu_FechaNacimiento.HasValue)
                {
                    estudiante.Alu_SignoZodiacal = CalcularSignoZodiacal(estudianteActualizado.Alu_FechaNacimiento.Value);
                }
                else
                {
                    estudiante.Alu_SignoZodiacal = null;
                }

                contexto.SubmitChanges();
            }
        }

        public bool Eliminar(int id)
        {
            var estudiante = (from e in contexto.Estudiante
                              where e.EstudianteId == id
                              select e).FirstOrDefault();

            if (estudiante != null)
            {
                contexto.Estudiante.DeleteOnSubmit(estudiante);
                contexto.SubmitChanges();
                return true;
            }
            return false;
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

        public bool TieneEstudiantesEnMateria(int materiaId)
        {
            var estudiantes = from e in contexto.Estudiante
                              where e.MateriaId == materiaId
                              select e;

            return estudiantes.Any();
        }
    }
}