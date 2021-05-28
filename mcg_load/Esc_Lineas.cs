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
    
    public partial class Esc_Lineas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Esc_Lineas()
        {
            this.Esc_Articulos = new HashSet<Esc_Articulos>();
            this.Esc_Horas = new HashSet<Esc_Horas>();
            this.Esc_OEEDesglose = new HashSet<Esc_OEEDesglose>();
            this.Esc_OEE = new HashSet<Esc_OEE>();
        }
    
        public string Linea { get; set; }
        public string Planta { get; set; }
        public int id_tipo_linea { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Nullable<bool> modificado { get; set; }
        public string UserId_modifico { get; set; }
        public Nullable<System.DateTime> fecha_modificacion { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual CAT_Plantas CAT_Plantas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Esc_Articulos> Esc_Articulos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Esc_Horas> Esc_Horas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Esc_OEEDesglose> Esc_OEEDesglose { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Esc_OEE> Esc_OEE { get; set; }
        public virtual Esc_Tipo_linea Esc_Tipo_linea { get; set; }
    }
}
