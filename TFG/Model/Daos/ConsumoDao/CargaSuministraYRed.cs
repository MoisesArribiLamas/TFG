using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao
{
    public class CargaSuministraYRed
    {


        public CargaSuministraYRed()
        {
        }

        public CargaSuministraYRed(DateTime fecha, double cargado, double suministrado, double red)
        {
            this.fecha = fecha;
            Cargado = cargado;
            Suministrado = suministrado;
            Red = red;
        }

        public DateTime fecha { get; set; }
        public double Cargado { get; set; }
        public double Suministrado { get; set; }
        public double Red { get; set; }

        public override bool Equals(object obj)
        {
            var red = obj as CargaSuministraYRed;
            return red != null &&
                   fecha == red.fecha &&
                   Cargado == red.Cargado &&
                   Suministrado == red.Suministrado &&
                   Red == red.Red;
        }

        public override int GetHashCode()
        {
            var hashCode = 1638469819;
            hashCode = hashCode * -1521134295 + fecha.GetHashCode();
            hashCode = hashCode * -1521134295 + Cargado.GetHashCode();
            hashCode = hashCode * -1521134295 + Suministrado.GetHashCode();
            hashCode = hashCode * -1521134295 + Red.GetHashCode();
            return hashCode;
        }
    }
}
