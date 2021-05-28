using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;
using mcg_load.Models;

namespace mcg_load.Code.Helpers
{
    public class ArticuloHelper
    {

        private static string SQL_ARTICULO_COPIA_ARTICULO = "INSERT INTO PVO_Escenarios.dbo.Esc_Articulos (id_articulo, id_escenario, [Org# ID], Escenario, Probabilidad, " +
                                                            "Probabilidad_base ,[Línea Moldeo], [Cliente], [Customer Name], [Industria],[Tipo Pieza] ,[Description],[No# Oracle]," +
                                                            "[Customer PN],[Unit Weight],[Mercado],[Tipo de Hierro],[Grado de Hierro],[OEM],[Plataforma],[Sistema],[Cavs /  Soplo]," +
                                                            "[Soplos / Hora],[Cor / pza],[Maquina],[id_maquina],[Planta],[Planta01],[Linea],[modificado],[UserId_modifico],[fecha_modificacion])" +
                                                            "(SELECT {0}, A.[id_escenario],[Org# ID],[Escenario],[Probabilidad],[Probabilidad_base],[Línea Moldeo],[Cliente],[Customer Name]," +
                                                            "[Industria],[Tipo Pieza],[Description],[No# Oracle],[Customer PN],[Unit Weight],[Mercado],[Tipo de Hierro],[Grado de Hierro]," +
                                                            "[OEM],[Plataforma],[Sistema],[Cavs /  Soplo],[Soplos / Hora],[Cor / pza],[Maquina],[id_maquina],[Planta],[Planta01],[Linea]," +
                                                            "[modificado],[UserId_modifico],[fecha_modificacion] " +
                                                            "FROM[PVO_Escenarios].[dbo].[Esc_Articulos] A " +
                                                            "WHERE A.id_articulo  = {1} " +
                                                            "AND   A.id_escenario = {2})";

        private static string SQL_ARTICULOS_COPIA_DEMANDA = "INSERT INTO PVO_Escenarios.dbo.Esc_Demanda " +
            "(id_articulo, Fecha, id_escenario, UserId, Valor, Valor_base, eliminado, modificacion" +
            ", UserId_modifico, fecha_modificacion) " +
            "(SELECT {0}, Fecha, id_escenario, UserId, Valor, Valor_base, eliminado, modificacion" +
            ", UserId_modifico, fecha_modificacion " +
            "FROM PVO_Escenarios.dbo.Esc_Demanda " +
            "WHERE id_articulo = {1} " +
            "AND   id_escenario = {2})";

        private static string SQL_ARTICULOS_COPIA_CAVIDADES = "INSERT INTO PVO_Escenarios.dbo.Esc_Cavidades " +
            "(id_articulo, Año, id_escenario, UserId, Cantidad, Cantidad_base, eliminado, modificado" +
            ", UserId_modifico, fecha_modificacion) " +
            "(SELECT {0}, Año, id_escenario, UserId, Cantidad, Cantidad_base, eliminado, modificado" +
            ", UserId_modifico, fecha_modificacion " +
            "FROM PVO_Escenarios.dbo.Esc_Cavidades " +
            "WHERE id_articulo = {1} " +
            "AND   id_escenario = {2})";


        private static string SQL_ARTICULOS_COPIA_RECHAZO = "INSERT INTO PVO_Escenarios.dbo.Esc_Rechazo " +
                                                            "(id_articulo, Año, id_escenario, UserId, Interno, Externo, Combinado" +
                                                            ", Interno_base, Externo_base, Combinado_base, eliminado, modificado" +
                                                            ", UserId_modifico, fecha_modificacion) " +
                                                            "(SELECT {0}, Año, id_escenario, UserId, Interno, Externo, Combinado" +
                                                            ", Interno_base, Externo_base, Combinado_base, eliminado, modificado" +
                                                            ", UserId_modifico, fecha_modificacion " +
                                                            "FROM PVO_Escenarios.dbo.Esc_Rechazo " +
                                                            "WHERE id_articulo = {1} " +
                                                            "AND   id_escenario = {2})";

