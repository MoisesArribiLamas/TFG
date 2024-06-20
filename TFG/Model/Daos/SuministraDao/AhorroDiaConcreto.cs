using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.SuministraDao
{
    public class AhorroDiaConcreto
    {
        public AhorroDiaConcreto()
        {
        }

        public AhorroDiaConcreto(long hora, double count)
        {
            Hora = hora;
            Count = count;
        }

        public long Hora { get; set; }
        public double Count { get; set; }

        public override bool Equals(object obj)
        {
            var concreto = obj as AhorroDiaConcreto;
            return concreto != null &&
                   Hora == concreto.Hora &&
                   Count == concreto.Count;
        }

        public override int GetHashCode()
        {
            var hashCode = 1686382361;
            hashCode = hashCode * -1521134295 + Hora.GetHashCode();
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            return hashCode;
        }
    }
}
