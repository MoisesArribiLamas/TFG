//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Es.Udc.DotNet.TFG.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ubicacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ubicacion()
        {
            this.Baterias = new HashSet<Bateria>();
        }
    
        public long ubicacionId { get; set; }
        public long codigoPostal { get; set; }
        public string localidad { get; set; }
        public string calle { get; set; }
        public string portal { get; set; }
        public long numero { get; set; }
        public string etiqueta { get; set; }
        public Nullable<long> bateriaSuministradora { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bateria> Baterias { get; set; }
    }
}
