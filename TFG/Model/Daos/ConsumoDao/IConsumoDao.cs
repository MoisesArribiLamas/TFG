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

        DateTime FechaUltimoConsumoSistema();

        List<Consumo> MostrarConsumosEnUnIntervalo(long ubicacionId, DateTime fecha, DateTime fecha2, int startIndex, int count);

        List<ConsumoPorDias> MostrarSuministradoXUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2);

        List<ConsumoDiaConcreto> MostrarLoSuministradoPorElSistemaDiaConcreto(long ubicacionID, DateTime fecha);

        List<ConsumoDiaConcreto> MostrarConsumosRedElectricaUbicacionDiaConcreto(long ubicacionID, DateTime fecha);

        List<ConsumoPorDias> MostrarConsumosRedElectricaUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2);

        List<ConsumoDiaConcreto> MostrarLoCargadoPorElSistemaDiaConcreto(long ubicacionID, DateTime fecha);

        List<ConsumoPorDias> MostrarLoCargadoPorElSistemaPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2);

        List<CargaSuministraYRed> MostrarSuministradoCargadoYRedXUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2);

        List<CargaSuministraYRedDiaConcreto> MostrarSuministradoCargadoYRedXUbicacionDiaConcreto(long ubicacionID, DateTime fecha);

        int numeroConsumosUbicacionPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2);

    }
}
