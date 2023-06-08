using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using System;
using System.Data.Entity;
using System.Linq;

namespace Es.Udc.DotNet.TFG.Model.Daos.UsuarioDao
{
    public class UsuarioEntityFramework : GenericDaoEntityFramework<Usuario, Int64>, IUsuarioDao
    {
        #region Public Constructors

        public UsuarioEntityFramework()
        {
        }

        #endregion Public Constructors

        #region IUsuarioDao Members. Specific Operations
        /// <exception cref="InstanceNotFoundException"/>
        public Usuario findUserByName(String username)
        {
            DbSet<Usuario> usuarios = Context.Set<Usuario>();
            Usuario user = null;

            var result =
                (from u in usuarios
                 where u.email == username
                 select u);
            user = result.FirstOrDefault();
            if (user == null)
                throw new InstanceNotFoundException(username,
                        typeof(Usuario).FullName);


            return user;
        }

        public bool updateInformacion(long userId, string nombre, string apellido1, string apellido2, string contraseña, string email, string language, string country)
        {
            Usuario u = Find(userId);
            if (u != null)
            {
                u.nombre = nombre;

                u.apellido1 = apellido1;

                u.apellido2 = apellido2;

                u.contraseña = contraseña;

                u.email = email;

                u.pais = country;

                u.idioma = language;


                Update(u);

                return true;
            }
            return false;
        }

        #endregion IUsuarioDao Members. Specific Operations
    }
}