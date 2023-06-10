using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao
{
    public class UbicacionDaoEntitFramework : GenericDaoEntityFramework<Ubicacion, Int64>, IUbicacionDao
    {
        public List<Ubicacion> FindAllUbicaciones()
        {
            DbSet<Ubicacion> Ubicaciones = Context.Set<Ubicacion>();


            var result =
                     (from c in Ubicaciones

                      select c).ToList();


            return result;





        }
    }
}
