using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public class CargaDTO
    {
        public CargaDTO(long cargaId, long bateriaId, long tarifaId,
            TimeSpan horaIni, TimeSpan horaFin, double kws)
        {
            this.cargaId = cargaId;
            this.bateriaId = bateriaId;
            this.tarifaId = tarifaId;
            this.horaIni = horaIni;
            this.horaFin = horaFin;
            this.kws = kws;
            
        }

        public long cargaId { get; set; }
        public long bateriaId { get; set; }
        public long tarifaId { get; set; }
        
        public TimeSpan horaIni { get; set; }

        public TimeSpan horaFin { get; set; }

        public double kws { get; private set; }

        public override bool Equals(object obj)
        {
            var details = obj as CargaDTO;
            return details != null &&
                   cargaId == details.cargaId &&
                   bateriaId == details.bateriaId &&
                   tarifaId == details.tarifaId &&
                   horaIni == details.horaIni
                   && (this.horaFin == details.horaFin)
                   && (this.kws == details.kws);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
