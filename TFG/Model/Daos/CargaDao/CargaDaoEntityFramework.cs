﻿using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.CargaDao
{
    public class CargaDaoEntitFramework : GenericDaoEntityFramework<Carga, Int64>, ICargaDao
    {
        
        public Carga getInfoCarga(long cargaId)
        {
            DbSet<Carga> Cargas = Context.Set<Carga>();

            var result =
                (from c in Cargas
                 where c.cargaId == cargaId
                 select c).FirstOrDefault();

            return result;
        }


        
        public List<Carga> MostrarCargasBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            DbSet<Carga> cargas = Context.Set<Carga>();

            var result =
                (from c in cargas
                 where ((c.Tarifa.fecha >= fecha) && (c.Tarifa.fecha <= fecha2) && (c.bateriaId == bateriaId))
                 select c).OrderBy(c => c.horaIni).Skip(startIndex).Take(count).ToList();

            return result;
        }
    }
}
