using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace mcg_load.Code.Helpers
{
    public class RechazoHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<Esc_Rechazo> GetEscRechazos()
        {
            List<Esc_Rechazo> escRechazos = db.Esc_Rechazo.ToList();
            return escRechazos;
        }

        public static List<Esc_Rechazo> GetEscRechazos(int id_articulo, int id_escenario)
        {
            List<Esc_Rechazo> escRechazos = db.Esc_Rechazo
                .Where(t => t.id_articulo == id_articulo && t.id_escenario == id_escenario).ToList();
            return escRechazos;
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

        public static Esc_Rechazo FindRecord(long id)
        {
            Esc_Rechazo escRechazo = db.Esc_Rechazo.FirstOrDefault(i => i.id_escenario == id);
            return escRechazo;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_Rechazo escRechazo)
        {
            db.Esc_Rechazo.Add(escRechazo);
            db.SaveChanges();
        }

        public static void UpdateRecord(Esc_Rechazo escRechazo)
        {
            Esc_Rechazo escRechazo01 =
                db.Esc_Rechazo.FirstOrDefault(t =>
                    t.id_escenario == escRechazo.id_escenario && t.Año == escRechazo.Año &&
                    t.id_articulo == escRechazo.id_articulo);

            Boolean bandera = false;
            if (escRechazo01 != null)
            {
                if (escRechazo01.eliminado == null || escRechazo01.eliminado != escRechazo.eliminado)
                {
                    escRechazo01.eliminado = escRechazo.eliminado ?? escRechazo01.eliminado;

                    bandera = true;
                }

                if (escRechazo01.Externo != escRechazo.Externo)
                {
                    escRechazo01.Externo = escRechazo.Externo;
                    bandera = true;
                }

                if (escRechazo01.Interno != escRechazo.Interno)
                {
                    escRechazo01.Interno = escRechazo.Interno;
                    bandera = true;
                }

                if (escRechazo01.UserId_modifico != escRechazo.UserId_modifico)
                {
                    escRechazo01.UserId_modifico = escRechazo.UserId_modifico;
                    bandera = true;
                }
            }

            if (bandera)
            {
                escRechazo01.Combinado = (1 - (1 - escRechazo.Interno) * (1 - escRechazo.Externo));
                escRechazo01.modificado = true;
                escRechazo01.fecha_modificacion = DateTime.Now;
                db.Esc_Rechazo.AddOrUpdate(escRechazo01);
                db.SaveChanges();
            }
        }


        public static void DeleteRecords(Esc_Rechazo escRechazo)
        {
            Esc_Rechazo escRechazo01 = db.Esc_Rechazo.FirstOrDefault(t =>
                t.id_escenario == escRechazo.id_escenario && t.Año == escRechazo.Año &&
                t.id_articulo == escRechazo.id_articulo);
            if (!escRechazo01.Equals(null))
            {
                db.Esc_Rechazo.Remove(escRechazo01);
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                Esc_Rechazo escRechazo = null;
                escRechazo = db.Esc_Rechazo.FirstOrDefault(t => t.id_escenario == selectedId);

                db.Esc_Rechazo.Remove(escRechazo);
                db.SaveChanges();
            }
        }
    }
}