using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.EstadoDao
{
    
    public class EstadoDaoEntitFramework : GenericDaoEntityFramework<Estado, Int64>, IEstadoDao
    {
        #region Todos los estados
        public List<Estado> FindAllEstados()
        {
            DbSet<Estado> estados = Context.Set<Estado>();


            var result =
                     (from c in estados

                      select c).OrderBy(c => c.estadoId).ToList();


            return result;

        }
        #endregion Estados


        #region buscar estado por nombre
        public long FindEstadoByName(string nombre)
        {
            DbSet<Estado> estados = Context.Set<Estado>();


            var result =
                     (from e in estados
                      where e.nombre == nombre
                      select e).FirstOrDefault();


            return result.estadoId;

        }
        #endregion Estados

    }


}
