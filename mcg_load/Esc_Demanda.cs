//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mcg_load
{
    using System;
    using System.Collections.Generic;
    
    public partial class Esc_Demanda
    {
        public double id_articulo { get; set; }
        public int Fecha { get; set; }
        public int id_escenario { get; set; }
        public string UserId { get; set; }
        public Nullable<double> Valor { get; set; }
        public Nullable<double> Valor_base { get; set; }
        public Nullable<bool> eliminado { get; set; }
        public Nullable<bool> modificacion { get; set; }
        public string UserId_modifico { get; set; }
        public Nullable<System.DateTime> fecha_modificacion { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual Esc_Articulos Esc_Articulos { get; set; }
        public virtual Esc_Calendario Esc_Calendario { get; set; }
        public virtual Esc_Escenario Esc_Escenario { get; set; }
    }
}
