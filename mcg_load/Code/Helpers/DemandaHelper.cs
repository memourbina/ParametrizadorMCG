using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraRichEdit.API.Native.Implementation;

namespace mcg_load.Code.Helpers
{
    public class DemandaHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();

        public static List<vw_demanda01> GetEscDemanda01List()
        {
            List<vw_demanda01> escDemanda01 = db.vw_demanda01.ToList();
            return escDemanda01;
        }

        public static List<vw_demanda01> GetEscDemanda01List(int id_articulo, int id_escenario)
        {
            List<vw_demanda01> escDemanda01 = db.vw_demanda01
                .Where(t => t.id_articulo == id_articulo && t.id_escenario == id_escenario).ToList();
                      
            return escDemanda01;
        }

        public static List<Esc_Tipo_Escenarios> GetEscTipoEscenarios()
        {
            List<Esc_Tipo_Escenarios> escTipoEscenariosList = db.Esc_Tipo_Escenarios.ToList();
            return escTipoEscenariosList;
        }

        public static List<Esc_Año> GetEscAños()
        {
            List<Esc_Año> escAños = db.Esc_Año.ToList();
            return escAños;
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

        public static List<Esc_File_Upload> GetFileUploads()
        {
            List<Esc_File_Upload> escFileUploads = db.Esc_File_Upload.Where(i => i.eliminado == false).ToList();
            return escFileUploads;
        }

        public static vw_demanda01 FindRecord(long id)
        {
            vw_demanda01 escDemanda01 = db.vw_demanda01.FirstOrDefault(i => i.id_escenario == id);
            return escDemanda01;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(vw_demanda01 escDemanda01)
        {
            db.vw_demanda01.Add(escDemanda01);
            db.SaveChanges();
        }

        public static void UpdateRecord(vw_demanda01 escDemanda01)
        {
            vw_demanda01 escDemanda0101 =
                db.vw_demanda01.FirstOrDefault(t =>
                    t.anio == escDemanda01.anio && t.id_articulo == escDemanda01.id_articulo &&
                    t.id_escenario == escDemanda01.id_escenario &&
                    t.anio == escDemanda01.anio);

            if (escDemanda0101 == null) return;

            double? valor = 0.0;
            double? valor_base = 0.0;

            for (int i = 0; i < 12; i++)
            {
                string strFecha = String.Format("{0:0000}{1:00}{2:00}", escDemanda01.anio, i + 1, 1);
                int intFecha = Convert.ToInt32(strFecha);
                bool result = false;

                switch (i + 1)
                {
                    case 1: //Enero
                        result = escDemanda0101.Enero != null && escDemanda0101.Enero.Equals(escDemanda01.Enero);
                        valor = escDemanda01.Enero;
                        valor_base= escDemanda01.Enero_Base;
                        break;
                    case 2: //Febrero
                        result = escDemanda0101.Febrero != null && escDemanda0101.Febrero.Equals(escDemanda01.Febrero);
                        valor = escDemanda01.Febrero;
                        valor_base = escDemanda01.Febrero_Base;
                        break;
                    case 3: //Marzo
                        result = escDemanda0101.Marzo != null && escDemanda0101.Marzo.Equals(escDemanda01.Marzo);
                        valor = escDemanda01.Marzo;
                        valor_base = escDemanda01.Marzo_Base;
                        break;
                    case 4: //Abril
                        result = escDemanda0101.Abril != null && escDemanda0101.Abril.Equals(escDemanda01.Abril);
                        valor = escDemanda01.Abril;
                        valor_base = escDemanda01.Abril_Base;
                        break;
                    case 5: //Mayo
                        result = escDemanda0101.Mayo != null && escDemanda0101.Mayo.Equals(escDemanda01.Mayo);
                        valor = escDemanda01.Mayo;
                        valor_base = escDemanda01.Mayo_Base;
                        break;
                    case 6: //Junio
                        result = escDemanda0101.Junio != null && escDemanda0101.Junio.Equals(escDemanda01.Junio);
                        valor = escDemanda01.Junio;
                        valor_base = escDemanda01.Junio_Base;
                        break;
                    case 7: //Julio
                        result = escDemanda0101.Julio != null && escDemanda0101.Julio.Equals(escDemanda01.Julio);
                        valor = escDemanda01.Julio;
                        valor_base = escDemanda01.Julio_Base;
                        break;
                    case 8: //Agosto
                        result = escDemanda0101.Agosto != null && escDemanda0101.Agosto.Equals(escDemanda01.Agosto);
                        valor = escDemanda01.Agosto;
                        valor_base = escDemanda01.Agosto_Base;
                        break;
                    case 9: //Septiembre
                        result = escDemanda0101.Septiembre != null && escDemanda0101.Septiembre.Equals(escDemanda01.Septiembre);
                        valor = escDemanda01.Septiembre;
                        valor_base = escDemanda01.Septiembre_Base;
                        break;
                    case 10: //Octubre
                        result = escDemanda0101.Octubre != null && escDemanda0101.Octubre.Equals(escDemanda01.Octubre);
                        valor = escDemanda01.Octubre;
                        valor_base = escDemanda01.Octubre_Base;
                        break;
                    case 11: //Noviembre
                        result = escDemanda0101.Noviembre != null && escDemanda0101.Noviembre.Equals(escDemanda01.Noviembre);
                        valor = escDemanda01.Noviembre;
                        valor_base = escDemanda01.Noviembre_Base;
                        break;
                    case 12: //Diciembre
                        result = escDemanda0101.Diciembre != null && escDemanda0101.Diciembre.Equals(escDemanda01.Diciembre);
                        valor = escDemanda01.Diciembre;
                        valor_base = escDemanda01.Diciembre_Base;
                        break;
                }

                if (!result)
                {
                    Esc_Demanda escDemanda =
                        db.Esc_Demanda.Find(escDemanda01.id_articulo, intFecha, escDemanda01.id_escenario);
                    escDemanda.Valor = valor;
                    db.Esc_Demanda.AddOrUpdate(escDemanda);
                    db.SaveChanges();
                }
            }

            RefreshAll();
        }

        public static void RefreshAll()
        {
            foreach (var entity in db.ChangeTracker.Entries())
            {
                entity.Reload();
            }
        }

        //public static void ReloadEntity<TEntity>(this DbContext context, TEntity entity) where TEntity : class {
        //    context.Entry(entity).Reload();
        //}

        public static void UpdateRecords(string selectedRowIds, vw_demanda01 escDemanda01)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                vw_demanda01 escDemanda0101 = db.vw_demanda01.FirstOrDefault(t => t.anio == escDemanda01.anio &&
                    t.id_articulo == selectedId && t.id_escenario == escDemanda01.id_escenario);

                Boolean bandera = false;    
                if (escDemanda0101 != null)
                {
                    if (!escDemanda0101.Enero.Equals(escDemanda01.Enero))
                    {
                        escDemanda0101.Enero = escDemanda01.Enero;
                        bandera = true;
                    }


                    if (bandera)
                    {
                        db.vw_demanda01.AddOrUpdate(escDemanda0101);
                        db.SaveChanges();
                    }
                }
            }
        }

        public static void DeleteRecords(int id)
        {
            vw_demanda01 escDemanda01 = db.vw_demanda01.FirstOrDefault(t => t.id_escenario == id);

            if (!escDemanda01.Equals(null))
            {
                db.vw_demanda01.Remove(escDemanda01);
                db.SaveChanges();
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            foreach (var selectedId in selectedIds)
            {
                vw_demanda01 escDemanda01 = null;
                escDemanda01 = db.vw_demanda01.FirstOrDefault(t => t.id_escenario == selectedId);

                db.vw_demanda01.Remove(escDemanda01);
                db.SaveChanges();
            }
        }
    }
}