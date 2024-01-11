using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class ConsumoDTO
    {
        public ConsumoDTO(long consumoId, long ubicacionId, double? kwTotal, DateTime fecha, TimeSpan horaIni, TimeSpan? horaFin, double consumoActual, long ubicacion)
        {
            this.consumoId = consumoId;
            this.ubicacionId = ubicacionId;        
            this.kwTotal = kwTotal;
            this.fecha = fecha;
            this.horaIni = horaIni;
            this.horaFin = horaFin;
            this.consumoActual = consumoActual;
            this.ubicacion = ubicacion;
        }

        public long consumoId { get; set; }

        public long ubicacionId { get; set; }
        public double? kwTotal { get; set; }
        public DateTime fecha { get; set; }

        public TimeSpan horaIni { get; set; }

        public TimeSpan? horaFin { get; set; }

        public double consumoActual { get; set; }

        public long ubicacion { get; set; }

        public override bool Equals(object obj)
        {
            var details = obj as ConsumoDTO;
            return details != null &&
                   ubicacionId == details.ubicacionId &&
                   kwTotal == details.kwTotal &&
                   consumoActual == details.consumoActual &&
                   ubicacion == details.ubicacion
                   && (this.fecha == details.fecha)
                 && (this.horaIni == details.horaIni)
                 && (this.horaFin == details.horaFin);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}

