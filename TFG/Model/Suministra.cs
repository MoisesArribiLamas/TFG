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
    
    public partial class Suministra
    {
        public long suministraId { get; set; }
        public long bateriaId { get; set; }
        public long tarifaId { get; set; }
        public System.TimeSpan horaIni { get; set; }
        public System.TimeSpan horaFin { get; set; }
        public double kwH { get; set; }
        public double ahorro { get; set; }
    
        public virtual Bateria Bateria { get; set; }
        public virtual Tarifa Tarifa { get; set; }
    }
}
