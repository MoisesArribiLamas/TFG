using Es.Udc.DotNet.ModelUtil.Dao;
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
        #region informacion de carga
        public Carga getInfoCarga(long cargaId)
        {
            DbSet<Carga> Cargas = Context.Set<Carga>();

            var result =
                (from c in Cargas
                 where c.cargaId == cargaId
                 select c).FirstOrDefault();

            return result;
        }
        #endregion

        #region mostrar cargas de una bateria por fechas

        public List<Carga> MostrarCargasBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            DbSet<Carga> cargas = Context.Set<Carga>();

            var result =
                (from c in cargas
                 where ((c.Tarifa.fecha >= fecha) && (c.Tarifa.fecha <= fecha2) && (c.bateriaId == bateriaId))
                 select c).OrderBy(c => c.Tarifa.fecha).ThenBy(c => c.horaIni).Skip(startIndex).Take(count).ToList();

            return result;
        }
        #endregion

        #region mostrar la ultima carga de una bateria

        public Carga UltimaCargaBareria(long bateriaId)
        {
            DbSet<Carga> Cargas = Context.Set<Carga>();

            var result =
                (from c in Cargas
                 where c.bateriaId == bateriaId
                 select c).OrderByDescending(c => c.Tarifa.fecha).ThenByDescending(c => c.horaIni).FirstOrDefault();
            

            return result;
        }
        #endregion

        #region finalizar carga
        public bool FinalizarCarga(long cargaID, TimeSpan horaFin, double kwH)
        {
            Carga c = Find(cargaID);
            if (c != null)
            {
                
                c.horaFin = horaFin;
                c.kwH = kwH;

                Update(c);

                return true;
            }
            return false;
        }
        #endregion
    }
}
