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

        #region suministros en un perriodo de tiempo
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

        //mostrar consumo actual
        #region mostrar ultimo consumo (numero) en una ubicacion

        public double ConsumoUbicacionActual(long ubicacionID)
        {
            Consumo consumo = UltimoConsumoUbicacion(ubicacionID);

            return consumo.consumoActual;
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