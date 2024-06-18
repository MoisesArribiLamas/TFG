using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao
{
    public class ConsumoEntityFramework : GenericDaoEntityFramework<Consumo, Int64>, IConsumoDao
    {
        #region Public Constructors

        public ConsumoEntityFramework()
        {
        }

        #endregion Public Constructors

        #region IConsumoaDao Members. Specific Operations
        /// <exception cref="InstanceNotFoundException"/>

       
                
        public List<Consumo> findConsumoByUbicacion(long ubicacionID, int startIndex, int count)
        {

            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
            (from c in Consumos
             where (c.ubicacionId == ubicacionID)
             select c).OrderBy(c => c.fecha).ThenBy(c => c.horaIni).Skip(startIndex).Take(count).ToList();

            return result;

        }

        #region consumos en un perriodo de tiempo
        public List<Consumo> MostrarConsumosUbicacionPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha >= fecha) && (c.fecha <= fecha2) && (c.ubicacionId == ubicacionID))
                 select c).OrderBy(c => c.fecha).ThenBy(c => c.horaIni).Skip(startIndex).Take(count).ToList();

            return result;
        }
        #endregion

        #region suministros en un periodo de tiempo por dias grafica
        public List<ConsumoPorDias> MostrarSuministradoXUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha >= fecha) && (c.fecha <= fecha2) && (c.ubicacionId == ubicacionID))
                 group c by c.fecha 
                 into g
                 select new ConsumoPorDias() { fecha = g.Key, Count = g.Sum(c => c.kwSuministrados ?? 0) } ).OrderBy(c => c.fecha).ToList();

            return result;
        }
        #endregion 

        #region suministros en día
        public List<ConsumoDiaConcreto> MostrarLoSuministradoPorElSistemaDiaConcreto(long ubicacionID, DateTime fecha)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha == fecha) && (c.ubicacionId == ubicacionID))
                 group c by c.horaIni.Hours
                 into g
                 select new ConsumoDiaConcreto() { Hora = g.Key, Count = g.Sum(c => c.kwSuministrados ?? 0) }).OrderBy(c => c.Hora).ToList();

            return result;
        }
        #endregion

        #region suministros cargas del sistema y consumo por la red en un periodo de tiempo por dias grafica
        public List<CargaSuministraYRed> MostrarSuministradoCargadoYRedXUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha >= fecha) && (c.fecha <= fecha2) && (c.ubicacionId == ubicacionID))
                 group c by c.fecha
                 into g
                 select new CargaSuministraYRed() { fecha = g.Key, Cargado = g.Sum(c => c.kwCargados ?? 0), Suministrado = g.Sum(c => c.kwSuministrados ?? 0), Red = g.Sum(c => c.kwRed ?? 0) }).OrderBy(c => c.fecha).ToList();

            return result;
        }
        #endregion 

        #region suministros cargas del sistema y consumo por la red en un día
        public List<CargaSuministraYRedDiaConcreto> MostrarSuministradoCargadoYRedXUbicacionDiaConcreto(long ubicacionID, DateTime fecha)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha == fecha) && (c.ubicacionId == ubicacionID))
                 group c by c.horaIni.Hours
                 into g
                 select new CargaSuministraYRedDiaConcreto() { Hora = g.Key, Cargado = g.Sum(c => c.kwCargados ?? 0), Suministrado = g.Sum(c => c.kwSuministrados ?? 0), Red = g.Sum(c => c.kwRed ?? 0) }).OrderBy(c => c.Hora).ToList();

            return result;
        }
        #endregion 

        #region consumos por la red electrica en un periodo de tiempo por dias grafica
        public List<ConsumoPorDias> MostrarConsumosRedElectricaUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha >= fecha) && (c.fecha <= fecha2) && (c.ubicacionId == ubicacionID))
                 group c by c.fecha
                 into g
                 select new ConsumoPorDias() { fecha = g.Key, Count = g.Sum(c => c.kwRed ?? 0) }).OrderBy(c => c.fecha).ToList();

            return result;
        }
        #endregion

        #region consumos por la red electrica en un Dia grafica
        public List<ConsumoDiaConcreto> MostrarConsumosRedElectricaUbicacionDiaConcreto(long ubicacionID, DateTime fecha)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha == fecha) && (c.ubicacionId == ubicacionID))
                 group c by c.horaIni.Hours
                 into g
                 select new ConsumoDiaConcreto() { Hora = g.Key, Count = g.Sum(c => c.kwRed ?? 0) }).OrderBy(c => c.Hora).ToList();

            return result;
        }
        #endregion

        #region Cargado por el sistema en un periodo de tiempo por dias grafica
        public List<ConsumoPorDias> MostrarLoCargadoPorElSistemaPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha >= fecha) && (c.fecha <= fecha2) && (c.ubicacionId == ubicacionID))
                 group c by c.fecha
                 into g
                 select new ConsumoPorDias() { fecha = g.Key, Count = g.Sum(c => c.kwCargados ?? 0) }).OrderBy(c => c.fecha).ToList();

            return result;
        }
        #endregion

        #region Cargado por el sistema en un día
        public List<ConsumoDiaConcreto> MostrarLoCargadoPorElSistemaDiaConcreto(long ubicacionID, DateTime fecha)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha == fecha) && (c.ubicacionId == ubicacionID))
                 group c by c.horaIni.Hours
                 into g
                 select new ConsumoDiaConcreto() { Hora = g.Key, Count = g.Sum(c => c.kwCargados ?? 0) }).OrderBy(c => c.Hora).ToList();

            return result;
        }
        #endregion



        #region numero de consumos en un perriodo de tiempo
        public int numeroConsumosUbicacionPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where ((c.fecha >= fecha) && (c.fecha <= fecha2) && (c.ubicacionId == ubicacionID))
                 select c).Count();

            return result;
        }
        #endregion

        #region mostrar ultimo consumo (Entidad) en una ubicacion

        public Consumo UltimoConsumoUbicacion(long ubicacionID)
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 where c.ubicacionId == ubicacionID
                 select c).OrderByDescending(c => c.fecha).ThenByDescending(c => c.horaIni).FirstOrDefault();

            return result;
        }
        #endregion

        #region mostrar ultimo consumo Del sistema

        public DateTime FechaUltimoConsumoSistema()
        {
            DbSet<Consumo> Consumos = Context.Set<Consumo>();

            var result =
                (from c in Consumos
                 select c).OrderByDescending(c => c.fecha).FirstOrDefault();

            return result.fecha;
        }
        #endregion

        //mostrar consumo actual
        #region mostrar ultimo consumo (numero) en una ubicacion

        public double ConsumoUbicacionActual(long ubicacionID)
        {
            Consumo consumo = UltimoConsumoUbicacion(ubicacionID);

            return consumo.consumoActual;
        }
        #endregion


        #region mostrar consumos de una ubicacion por fechas

        public List<Consumo> MostrarConsumosEnUnIntervalo(long ubicacionId, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            DbSet<Consumo> consumos = Context.Set<Consumo>();

            var result =
                (from c in consumos
                 where ((c.fecha >= fecha) && (c.fecha <= fecha2) && (c.ubicacionId == ubicacionId))
                 select c).OrderBy(c => c.fecha).ThenBy(c => c.horaIni).Skip(startIndex).Take(count).ToList();

            return result;
        }
        #endregion

        //#region finalizar Consumo
        //public bool FinalizarConsumo(long consumoID, double kwTotal, TimeSpan horaFin)
        //{
        //    Consumo c = Find(consumoID);
        //    if (c != null)
        //    {

        //        c.horaFin = horaFin;
        //        c.kwTotal = kwTotal;

        //        Update(c);

        //        return true;
        //    }
        //    return false;
        //}
        //#endregion


        #endregion IConsumoaDao Members. Specific Operations


    }
}