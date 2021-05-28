using DevExpress.Web.Mvc;
using mcg_load.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mcg_load.Code.Helpers
{
    public class LineasHelper
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();
        private static List<TipoMovimiento> TipoMoivientoList = new List<TipoMovimiento>
        {new TipoMovimiento { Nombre = "Todo",TipoMovimientos ="1"},
        new TipoMovimiento { Nombre = "Porcentaje",TipoMovimientos ="2"},
        new TipoMovimiento { Nombre = "Monto",TipoMovimientos ="3"}};

        public static List<Esc_Lineas> GetEscLineas()
        {
            List<Esc_Lineas> escLineas = db.Esc_Lineas.ToList();  
            return escLineas;
        }

        public static List<Esc_Lineas> GetEscLineas(string Planta)
        {
            List<Esc_Lineas> escLineas = db.Esc_Lineas.Where(t=> t.Planta.Equals(Planta)) .ToList();
            return escLineas;
        }

        public static List<CAT_Plantas> GetCatPlantasList()
        {
            List<CAT_Plantas> escTipoEscenariosList = db.CAT_Plantas.ToList();
            return escTipoEscenariosList;
        }

        public static List<Esc_Tipo_linea> GetEscTipoLineaList()
        {
            List<Esc_Tipo_linea> escTipoLineasList = db.Esc_Tipo_linea.ToList();
            return escTipoLineasList;
        }

        public static List<Esc_Tipo_Escenarios> GetEscTipoEscenarios()
        {
            List<Esc_Tipo_Escenarios> escTipoEscenariosList = db.Esc_Tipo_Escenarios.ToList();
            return escTipoEscenariosList;
        }

        public static List<TipoMovimiento> GetEscTipoMovimiento()
        {            
            return TipoMoivientoList;
        }

        public static Esc_Lineas FindRecord(string strlinea)
        {
            Esc_Lineas escLineas = db.Esc_Lineas.FirstOrDefault(i => i.Linea == strlinea);
            return escLineas;
        }

        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }

        public static void AddNewRecord(Esc_Lineas escLineas)
        {
            db.Esc_Lineas.Add(escLineas);
            db.SaveChanges();
        }

        public static void UpdateRecord(Esc_Lineas escLineas)
        {
            Esc_Lineas escLineas01 = db.Esc_Lineas.FirstOrDefault(t => t.Linea == escLineas.Linea);

            Boolean bandera = false;
            if (escLineas01 != null)
            {


                if (bandera)
                {
                    //escLineas01. .UserId_modifico = escLineas.UserId_modifico;
                    //escLineas01.modificado = true;
                    //escLineas01.fecha_modificacion = DateTime.Now;
                    //db.Esc_Lineas. .up AddOrUpdate(escLineas01);
                    db.SaveChanges();
                }
            }
        }

        public static void CopyRecord(Esc_Lineas escLineas)
        {
            Esc_Lineas escLineas01 = db.Esc_Lineas.FirstOrDefault(t => t.Linea == escLineas.Linea);
            Esc_Lineas escLineas02 = new Esc_Lineas();

            //Boolean bandera = false;
            if (escLineas01 != null)
            {
                escLineas02.Linea = escLineas.nombre;
                escLineas02.Planta = escLineas.Planta;
                escLineas02.id_tipo_linea = 2;
                escLineas02.nombre = escLineas.nombre;
                escLineas02.descripcion = escLineas.descripcion;
                db.Esc_Lineas.Add(escLineas02);
                db.SaveChanges();

                foreach (var item in escLineas01.Esc_Horas)
                {
                    Esc_Horas esc_Horas = new Esc_Horas();
                    esc_Horas.Planta = escLineas02.Planta;
                    esc_Horas.Linea = escLineas02.Linea;
                    esc_Horas.Fecha = item.Fecha;
                    esc_Horas.id_escenario = item.id_escenario;
                    esc_Horas.UserId = item.UserId;
                    esc_Horas.Horas = item.Horas;
                    esc_Horas.Horas_base = item.Horas_base;
                    esc_Horas.modificado = true;
                    esc_Horas.fecha_modificacion = DateTime.Now;
                    db.Esc_Horas.Add(esc_Horas); 
                }

                db.SaveChanges();

                foreach (var item in escLineas01.Esc_OEE)
                {
                    Esc_OEE esc_OEE = new Esc_OEE();
                    esc_OEE.Planta = escLineas02.Planta;
                    esc_OEE.Linea = escLineas02.Linea;
                    esc_OEE.Fecha = item.Fecha;
                    esc_OEE.id_escenario = item.id_escenario;
                    esc_OEE.UserId = item.UserId;

                    esc_OEE.OEE_SinRechazo = item.OEE_SinRechazo;
                    esc_OEE.OEE_SinRechazo_base = item.OEE_SinRechazo_base;
                    esc_OEE.modificado = true;
                    esc_OEE.fecha_modificacion = DateTime.Now;
                    db.Esc_OEE.Add(esc_OEE);
                }

                db.SaveChanges();

                foreach (var item in escLineas01.Esc_OEEDesglose)
                {
                    Esc_OEEDesglose esc_OEEDesglose = new Esc_OEEDesglose();
                    esc_OEEDesglose.Planta = escLineas02.Planta;
                    esc_OEEDesglose.Linea = escLineas02.Linea;
                    esc_OEEDesglose.Año = item.Año;
                    esc_OEEDesglose.id_escenario = item.id_escenario;
                    esc_OEEDesglose.UserId = item.UserId;

                    esc_OEEDesglose.Disponibilidad = item.Disponibilidad;
                    esc_OEEDesglose.Disponibilidad_base = item.Disponibilidad_base;

                    esc_OEEDesglose.Rechazo = item.Rechazo;
                    esc_OEEDesglose.Rechazo_base = item.Rechazo_base;

                    esc_OEEDesglose.Rendimiento = item.Rendimiento;
                    esc_OEEDesglose.Rendimiento_base = item.Rendimiento_base;

                    esc_OEEDesglose.OEE_sin_rechazo = item.OEE_sin_rechazo;
                    esc_OEEDesglose.OEE_sin_rechazo_base = item.OEE_sin_rechazo_base;

                    esc_OEEDesglose.OEE = item.OEE;
                    esc_OEEDesglose.OEE_base = item.OEE_base;
                    esc_OEEDesglose.modificado = true;
                    esc_OEEDesglose.fecha_modificacion = DateTime.Now;
                    db.Esc_OEEDesglose.Add(esc_OEEDesglose);
                }
                db.SaveChanges();
            }
        }

        public static void DeleteRecords(Esc_Lineas escLineas)
        {
            Esc_Lineas escEscenario = db.Esc_Lineas.FirstOrDefault(t =>
                t.Linea == escLineas.Linea);

            if (!escEscenario.Equals(null))
            {
                db.Esc_Lineas.Remove(escEscenario);
            }
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<String> selectedIds = selectedRowIds.Split(',').ToList();
            foreach (var selectedId in selectedIds)
            {
                Esc_Lineas escEscenario = null;
                escEscenario = db.Esc_Lineas.FirstOrDefault(t => t.Linea == selectedId);

                db.Esc_Lineas.Remove(escEscenario);
                db.SaveChanges();
            }
        }
    }
}
