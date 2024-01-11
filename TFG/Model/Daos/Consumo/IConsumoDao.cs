using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao
{
    public interface IConsumoDao : IGenericDao<Consumo, Int64>
    {
        List<Consumo> findConsumoByUbicacion(long ubicacionID, int startIndex, int count);
        List<Consumo> MostrarConsumosUbicacionPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2, int startIndex, int count);
        Consumo UltimoConsumoUbicacion(long ubicacionID);
        double ConsumoUbicacionActual(long ubicacionID);
    }
}
