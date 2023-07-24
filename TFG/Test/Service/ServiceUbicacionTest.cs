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
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Service.Util;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceUbicacionTest
    {
        private static IKernel kernel;
        private static IServiceUbicacion servicio;

        private static IUbicacionDao ubicacionDao;

        private const long codigoPostal = 15000;
        private const string localidad = "localidad";
        private const string calle = "calle";
        private const string portal = "portal";
        private const long numero = 1;



        private UbicacionProfileDetails ubicacionDetails = new UbicacionProfileDetails(codigoPostal, localidad, calle, portal, numero);

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
            servicio = kernel.Get<IServiceUbicacion>();
            ubicacionDao = kernel.Get<IUbicacionDao>();

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
        public void crearUbicacionTest()
        {
            using (var scope = new TransactionScope())
            {
                var ubicacionId = servicio.crearUbicacion(ubicacionDetails);

                var ubicacionProfile = ubicacionDao.Find(ubicacionId);


                Assert.AreEqual(ubicacionId, ubicacionProfile.ubicacionId);
                Assert.AreEqual(codigoPostal, ubicacionProfile.codigoPostal);
                Assert.AreEqual(localidad, ubicacionProfile.localidad);
                Assert.AreEqual(calle, ubicacionProfile.calle);
                Assert.AreEqual(portal, ubicacionProfile.portal);
                Assert.AreEqual(numero, ubicacionProfile.numero);
                
            }
        }

            [TestMethod()]
        public void modificarUbicacionTest()
        {
            using (var scope = new TransactionScope())
            {

                Ubicacion u = new Ubicacion();
                u.codigoPostal = 15401;
                u.localidad = "Ferrol";
                u.calle = "Real";
                u.portal = "D";
                u.numero = 2;

                ubicacionDao.Create(u);

                Ubicacion u2 = new Ubicacion();
                u2.codigoPostal = 15009;
                u2.localidad = "Coruña";
                u2.calle = "Real";
                u2.portal = "B";
                u2.numero = 2;

                ubicacionDao.Create(u2);

                servicio.modificarUbicacion(u.ubicacionId, new UbicacionProfileDetails(  u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero));

                var obtained =
                    ubicacionDao.Find(u.ubicacionId);

                // Check changes
                Assert.AreEqual(u, obtained);
            }
        }
/*
        [TestMethod()]
        public void logearUsuarioTest()
        {
            var userId = servicio.registrarUsuario( clearPassword, userDetails);
            //        public LoginResult(long userId, String nombre, String apellido1, String apellido2, String passEncriptada, String email, string language, string country)

            var expected = new LoginResult(userId, nombre, apellido1, apellido2, PasswordEncrypter.Crypt(clearPassword), email,language, country);

            var actual =
                  servicio.logearUsuario(email, clearPassword, false);

            Assert.AreEqual(expected, actual);

        }*/
        
    }
}
