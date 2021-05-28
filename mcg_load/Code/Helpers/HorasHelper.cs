using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace mcg_load.Code.Helpers
{
    public class HorasHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<Esc_Horas> GetEscHoras()
        {
            List<Esc_Horas> EscHoras = db.Esc_Horas.ToList();
            return EscHoras;
        }
        public static List<Esc_Horas> GetEscHoras(int id_escenario,string Linea)
        {
            List<Esc_Horas> EscHoras = db.Esc_Horas.Where(t => t.id_escenario == id_escenario && t.Linea.Equals(Linea)).ToList();
            return EscHoras;
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

        public static Esc_Horas FindRecord(long id)
        {
            Esc_Horas EscHoras = db.Esc_Horas.FirstOrDefault(i => i.id_escenario == id);
            return EscHoras;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_Horas EscHoras)
        {
            db.Esc_Horas.Add(EscHoras);
            db.SaveChanges();
        }

        public static void UpdateRecord(Esc_Horas EscHoras)
        {
            Esc_Horas EscHoras01 =
                db.Esc_Horas.FirstOrDefault(t => t.id_escenario == EscHoras.id_escenario && t.Fecha == EscHoras.Fecha);

            Boolean bandera = false;
            if (EscHoras01 != null)
            {
                if (EscHoras01.Fecha != EscHoras.Fecha)
                {
                    EscHoras01.Fecha = EscHoras.Fecha;
                    bandera = true;
                }

                //if (escRechazo01.Combinado != escRechazo.Combinado)
                //{
                //    escRechazo01.Combinado = escRechazo.Combinado;
                //    bandera = true;
                //}

                //if (escRechazo01.Externo != escRechazo.Externo)
                //{
                //    escRechazo01.Externo = escRechazo.Externo;
                //    bandera = true;
                //}

                //if (escRechazo01.Interno != escRechazo.Interno)
                //{
                //    escRechazo01.Interno = escRechazo.Interno;
                //    bandera = true;
                //}

                //if (escRechazo01.UserId != escRechazo.UserId)
                //{
                //    escRechazo01.UserId = escRechazo.UserId;
                //    bandera = true;
                //}
            }

            if (bandera)
            {
                db.Esc_Horas.AddOrUpdate(EscHoras01);
                db.SaveChanges();
            }
        }


        public static void DeleteRecords(Esc_Horas escHoras)
        {
            Esc_Horas escRechazo01 = db.Esc_Horas.FirstOrDefault(t =>
                t.id_escenario == escHoras.id_escenario && t.Fecha == escHoras.Fecha);
            if (!escRechazo01.Equals(null))
            {
                db.Esc_Horas.Remove(escRechazo01);
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                Esc_Horas escRechazo = null;
                escRechazo = db.Esc_Horas.FirstOrDefault(t => t.id_escenario == selectedId);

                db.Esc_Horas.Remove(escRechazo);
                db.SaveChanges();
            }
        }
    }
}