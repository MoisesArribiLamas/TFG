﻿using Es.Udc.DotNet.ModelUtil.Dao;
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

        public bool updateInformacion(long bateriaId, long ubicacionId, long usuarioId, double precioMedio,
            double kwHAlmacenados, double almacenajeMaximoKwH, DateTime fechaDeAdquisicion, string marca,
            string modelo,double ratioCarga, double ratioCompra, double ratioUso)
        {
            Bateria b = Find(bateriaId);
            if (b != null)
            {
                if (ubicacionId != null)
                {
                    b.ubicacionId = ubicacionId;
                }
                if (usuarioId != null)
                {
                    b.usuarioId = usuarioId;
                }
                if (precioMedio != null)
                {
                    b.precioMedio = precioMedio;
                }
                if (kwHAlmacenados != null)
                {
                    b.kwHAlmacenados = kwHAlmacenados;
                }
                if (almacenajeMaximoKwH != null)
                {
                    b.almacenajeMaximoKwH = almacenajeMaximoKwH;
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
                    b.ratioCarga = ratioCarga;
                }
                if (ratioCompra != null)
                {
                    b.ratioCompra = ratioCompra;
                }
                if (ratioUso != null)
                {
                    b.ratioUso = ratioUso;
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