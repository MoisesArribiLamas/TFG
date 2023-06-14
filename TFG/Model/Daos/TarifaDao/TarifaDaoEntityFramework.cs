using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Data.Entity;
using System.Linq;

namespace Es.Udc.DotNet.TFG.Model.Daos.TarifaDao
{
    public class TarifaEntityFramework : GenericDaoEntityFramework<Tarifa, Int64>, ITarifaDao
    {
        #region Public Constructors

        public TarifaEntityFramework()
        {
        }

        #endregion Public Constructors

        #region ITarifaDao Members. Specific Operations
        /// <exception cref="InstanceNotFoundException"/>

        public bool updateInformacion(long tarifaId, long precio, long hora, DateTime fecha)
        {
            Tarifa u = Find(tarifaId);
            if (u != null)
            {
                u.precio = precio;

                u.hora = hora;

                u.fecha = fecha;


                Update(u);

                return true;
            }
            return false;
        }

        public Tarifa BuscarMejorTarifa(DateTime fecha)
        {
            DbSet<Tarifa> tarifa = Context.Set<Tarifa>();
            Tarifa mTarifa = null;

            var result =
                (from t in tarifa
                 where (t.fecha == fecha) 
                 select t).OrderBy(t => t.precio );
            mTarifa = result.FirstOrDefault();
            if (mTarifa == null)
                throw new InstanceNotFoundException(mTarifa,
                        typeof(Tarifa).FullName);


            return mTarifa;
        }

        public Tarifa BuscarPeorTarifa(DateTime fecha)
        {
            DbSet<Tarifa> tarifa = Context.Set<Tarifa>();
            Tarifa pTarifa = null;

            var result =
                (from t in tarifa
                 where (t.fecha == fecha)
                 select t).OrderByDescending(t => t.precio);
            pTarifa = result.FirstOrDefault();
            if (pTarifa == null)
                throw new InstanceNotFoundException(pTarifa,
                        typeof(Tarifa).FullName);


            return pTarifa;
        }
        /*System.Nullable<Decimal> averageFreight =
    (from ord in db.Orders
    select ord.Freight)
    .Average();
    double totalWeight = packages.AsQueryable().Sum(pkg => pkg.Weight);

            var consulta = from a in TB.C203_tblClientes 
					where a.TRANSFINAL == 9030 &&
					((a.TRANSFECHA > inicio) && (a.TRANSFECHA < fin))
				select a;
             
             */
        public double CalcularMediaTarifa(DateTime fecha, DateTime fecha2) {
            DbSet<Tarifa> tarifa = Context.Set<Tarifa>();
            Tarifa pTarifa = null;

            var result =
                (from t in tarifa
                 where ((t.fecha >= fecha) && (t.fecha <= fecha2))
                 select t);

            pTarifa = result.FirstOrDefault();
            if (pTarifa == null)
                throw new InstanceNotFoundException(pTarifa,
                        typeof(Tarifa).FullName);


            return result.AsQueryable().Average(media => media.precio);
        }


        #endregion ITarifaDao Members. Specific Operations
    }
}