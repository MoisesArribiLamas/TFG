using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Service.Exceptions;
using Es.Udc.DotNet.TFG.Model.Service.Util;

using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class ServiceUsuario : IServiceUsuario
    {

        [Inject]
        public IUsuarioDao UsuarioDao { private get; set; }



        public void modificarContraseña(long userId, string viejaPass,
           string nuevaPass)
        {
            Usuario user = UsuarioDao.Find(userId);
            String storedPassword = user.contraseña;

            if (!PasswordEncrypter.IsClearPasswordCorrect(viejaPass,
                 storedPassword))
            {
                throw new IncorrectPasswordException(user.nombre);
            }

            user.contraseña =
            PasswordEncrypter.Crypt(nuevaPass);

            UsuarioDao.Update(user);
        }
        [Transactional]
        public long registrarUsuario( string clearPassword, UserProfileDetails userProfileDetails)
        {
            try
            {
                UsuarioDao.findUserByName(userProfileDetails.Email);

                throw new DuplicateInstanceException(userProfileDetails.Email,
                    typeof(Usuario).FullName);
            }
            catch (InstanceNotFoundException)
            {
                String encryptedPassword = PasswordEncrypter.Crypt(clearPassword);

                Usuario user = new Usuario();

                user.nombre = userProfileDetails.Nombre;
                user.apellido1 = userProfileDetails.Apellido1;
                user.apellido2 = userProfileDetails.Apellido2;
                user.contraseña = encryptedPassword;
                user.email = userProfileDetails.Email;
                user.telefono = userProfileDetails.Telefono;
                user.idioma = userProfileDetails.Language;
                user.pais = userProfileDetails.Country;

                UsuarioDao.Create(user);
                return user.usuarioId;
            }

        }

        [Transactional]
        public void modificarUsuario(long usuarioId, UserProfileDetails userProfileDetails)
        {

            Usuario user = UsuarioDao.Find(usuarioId);
            /*String contrasenaGuardada = user.contraseña;

            if (!PasswordEncrypter.IsClearPasswordCorrect(contrasena,
               contrasenaGuardada))
            {
                throw new IncorrectPasswordException(user.nombre);
            }

            user.contraseña = PasswordEncrypter.Crypt(contrasena);
            */
            user.email = userProfileDetails.Email;
            user.nombre = userProfileDetails.Nombre;
            user.apellido1 = userProfileDetails.Apellido1;
            user.apellido2 = userProfileDetails.Apellido2;
            user.email = userProfileDetails.Email;
            user.telefono = userProfileDetails.Telefono;
            user.idioma = userProfileDetails.Language;
            user.pais = userProfileDetails.Country;
            UsuarioDao.Update(user);
        }



        [Transactional]
        public LoginResult logearUsuario(string email, string password, bool passwordIsEncrypted)
        {
            Usuario usuario = UsuarioDao.findUserByName(email);

            String storedPassword = usuario.contraseña;

            if (passwordIsEncrypted)
            {

                if (!password.Equals(storedPassword))
                {
                    throw new IncorrectPasswordException(email);
                }
            }
            else
            {
                if (!PasswordEncrypter.IsClearPasswordCorrect(password,
                        storedPassword))
                {
                    throw new IncorrectPasswordException(email);
                }
            }


            return new LoginResult(usuario.usuarioId, usuario.nombre, usuario.apellido1, usuario.apellido2,
                    storedPassword, usuario.email, usuario.idioma, usuario.pais);

        }
    
    }
}
