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
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceUbicacionTest
    {
        private static IKernel kernel;
        private static IServiceUbicacion servicio;

        private static IUbicacionDao ubicacionDao;
        private static IUsuarioDao usuarioDao;
        private static IBateriaDao bateriaDao;

        private const long codigoPostal = 15000;
        private const string localidad = "localidad";
        private const string calle = "calle";
        private const string portal = "portal";
        private const long numero = 1;
        private const string etiqueta = "Trastero";



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
            usuarioDao = kernel.Get<IUsuarioDao>();
            bateriaDao = kernel.Get<IBateriaDao>();

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
                var ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);

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
                u.etiqueta = "bateria principal";

                ubicacionDao.Create(u);

                Ubicacion u2 = new Ubicacion();
                u2.codigoPostal = 15009;
                u2.localidad = "Coruña";
                u2.calle = "Real";
                u2.portal = "B";
                u2.numero = 2;
                u2.etiqueta = "bateria auxiliar";

                ubicacionDao.Create(u2);

                servicio.modificarUbicacion(u.ubicacionId, u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero, u2.etiqueta);

                var obtained =
                    ubicacionDao.Find(u.ubicacionId);

                // Check changes
                Assert.AreEqual(u, obtained);
            }
        }


        [TestMethod()]
        public void CambiarBateriaSuministradoraTest()
        {
            using (var scope = new TransactionScope())
            {

                Ubicacion u = new Ubicacion();
                u.codigoPostal = 15401;
                u.localidad = "Ferrol";
                u.calle = "Real";
                u.portal = "D";
                u.numero = 2;
                u.etiqueta = "bateria principal";

                ubicacionDao.Create(u);

                //comprobamos que no tiene ninguna bateria suministrando
                Assert.AreEqual(u.bateriaSuministradora, null);

                //ponemos una bateria
                long? bateriaSuministradora = 2;
                servicio.CambiarBateriaSuministradora(u.ubicacionId,  bateriaSuministradora);

                var obtained =
                    ubicacionDao.Find(u.ubicacionId);

                // Check changes
                Assert.AreEqual(u.bateriaSuministradora, 2);
            }
        }

        [TestMethod()]
        public void verUbicacionesDeUnUsuarioTest()
        {
            using (var scope = new TransactionScope())
            {

                // CREAMOS UBICACIONES
                Ubicacion u = new Ubicacion();
                u.codigoPostal = 15405;
                u.localidad = "Ferrol";
                u.calle = "calle de Ferrol";
                u.portal = "A";
                u.numero = 1;
                ubicacionDao.Create(u);


                Ubicacion u2 = new Ubicacion();
                u2.localidad = "A Coruña";
                u2.codigoPostal = 15005;
                u2.calle = "calle de Coruña";
                u2.portal = "B";
                u2.numero = 1;
                ubicacionDao.Create(u2);

                //CREAMOS LOS USUARIOS
                Usuario user = new Usuario();
                user.nombre = "Dani";
                user.email = "micorreo@gmail.com";
                user.apellido1 = "Díaz";
                user.apellido2 = "González";
                user.contraseña = "unacontraseña";
                user.telefono = "981123456";
                user.pais = "España";
                user.idioma = "es-ES";
                usuarioDao.Create(user);

                Usuario user2 = new Usuario();
                user2.nombre = "María";
                user2.contraseña = "nos olvidamos ups";
                user2.email = "micorreo@gmail.com";
                user2.apellido1 = "Pérez";
                user2.apellido2 = "Fernández";
                user2.telefono = "981123457";
                user2.idioma = "es-ES";
                user2.pais = "España";
                usuarioDao.Create(user2);

                Usuario user3 = new Usuario();
                user3.nombre = "María";
                user3.contraseña = "nos olvidamos ups";
                user3.email = "micorreo@gmail.com";
                user3.apellido1 = "Pérez";
                user3.apellido2 = "Fernández";
                user3.telefono = "981123457";
                user3.idioma = "es-ES";
                user3.pais = "España";
                usuarioDao.Create(user3);

                //CREAMOS LA BATERIA
                Bateria b = new Bateria();
                b.ubicacionId = u.ubicacionId;
                b.usuarioId = user.usuarioId;
                b.precioMedio = 111;
                b.kwHAlmacenados = 1000;
                b.almacenajeMaximoKwH = 1000;
                b.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b.marca = "MARCA 1";
                b.modelo = "MODELO 1";
                b.ratioCarga = 10;
                b.ratioCompra = 10;
                b.ratioUso = 10;
                bateriaDao.Create(b);

                Bateria b2 = new Bateria();
                b2.ubicacionId = u2.ubicacionId;
                b2.usuarioId = user2.usuarioId;
                b2.precioMedio = 222;
                b2.kwHAlmacenados = 2000;
                b2.almacenajeMaximoKwH = 2000;
                b2.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b2.marca = "MARCA 2";
                b2.modelo = "MODELO 2";
                b2.ratioCarga = 20;
                b2.ratioCompra = 20;
                b2.ratioUso = 20;
                bateriaDao.Create(b2);

                //MISMA UBICACION Y MISMO USUARIO QUE LA B2
                Bateria b3 = new Bateria();
                b3.ubicacionId = u2.ubicacionId;
                b3.usuarioId = user2.usuarioId;
                b3.precioMedio = 222;
                b3.kwHAlmacenados = 2000;
                b3.almacenajeMaximoKwH = 2000;
                b3.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b3.marca = "MARCA 2";
                b3.modelo = "MODELO 2";
                b3.ratioCarga = 20;
                b3.ratioCompra = 20;
                b3.ratioUso = 20;
                bateriaDao.Create(b3);

                int count = 2;
                int startOfIndex = 0;



                List<UbicacionProfileDetails> obteined = servicio.verUbicaciones(user.usuarioId, startOfIndex, count);
                List<UbicacionProfileDetails> obteined2 = servicio.verUbicaciones(user2.usuarioId, startOfIndex, count);



                UbicacionProfileDetails o1 = new UbicacionProfileDetails(u.ubicacionId, u.codigoPostal, u.localidad, u.calle, u.portal, u.numero);
                UbicacionProfileDetails o2 = new UbicacionProfileDetails(u2.ubicacionId, u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero);


                //COMPROBAMOS


                Assert.AreEqual(obteined[0], o1);
                Assert.AreEqual(obteined.Count, 1);
                Assert.AreEqual(obteined2[0], o2);
                Assert.AreEqual(obteined2.Count, 1);
            }
        }
    }
}
