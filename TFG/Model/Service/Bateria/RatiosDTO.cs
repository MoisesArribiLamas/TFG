using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public class RatiosDTO
    {
        public RatiosDTO(double ratioCarga, double ratioCompra, double ratioUso)
        {
            
            this.ratioCarga = ratioCarga;
            this.ratioCompra = ratioCompra;
            this.ratioUso = ratioUso;
            
        }
        

        public double ratioCarga { get; private set; }

        public double ratioCompra { get; private set; }
        public double ratioUso { get; private set; }
        

        public override bool Equals(object obj)
        {
            var details = obj as BateriaDTO;
            return details != null 
                   && (this.ratioCarga == details.ratioCarga)
                   && (this.ratioCompra == details.ratioCompra)
                   && (this.ratioUso == details.ratioUso);
        }

        public override int GetHashCode()
        {
            var hashCode = 658225453;
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
