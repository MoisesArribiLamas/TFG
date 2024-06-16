using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class UbicacionProfileDetails
    {
        public UbicacionProfileDetails(long ubicacionId,long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta)
        {
            this.ubicacionId = ubicacionId;
            this.codigoPostal = codigoPostal;
            this.localidad = localidad;
            this.calle = calle;
            this.portal = portal;
            this.numero = numero;
            this.etiqueta = etiqueta;
            this.consumoActual = null;

        }

        public UbicacionProfileDetails(long ubicacionId, string etiqueta, double? consumoActual, string estado, string porcentaje)
        {
            this.ubicacionId = ubicacionId;
            this.etiqueta = etiqueta;
            this.consumoActual = consumoActual;
            this.estado = estado;
            this.porcentaje = porcentaje;
        }

        public UbicacionProfileDetails(long ubicacionId, long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta, double? consumoActual, string bateriaSuministradora)
        {
            this.ubicacionId = ubicacionId;
            this.codigoPostal = codigoPostal;
            this.localidad = localidad;
            this.calle = calle;
            this.portal = portal;
            this.numero = numero;
            this.etiqueta = etiqueta;
            this.consumoActual = consumoActual;
            this.bateriaSuministradora = bateriaSuministradora;
        }

        public long ubicacionId { get; set; }

        public long codigoPostal { get; set; }
        public string localidad { get; set; }
        public string calle { get; set; }

        public string portal { get; private set; }

        public long numero { get; private set; }

        public string etiqueta { get; set; }

        public double? consumoActual { get; private set; }

        public string bateriaSuministradora { get; set; }

        public string estado { get; set; }

        public string porcentaje { get; set; }
        

        public override bool Equals(object obj)
        {
            var details = obj as UbicacionProfileDetails;
            return details != null &&
                   ubicacionId == details.ubicacionId &&
                   codigoPostal == details.codigoPostal &&
                   localidad == details.localidad &&
                   calle == details.calle &&
                   portal == details.portal &&
                   numero == details.numero &&
                   etiqueta == details.etiqueta &&
                   EqualityComparer<double?>.Default.Equals(consumoActual, details.consumoActual) &&
                   bateriaSuministradora == details.bateriaSuministradora;
        }

        public override int GetHashCode()
        {
            var hashCode = -104618586;
            hashCode = hashCode * -1521134295 + ubicacionId.GetHashCode();
            hashCode = hashCode * -1521134295 + codigoPostal.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(localidad);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(calle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(portal);
            hashCode = hashCode * -1521134295 + numero.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(etiqueta);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(consumoActual);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(bateriaSuministradora);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
