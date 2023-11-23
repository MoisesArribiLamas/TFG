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
        public long crearUbicacion( long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta)
        {
            try
            {
                ubicacionDao.findUbicacionExistente(codigoPostal, localidad, calle, portal, numero, etiqueta);

                throw new DuplicateInstanceException(localidad,
                    typeof(Ubicacion).FullName);
            }
            catch (InstanceNotFoundException)
            {
                Ubicacion u = new Ubicacion();

                u.codigoPostal = codigoPostal;
                u.localidad = localidad;
                u.calle = calle;
                u.portal = portal;
                u.numero = numero;
                u.etiqueta = etiqueta;
                u.bateriaSuministradora = null;

                ubicacionDao.Create(u);
                return u.ubicacionId;

            }
        }

            #endregion crear Ubicación
        #region Modificacar Ubicacion
        [Transactional]
        public void modificarUbicacion(long ubicacionId, long? codigoPostal, string localidad, string calle, string portal, long? numero, string etiqueta)
        {

            ubicacionDao.updateInformacion(ubicacionId, codigoPostal, localidad, calle, portal, numero, etiqueta);
        }
        #endregion Modificar

        #region Cambiar bateria suministradora
        [Transactional]
        public void CambiarBateriaSuministradora(long ubicacionId, long? bateriaSuministradora)
        {
            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);

            ubicacion.bateriaSuministradora = bateriaSuministradora;

            ubicacionDao.Update(ubicacion);
        }
                
        #endregion Cambiar bateria

        #region ubicaciones del Usuario
        [Transactional]
        public List<UbicacionProfileDetails> verUbicaciones(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<UbicacionProfileDetails> ubicacionesDTO = new List<UbicacionProfileDetails>();

                List<Ubicacion> ubicaciones = ubicacionDao.ubicacionesUsuario(idUsuario, startIndex, count);

                foreach (Ubicacion u in ubicaciones)
                {
                    ubicacionesDTO.Add(new UbicacionProfileDetails(u.ubicacionId, u.codigoPostal, u.localidad, u.calle, u.portal, u.numero));
                }
                return ubicacionesDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        /*
        #region Eliminar Ubicacion
        [Transactional]
        public void eliminarUbicacion(long ubicacionId)
        {

            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);

            
            ubicacionDao.Remove(ubicacion.ubicacionId);
        }
        #endregion Modificar

    */
    }

}
