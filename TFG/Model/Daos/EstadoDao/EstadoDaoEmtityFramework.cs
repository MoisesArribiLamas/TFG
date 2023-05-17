using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.Daos.EstadoDao
{
    class EstadoDaoEmtityFramework : GenericDaoEntityFramework<Estado, Int64>, IEstadoDao
    {
        public List<Estado> FindAllEstados()
        {
            DbSet<Estado> estados = Context.Set<Estado>();


            var result =
                     (from c in estados

                      select c).ToList();


            return result;





        }
    }
}
