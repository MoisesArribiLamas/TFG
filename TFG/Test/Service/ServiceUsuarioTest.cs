using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Service.Util;
using Es.Udc.DotNet.TFG.Model;

namespace Es.Udc.DotNet.PracticaMaD.Model.Service.Tests
{
    [TestClass()]
    public class ServiceUsuarioTest
    {
        private static IKernel kernel;
        private static IServiceUsuario servicio;

        private static IUsuarioDao usuarioDao;


        public const string clearPassword = "password";
        public const string nombre = "name";
        private const string apellido1 = "lastName";
        private const string apellido2 = "lastName";
        private const string email = "user@udc.es";
        private const string telefono = "123456789";
        private const string language = "es-ES";
        private const string country = "Spain";

        private const long NON_EXISTENT_USER_ID = -1;

        private UserProfileDetails userDetails = new UserProfileDetails(email, nombre, apellido1, apellido2, telefono, language, country);
        // Variables used in several tests are initialized here
        private const long idLibro = 1;

        private const long NON_EXISTENT_LIBRO_ID = -2;

        private TransactionScope transactionScope;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private TestContext testContextInstance;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            kernel = TestManager.ConfigureNInjectKernel();
            servicio = kernel.Get<IServiceUsuario>();
            usuarioDao = kernel.Get<IUsuarioDao>();

        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            TestManager.ClearNInjectKernel(kernel);
        }


        [TestInitialize()]
        public void MyTestInitialize()
        {
            transactionScope = new TransactionScope();
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            transactionScope.Dispose();
        }



        [TestMethod()]
        public void registrarUsuarioTest()
        {
            using (var scope = new TransactionScope())
            {
                var userId = servicio.registrarUsuario(clearPassword, userDetails);

                var userProfile = usuarioDao.Find(userId);


                Assert.AreEqual(userId, userProfile.usuarioId);
                Assert.AreEqual(email, userProfile.email);
                Assert.AreEqual(PasswordEncrypter.Crypt(clearPassword), userProfile.contraseña);
                Assert.AreEqual(nombre, userProfile.nombre);
                Assert.AreEqual(apellido1, userProfile.apellido1);
                Assert.AreEqual(apellido2, userProfile.apellido2);
                Assert.AreEqual(telefono, userProfile.telefono);
                Assert.AreEqual(email, userProfile.email);

            }
        }

        [TestMethod()]
        public void modificarContraseñaTest()
        {
          
           string contraseña = "unacontraseña";
           string nuevaPass = "doscontraseñas";
           long id =  servicio.registrarUsuario(contraseña, userDetails);
  
           servicio.modificarContraseña(id, contraseña, nuevaPass );
           servicio.logearUsuario(userDetails.email, nuevaPass, false);


        }
            [TestMethod()]
        public void modificarUsuarioTest()
        {
            using (var scope = new TransactionScope())
            {

                Usuario user = new Usuario();
                user.nombre = "Pedro";
                user.email = "micorreo@gmail.com";
                user.apellido1 = "Alonso";
                user.apellido2 = "Díaz";
                user.telefono = "987654321";
                user.contraseña = "unacontraseña";
                user.idioma = "es-ES";
                user.pais = "Spain";


                usuarioDao.Create(user);

                Usuario user2 = new Usuario();
                user2.nombre = "Manuel";
                user2.email = "micorreo2@gmail.com";
                user2.apellido1 = "Alonso";
                user2.apellido2 = "Díaz";
                user2.telefono = "123456789";
                user2.contraseña = "unacontraseña2";
                user2.idioma = "en-GR";
                user2.pais = "England";
                usuarioDao.Create(user2);

                servicio.modificarUsuario(user.usuarioId, new UserProfileDetails(  user2.email, user2.nombre, user2.apellido1, user2.apellido2, telefono,  language, country));

                var obtained =
                    usuarioDao.findUserByName(user.email);

                // Check changes
                Assert.AreEqual(user, obtained);
            }
        }

        [TestMethod()]
        public void logearUsuarioTest()
        {
            var userId = servicio.registrarUsuario( clearPassword, userDetails);
            //        public LoginResult(long userId, String nombre, String apellido1, String apellido2, String passEncriptada, String email, string language, string country)

            var expected = new LoginResult(userId, nombre, apellido1, apellido2, PasswordEncrypter.Crypt(clearPassword), email,language, country);

            var actual =
                  servicio.logearUsuario(email, clearPassword, false);

            Assert.AreEqual(expected, actual);

        }
        
    }
}
