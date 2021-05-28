using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mcg_load.Models
{
    public class ArticuloKey
    {
        public double id_articulo { get; set; }
        public int id_escenario { get; set; }

        public ArticuloKey()
        {
        }

        public ArticuloKey(double idArticulo, int idEscenario)
        {
            id_articulo = idArticulo;
            id_escenario = idEscenario;
        }
    }
}