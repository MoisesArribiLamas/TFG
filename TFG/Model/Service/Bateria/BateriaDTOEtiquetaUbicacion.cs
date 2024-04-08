using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public class BateriaDTOEtiquetaUbicacion
    {
        public BateriaDTOEtiquetaUbicacion(long bateriaId , String etiquetaUbicacion, double precioMedio, double porcentajeCarga, 
             string nSerie, double ratioCarga, double ratioCompra, double ratioUso)
        {
            this.bateriaid = bateriaId;
            this.etiquetaUbicacion = etiquetaUbicacion;
            this.precioMedio = precioMedio;
            this.porcentajeCarga = porcentajeCarga;

            this.nSerie = nSerie;
            this.ratioCarga = ratioCarga;
            this.ratioCompra = ratioCompra;
            this.ratioUso = ratioUso;

        }

        public long bateriaid { get; set; }

        public string etiquetaUbicacion { get; set; }

        public double precioMedio { get; set; }

        public double porcentajeCarga { get; private set; }

        public string nSerie { get; set; }

        public double ratioCarga { get; private set; }

        public double ratioCompra { get; private set; }
        public double ratioUso { get; private set; }

        public override bool Equals(object obj)
        {
            var ubicacion = obj as BateriaDTOEtiquetaUbicacion;
            return ubicacion != null &&
                   bateriaid == ubicacion.bateriaid &&
                   etiquetaUbicacion == ubicacion.etiquetaUbicacion &&
                   precioMedio == ubicacion.precioMedio &&
                   porcentajeCarga == ubicacion.porcentajeCarga &&
                   nSerie == ubicacion.nSerie &&
                   ratioCarga == ubicacion.ratioCarga &&
                   ratioCompra == ubicacion.ratioCompra &&
                   ratioUso == ubicacion.ratioUso;
        }

        public override int GetHashCode()
        {
            var hashCode = 1019851621;
            hashCode = hashCode * -1521134295 + bateriaid.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(etiquetaUbicacion);
            hashCode = hashCode * -1521134295 + precioMedio.GetHashCode();
            hashCode = hashCode * -1521134295 + porcentajeCarga.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nSerie);
            hashCode = hashCode * -1521134295 + ratioCarga.GetHashCode();
            hashCode = hashCode * -1521134295 + ratioCompra.GetHashCode();
            hashCode = hashCode * -1521134295 + ratioUso.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
