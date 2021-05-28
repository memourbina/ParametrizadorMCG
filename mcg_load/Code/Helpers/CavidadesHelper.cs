using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace mcg_load.Code.Helpers
{
    public class CavidadesHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<Esc_Cavidades> GetEscCavidades()
        {
            List<Esc_Cavidades> escCavidades = db.Esc_Cavidades.ToList();
            return escCavidades;
        }

        public static List<Esc_Cavidades> GetEscCavidades(int id_articulo, int id_escenario)
        {
            List<Esc_Cavidades> cavidadesList= db.Esc_Cavidades.Where(t => t.id_articulo == id_articulo && t.id_escenario == id_escenario).ToList();
            return cavidadesList;
        }

        public static List<Esc_Tipo_Escenarios> GetEscTipoEscenarios()
        {
            List<Esc_Tipo_Escenarios> escTipoEscenariosList = db.Esc_Tipo_Escenarios.ToList();
            return escTipoEscenariosList;
        }

        public static List<Esc_File_Upload> GetFileUploads()
        {
            List<Esc_File_Upload> escFileUploads = db.Esc_File_Upload.Where(i => i.eliminado == false).ToList();
            return escFileUploads;
        }

        public static Esc_Cavidades FindRecord(long id)
        {
            Esc_Cavidades escCavidad = db.Esc_Cavidades.FirstOrDefault(i => i.id_escenario == id);
            return escCavidad;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_Cavidades escCavidad)
        {
            db.Esc_Cavidades.Add(escCavidad);
            db.SaveChanges();
        }

        public static void UpdateRecord(Esc_Cavidades escCavidad)
        {
            Esc_Cavidades escCavidad01 =
                db.Esc_Cavidades.FirstOrDefault(t =>
                    t.id_escenario == escCavidad.id_escenario && t.Año == escCavidad.Año &&
                    t.id_articulo == escCavidad.id_articulo);

            Boolean bandera = false;
            if (escCavidad01 != null)
            {
                if (escCavidad01.eliminado != escCavidad.eliminado)
                {
                    escCavidad01.eliminado = escCavidad.eliminado ?? escCavidad01.eliminado;
                    bandera = true;
                }

                if (escCavidad01.Cantidad != escCavidad.Cantidad)
                {
                    escCavidad01.Cantidad = escCavidad.Cantidad;
                    bandera = true;
                }

                if (escCavidad01.UserId != escCavidad.UserId)
                {
                    escCavidad01.UserId = escCavidad.UserId ?? escCavidad01.UserId;
                    bandera = true;
                }

                if (bandera)
                {
                    escCavidad01.UserId_modifico = escCavidad.UserId_modifico;
                    escCavidad01.modificado = true;
                    escCavidad01.fecha_modificacion = DateTime.Now;
                    db.Esc_Cavidades.AddOrUpdate(escCavidad01);
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecords(Esc_Cavidades escCavidad)
        {
            Esc_Cavidades escEscenario = db.Esc_Cavidades.FirstOrDefault(t =>
                t.id_escenario == escCavidad.id_escenario && t.Año == escCavidad.Año &&
                t.id_articulo == escCavidad.id_articulo);

            if (!escEscenario.Equals(null))
            {
                db.Esc_Cavidades.Remove(escEscenario);
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                Esc_Cavidades escEscenario = null;
                escEscenario = db.Esc_Cavidades.FirstOrDefault(t => t.id_escenario == selectedId);

                db.Esc_Cavidades.Remove(escEscenario);
                db.SaveChanges();
            }
        }
    }
}