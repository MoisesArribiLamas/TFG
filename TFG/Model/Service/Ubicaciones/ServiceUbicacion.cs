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



        #region crear Ubicación
        [Transactional]
        public long crearUbicacion(UbicacionProfileDetails ubicacionProfileDetails)
        {
            try
            {
                ubicacionDao.findUbicacionExistente(ubicacionProfileDetails.codigoPostal, ubicacionProfileDetails.localidad, ubicacionProfileDetails.calle, ubicacionProfileDetails.portal, ubicacionProfileDetails.numero);

                throw new DuplicateInstanceException(ubicacionProfileDetails.localidad,
                    typeof(Ubicacion).FullName);
            }
            catch (InstanceNotFoundException)
            {
                Ubicacion u = new Ubicacion();

                u.codigoPostal = ubicacionProfileDetails.codigoPostal;
                u.localidad = ubicacionProfileDetails.localidad;
                u.calle = ubicacionProfileDetails.calle;
                u.portal = ubicacionProfileDetails.portal;
                u.numero = ubicacionProfileDetails.numero;

                ubicacionDao.Create(u);
                return u.ubicacionId;

            }
        }

            #endregion crear Ubicación
        #region Modificacar Ubicacion
        [Transactional]
        public void modificarUbicacion(long ubicacionId, UbicacionProfileDetails ubicacionProfileDetails)
        {

            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);
          
            ubicacion.codigoPostal = ubicacionProfileDetails.codigoPostal;
            ubicacion.localidad = ubicacionProfileDetails.localidad;
            ubicacion.calle = ubicacionProfileDetails.calle;
            ubicacion.portal = ubicacionProfileDetails.portal;
            ubicacion.numero = ubicacionProfileDetails.numero;
            ubicacionDao.Update(ubicacion);
        }
        #endregion Modificar
        [Transactional]
        public List<UbicacionProfileDetails> verUbicaciones(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<UbicacionProfileDetails> ubicacionesDTO = new List<UbicacionProfileDetails>();

                List<Ubicacion> ubicaciones = ubicacionDao.ubicacionesUsuario(idUsuario, startIndex, count);

                foreach (Ubicacion u in ubicaciones) 
                {
                    ubicacionesDTO.Add(new UbicacionProfileDetails(u.codigoPostal, u.localidad, u.calle, u.portal, u.numero));
                }
                return ubicacionesDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }
        #region ubicaciones del Usuario


        #endregion

        /*
        #region Eliminar Ubicacion
        [Transactional]
        public void eliminarUbicacion(long ubicacionId, UbicacionProfileDetails ubicacionProfileDetails)
        {

            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);

            
            ubicacionDao.Remove(ubicacion.ubicacionId);
        }
        #endregion Modificar

    */
    }

}
