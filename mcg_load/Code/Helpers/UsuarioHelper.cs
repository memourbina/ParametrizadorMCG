using System.Collections.Generic;
using System.Linq;
using DevExpress.Web.Mvc;

namespace mcg_load.Code.Helpers
{
    public class UsuarioHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<AspNetUsers> GetAspNetUsers()
        {
            List<AspNetUsers> EscUsuario = db.AspNetUsers.ToList();
            return EscUsuario;
        }
        public static List<AspNetUsers> GetAspNetUsers(string Id)
        {
            List<AspNetUsers> EscUsuario = db.AspNetUsers.Where(t => t.Id.Equals(Id)).ToList();
            return EscUsuario;
        }

        public static List<AspNetRoles> GetEscRoles()
        {
            List<AspNetRoles> escAspNetRoles = db.AspNetRoles.ToList();
            return escAspNetRoles;
        }

        public static List<Esc_File_Upload> GetFileUploads()
        {
            List<Esc_File_Upload> escFileUploads = db.Esc_File_Upload.Where(i => i.eliminado == false).ToList();
            return escFileUploads;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(AspNetUsers EscUsuario)
        {
            db.AspNetUsers.Add(EscUsuario);
            db.SaveChanges();
        }

        //public static void UpdateRecord(AspNetUsers EscUsuario)
        //{
        //    AspNetUsers EscUsuario01 =
        //        db.AspNetUsers.FirstOrDefault(t => t.id_escenario == EscUsuario.id_escenario && t.Fecha == EscUsuario.Fecha);

        //    Boolean bandera = false;
        //    if (EscUsuario01 != null)
        //    {
        //        if (EscUsuario01.Fecha != EscUsuario.Fecha)
        //        {
        //            EscUsuario01.Fecha = EscUsuario.Fecha;
        //            bandera = true;
        //        }

        //        //if (escRechazo01.Combinado != escRechazo.Combinado)
        //        //{
        //        //    escRechazo01.Combinado = escRechazo.Combinado;
        //        //    bandera = true;
        //        //}

        //        //if (escRechazo01.Externo != escRechazo.Externo)
        //        //{
        //        //    escRechazo01.Externo = escRechazo.Externo;
        //        //    bandera = true;
        //        //}

        //        //if (escRechazo01.Interno != escRechazo.Interno)
        //        //{
        //        //    escRechazo01.Interno = escRechazo.Interno;
        //        //    bandera = true;
        //        //}

        //        //if (escRechazo01.UserId != escRechazo.UserId)
        //        //{
        //        //    escRechazo01.UserId = escRechazo.UserId;
        //        //    bandera = true;
        //        //}
        //    }

        //    if (bandera)
        //    {
        //        db.AspNetUsers.AddOrUpdate(EscUsuario01);
        //        db.SaveChanges();
        //    }
        //}


        //public static void DeleteRecords(AspNetUsers escHoras)
        //{
        //    AspNetUsers escRechazo01 = db.AspNetUsers.FirstOrDefault(t =>
        //        t.id_escenario == escHoras.id_escenario && t.Fecha == escHoras.Fecha);
        //    if (!escRechazo01.Equals(null))
        //    {
        //        db.AspNetUsers.Remove(escRechazo01);
        //    }
        //}

        //public static void DeleteRecords(string selectedRowIds)
        //{
        //    List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
        //    foreach (var selectedId in selectedIds)
        //    {
        //        AspNetUsers escRechazo = null;
        //        escRechazo = db.AspNetUsers.FirstOrDefault(t => t.id_escenario == selectedId);

        //        db.AspNetUsers.Remove(escRechazo);
        //        db.SaveChanges();
        //    }
        //}
    }
}