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
    public interface IServicePedido
    {


        #region crear Ubicación
        [Transactional]
        Ubicacion crearUbicacion(String descripcion, String direccion, String tarjeta);
        #endregion

        int contarUbicaciones(long idUsuario);

/*
        #region caso uso 7
        [Transactional]
        List<PedidosDTO> verPedidos(long idUsuario, int startIndex, int count);
        #endregion caso uso 7

        List<LineaPedidoDTO> getLineasFromPedido(long idPedido, int startIndex, int count);

        int contarLineasPedido(long idPedido);

    */
    }
}
