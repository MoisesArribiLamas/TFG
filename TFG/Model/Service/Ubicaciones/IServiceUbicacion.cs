using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service.Ubicaciones
{
    public interface IServiceUbicacion
    {


        #region crear Ubicación
        [Transactional]
        long crearUbicacion(UbicacionProfileDetails ubicacionProfileDetails);
        #endregion

        [Transactional]
        void modificarUbicacion(long ubicacionId, UbicacionProfileDetails ubicacionProfileDetails);

        [Transactional]
        List<UbicacionProfileDetails> verUbicaciones(long idUsuario, int startIndex, int count);


        /*
        [Transactional]
        void eliminarUbicacion(long ubicacionId, UbicacionProfileDetails ubicacionProfileDetails)
        */

    }
}
