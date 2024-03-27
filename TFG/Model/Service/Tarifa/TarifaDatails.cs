using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class TarifaDetails
    {

        public TarifaDetails(double precio, string hora)
        {
            this.precio = precio;
            this.hora = hora;
            
        }


        public double precio { get; set; }
        public string hora { get; set; }

        public override bool Equals(object obj)
        {
            var datails = obj as TarifaDetails;
            return datails != null &&
                   precio == datails.precio &&
                   hora == datails.hora;
        }

        public override int GetHashCode()
        {
            var hashCode = -1293720439;
            hashCode = hashCode * -1521134295 + precio.GetHashCode();
            hashCode = hashCode * -1521134295 + hora.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
