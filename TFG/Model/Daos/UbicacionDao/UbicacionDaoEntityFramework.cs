using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
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
        
        public bool updateInformacion(long ubicacionId, long codigoPostal, string localidad, string calle, long portal, long numero)
        {
            Ubicacion u = Find(ubicacionId);
            if (u != null)
            {
                u.codigoPostal = codigoPostal;

                u.localidad = localidad;

                u.calle = calle;

                u.portal = portal;

                u.numero = numero;
                
                Update(u);

                return true;
            }
            return false;
        }

        #endregion IUbicacionDao Members. Specific Operations

        #region Buscar Ubicacion
        public bool findUbicacionExistente(long codigoPostal, string localidad, string calle, long portal, long numero)
        {
            DbSet<Ubicacion> Ubicaciones = Context.Set<Ubicacion>();
            Ubicacion ubicacion = null;

            var result =
                (from u in Ubicaciones
                 where u.codigoPostal.Equals(codigoPostal) && u.localidad ==localidad && u.calle == calle && u.portal.Equals(portal) && u.numero.Equals(numero)
                 select u);
            ubicacion = result.FirstOrDefault();
            if (ubicacion != null)
                throw new DuplicateInstanceException(calle,
                        typeof(Ubicacion).FullName);


            return true;
        }
        #endregion
    }
}