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
            this.codigoPostal = codigoPostal;
            this.localidad = localidad;
            this.calle = calle;
            this.portal = portal;
            this.numero = numero;
            this.etiqueta = etiqueta;
        }

        public long ubicacionId { get; set; }

        public long codigoPostal { get; set; }
        public string localidad { get; set; }
        public string calle { get; set; }

        public string portal { get; private set; }

        public long numero { get; private set; }

        public string etiqueta { get; set; }

        public override bool Equals(object obj)
        {
            var details = obj as UbicacionProfileDetails;
            return details != null &&
                   ubicacionId == details.ubicacionId &&
                   codigoPostal == details.codigoPostal &&
                   localidad == details.localidad &&
                   etiqueta == details.etiqueta &&
                   calle == details.calle
                   && (this.portal == details.portal)
                 && (this.numero == details.numero);
        }

        public override int GetHashCode()
        {
            var hashCode = 1358148757;
            hashCode = hashCode * -1521134295 + ubicacionId.GetHashCode();
            hashCode = hashCode * -1521134295 + codigoPostal.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(localidad);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(calle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(portal);
            hashCode = hashCode * -1521134295 + numero.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(etiqueta);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
