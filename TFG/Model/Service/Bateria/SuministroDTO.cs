using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public class SuministroDTO
    {
        public SuministroDTO(long suministroId, long bateriaId, long tarifaId, double ahorro,
            TimeSpan horaIni, TimeSpan horaFin, double kwH)
        {
            this.suministroId = suministroId;
            this.bateriaId = bateriaId;
            this.tarifaId = tarifaId;
            this.ahorro = ahorro;
            this.horaIni = horaIni;
            this.horaFin = horaFin;
            this.kwH = kwH;
            
        }

        public long suministroId { get; set; }
        public long bateriaId { get; set; }
        public long tarifaId { get; set; }
        public double ahorro { get; set; }

        public TimeSpan horaIni { get; set; }

        public TimeSpan horaFin { get; set; }

        public double kwH { get; private set; }

        public override bool Equals(object obj)
        {
            var details = obj as SuministroDTO;
            return details != null &&
                   suministroId == details.suministroId &&
                   bateriaId == details.bateriaId &&
                   tarifaId == details.tarifaId &&
                   ahorro == details.ahorro &&
                   horaIni == details.horaIni
                   && (this.horaFin == details.horaFin)
                   && (this.kwH == details.kwH);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
