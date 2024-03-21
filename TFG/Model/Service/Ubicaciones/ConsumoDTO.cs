﻿using System;
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

        public override bool Equals(object obj)
        {
            var details = obj as ConsumoDTO;
            return details != null &&
                   ubicacionId == details.ubicacionId &&
                   kwCargados == details.kwCargados &&
                   kwSuministrados == details.kwSuministrados &&
                   kwRed == details.kwRed &&
                   consumoActual == details.consumoActual &&
                   ubicacion == details.ubicacion
                   && (this.fecha == details.fecha)
                 && (this.horaIni == details.horaIni)
                 && (this.horaFin == details.horaFin);
        }

        public override int GetHashCode()
        {
            var hashCode = 315884658;
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
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}

