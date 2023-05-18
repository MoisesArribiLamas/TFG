using Microsoft.VisualStudio.TestTools.UnitTesting;
using Es.Udc.DotNet.PracticaMaD.Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.PracticaMaD.Test;
using Es.Udc.DotNet.PracticaMaD.Model.PedidoDao;
using Es.Udc.DotNet.PracticaMaD.Model.ProductoDao;
using Es.Udc.DotNet.PracticaMaD.Model.StockDao;
using Es.Udc.DotNet.PracticaMaD.Model.CategoriaDao;
using Es.Udc.DotNet.PracticaMaD.Model.LibroDao;
using Es.Udc.DotNet.PracticaMaD.Model.Tarjeta_creditoDao;
using Es.Udc.DotNet.PracticaMaD.Model.UsuarioDao;
using Es.Udc.DotNet.PracticaMaD.Model.Service.Util;
using Es.Udc.DotNet.PracticaMaD.Model.Linea_pedidoDao;
using Es.Udc.DotNet.PracticaMaD.Model.ComentariosDao;
using Es.Udc.DotNet.PracticaMaD.Model.EtiquetasDao;
using Es.Udc.DotNet.PracticaMaD.Model;

namespace Es.Udc.DotNet.PracticaMaD.Model.Service.Tests
{
    [TestClass()]
    public class ServiceUsuarioTest
    {
        private static IKernel kernel;
        private static IServiceUsuario servicio;


        private static IPedidoDao pedidoDao;
        private static IProductoDao productoDao;
        private static IStockDao stockDao;
        private static ICategoriaDao categoriaDao;
        private static ILibroDao libroDao;
        private static ITarjeta_creditoDao tarjeta_CreditoDao;
        private static IUsuarioDao usuarioDao;
        private static ILinea_pedidoDao linea_PedidoDao;
        private static IComentarioDao comentarioDao;
        private static IEtiquetaDao etiquetaDao;


        public const string clearPassword = "password";
        public const string firstName = "name";
        private const string lastName = "lastName";
        private const string email = "user@udc.es";
        private const string codigoPostal = "direccion";
        private const string language = "es-ES";
        private const string country = "Spain";
        private const string tipo_usuario = "default";

        private const long NON_EXISTENT_USER_ID = -1;
        private UserProfileDetails userDetails = new UserProfileDetails(email, firstName, lastName, codigoPostal, language, country, tipo_usuario);
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
            pedidoDao = kernel.Get<IPedidoDao>();
            stockDao = kernel.Get<IStockDao>();
            productoDao = kernel.Get<IProductoDao>();
            categoriaDao = kernel.Get<ICategoriaDao>();
            libroDao = kernel.Get<ILibroDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();
            tarjeta_CreditoDao = kernel.Get<ITarjeta_creditoDao>();
            linea_PedidoDao = kernel.Get<ILinea_pedidoDao>();
            comentarioDao = kernel.Get<IComentarioDao>();
            etiquetaDao = kernel.Get<IEtiquetaDao>();

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


                Assert.AreEqual(userId, userProfile.id_usuario);
                Assert.AreEqual(email, userProfile.email);
                Assert.AreEqual(PasswordEncrypter.Crypt(clearPassword), userProfile.contraseña);
                Assert.AreEqual(firstName, userProfile.nombre);
                Assert.AreEqual(lastName, userProfile.apellidos);
                Assert.AreEqual(email, userProfile.email);
                Assert.AreEqual(codigoPostal, userProfile.codigo_postal);

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

                usuario user = new usuario();
                user.nombre = "Dani";
                user.tipo_usuario = "admin";
                user.email = "micorreo@gmail.com";
                user.apellidos = "Díaz Glez";
                user.contraseña = "unacontraseña";


                user.codigo_postal = "direccion";

                usuarioDao.Create(user);

                usuario user2 = new usuario();
                user2.nombre = "Eloy";
                user2.tipo_usuario = "admin";
                user2.email = "micorreo2@gmail.com";
                user2.apellidos = "Lago Graña";
                user2.contraseña = "unacontraseña2";
                user2.codigo_postal = "direccion";
                usuarioDao.Create(user2);

                servicio.modificarUsuario(user.id_usuario, new UserProfileDetails(  user2.email, user2.nombre, user2.apellidos,  user2.codigo_postal,  language, country, "default"));

                var obtained =
                    usuarioDao.findUserByName(user.email);

                // Check changes
                Assert.AreEqual(user, obtained);
            }
        }

        [TestMethod()]
        public void anhadirTarjetaTest()
        {
            usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";


            user.codigo_postal = "direccion";

            usuarioDao.Create(user);

            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "8654321";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = false;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";

            servicio.anhadirTarjeta(tarjeta.numero, tarjeta.cvv, tarjeta.tipo, tarjeta.fecha,false, user.id_usuario);

            Assert.AreEqual(tarjeta, user.tarjeta_credito.ElementAt(0));

        }


        [TestMethod()]
        public void modificarTarjetaTest()
        {
            var userId = servicio.registrarUsuario(clearPassword, userDetails);

            var expected = new LoginResult(userId, firstName, lastName, PasswordEncrypter.Crypt(clearPassword), email, codigoPostal,"default", language, country);
           var actual =
                  servicio.logearUsuario(email, clearPassword, false);

            //Creamos user
            usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";
            user.codigo_postal = "direccion";
            usuarioDao.Create(user);
            //Creamos tarjeta
            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "1234568";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = false;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta);
            //Tarjeta 2
            tarjeta_credito tarjeta2 = new tarjeta_credito();
            tarjeta2.numero = "6543210";
            tarjeta2.fecha = DateTime.Now;
            tarjeta2.por_defecto = true;
            tarjeta2.pertenece_a = user.id_usuario;
            tarjeta2.cvv = "321";
            tarjeta2.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta2);


            servicio.modificarTarjeta(tarjeta.numero);
            Assert.AreEqual(true, tarjeta.por_defecto);
        }


        [TestMethod()]
        public void modificarTarjetaInicialTest()
        {
            var userId = servicio.registrarUsuario(clearPassword, userDetails);

            var expected = new LoginResult(userId, firstName, lastName, PasswordEncrypter.Crypt(clearPassword), email, codigoPostal, "default", language, country);

            var actual =
                  servicio.logearUsuario(email, clearPassword, false);

            //Creamos user
            usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";
            user.codigo_postal = "direccion";
            usuarioDao.Create(user);
            //Creamos tarjeta
            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "1134568";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = false;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta);
           

            servicio.modificarTarjeta(tarjeta.numero);
            Assert.AreEqual(true, tarjeta.por_defecto);
        }

        [TestMethod()]
        public void logearUsuarioTest()
        {
            var userId = servicio.registrarUsuario( clearPassword, userDetails);

            var expected = new LoginResult(userId, firstName, lastName, PasswordEncrypter.Crypt(clearPassword), email, codigoPostal, "default",language, country);

            var actual =
                  servicio.logearUsuario(email, clearPassword, false);

            Assert.AreEqual(expected, actual);

        }
        
    }
}
