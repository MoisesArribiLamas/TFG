using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.BateriaDao
{
    public interface IBateriaDao : IGenericDao<Bateria, Int64>
    {
        bool updateInformacion(long bateriaId);



    }
}
