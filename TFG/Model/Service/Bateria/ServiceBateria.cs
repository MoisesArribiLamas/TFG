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
        public long crearBateria(BateriaDTO bateriaDTO)
        {


            Bateria b = new Bateria();
                b.ubicacionId = bateriaDTO.ubicacionId;
                b.usuarioId = bateriaDTO.usuarioId;
                b.precioMedio = bateriaDTO.precioMedio;
                b.kwAlmacenados = bateriaDTO.kwAlmacenados;
                b.almacenajeMaximoKw = bateriaDTO.almacenajeMaximoKw;
                b.fechaDeAdquisicion = bateriaDTO.fechaDeAdquisicion;
                b.marca = bateriaDTO.marca;
                b.modelo = bateriaDTO.modelo;
                b.ratioCarga = bateriaDTO.ratioCarga;
                b.ratioCompra = bateriaDTO.ratioCompra;
                b.ratioUso = bateriaDTO.ratioUso;
                
            bateriaDao.Create(b);
            return b.bateriaId;

            
        }

            #endregion crear baterias
        #region Modificacar Bateria
        [Transactional]
        public void modificarBateria(long bateriaId, BateriaDTO bateriaDTO)
        {
            bateriaDao.updateInformacion(bateriaId, bateriaDTO.ubicacionId, bateriaDTO.usuarioId, bateriaDTO.precioMedio, bateriaDTO.kwAlmacenados,
                bateriaDTO.almacenajeMaximoKw, bateriaDTO.fechaDeAdquisicion, bateriaDTO.marca, bateriaDTO.modelo, bateriaDTO.ratioCarga,
                bateriaDTO.ratioCompra, bateriaDTO.ratioUso);
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
                    bateriasDTO.Add(new BateriaDTO( b.ubicacionId, b.usuarioId, b.precioMedio, b.kwAlmacenados,
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

        /*
        #region Eliminar Bateria
        [Transactional]
        public void eliminarBateria(long bateriaId, BateriaDTO bateriaDTO)
        {

            Bateria bateria = bateriaDao.Find(bateriaId);

            
            bateriaDao.Remove(bateria.bateriaId);
        }
        #endregion Modificar

    */
    }

}
