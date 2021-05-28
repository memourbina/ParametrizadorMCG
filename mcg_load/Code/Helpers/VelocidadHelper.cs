using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace mcg_load.Code.Helpers
{
    public class VelocidadHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<Esc_Velocidad> GetEscVelocidades()
        {
            List<Esc_Velocidad> escVelocidades = db.Esc_Velocidad.ToList();
            return escVelocidades;
        }

        public static List<Esc_Velocidad> GetEscVelocidades(int id_articulo, int id_escenario)
        {
            List<Esc_Velocidad> escVelocidades = db.Esc_Velocidad.Where(t=> t.id_articulo == id_articulo && t.id_escenario == id_escenario).ToList();
            return escVelocidades;
        }


        public static Esc_Velocidad FindRecord(long id)
        {
            Esc_Velocidad escVelocidad = db.Esc_Velocidad.FirstOrDefault(i => i.id_escenario == id);
            return escVelocidad;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_Velocidad escVelocidad)
        {
            db.Esc_Velocidad.Add(escVelocidad);
            db.SaveChanges();
        }

        public static void UpdateRecord(Esc_Velocidad escVelocidad)
        {
            Esc_Velocidad escVelocidad01 = db.Esc_Velocidad.FirstOrDefault(t =>
                t.id_escenario == escVelocidad.id_escenario && t.id_articulo == escVelocidad.id_articulo &&
                t.Año == escVelocidad.Año);

            Boolean bandera = false;
            if (escVelocidad01 != null)
            {
                if (escVelocidad01.eliminado != escVelocidad.eliminado)
                {
                    if (escVelocidad.eliminado!=null)
                    escVelocidad01.eliminado = escVelocidad.eliminado;
                    
                    bandera = true;
                }

                if (escVelocidad01.Valor != escVelocidad.Valor)
                {
                    escVelocidad01.Valor = escVelocidad.Valor;
                    bandera = true;
                }

                if (bandera)
                {
                    escVelocidad01.UserId_modifico = escVelocidad.UserId_modifico;
                    escVelocidad01.modificado = true;
                    db.Esc_Velocidad.AddOrUpdate(escVelocidad01);
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecords(int id)
        {
            Esc_Velocidad escVelocidad = db.Esc_Velocidad.FirstOrDefault(t => t.id_escenario == id);

            if (!escVelocidad.Equals(null))
            {
                db.Esc_Velocidad.Remove(escVelocidad);
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                Esc_Velocidad escVelocidad = null;
                escVelocidad = db.Esc_Velocidad.FirstOrDefault(t => t.id_escenario == selectedId);

                db.Esc_Velocidad.Remove(escVelocidad);
                db.SaveChanges();
            }
        }
    }
}