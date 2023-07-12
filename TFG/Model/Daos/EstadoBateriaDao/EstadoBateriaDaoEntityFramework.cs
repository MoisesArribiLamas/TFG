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
        
        public List<SeEncuentra> MostrarEstadoBareriaPorFecha(DateTime fecha, DateTime fecha2)
        {
            DbSet<SeEncuentra> seEncuentra = Context.Set<SeEncuentra>();

            var result =
                (from t in seEncuentra
                 where ((t.fecha >= fecha) && (t.fecha <= fecha2))
                 select t).ToList();

            return result;
        }
    }
}
