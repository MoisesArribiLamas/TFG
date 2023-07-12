using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.EstadoBateriaDao
{
    public interface IEstadoBateriaDao : IGenericDao<SeEncuentra, Int64>
    {
        List<SeEncuentra> MostrarEstadoBareriaPorFecha(DateTime fecha, DateTime fecha2);
    }
}
