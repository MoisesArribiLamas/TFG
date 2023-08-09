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

        List<Suministra> MostrarSuministrosBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count);

        double ahorroBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2);
    }
}
