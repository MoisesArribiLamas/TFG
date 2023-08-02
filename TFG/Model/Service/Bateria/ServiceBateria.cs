using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public class ServiceBateria : IServiceBateria
    {

        [Inject]
        public IBateriaDao bateriaDao { private get; set; }
        [Inject]
        public IUsuarioDao UsuarioDao { private get; set; }


        #region crear baterias
        [Transactional]
        public long crearBateria(long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso)
        {


            Bateria b = new Bateria();
                b.ubicacionId = ubicacionId;
                b.usuarioId = usuarioId;
                b.precioMedio = precioMedio;
                b.kwAlmacenados = kwAlmacenados;
                b.almacenajeMaximoKw = almacenajeMaximoKw;
                b.fechaDeAdquisicion = fechaDeAdquisicion;
                b.marca = marca;
                b.modelo = modelo;
                b.ratioCarga = ratioCarga;
                b.ratioCompra = ratioCompra;
                b.ratioUso = ratioUso;
                
            bateriaDao.Create(b);
            return b.bateriaId;

            
        }

            #endregion crear baterias
        #region Modificacar Bateria
        [Transactional]
        public void modificarBateria(long bateriaId, long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso)
        {
            bateriaDao.updateInformacion(bateriaId, ubicacionId, usuarioId, precioMedio, kwAlmacenados,
                almacenajeMaximoKw, fechaDeAdquisicion, marca, modelo, ratioCarga,
                ratioCompra, ratioUso);
        }
        #endregion Modificar

        #region baterias del Usuario
        [Transactional]
        public List<BateriaDTO> verBaterias(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<BateriaDTO> bateriasDTO = new List<BateriaDTO>();

                List<Bateria> baterias = bateriaDao.findBateriaByUser(idUsuario, startIndex, count);

                foreach (Bateria b in baterias)
                {
                    bateriasDTO.Add(new BateriaDTO(b.bateriaId, b.ubicacionId, b.usuarioId, b.precioMedio, b.kwAlmacenados,
                b.almacenajeMaximoKw, b.fechaDeAdquisicion, b.marca, b.modelo, b.ratioCarga,
                b.ratioCompra, b.ratioUso));
                }
                return bateriasDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region Buscar Bateria por ID
        [Transactional]
        public Bateria BuscarBateriaById(long bateriaId)
        {

            return bateriaDao.Find(bateriaId);
            
        }
        #endregion Buscar por ID


        #region Eliminar Bateria
        [Transactional]
        public void eliminarBateria(long bateriaId)
        {

            Bateria bateria = bateriaDao.Find(bateriaId);

            
            bateriaDao.Remove(bateria.bateriaId);
        }
        #endregion Eliminar


    }

}
