using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public interface IServiceBateria
    {


        #region crear bateria
        [Transactional]
        long crearBateria(BateriaDTO bateriaDTO);
        #endregion

        [Transactional]
        void modificarBateria(long bateriaId, BateriaDTO bateriaDTO);

        [Transactional]
        List<BateriaDTO> verBaterias(long idUsuario, int startIndex, int count);


        /*
        [Transactional]
        void eliminarBateria(long bateriaId, BateriaProfileDetails bateriaProfileDetails)
        */

    }
}
