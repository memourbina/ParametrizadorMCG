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
    
    public partial class Esc_Actualizacion
    {
        public int id_actualizacion { get; set; }
        public System.DateTime fecha_actualizacion { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string UserId_modifico { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