        private static string SQL_ARTICULOS_COPIA_VELOCIDAD = "INSERT INTO PVO_Escenarios.dbo.Esc_Velocidad " +
            " (id_articulo, Año, id_escenario, UserId, Valor, Valor_base, eliminado, modificado" +
            ", fecha_modificacion, UserId_modifico) " +
            "(SELECT {0}, Año, id_escenario, UserId, Valor, Valor_base, eliminado, modificado" +
            ", fecha_modificacion, UserId_modifico " +
            "FROM PVO_Escenarios.dbo.Esc_Velocidad " +
            "WHERE id_articulo = {1} " +
            "AND   id_escenario = {2})";


        private static string SQL_DEMANDA_FECHA_BASE = "UPDATE A SET Valor_base = 0 FROM PVO_Escenarios.dbo.Esc_Demanda A WHERE id_articulo = {0} AND id_escenario = {1}";
        private static string SQL_DEMANDA_FECHA_VALOR_OLD = "UPDATE A SET [Valor] = 0 FROM PVO_Escenarios.dbo.Esc_Demanda A WHERE id_articulo = {0} AND id_escenario = {1} AND Fecha >= {2}";
        private static string SQL_DEMANDA_FECHA_VALOR_NEW = "UPDATE A SET [Valor] = 0 FROM PVO_Escenarios.dbo.Esc_Demanda A WHERE id_articulo = {0} AND id_escenario = {1} AND Fecha < {2}";
        
        //private static string SQL_RECHAZO_FECHA_BASE = "UPDATE A SET Interno_base = 0 ,Externo_base = 0, Combinado_base = 0  FROM PVO_Escenarios.dbo.Esc_Rechazo A WHERE id_articulo = {0} AND id_escenario = {1}";
        //private static string SQL_RECHAZO_FECHA_VALOR_OLD = "UPDATE A SET Interno = 0 ,Externo = 0  ,Combinado = 0  FROM PVO_Escenarios.dbo.Esc_Rechazo A WHERE id_articulo = {0} AND id_escenario = {1} AND [Año] >= {2}";
        //private static string SQL_RECHAZO_FECHA_VALOR_NEW = "UPDATE A SET Interno = 0, Externo = 0, Combinado = 0  FROM PVO_Escenarios.dbo.Esc_Rechazo A WHERE id_articulo = {0} AND id_escenario = {1} AND [Año] < {2}";
        
        //private static string SQL_VELOCIDAD_FECHA_BASE = "UPDATE A SET Valor_base = 0 FROM PVO_Escenarios.dbo.Esc_Velocidad A WHERE id_articulo = {0} AND id_escenario = {1}";
        //private static string SQL_VELOCIDAD_FECHA_VALOR_OLD = "UPDATE A SET Valor = 0 FROM PVO_Escenarios.dbo.Esc_Velocidad A WHERE id_articulo = {0} AND id_escenario = {1} AND [Año] >= {2}";
        //private static string SQL_VELOCIDAD_FECHA_VALOR_NEW = "UPDATE A SET Valor = 0 FROM PVO_Escenarios.dbo.Esc_Velocidad A WHERE id_articulo = {0} AND id_escenario = {1} AND [Año] < {2}";


        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<Esc_Articulos> GetEscArticulosList()
        {
            List<Esc_Articulos> escArticulos = db.Esc_Articulos.ToList();
            return escArticulos;
        }

        public static List<Esc_Articulos> GetEscArticulosList(int id_escenario)
        {
            List<Esc_Articulos> escArticulos = db.Esc_Articulos.Where(t => t.id_escenario == id_escenario).OrderBy(t => t.id_articulo).ToList();
            return escArticulos;
        }

        public static List<Esc_Articulos> GetEscArticulosList(int id_escenario, double id_articulo)
        {
            List<Esc_Articulos> escArticulos = db.Esc_Articulos.Where(t => t.id_escenario == id_escenario && t.id_articulo == id_articulo).OrderBy(t => t.id_articulo).ToList();
            return escArticulos;
        }

        public static List<Esc_Tipo_Escenarios> GetEscTipoEscenarios()
        {
            List<Esc_Tipo_Escenarios> escTipoEscenariosList = db.Esc_Tipo_Escenarios.ToList();
            return escTipoEscenariosList;
        }

        public static List<CAT_Plantas> GetCatPlantasList()
        {
            List<CAT_Plantas> escTipoEscenariosList = db.CAT_Plantas.ToList();
            return escTipoEscenariosList;
        }

