using System;
using System.Collections.Generic;
using System.Linq;
using Capa_Datos;

namespace TuProyectoWeb.Negocio
{
    public class MateriaNegocio
    {
        // === ¡MÉTODO RESTAURADO! ===
        public List<Materia> ObtenerTodas()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return (from m in db.Materia select m).ToList();
            }
        }

        // === Método para el GridView con datos combinados ===
        public List<object> ObtenerTodasConProfesor()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return (from m in db.Materia
                        join p in db.Profesor on m.ProfesorId equals p.ProfesorId
                        select new
                        {
                            m.MateriaId,
                            m.Mat_Nombre,
                            m.Mat_Descripcion,
                            ProfesorNombre = p.Pro_Nombre,
                            ProfesorFoto = p.Pro_FotoPath
                        }).ToList<object>();
            }
        }

        public Materia ObtenerPorId(int id)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return (from m in db.Materia where m.MateriaId == id select m).FirstOrDefault();
            }
        }

        public void Insertar(Materia materia)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                // Prevenir error de Llave Primaria en Materia si no hay autoincremento
                if (materia.MateriaId == 0)
                {
                    int maxId = db.Materia.Any() ? db.Materia.Max(m => m.MateriaId) : 0;
                    materia.MateriaId = maxId + 1;
                }

                db.Materia.InsertOnSubmit(materia);
                db.SubmitChanges();
            }
        }

        public void Actualizar(Materia materia)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var existente = (from m in db.Materia where m.MateriaId == materia.MateriaId select m).FirstOrDefault();
                if (existente != null)
                {
                    existente.Mat_Nombre = materia.Mat_Nombre;
                    existente.Mat_Descripcion = materia.Mat_Descripcion;
                    existente.ProfesorId = materia.ProfesorId;
                    db.SubmitChanges();
                }
            }
        }

        public bool Eliminar(int id)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                bool tieneEstudiantes = db.Estudiante.Any(e => e.MateriaId == id);
                if (tieneEstudiantes) return false;

                var materia = db.Materia.FirstOrDefault(m => m.MateriaId == id);
                if (materia != null)
                {
                    db.Materia.DeleteOnSubmit(materia);
                    db.SubmitChanges();
                    return true;
                }
                return false;
            }
        }
    }
}