using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao
{
    public class CargaSuministraYRedDiaConcreto
    {


        public CargaSuministraYRedDiaConcreto()
        {
        }

        public CargaSuministraYRedDiaConcreto(int hora, double cargado, double suministrado, double red)
        {
            Hora = hora;
            Cargado = cargado;
            Suministrado = suministrado;
            Red = red;
        }

        public int Hora { get; set; }
        public double Cargado { get; set; }
        public double Suministrado { get; set; }
        public double Red { get; set; }

        public override bool Equals(object obj)
        {
            var concreto = obj as CargaSuministraYRedDiaConcreto;
            return concreto != null &&
                   Hora == concreto.Hora &&
                   Cargado == concreto.Cargado &&
                   Suministrado == concreto.Suministrado &&
                   Red == concreto.Red;
        }

        public override int GetHashCode()
        {
            var hashCode = 1196414830;
            hashCode = hashCode * -1521134295 + Hora.GetHashCode();
            hashCode = hashCode * -1521134295 + Cargado.GetHashCode();
            hashCode = hashCode * -1521134295 + Suministrado.GetHashCode();
            hashCode = hashCode * -1521134295 + Red.GetHashCode();
            return hashCode;
        }
    }
}
