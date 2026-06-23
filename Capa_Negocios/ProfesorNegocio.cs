using System;
using System.Collections.Generic;
using System.Linq;
using Capa_Datos;

namespace Capa_Negocios
{
    public class ProfesorNegocio
    {
        private DataClasses1DataContext contexto;

        public ProfesorNegocio()
        {
            contexto = new DataClasses1DataContext();
        }

        public List<Profesor> Listar()
        {
            return contexto.Profesor.ToList();
        }

        public Profesor ObtenerPorId(int id)
        {
            return contexto.Profesor.FirstOrDefault(p => p.ProfesorId == id);
        }

        public void Insertar(Profesor profesor)
        {
            // CORRECCIÓN 1: Evitar error de Llave Primaria (0)
            // Si el ID es 0 (no se asignó), buscamos el último ID en la BD y le sumamos 1.
            if (profesor.ProfesorId == 0)
            {
                int maxId = contexto.Profesor.Any() ? contexto.Profesor.Max(p => p.ProfesorId) : 0;
                profesor.ProfesorId = maxId + 1;
            }

            // CORRECCIÓN 2: Evitar error si la fecha de nacimiento viene nula
            if (profesor.Pro_FechaNacimiento.HasValue)
            {
                profesor.Pro_SignoZodiacal = CalcularSignoZodiacal(profesor.Pro_FechaNacimiento.Value);
            }

            contexto.Profesor.InsertOnSubmit(profesor);
            contexto.SubmitChanges();
        }

        public void Actualizar(Profesor profesorActualizado)
        {
            var profesor = contexto.Profesor.FirstOrDefault(p => p.ProfesorId == profesorActualizado.ProfesorId);

            if (profesor != null)
            {
                profesor.Pro_Nombre = profesorActualizado.Pro_Nombre;
                profesor.Pro_FechaNacimiento = profesorActualizado.Pro_FechaNacimiento;
                profesor.Pro_Especialidad = profesorActualizado.Pro_Especialidad;
                profesor.Pro_FotoPath = profesorActualizado.Pro_FotoPath;

                // CORRECCIÓN 2: Validar también en la actualización
                if (profesorActualizado.Pro_FechaNacimiento.HasValue)
                {
                    profesor.Pro_SignoZodiacal = CalcularSignoZodiacal(profesorActualizado.Pro_FechaNacimiento.Value);
                }
                else
                {
                    profesor.Pro_SignoZodiacal = null; // O el valor por defecto que prefieras
                }

                contexto.SubmitChanges();
            }
        }

        public bool Eliminar(int id)
        {
            if (TieneMateriasAsignadas(id))
                return false;

            var profesor = contexto.Profesor.FirstOrDefault(p => p.ProfesorId == id);
            if (profesor != null)
            {
                contexto.Profesor.DeleteOnSubmit(profesor);
                contexto.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool TieneMateriasAsignadas(int profesorId)
        {
            return contexto.Materia.Any(m => m.ProfesorId == profesorId);
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
    }
}