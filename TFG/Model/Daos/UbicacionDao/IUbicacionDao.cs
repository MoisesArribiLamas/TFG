using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Dao.UbicacionDao
{
    public interface IUbicacionDao : IGenericDao<Ubicacion, Int64>
    {
        bool updateInformacion(long ubicacionId, long codigoPostal, string localidad, string calle, long portal, long numero);



    }
}
