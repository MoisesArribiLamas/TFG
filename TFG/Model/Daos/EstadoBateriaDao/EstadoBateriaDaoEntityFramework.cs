using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.EstadoBateriaDao
{
    public class EstadoBateriaDaoEntitFramework : GenericDaoEntityFramework<SeEncuentra, Int64>, IEstadoBateriaDao
    {

        public List<SeEncuentra> MostrarEstadoBateriaPorFecha(long bateriaID, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            DbSet<SeEncuentra> seEncuentra = Context.Set<SeEncuentra>();

            var result =
                (from t in seEncuentra
                 where ((t.bateriaId == bateriaID) && (t.fecha >= fecha) && (t.fecha <= fecha2))
                 select t).OrderBy(t => t.fecha).ThenBy(t => t.horaIni).Skip(startIndex).Take(count).ToList();

            return result;
        }

        public bool PonerHorafinEstadoBateria(long estadobateriaID, TimeSpan hora)
        {
            SeEncuentra eb = Find(estadobateriaID);
            if (eb != null)
            {
                if (hora != null)
                {
                    eb.horaFin = hora;
                }
<<<<<<< HEAD
                
=======

>>>>>>> 9331010
                Update(eb);

                return true;
            }
            return false;
        }
    }
}
