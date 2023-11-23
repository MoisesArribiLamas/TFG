using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Es.Udc.DotNet.TFG.Model.Daos.BateriaDao
{
    public class BateriaEntityFramework : GenericDaoEntityFramework<Bateria, Int64>, IBateriaDao
    {
        #region Public Constructors

        public BateriaEntityFramework()
        {
        }

        #endregion Public Constructors

        #region IBateriaDao Members. Specific Operations
        /// <exception cref="InstanceNotFoundException"/>

        public bool updateInformacion(long bateriaId, long? ubicacionId, long? usuarioId, double? precioMedio,
            double? kwHAlmacenados, double? almacenajeMaximoKwH, DateTime fechaDeAdquisicion, string marca,
            string modelo,double? ratioCarga, double? ratioCompra, double? ratioUso, double? capacidadCargador)
        {
            Bateria b = Find(bateriaId);
            if (b != null)
            {
                if (ubicacionId != null)
                {
                    b.ubicacionId = (long)ubicacionId;
                }
                if (usuarioId != null)
                {
                    b.usuarioId = (long)usuarioId;
                }
                if (precioMedio != null)
                {
                    b.precioMedio = (double)precioMedio;
                }
                if (kwHAlmacenados != null)
                {
                    b.kwHAlmacenados = (double)kwHAlmacenados;
                }
                if (almacenajeMaximoKwH != null)
                {
                    b.almacenajeMaximoKwH = (double)almacenajeMaximoKwH;
                }
                if (fechaDeAdquisicion != null)
                {
                    b.fechaDeAdquisicion = fechaDeAdquisicion;
                }
                if (marca != null)
                {
                    b.marca = marca;
                }
                if (modelo != null)
                {
                    b.modelo = modelo;
                }
                if (ratioCarga != null)
                {
                    b.ratioCarga = (double)ratioCarga;
                }
                if (ratioCompra != null)
                {
                    b.ratioCompra = (double)ratioCompra;
                }
                if (ratioUso != null)
                {
                    b.ratioUso = (double)ratioUso;
                }
                if (capacidadCargador != null)
                {
                    b.capacidadCargador = (double)capacidadCargador;
                }
                
                Update(b);

                return true;
            }
            return false;
        }

        public List<Bateria> findBateriaByUser(long usuarioID, int startIndex, int count)
        {

            DbSet<Bateria> baterias = Context.Set<Bateria>();

            var result =
            (from b in baterias
             where (b.usuarioId == usuarioID)
             select b).OrderBy(b => b.bateriaId).Skip(startIndex).Take(count).ToList();

            return result;
           
        }


        public List<Bateria> findBateriaByUbicacion(long ubicacionID, int startIndex, int count)
        {

            DbSet<Bateria> baterias = Context.Set<Bateria>();

            var result =
            (from b in baterias
             where (b.ubicacionId == ubicacionID)
             select b).OrderBy(b => b.bateriaId).Skip(startIndex).Take(count).ToList();

            return result;

        }

        #endregion IBateriaDao Members. Specific Operations
    }
}