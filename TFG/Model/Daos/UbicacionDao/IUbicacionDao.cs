using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao
{
    public interface IUbicacionDao : IGenericDao<Ubicacion, Int64>
    {
        bool updateInformacion(long ubicacionId, long? codigoPostal, string localidad, string calle, string portal, long? numero, string etiqueta);

        Ubicacion findUbicacionExistente(long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta);

        List<Ubicacion> ubicacionesUsuario(long userId, int startIndex, int count);


    }
}
