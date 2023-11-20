using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao
{
    public class UbicacionEntityFramework : GenericDaoEntityFramework<Ubicacion, Int64>, IUbicacionDao
    {
        #region Public Constructors

        public UbicacionEntityFramework()
        {
        }

        #endregion Public Constructors

        #region IUbicacionDao Members. Specific Operations
        /// <exception cref="InstanceNotFoundException"/>
        
        public bool updateInformacion(long ubicacionId, long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta )
        {
            Ubicacion u = Find(ubicacionId);
            if (u != null)
            {
                u.codigoPostal = codigoPostal;

                u.localidad = localidad;

                u.calle = calle;

                u.portal = portal;

                u.numero = numero;

                u.etiqueta = etiqueta;
                
                Update(u);

                return true;
            }
            return false;
        }

        #endregion IUbicacionDao Members. Specific Operations

        #region Buscar Ubicacion
        public Ubicacion findUbicacionExistente(long codigoPostal, string localidad, string calle, string portal, long numero)
        {
            DbSet<Ubicacion> Ubicaciones = Context.Set<Ubicacion>();
            Ubicacion ubicacion = null;

            var result =
                (from u in Ubicaciones
                 where u.codigoPostal.Equals(codigoPostal) && u.localidad ==localidad && u.calle == calle && u.portal.Equals(portal) && u.numero.Equals(numero)
                 select u);
            ubicacion = result.FirstOrDefault();
            if (ubicacion == null)
                throw new InstanceNotFoundException(calle,
                        typeof(Ubicacion).FullName);


            return ubicacion;
        }
        #endregion

        #region Ubicaciones del usuario
        public List<Ubicacion> ubicacionesUsuario(long userId, int startIndex, int count)
        {


            DbSet<Ubicacion> ubicacion = Context.Set<Ubicacion>();
            DbSet<Bateria> baterias = Context.Set<Bateria>();
           

            var result =
                (from u in ubicacion
                 join b in baterias on u.ubicacionId equals b.ubicacionId
                 where b.usuarioId == userId
                 select u).Distinct().OrderByDescending(u => u.ubicacionId).Skip(startIndex).Take(count).ToList();

            return result;

        }
        #endregion
    }
}