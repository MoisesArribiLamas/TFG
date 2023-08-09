using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.TarifaDao
{
    public interface ITarifaDao : IGenericDao<Tarifa, Int64>
    {
        bool updateInformacion(long tarifaId, long precio, long hora, DateTime fecha);
        long BuscarMejorTarifa(DateTime fecha);
        long BuscarPeorTarifa(DateTime fecha);
        double CalcularMediaTarifa(DateTime fecha, DateTime fecha2);
        List<Tarifa> verTarifasDelDia(DateTime fecha);

    }
}
