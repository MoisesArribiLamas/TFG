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
        bool updateInformacion(long tarifaId, double precio, long hora, DateTime fecha);
        Tarifa PrecioPeorTarifa(DateTime fecha);
        Tarifa PrecioMejorTarifa(DateTime fecha);
        double CalcularMediaTarifa(DateTime fecha, DateTime fecha2);

        double CalcularMediaTarifasHoy(DateTime fecha);
        List<Tarifa> verTarifasDelDia(DateTime fecha);
        List<Tarifa> OrdenarPeorPrecioTarifasDelDia(DateTime fecha);
        List<Tarifa> OrdenarMejorPrecioTarifasDelDia(DateTime fecha);
        Tarifa TarifaActual(DateTime fecha, int hora);
        bool ExistenTarifasDelDia(DateTime fecha);

    }
}
