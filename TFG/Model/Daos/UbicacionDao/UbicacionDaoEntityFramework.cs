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
        
        public bool updateInformacion(long ubicacionId, long? codigoPostal, string localidad, string calle, string portal, long? numero, string etiqueta)
        {
            Ubicacion u = Find(ubicacionId);
            if (u != null)
            {
                if (codigoPostal != null)
                {
                    u.codigoPostal = (long)codigoPostal;
                }
                if (localidad != null)
                {
                    u.localidad = localidad;
                }
                if (calle != null)
                {
                    u.calle = calle;
                }
                if (portal != null)
                {
                    u.portal = portal;
                }
                if (numero != null)
                {
                    u.numero = (long)numero;
                }
                if (etiqueta != null)
                {
                    u.etiqueta = etiqueta;
                }

                //u.bateriaSuministradora = bateriaSuministradora; // esto lo quitamos hacemos dos funciones diferentes una actualizar datos y otra cambiar bateriasuministradora

                Update(u);

                return true;
            }
            return false;
        }

        #endregion IUbicacionDao Members. Specific Operations

        #region Buscar Ubicacion
        public Ubicacion findUbicacionExistente(long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta)
        {
            DbSet<Ubicacion> Ubicaciones = Context.Set<Ubicacion>();
            Ubicacion ubicacion = null;

            var result =
                (from u in Ubicaciones
                 where u.codigoPostal.Equals(codigoPostal) && u.localidad ==localidad && u.calle == calle && u.portal.Equals(portal) && u.numero.Equals(numero) && u.etiqueta.Equals(etiqueta)
                 select u);
            ubicacion = result.FirstOrDefault();
            if (ubicacion == null)
                throw new InstanceNotFoundException(calle,
                        typeof(Ubicacion).FullName);


            return ubicacion;
        }
        #endregion

        #region Ubicaciones del usuario (por bateria)
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

        #region Ubicaciones del usuario
        public List<Ubicacion> ubicacionesPertenecientesAlUsuario(long userId, int startIndex, int count)
        {
            DbSet<Ubicacion> ubicacion = Context.Set<Ubicacion>();

            var result =
                (from u in ubicacion
                 where u.usuario == userId
                 select u).Distinct().OrderByDescending(u => u.ubicacionId).Skip(startIndex).Take(count).ToList();

            return result;

        }
        #endregion

        public int numeroUbicacionesUsuario(long userId)
        {
            DbSet<Ubicacion> ubicacion = Context.Set<Ubicacion>();
            DbSet<Bateria> baterias = Context.Set<Bateria>();
            
                int result =
                           (from u in ubicacion
                            join b in baterias on u.ubicacionId equals b.ubicacionId
                            where b.usuarioId == userId
                            select u).Distinct().Count();



                return result;
            
        }

        #region Todas las Ubicaciones 
        public List<Ubicacion> TodasLasUbicaciones()
        {


            DbSet<Ubicacion> ubicacion = Context.Set<Ubicacion>();

            var result =
                (from u in ubicacion
                 select u).Distinct().OrderByDescending(u => u.ubicacionId).ToList();

            return result;

        }
        #endregion


        

    }
}