        public static List<Esc_Lineas> GetEscLineasList(string Planta)
        {
            List<Esc_Lineas> escLineasList = db.Esc_Lineas.Where(t => t.Planta.Equals(Planta)).ToList();
            return escLineasList;
        }

        public static List<Esc_Lineas> GetEscLineasList()
        {
            List<Esc_Lineas> escLineasList = db.Esc_Lineas.ToList();
            return escLineasList;
        }

        public static List<Esc_Maquina> GetEscMaquinaList()
        {
            List<Esc_Maquina> escMaquinaList = db.Esc_Maquina.ToList();
            return escMaquinaList;
        }

        public static List<Esc_File_Upload> GetFileUploads()
        {
            List<Esc_File_Upload> escFileUploads = db.Esc_File_Upload.Where(i => i.eliminado == false).ToList();
            return escFileUploads;
        }

        public static Esc_Articulos FindRecord(ArticuloKey kyArticuloKey)
        {
            Esc_Articulos escArticulo = db.Esc_Articulos.FirstOrDefault(i => i.id_escenario == kyArticuloKey.id_escenario && i.id_articulo == kyArticuloKey.id_articulo);
            return escArticulo;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_Articulos escArticulo)
        {
            //TODO: Agregar Busqueda de registro de copia y Crear cambiando el id del producto.
            db.Esc_Articulos.Add(escArticulo);
            db.SaveChanges();
        }

        public static void AddNewRecordMov(CArticuloMov articuloMov)
        {
        }

        public static void UpdateRecord(Esc_Articulos escArticulo)
        {
            Esc_Articulos escArticulo01 =
                db.Esc_Articulos.FirstOrDefault(t =>
                    t.id_articulo == escArticulo.id_articulo && t.id_escenario == escArticulo.id_escenario);

            Boolean bandera = false;
            if (escArticulo01 != null)
            {
                bool result = escArticulo01.Linea == null ? true : !escArticulo01.Linea.Equals(escArticulo.Linea);
                if (result)
                {
                    escArticulo01.Linea = escArticulo.Linea;
                    bandera = true;
                }

                result = escArticulo01.Planta01 == null ? true : !escArticulo01.Planta01.Equals(escArticulo.Linea);

                if (result)
                {
                    escArticulo01.Planta01 = escArticulo.Planta01;
                    bandera = true;
                }

                result = escArticulo01.id_maquina == null ? true : !escArticulo01.id_maquina.Equals(escArticulo.id_maquina);

                if (result)
                {
                    escArticulo01.id_maquina = escArticulo.id_maquina;
                    bandera = true;
                }


                result = escArticulo01.Probabilidad == null ? true : !escArticulo01.Probabilidad.Equals(escArticulo.Probabilidad);

                if (result)
                {
                    escArticulo01.Probabilidad = escArticulo.Probabilidad;
                    bandera = true;
                }

                if (bandera)
                {
                    db.Esc_Articulos.AddOrUpdate(escArticulo01);
                    db.SaveChanges();
                }
            }
        }

        private static double FindMaxId(Esc_Articulos escArticulo)
        {
            Esc_Articulos tempo = db.Esc_Articulos.OrderByDescending(u => u.id_articulo).Where(t => t.id_escenario == escArticulo.id_escenario).FirstOrDefault();

            return tempo.id_articulo;
        }

        private static double FindMaxId02(CArticuloMov escArticulo)
        {
            Esc_Articulos tempo = db.Esc_Articulos.OrderByDescending(u => u.id_articulo).Where(t => t.id_escenario == escArticulo.id_escenario).FirstOrDefault();

            return tempo.id_articulo;
        }

