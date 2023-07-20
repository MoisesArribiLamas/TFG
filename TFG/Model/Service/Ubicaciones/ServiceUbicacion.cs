using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Ninject;


namespace Es.Udc.DotNet.TFG.Model.Service.Ubicaciones
{
    public class ServiceUbicacion : IServiceUbicacion
    {

        [Inject]
        public IUbicacionDao ubicacionDao { private get; set; }


/*
        #region crear Ubicación
        [Transactional]
        public Ubicacion crearUbicacion(UbicacionProfileDetails ubicacionProfileDetails)
        {
            try
            {
                ubicacionDao.findUbicacion(ubicacionProfileDetails.email);

                throw new DuplicateInstanceException(ubicacionProfileDetails.email,
                    typeof(Ubicacion).FullName);
            }
            catch (InstanceNotFoundException)
            {
                String encryptedPassword = PasswordEncrypter.Crypt(clearPassword);

                usuario user = new usuario();

                user.nombre = userProfileDetails.nombre;
                user.apellidos = userProfileDetails.apellidos;
                user.contraseña = encryptedPassword;
                user.codigo_postal = userProfileDetails.direccion_postal;
                user.email = userProfileDetails.email;
                user.tipo_usuario = "default";
                user.idioma = userProfileDetails.Language;
                user.pais = userProfileDetails.Country;

                usuarioDao.Create(user);
                return user.id_usuario;
            }

        }*/
        /*
                #endregion crear Ubicación

                public int contarPedidos(long idUsuario)
                {

                    return pedidoDao.getNumberOfPedidos(idUsuario);

                }

                #region Caso de uso 7
                [Transactional]
                public List<PedidosDTO> verPedidos(long idUsuario, int startIndex, int count)
                {
                    try
                    {
                        List<PedidosDTO> pedidosDTO = new List<PedidosDTO>();

                        List<pedido> pedidos = pedidoDao.findPedidos(idUsuario, startIndex, count);

                        bool existMorePedidos = (pedidos.Count == count + 1);

                        foreach (pedido p in pedidos)
                        {
                            pedidosDTO.Add(new PedidosDTO(p.id_pedido, p.descripcion, p.fecha, p.precio, p.direccion));
                        }
                        return pedidosDTO;

                    }
                    catch (InstanceNotFoundException)
                    {
                        return null;
                    }
                }




                #endregion Caso de uso 7
                [Transactional]

                public List<LineaPedidoDTO> getLineasFromPedido(long idPedido, int startIndex, int count)
                {
                    try
                    {
                        List<LineaPedidoDTO> lineasPedidosDTO = new List<LineaPedidoDTO>();

                        List<linea_pedido> lineasPedido = lineaPedidoDao.getLineasPedido(idPedido, startIndex, count);


                        foreach (linea_pedido lp in lineasPedido)
                        {
                            String nombre = productoDao.Find(lp.producto).nombre;
                            lineasPedidosDTO.Add(new LineaPedidoDTO(lp.producto, nombre, lp.cantidad, lp.precio, lp.a_envolver));
                        }
                        return lineasPedidosDTO;

                    }
                    catch (InstanceNotFoundException)
                    {
                        return null;
                    }

                }

                public int contarLineasPedido(long idPedido)
                {
                    return lineaPedidoDao.getNumberLineasPedido(idPedido);

                }
                */

    }

    public interface IServiceUbicacion
    {
    }
}
