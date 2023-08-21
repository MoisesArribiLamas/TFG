using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.EstadoDao
{
    public interface IEstadoDao : IGenericDao<Estado, Int64>
    {
        List<Estado> FindAllEstados();

        long FindEstadoByName(string nombre);


    }
}
