using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class SeEncuentraDTO
    {
        public SeEncuentraDTO(long seEncuentraId, TimeSpan horaIni, TimeSpan horaFin, DateTime fecha, long bateriaId, long estadoId)
        {
            this.seEncuentraId = seEncuentraId;
            this.horaIni = horaIni;
            this.horaFin = horaFin;
            this.fecha = fecha;
            this.bateriaId = bateriaId;
            this.estadoId = estadoId;

        }

        
        public long seEncuentraId { get; set; }
        public TimeSpan horaIni { get; set; }
        public TimeSpan horaFin { get; set; }
        public DateTime fecha { get; set; }
        public long bateriaId { get; set; }
        public long estadoId { get; set; }


        public override bool Equals(object obj)
        {
            var details = obj as SeEncuentraDTO;
            return details != null 
                   && (this.seEncuentraId == details.seEncuentraId)
                   && (this.horaIni == details.horaIni)
                   && (this.horaFin == details.horaFin)
                   && (this.fecha == details.fecha)
                   && (this.bateriaId == details.bateriaId)
                   && (this.estadoId == details.estadoId);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
