using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class TarifaDTO
    {

        public TarifaDTO(long tarifaId, double precio, long hora, DateTime fecha)
        {
            this.tarifaId = tarifaId;
            this.precio = precio;
            this.hora = hora;
            this.fecha = fecha;
            
        }


        public long tarifaId { get; set; }

        public double precio { get; set; }
        public long hora { get; set; }
        public DateTime fecha { get; set; }


        public override bool Equals(object obj)
        {
            var details = obj as TarifaDTO;
            return details != null
                   //&& (this.tarifaId == details.tarifaId)
                   && (this.precio == details.precio)
                   && (this.hora == details.hora)
                   && (this.fecha == details.fecha);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
