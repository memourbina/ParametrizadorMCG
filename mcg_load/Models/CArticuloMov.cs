using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mcg_load.Models
{
    public class CArticuloMov
    {
        public double id_articulo { get; set; }
        public int id_escenario { get; set; }
        public Nullable<double> Org__ID { get; set; }
        public string Escenario { get; set; }
        public string Probabilidad { get; set; }
        public string Línea_Moldeo { get; set; }
        public string Cliente { get; set; }
        public string Customer_Name { get; set; }
        public string Industria { get; set; }
        public string Tipo_Pieza { get; set; }
        public string Description { get; set; }
        public string No__Oracle { get; set; }
        public string Customer_PN { get; set; }
        public Nullable<double> Unit_Weight { get; set; }
        public string Mercado { get; set; }
        public string Tipo_de_Hierro { get; set; }
        public string Grado_de_Hierro { get; set; }
        public string OEM { get; set; }
        public string Plataforma { get; set; }
        public string Sistema { get; set; }
        public Nullable<double> Cavs____Soplo { get; set; }
        public Nullable<double> Soplos___Hora { get; set; }
        public Nullable<double> Cor___pza { get; set; }
        public string Maquina { get; set; }
        public Nullable<int> id_maquina { get; set; }
        public string Planta { get; set; }
        public string Planta01 { get; set; }
        public string Linea { get; set; }
        public Nullable<bool> modificado { get; set; }
        public string UserId_modifico { get; set; }
        public Nullable<System.DateTime> fecha_modificacion { get; set; }

        public Nullable<System.DateTime> fecha_start { get; set; }

        public Nullable<System.DateTime> fecha_end { get; set; }

        public string tipo_traslado { get; set; }

        public Nullable<double> Cantidad { get; set; }

    }
}