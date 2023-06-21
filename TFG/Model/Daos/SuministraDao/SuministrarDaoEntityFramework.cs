using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.SuministraDao
{
    public class SuministraDaoEntitFramework : GenericDaoEntityFramework<Suministra, Int64>, ISuministraDao
    {
        
        public Suministra getInfoSuministra(long suministraId)
        {
            DbSet<Suministra> suministraciones = Context.Set<Suministra>();

            var result =
                (from s in suministraciones
                 where s.suministraId == suministraId
                 select s).FirstOrDefault();

            return result;
        }
    }
}
