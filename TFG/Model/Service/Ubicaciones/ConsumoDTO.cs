using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class ConsumoDTO
    {
        public ConsumoDTO(long consumoId, long ubicacionId, double? kwCargados, double? kwSuministrados, double? kwRed, DateTime fecha, TimeSpan horaIni, TimeSpan? horaFin, double consumoActual, long ubicacion)
        {
            this.consumoId = consumoId;
            this.ubicacionId = ubicacionId;        
            this.kwCargados = kwCargados;
            this.kwSuministrados = kwSuministrados;
            this.kwRed = kwRed;
            this.fecha = fecha;
            this.horaIni = horaIni;
            this.horaFin = horaFin;
            this.consumoActual = consumoActual;
            this.ubicacion = ubicacion;
            this.criterio = null;
        }

        public ConsumoDTO(long consumoId, long ubicacionId, double? criterio, DateTime fecha, TimeSpan horaIni, TimeSpan? horaFin, double consumoActual, long ubicacion)
        {
            this.consumoId = consumoId;
            this.ubicacionId = ubicacionId;
            this.criterio = criterio;
            this.kwCargados = null;
            this.kwSuministrados = null;
            this.kwRed = null;
            this.fecha = fecha;
            this.horaIni = horaIni;
            this.horaFin = horaFin;
            this.consumoActual = consumoActual;
            this.ubicacion = ubicacion;
        }

        public long consumoId { get; set; }

        public long ubicacionId { get; set; }

        public double? kwCargados { get; set; }

        public double? kwSuministrados { get; set; }

        public double? kwRed { get; set; }

        public DateTime fecha { get; set; }

        public TimeSpan horaIni { get; set; }

        public TimeSpan? horaFin { get; set; }

        public double consumoActual { get; set; }

        public long ubicacion { get; set; }

        public double? criterio { get; set; }

        

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            var dTO = obj as ConsumoDTO;
            return dTO != null &&
                   consumoId == dTO.consumoId &&
                   ubicacionId == dTO.ubicacionId &&
                   EqualityComparer<double?>.Default.Equals(kwCargados, dTO.kwCargados) &&
                   EqualityComparer<double?>.Default.Equals(kwSuministrados, dTO.kwSuministrados) &&
                   EqualityComparer<double?>.Default.Equals(kwRed, dTO.kwRed) &&
                   fecha == dTO.fecha &&
                   horaIni.Equals(dTO.horaIni) &&
                   EqualityComparer<TimeSpan?>.Default.Equals(horaFin, dTO.horaFin) &&
                   consumoActual == dTO.consumoActual &&
                   ubicacion == dTO.ubicacion &&
                   EqualityComparer<double?>.Default.Equals(criterio, dTO.criterio);
        }

        public override int GetHashCode()
        {
            var hashCode = -798318096;
            hashCode = hashCode * -1521134295 + consumoId.GetHashCode();
            hashCode = hashCode * -1521134295 + ubicacionId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(kwCargados);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(kwSuministrados);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(kwRed);
            hashCode = hashCode * -1521134295 + fecha.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TimeSpan>.Default.GetHashCode(horaIni);
            hashCode = hashCode * -1521134295 + EqualityComparer<TimeSpan?>.Default.GetHashCode(horaFin);
            hashCode = hashCode * -1521134295 + consumoActual.GetHashCode();
            hashCode = hashCode * -1521134295 + ubicacion.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(criterio);
            return hashCode;
        }
    }
}

