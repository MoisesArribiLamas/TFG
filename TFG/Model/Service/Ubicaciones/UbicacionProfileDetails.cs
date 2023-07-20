using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class UbicacionProfileDetails
    {
        public UbicacionProfileDetails(string codigoPostal, string localidad, string calle, string portal, string numero)
        {
            this.codigoPostal = codigoPostal;
            this.localidad = localidad;
            this.calle = calle;
            this.portal = portal;
            this.numero = numero;
        }

        public string codigoPostal { get; set; }
        public string localidad { get; set; }
        public string calle { get; set; }

        public string portal { get; private set; }

        public string numero { get; private set; }

        public override bool Equals(object obj)
        {
            var details = obj as UbicacionProfileDetails;
            return details != null &&
                   codigoPostal == details.codigoPostal &&
                   localidad == details.localidad &&
                   calle == details.calle
                   && (this.portal == details.portal)
                 && (this.numero == details.numero);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
