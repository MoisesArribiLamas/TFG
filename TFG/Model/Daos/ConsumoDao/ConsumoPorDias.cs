using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao
{
    public class ConsumoPorDias
    {


        public ConsumoPorDias(DateTime fecha, double Count)
        {
            this.fecha = fecha;
            this.Count = Count;
        }

        public ConsumoPorDias()
        {
        }

        public DateTime fecha { get; set; }
        public double Count { get; set; }

        public override bool Equals(object obj)
        {
            var dias = obj as ConsumoPorDias;
            return dias != null &&
                   fecha == dias.fecha &&
                   Count == dias.Count;
        }

        public override int GetHashCode()
        {
            var hashCode = 667283990;
            hashCode = hashCode * -1521134295 + fecha.GetHashCode();
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            return hashCode;
        }
    }
}
