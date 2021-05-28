using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace mcg_load.Code.Helpers
{
    public class ParametrosHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<Esc_Parametros> GetEscParametros()
        {
            List<Esc_Parametros> EscParametros = db.Esc_Parametros.ToList();
            return EscParametros;
        }
        public static List<Esc_Parametros> GetEscParametros(int id_articulo, int id_escenario)
        {
            List<Esc_Parametros> EscParametros = db.Esc_Parametros.Where(t => t.id_articulo == id_articulo && t.id_escenario == id_escenario).ToList();
            return EscParametros;
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

        public static Esc_Parametros FindRecord(long id)
        {
            Esc_Parametros escParametros = db.Esc_Parametros.FirstOrDefault(i => i.id_escenario == id);
            return escParametros;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_Parametros escParametros)
        {
            db.Esc_Parametros.Add(escParametros);
            db.SaveChanges();
        }

        public static void UpdateRecord(Esc_Parametros escParametros)
        {
            Esc_Parametros escParametros01 =
                db.Esc_Parametros.FirstOrDefault(t =>
                    t.id_escenario == escParametros.id_escenario && t.Año == escParametros.Año &&
                    t.id_articulo == escParametros.id_articulo);

            Boolean bandera = false;
            if (escParametros01 != null)
            {
                if (escParametros01.Año != escParametros.Año)
                {
                    escParametros01.Año = escParametros.Año;
                    bandera = true;
                }

                if (escParametros01.Valor!= escParametros.Valor)
                {
                    escParametros01.Valor = escParametros.Valor;
                    bandera = true;
                }
            }

            if (bandera)
            {
                escParametros01.UserId_modifico = escParametros.UserId_modifico;
                escParametros01.modificado = true;
                escParametros01.fecha_modificacion = DateTime.Now;
                db.Esc_Parametros.AddOrUpdate(escParametros01);
                db.SaveChanges();
            }
        }


        public static void DeleteRecords(Esc_Parametros escRechazo)
        {
            Esc_Parametros escRechazo01 = db.Esc_Parametros.FirstOrDefault(t =>
                t.id_escenario == escRechazo.id_escenario && t.Año == escRechazo.Año &&
                t.id_articulo == escRechazo.id_articulo);
            if (!escRechazo01.Equals(null))
            {
                db.Esc_Parametros.Remove(escRechazo01);
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                Esc_Parametros escRechazo = null;
                escRechazo = db.Esc_Parametros.FirstOrDefault(t => t.id_escenario == selectedId);

                db.Esc_Parametros.Remove(escRechazo);
                db.SaveChanges();
            }
        }
    }
}