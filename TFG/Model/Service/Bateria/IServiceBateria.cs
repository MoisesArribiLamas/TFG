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
        long crearBateria(long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso);
        #endregion

        [Transactional]
        void modificarBateria(long bateriaId, long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso);


        [Transactional]
        List<BateriaDTO> verBaterias(long idUsuario, int startIndex, int count);


        /*
        [Transactional]
        void eliminarBateria(long bateriaId, BateriaProfileDetails bateriaProfileDetails)
        */

    }
}