        private static bool modifica(Esc_Articulos escArticulo, double id_articulo)
        {
            Esc_Articulos escArticulo01 =
                db.Esc_Articulos.FirstOrDefault(t =>
                    t.id_articulo == id_articulo && t.id_escenario == escArticulo.id_escenario);

            Esc_Articulos escArticulo02 = new Esc_Articulos();

            if (escArticulo01 != null)
            {
                escArticulo01.Probabilidad = escArticulo.Probabilidad;
                escArticulo01.Cliente = escArticulo.Cliente;
                escArticulo01.Customer_Name = escArticulo.Customer_Name;
                escArticulo01.Industria = escArticulo.Industria;
                escArticulo01.Tipo_Pieza = escArticulo.Tipo_Pieza;
                escArticulo01.Planta01 = escArticulo.Planta01;
                escArticulo01.Linea = escArticulo.Linea;

                //db.Esc_Articulos.Add(escArticulo02);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        private static bool modifica02(CArticuloMov escArticulo, double id_articulo)
        {
            Esc_Articulos escArticulo01 =
                db.Esc_Articulos.FirstOrDefault(t =>
                    t.id_articulo == id_articulo && t.id_escenario == escArticulo.id_escenario);

            Esc_Articulos escArticulo02 = new Esc_Articulos();

            if (escArticulo01 != null)
            {
                escArticulo01.Probabilidad = escArticulo.Probabilidad;
                escArticulo01.Cliente = escArticulo.Cliente;
                escArticulo01.Customer_Name = escArticulo.Customer_Name;
                escArticulo01.Industria = escArticulo.Industria;
                escArticulo01.Tipo_Pieza = escArticulo.Tipo_Pieza;
                escArticulo01.Planta01 = escArticulo.Planta01;
                escArticulo01.Linea = escArticulo.Linea;

                //db.Esc_Articulos.Add(escArticulo02);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static void CopyRecord(Esc_Articulos escArticulo)
        {
            double intIdArticulo_copy = FindMaxId(escArticulo) + 1;
            double intIdArticulo_orig = escArticulo.id_articulo;
            double intIdEscenario_orig = escArticulo.id_escenario;

            String strQuery = String.Format(SQL_ARTICULO_COPIA_ARTICULO, intIdArticulo_copy, intIdArticulo_orig,
                escArticulo.id_escenario);

            db.Database.ExecuteSqlCommand(strQuery);
            if (modifica(escArticulo, intIdArticulo_copy))
            {
                strQuery = String.Format(SQL_ARTICULOS_COPIA_DEMANDA, intIdArticulo_copy,
                    intIdArticulo_orig, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);

                strQuery = String.Format(SQL_ARTICULOS_COPIA_CAVIDADES, intIdArticulo_copy,
                    intIdArticulo_orig, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);

                strQuery = String.Format(SQL_ARTICULOS_COPIA_RECHAZO, intIdArticulo_copy,
                    intIdArticulo_orig, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);

                strQuery = String.Format(SQL_ARTICULOS_COPIA_VELOCIDAD, intIdArticulo_copy,
                    intIdArticulo_orig, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);
            }

        }

        public static void MoveRecord(CArticuloMov escArticulo)
        {
            double intIdArticulo_copy = FindMaxId02(escArticulo) + 1;
            double intIdArticulo_orig = escArticulo.id_articulo;
            double intIdEscenario_orig = escArticulo.id_escenario;
            string strDesde = "20210100";
            string strAnioDesde = "2021";

            DateTime? dDesde = escArticulo.fecha_start;
            if (dDesde != null)
            {
                strDesde = dDesde != null ? dDesde.Value.ToString("yyyyMM") + "00" : "20210100";
                strAnioDesde = dDesde != null ? dDesde.Value.ToString("yyyy") : "20210100";
            }


            String strQuery = String.Format(SQL_ARTICULO_COPIA_ARTICULO, intIdArticulo_copy, intIdArticulo_orig,
                escArticulo.id_escenario);

            db.Database.ExecuteSqlCommand(strQuery);
            if (modifica02(escArticulo, intIdArticulo_copy))
            {
                strQuery = String.Format(SQL_ARTICULOS_COPIA_DEMANDA, intIdArticulo_copy, intIdArticulo_orig, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);

                strQuery = String.Format(SQL_ARTICULOS_COPIA_CAVIDADES, intIdArticulo_copy, intIdArticulo_orig, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);

                strQuery = String.Format(SQL_ARTICULOS_COPIA_RECHAZO, intIdArticulo_copy, intIdArticulo_orig, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);

                strQuery = String.Format(SQL_ARTICULOS_COPIA_VELOCIDAD, intIdArticulo_copy, intIdArticulo_orig, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);


                strQuery = String.Format(SQL_DEMANDA_FECHA_BASE, intIdArticulo_copy, intIdEscenario_orig);
                db.Database.ExecuteSqlCommand(strQuery);

                strQuery = String.Format(SQL_DEMANDA_FECHA_VALOR_OLD, intIdArticulo_orig, intIdEscenario_orig, strDesde);
                db.Database.ExecuteSqlCommand(strQuery);

                strQuery = String.Format(SQL_DEMANDA_FECHA_VALOR_NEW, intIdArticulo_copy, intIdEscenario_orig, strDesde);
                db.Database.ExecuteSqlCommand(strQuery);

                //strQuery = String.Format(SQL_RECHAZO_FECHA_BASE, intIdArticulo_copy, intIdEscenario_orig);
                //db.Database.ExecuteSqlCommand(strQuery);

                //strQuery = String.Format(SQL_RECHAZO_FECHA_VALOR_OLD, intIdArticulo_orig, intIdEscenario_orig, strAnioDesde);
                //db.Database.ExecuteSqlCommand(strQuery);

                //strQuery = String.Format(SQL_RECHAZO_FECHA_VALOR_NEW, intIdArticulo_copy, intIdEscenario_orig, strAnioDesde);
                //db.Database.ExecuteSqlCommand(strQuery);

                //strQuery = String.Format(SQL_VELOCIDAD_FECHA_BASE, intIdArticulo_copy, intIdEscenario_orig);
                //db.Database.ExecuteSqlCommand(strQuery);

                //strQuery = String.Format(SQL_VELOCIDAD_FECHA_VALOR_OLD, intIdArticulo_orig, intIdEscenario_orig, strAnioDesde);
                //db.Database.ExecuteSqlCommand(strQuery);

                //strQuery = String.Format(SQL_VELOCIDAD_FECHA_VALOR_NEW, intIdArticulo_copy, intIdEscenario_orig, strAnioDesde);
                //db.Database.ExecuteSqlCommand(strQuery);
            }
        }

        public static void UpdateRecords(string selectedRowIds, Esc_Articulos escArticulo)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                Esc_Articulos escArticulo01 = db.Esc_Articulos.FirstOrDefault(t => t.id_articulo == selectedId && t.id_escenario == escArticulo.id_escenario);

                bool bandera = false;
                bool result = false;
                if (escArticulo01 != null)
                {
                    if (escArticulo01.Linea == null || !escArticulo01.Linea.Equals(escArticulo.Linea))
                    {
                        escArticulo01.Linea = escArticulo.Linea;
                        bandera = true;
                    }

                    if (!escArticulo01.Planta01.Equals(escArticulo.Planta01))
                    {
                        escArticulo01.Planta01 = escArticulo.Planta01;
                        bandera = true;
                    }

                    result = escArticulo01.id_maquina == null || !escArticulo01.id_maquina.Equals(escArticulo.id_maquina);

                    if (result)
                    {
                        escArticulo01.id_maquina = escArticulo.id_maquina;
                        bandera = true;
                    }

                    result = !escArticulo01.Probabilidad?.Equals(escArticulo.Probabilidad) ?? true;

                    if (result)
                    {
                        escArticulo01.Probabilidad = escArticulo.Probabilidad;
                        bandera = true;
                    }


                    if (bandera)
                    {
                        escArticulo01.modificado = true;
                        escArticulo01.UserId_modifico = escArticulo.UserId_modifico;
                        escArticulo01.fecha_modificacion = DateTime.Now;
                        db.Esc_Articulos.AddOrUpdate(escArticulo01);
                        db.SaveChanges();
                    }
                }
            }
        }

        public static void DeleteRecords(int id)
        {
            Esc_Articulos escArticulo = db.Esc_Articulos.FirstOrDefault(t => t.id_escenario == id);

            if (!escArticulo.Equals(null))
            {
                db.Esc_Articulos.Remove(escArticulo);
                db.SaveChanges();
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                Esc_Articulos escArticulo = null;
                escArticulo = db.Esc_Articulos.FirstOrDefault(t => t.id_escenario == selectedId);

                db.Esc_Articulos.Remove(escArticulo);
                db.SaveChanges();
            }
        }
    }
}