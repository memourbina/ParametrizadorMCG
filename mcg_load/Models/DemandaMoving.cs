using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;

namespace mcg_load.Models
{
    public class DemandaMoving
    {
        [Display(Name = "Código Articulo")]
        public double id_articulo { get; set; }

        [Display(Name = "Código Escenario")]
        public int id_escenario { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime Start { get; set; }

        [Display(Name = "Fecha de Fin")]
        [DateRange(StartDateEditFieldName = "Start", MinDayCount = 1, MaxDayCount = 1825)]
        public DateTime End { get; set; }

        [Display(Name = "Politica de Traslado")]
        public string intValueOption { get; set; }

        [Display(Name = "Monto o %")]
        public double monto { get; set; }

        [Display(Name = "Planta")]
        public string Planta { get; set; }

        [Display(Name = "Línea")]
        public string Linea { get; set; }

        [Display(Name = "Probabilidad")]
        public string Probabilidad { get; set; }

        [Display(Name = "Cliente")]
        public string Cliente { get; set; }

        [Display(Name = "Nombre Cliente")]
        public string Customer_Name { get; set; }

        [Display(Name = "Industría")]
        public string Industria { get; set; }

        [Display(Name = "Tipo de Pieza")]
        public string Tipo_Pieza { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public string TipMovi { get; set; }
        public DemandaMoving()
        {
        }
    }
}