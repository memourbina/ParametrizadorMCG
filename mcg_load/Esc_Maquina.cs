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
    
    public partial class Esc_Maquina
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Esc_Maquina()
        {
            this.Esc_Articulos = new HashSet<Esc_Articulos>();
        }
    
        public int id_maquina { get; set; }
        public string maquina { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Nullable<System.DateTime> fecha_alta { get; set; }
        public string UserId_modifico { get; set; }
        public string Planta { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual CAT_Plantas CAT_Plantas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Esc_Articulos> Esc_Articulos { get; set; }
    }
}
