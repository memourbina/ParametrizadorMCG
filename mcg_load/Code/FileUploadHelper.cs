using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;
using System.Security.Cryptography;

namespace mcg_load.Code
{
    public class FileUploadHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();
        private static SHA256 Sha256 = SHA256.Create();

        public static List<Esc_File_Upload> GetFileUploads()
        {
            List<Esc_File_Upload> escFileUploads = db.Esc_File_Upload.ToList();
            return escFileUploads;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_File_Upload escFileUpload)
        {
            db.Esc_File_Upload.Add(escFileUpload);
        }

        public static void UpdateRecord(Esc_File_Upload escFileUpload)
        {
            Esc_File_Upload escFileUpload01 =
                db.Esc_File_Upload.FirstOrDefault(t => t.id_UpLoad == escFileUpload.id_UpLoad);

            Boolean bandera = false;
            if (escFileUpload01 != null)
            {
                if (escFileUpload01.eliminado != escFileUpload.eliminado)
                {
                    escFileUpload01.eliminado = escFileUpload.eliminado;
                    bandera = true;
                }

                if (escFileUpload01.nombre != escFileUpload.nombre)
                {
                    escFileUpload01.nombre = escFileUpload.nombre;
                    bandera = true;
                }

                if (escFileUpload01.comentario != escFileUpload.comentario)
                {
                    escFileUpload01.comentario = escFileUpload.comentario;
                    bandera = true;
                }

                if (bandera)
                {
                    db.Esc_File_Upload.AddOrUpdate(escFileUpload01);
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecords(int id)
        {
            Esc_File_Upload escFileUpload = db.Esc_File_Upload.FirstOrDefault(t => t.id_UpLoad == id);
            //List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            //DataProvider.DeleteIssues(selectedIds);
            if (!escFileUpload.Equals(null))
            {
                db.Esc_File_Upload.Remove(escFileUpload);
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));


            foreach (var selectedId in selectedIds)
            {
                Esc_File_Upload escFileUpload = null;
                escFileUpload = db.Esc_File_Upload.FirstOrDefault(t => t.id_UpLoad == selectedId);
                if (escFileUpload != null && escFileUpload.Esc_Escenario.Count == 0)
                {
                    db.Esc_File_Upload.Remove(escFileUpload);
                    db.SaveChanges();
                }
            }
        }

        #region Validación HASH

        public static bool ValidateHashFile(string filename, Stream stream, ref string strHash, ref Esc_File_Upload EscFileUpload)
        {
            //string strHash02 = BytesToString(GetHashSha256(stream));

            //string strHash02 = BitConverter.ToString(GetHashSha256(stream)).Replace("-", "").ToLowerInvariant();

            string strHash02 = BitConverter.ToString(GetHashSha256(filename)).Replace("-", "").ToLowerInvariant();

            bool bandera = false;

            strHash = strHash02;

            EscFileUpload = db.Esc_File_Upload.FirstOrDefault(t => t.hashFile.Equals(strHash02));

            bandera = EscFileUpload == null;

            return bandera;
        }

        private static byte[] GetHashSha256(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }

        private static byte[] GetHashSha256(Stream stream)
        {
            return Sha256.ComputeHash(stream);
        }

        // Return a byte array as a sequence of hex values.
        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }

        #endregion Validación HASH
    }
}