using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.SuministraDao
{
    public interface ISuministraDao : IGenericDao<Suministra, Int64>
    {
        Suministra getInfoSuministra(long suministraId);

        Suministra UltimaSuministraBareria(long bateriaId);

        List<Suministra> MostrarSuministrosBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count);

        double ahorroBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2);

        double ahorroUsuarioPorFecha(long usuarioId, DateTime fecha, DateTime fecha2);

        bool FinalizarSuministra(long cargaID, TimeSpan horaFin, double kwH, double ahorro);
    }
}
