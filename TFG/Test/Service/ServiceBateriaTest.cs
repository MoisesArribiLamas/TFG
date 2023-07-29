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
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Es.Udc.DotNet.TFG.Model.Service.Util;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceBateriaTest
    {
        private static IKernel kernel;
        private static IServiceBateria servicio;

        private static IBateriaDao bateriaDao;
        private static IUsuarioDao usuarioDao;


        //Usuario

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

        //Ubicacion

        private const long codigoPostal = 15000;
        private const string localidad = "localidad";
        private const string calle = "calle";
        private const string portal = "portal";
        private const long numero = 1;
/*
        private UbicacionProfileDetails ubicacionDetails = new UbicacionProfileDetails(codigoPostal, localidad, calle, portal, numero);

        // bateria

        private BateriaDTO userDetails = new BateriaDTO(long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
           DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso)



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
            servicio = kernel.Get<IServiceBateria>();
            bateriaDao = kernel.Get<IBateriaDao>();
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
        public void crearBateriaTest()
        {
            using (var scope = new TransactionScope())
            {
                var bateriaId = servicio.crearBateria(bateriaDetails);

                var bateriaProfile = bateriaDao.Find(bateriaId);


                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(codigoPostal, bateriaProfile.codigoPostal);
                Assert.AreEqual(localidad, bateriaProfile.localidad);
                Assert.AreEqual(calle, bateriaProfile.calle);
                Assert.AreEqual(portal, bateriaProfile.portal);
                Assert.AreEqual(numero, bateriaProfile.numero);
                
            }
        }

            [TestMethod()]
        public void modificarBateriaTest()
        {
            using (var scope = new TransactionScope())
            {

                Bateria u = new Bateria();
                u.codigoPostal = 15401;
                u.localidad = "Ferrol";
                u.calle = "Real";
                u.portal = "D";
                u.numero = 2;

                bateriaDao.Create(u);

                Bateria u2 = new Bateria();
                u2.codigoPostal = 15009;
                u2.localidad = "Coruña";
                u2.calle = "Real";
                u2.portal = "B";
                u2.numero = 2;

                bateriaDao.Create(u2);

                servicio.modificarBateria(u.bateriaId, new BateriaProfileDetails(  u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero));

                var obtained =
                    bateriaDao.Find(u.bateriaId);

                // Check changes
                Assert.AreEqual(u, obtained);
            }
        }

        [TestMethod()]
        public void verBateriaesDeUnUsuarioTest()
        {
            using (var scope = new TransactionScope())
            {

                // CREAMOS UBICACIONES
                Bateria u = new Bateria();
                u.codigoPostal = 15405;
                u.localidad = "Ferrol";
                u.calle = "calle de Ferrol";
                u.portal = "A";
                u.numero = 1;
                bateriaDao.Create(u);


                Bateria u2 = new Bateria();
                u2.localidad = "A Coruña";
                u2.codigoPostal = 15005;
                u2.calle = "calle de Coruña";
                u2.portal = "B";
                u2.numero = 1;
                bateriaDao.Create(u2);

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
                b.bateriaId = u.bateriaId;
                b.usuarioId = user.usuarioId;
                b.precioMedio = 111;
                b.kwAlmacenados = 1000;
                b.almacenajeMaximoKw = 1000;
                b.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b.marca = "MARCA 1";
                b.modelo = "MODELO 1";
                b.ratioCarga = 10;
                b.ratioCompra = 10;
                b.ratioUso = 10;
                bateriaDao.Create(b);

                Bateria b2 = new Bateria();
                b2.bateriaId = u2.bateriaId;
                b2.usuarioId = user2.usuarioId;
                b2.precioMedio = 222;
                b2.kwAlmacenados = 2000;
                b2.almacenajeMaximoKw = 2000;
                b2.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b2.marca = "MARCA 2";
                b2.modelo = "MODELO 2";
                b2.ratioCarga = 20;
                b2.ratioCompra = 20;
                b2.ratioUso = 20;
                bateriaDao.Create(b2);

                //MISMA UBICACION Y MISMO USUARIO QUE LA B2
                Bateria b3 = new Bateria();
                b3.bateriaId = u2.bateriaId;
                b3.usuarioId = user2.usuarioId;
                b3.precioMedio = 222;
                b3.kwAlmacenados = 2000;
                b3.almacenajeMaximoKw = 2000;
                b3.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b3.marca = "MARCA 2";
                b3.modelo = "MODELO 2";
                b3.ratioCarga = 20;
                b3.ratioCompra = 20;
                b3.ratioUso = 20;
                bateriaDao.Create(b3);

                int count = 2;
                int startOfIndex = 0;



                List<BateriaProfileDetails> obteined = servicio.verBateriaes(user.usuarioId, startOfIndex, count);
                List<BateriaDTO> obteined2 = servicio.verUbicaciones(user2.usuarioId, startOfIndex, count);



                BateriaDTO o1 = new BateriaDTO(u.codigoPostal, u.localidad, u.calle, u.portal, u.numero);
                BateriaDTO o2 = new BateriaDTO(u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero);


                //COMPROBAMOS


                Assert.AreEqual(obteined[0], o1);
                Assert.AreEqual(obteined.Count, 1);
                Assert.AreEqual(obteined2[0], o2);
                Assert.AreEqual(obteined2.Count, 1);
            }
        }*/
    }
}
