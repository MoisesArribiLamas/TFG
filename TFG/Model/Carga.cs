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
    
    public partial class Carga
    {
        public long cargaId { get; set; }
        public long bateriaId { get; set; }
        public long tarifaId { get; set; }
        public System.TimeSpan horaIni { get; set; }
        public System.TimeSpan horaFin { get; set; }
        public double kws { get; set; }
    
        public virtual Bateria Bateria { get; set; }
        public virtual Tarifa Tarifa { get; set; }
    }
}
