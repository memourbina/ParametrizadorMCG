using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace mcg_load.Code
{
    public class EscenariosHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        private static string SQL_ESCENARIO_ARTICULOS = "INSERT INTO PVO_Escenarios.dbo.Esc_Articulos " +
                                                        "([id_articulo],[id_escenario],[Org# ID],[Escenario],[Probabilidad],[Probabilidad_base],[Línea Moldeo],[Cliente],[Customer Name],[Industria]" +
                                                        ",[Tipo Pieza],[Description],[No# Oracle],[Customer PN],[Unit Weight],[Mercado],[Tipo de Hierro],[Grado de Hierro],[OEM],[Plataforma]" +
                                                        ",[Sistema],[Cavs /  Soplo],[Soplos / Hora],[Cor / pza],[Maquina],[Planta],[Planta01],[Linea],[modificado],[UserId_modifico],[fecha_modificacion])" +
                                                        "(SELECT A.[id_articulo], {0}, A.[Org# ID], A.[Escenario], A.[Probabilidad], A.[Probabilidad_base], A.[Línea Moldeo], A.[Cliente], A.[Customer Name], A.[Industria]" +
                                                        ", A.[Tipo Pieza], A.[Description], A.[No# Oracle], A.[Customer PN], A.[Unit Weight], A.[Mercado], A.[Tipo de Hierro], A.[Grado de Hierro], A.[OEM], A.[Plataforma]" +
                                                        ", A.[Sistema], A.[Cavs /  Soplo], A.[Soplos / Hora], A.[Cor / pza], A.[Maquina], A.[Planta], A.[Planta01], A.[Linea], A.[modificado], A.[UserId_modifico], A.[fecha_modificacion] " +
                                                        "FROM PVO_Escenarios.[dbo].[Esc_Articulos] A " +
                                                        "INNER JOIN PVO_Escenarios.[dbo].[Esc_Escenario] E " +
                                                        "ON  A.[id_escenario] = E.[id_escenario] " +
                                                        "AND E.[id_tipo_escenario] = 1 " +
                                                        "WHERE E.[id_UpLoad] = {1} )";

        private static string SQL_ESCENARIO_CAVIDADES =
            @"INSERT INTO PVO_Escenarios.dbo.Esc_Cavidades ([id_articulo], [Año], [id_escenario], [UserId], [Cantidad], [Cantidad_base], [eliminado], [modificado], [UserId_modifico], [fecha_modificacion]) " +
            "(SELECT C.[id_articulo], C.[Año], {0}, C.[UserId], C.[Cantidad], C.[Cantidad_base], C.[eliminado], C.[modificado], C.[UserId_modifico], C.[fecha_modificacion] " +
            "FROM PVO_Escenarios.[dbo].[Esc_Cavidades] C " +
            "INNER JOIN PVO_Escenarios.[dbo].[Esc_Escenario] E " +
            "ON  C.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1 " +
            "WHERE E.[id_UpLoad] = {1});";

        private static string SQL_ESCENARIO_DEMANDA =
            @"INSERT INTO PVO_Escenarios.[dbo].[Esc_Demanda] ([id_articulo], [Fecha], [id_escenario], [UserId], [Valor], [valor_base], [eliminado], [modificacion]) " +
            "(SELECT D.[id_articulo], D.[Fecha], {0}, D.[UserId], D.[Valor], D.[valor_base], D.[eliminado], D.[modificacion] " +
            "FROM PVO_Escenarios.[dbo].[Esc_Demanda] D " +
            "INNER JOIN PVO_Escenarios.[dbo].[Esc_Escenario] E " +
            "ON  D.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1 " +
            "WHERE E.[id_UpLoad] = {1} )";

        private static string SQL_ESCENARIO_HORAS =
            "INSERT INTO PVO_Escenarios.[dbo].[Esc_Horas] ([Planta],[Linea],[Fecha],[id_escenario],[UserId],[Horas],[Horas_base],[modificado],[UserId_modifico]" +
            ",[fecha_modificacion])" +
            "(SELECT H.[Planta], H.[Linea], H.[Fecha], {0}, H.[UserId], H.[Horas], H.[Horas_base], H.[modificado], H.[UserId_modifico], H.[fecha_modificacion] " +
            "FROM PVO_Escenarios.dbo.Esc_Horas H " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Escenario E " +
            "ON  H.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1  " +
            "WHERE E.[id_UpLoad] = {1});";

        private static string SQL_ESCENARIO_HORASCORAZON =
            "INSERT INTO [dbo].[Esc_HorasCorazones] ([Maquina], [Fecha], [id_escenario], [UserId], [Horas], [Horas_base], [modificado], [UserId_modifico], [fecha_modificacion]) " +
            "(SELECT H.[Maquina], H.[Fecha], {0}, H.[UserId], H.[Horas], H.[Horas_base], H.[modificado], H.[UserId_modifico], H.[fecha_modificacion] " +
            "FROM PVO_Escenarios.dbo.Esc_HorasCorazones H " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Escenario E " +
            "ON  H.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1 " +
            "WHERE E.[id_UpLoad] = {1});";

        private static string SQL_ESCENARIO_OEE =
            "INSERT INTO PVO_Escenarios.[dbo].[Esc_OEE] ([Planta], [Linea], [Fecha], [id_escenario], [UserId], [OEE_SinRechazo], [OEE_SinRechazo_base], [modificado], [UserId_modifico], [fecha_modificacion]) " +
            "(SELECT H.[Planta], H.[Linea], H.[Fecha], {0}, H.[UserId], H.[OEE_SinRechazo], H.[OEE_SinRechazo_base], H.[modificado], H.[UserId_modifico],H.[fecha_modificacion] " +
            "FROM PVO_Escenarios.dbo.Esc_OEE H " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Escenario E " +
            "ON  H.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1  " +
            "WHERE E.[id_UpLoad] = {1});";

        private static string SQL_ESCENARIO_OEECORAZON =
            "INSERT INTO PVO_Escenarios.dbo.Esc_OEECorazones ([Año], [id_escenario], [UserId], [Rendimiento], [Rendimiento_base], [Disponibilidad], [Disponibilidad_base], [Rechazo], " +
            "[Rechazo_base], [OEE sin rechazo], [OEE sin rechazo_base], [OEE], [OEE_base], [modificado], [UserId_modifico], [fecha_modificacion]) " +
            "(SELECT H.[Año], {0}, H.[UserId], H.[Rendimiento], H.[Rendimiento_base], H.[Disponibilidad], H.[Disponibilidad_base], H.[Rechazo], " +
            "H.[Rechazo_base], H.[OEE sin rechazo], H.[OEE sin rechazo_base], H.[OEE], H.[OEE_base], H.[modificado], H.[UserId_modifico], H.[fecha_modificacion] " +
            "FROM PVO_Escenarios.dbo.Esc_OEECorazones H " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Escenario E " +
            "ON  H.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1  " +
            "WHERE E.[id_UpLoad] = {1});";

        private static string SQL_ESCENARIO_OEEDESGLOSE =
            "INSERT INTO PVO_Escenarios.[dbo].[Esc_OEEDesglose] ([Planta], [Linea], [Año], [id_escenario], [UserId], [Disponibilidad], [Disponibilidad_base], [Rechazo], [Rechazo_base], " +
            "[Rendimiento], [Rendimiento_base], [OEE sin rechazo], [OEE sin rechazo_base], [OEE], [OEE_base], [modificado], [UserId_modifico], [fecha_modificacion]) " +
            "(SELECT H.[Planta], H.[Linea], H.[Año], {0}, H.[UserId], H.[Disponibilidad], H.[Disponibilidad_base], H.[Rechazo], H.[Rechazo_base], " +
            "H.[Rendimiento], H.[Rendimiento_base], H.[OEE sin rechazo], H.[OEE sin rechazo_base], H.[OEE], H.[OEE_base], H.[modificado], H.[UserId_modifico], H.[fecha_modificacion] " +
            "FROM PVO_Escenarios.dbo.Esc_OEEDesglose H " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Escenario E " +
            "ON  H.[id_escenario] = E.[id_escenario] AND E.[id_tipo_escenario] = 1 " +
            "WHERE E.[id_UpLoad] = {1});";

        private static string SQL_ESCENARIO_PARAMETROS =
            "INSERT INTO PVO_Escenarios.[dbo].[Esc_Parametros] ([id_articulo], [Año], [Parametro], [id_escenario], [UserId], [Valor], [Valor_base], [modificado], [UserId_modifico], [fecha_modificacion]) " +
            "(SELECT H.[id_articulo], H.[Año], H.[Parametro], {0}, H.[UserId], H.[Valor], H.[Valor_base], H.[modificado], H.[UserId_modifico], H.[fecha_modificacion] " +
            "FROM PVO_Escenarios.dbo.Esc_Parametros H " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Escenario E " +
            "ON  H.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1  " +
            "WHERE E.[id_UpLoad] = {1});";

        private static string SQL_ESCENARIO_RECHAZOS =
            "INSERT INTO PVO_Escenarios.[dbo].[Esc_Rechazo] ([id_articulo], [Año], [id_escenario], [UserId], [Interno], [Externo], [Combinado], [Interno_base], " +
            "[Externo_base], [Combinado_base], [eliminado], [modificado] ) " +
            "(SELECT R.[id_articulo], R.[Año], {0}, R.[UserId], R.[Interno], R.[Externo], R.[Combinado], R.[Interno_base], " +
            "R.[Externo_base], R.[Combinado_base], R.[eliminado], R.[modificado] " +
            "FROM PVO_Escenarios.dbo.Esc_Rechazo R " +
            "INNER JOIN PVO_Escenarios.dbo.Esc_Escenario E " +
            "ON  R.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1 " +
            "WHERE E.[id_UpLoad] = {1});";

        private static string SQL_ESCENARIO_VELOCIDAD =
            "INSERT INTO PVO_Escenarios.[dbo].[Esc_Velocidad] ([id_articulo], [Año], [id_escenario], [UserId], [Valor], [Valor_base], [eliminado], [modificado]) " +
            "(SELECT V.[id_articulo], V.[Año], {0}, V.[UserId], V.[Valor], V.[Valor_base], V.[eliminado], V.[modificado] " +
            "FROM PVO_Escenarios.[dbo].[Esc_Velocidad] V " +
            "INNER JOIN PVO_Escenarios.[dbo].[Esc_Escenario] E " +
            "ON V.[id_escenario] = E.[id_escenario] " +
            "AND E.[id_tipo_escenario] = 1 " +
            "WHERE E.[id_UpLoad] = {1});";

        public static List<Esc_Escenario> GetEscEscenarios()
        {
            List<Esc_Escenario> escEscenarios = db.Esc_Escenario.ToList();
            return escEscenarios;
        }

        public static List<Esc_Escenario> GetEscEscenarios(int id_tipo_escenario)
        {
            List<Esc_Escenario> escEscenarios;

            escEscenarios = db.Esc_Escenario.Where(t => t.id_tipo_escenario == id_tipo_escenario).ToList();

            return escEscenarios;
        }

        public static List<Esc_Tipo_Escenarios> GetEscTipoEscenarios()
        {
            List<Esc_Tipo_Escenarios> escTipoEscenariosList = db.Esc_Tipo_Escenarios.ToList();
            return escTipoEscenariosList;
        }

        public static Esc_Tipo_Escenarios GetEscTipoEscenarios(string strTipoEscenario)
        {
            Esc_Tipo_Escenarios escTipoEscenariosList =
                db.Esc_Tipo_Escenarios.FirstOrDefault(t => t.nombre.Equals(strTipoEscenario));
            return escTipoEscenariosList;
        }

        public static List<Esc_File_Upload> GetFileUploads()
        {
            List<Esc_File_Upload> escFileUploads = db.Esc_File_Upload.Where(i => i.eliminado == false).ToList();
            return escFileUploads;
        }

        public static Esc_Escenario FindRecord(long id)
        {
            Esc_Escenario escEscenario = db.Esc_Escenario.FirstOrDefault(i => i.id_escenario == id);
            return escEscenario;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_Escenario escEscenario)
        {
            string strQuery = null;
            escEscenario.modificado = false;
            db.Esc_Escenario.Add(escEscenario);
            db.SaveChanges();

            db.Database.CommandTimeout = 180;

            strQuery = String.Format(SQL_ESCENARIO_ARTICULOS, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_CAVIDADES, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_DEMANDA, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_HORAS, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_HORASCORAZON, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_OEE, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_OEECORAZON, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_OEEDESGLOSE, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_PARAMETROS, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_RECHAZOS, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = String.Format(SQL_ESCENARIO_VELOCIDAD, escEscenario.id_escenario, escEscenario.id_UpLoad);
            db.Database.ExecuteSqlCommand(strQuery);
        }

        public static void UpdateRecord(Esc_Escenario escEscenario)
        {
            Esc_Escenario escEscenario01 =
                db.Esc_Escenario.FirstOrDefault(t => t.id_escenario == escEscenario.id_escenario);

            Boolean bandera = false;
            if (escEscenario01 != null)
            {
                //if (escEscenario01.eliminado != escEscenario.eliminado)
                //{
                //    escEscenario01.eliminado = escEscenario.eliminado;
                //    bandera = true;
                //}

                if (escEscenario01.nombre != escEscenario.nombre)
                {
                    escEscenario01.nombre = escEscenario.nombre;
                    bandera = true;
                }

                if (escEscenario01.activo != escEscenario.activo)
                {
                    escEscenario01.activo = escEscenario.activo;
                    bandera = true;
                }

                //if (escEscenario01.id_tipo_escenario != escEscenario.id_tipo_escenario)
                //{
                //    escEscenario01.id_tipo_escenario = escEscenario.id_tipo_escenario;
                //    bandera = true;
                //}

                //if (escEscenario01.id_UpLoad != escEscenario.id_UpLoad)
                //{
                //    escEscenario01.id_UpLoad = escEscenario.id_UpLoad;
                //    bandera = true;
                //}

                if (escEscenario01.comentario != escEscenario.comentario)
                {
                    escEscenario01.comentario = escEscenario.comentario;
                    bandera = true;
                }

                if (bandera)
                {
                    db.Esc_Escenario.AddOrUpdate(escEscenario01);
                    db.SaveChanges();
                }
            }
        }

        public static void DeleteRecords(int id)
        {
            Esc_Escenario escEscenario = db.Esc_Escenario.FirstOrDefault(t => t.id_escenario == id);

            if (!escEscenario.Equals(null))
            {
                db.Esc_Escenario.Remove(escEscenario);
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                Esc_Escenario escEscenario = null;
                escEscenario = db.Esc_Escenario.FirstOrDefault(t => t.id_escenario == selectedId);
                if (escEscenario != null && escEscenario.Esc_Articulos.Count == 0)
                {
                    db.Esc_Escenario.Remove(escEscenario);
                    db.SaveChanges();
                }
            }
        }
    }
}