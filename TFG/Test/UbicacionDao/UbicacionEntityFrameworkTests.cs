using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ninject;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using System.Collections.Generic;

namespace Es.Udc.DotNet.TFG.Model.UbicacionDao.Tests
{
    [TestClass()]
    public class UbicacionEntityFrameworkTests
    {
        private static IKernel kernel;
        private static IUbicacionDao ubicacionDao;
        private static IUsuarioDao usuarioDao;
        private static IBateriaDao bateriaDao;

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
        public void updateInformacionTest()
        {

            Ubicacion u0 = new Ubicacion();
            u0.codigoPostal = 15405;
            u0.localidad = "Ferrol";
            u0.calle = "calle de Ferrol";
            //u0.portal = "B";
            u0.numero = 1;
            ubicacionDao.Create(u0);

            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            u.portal = "B";
            u.numero = 1;
            u.etiqueta = "buhardilla";
            ubicacionDao.Create(u);


            Ubicacion u2 = new Ubicacion();

            u2.localidad = "A Coruña";
            u2.codigoPostal = 15005;
            u2.calle = "calle de Ferrol";
            u2.portal = "B";
            u2.numero = 2;
            u2.etiqueta = "Trastero";
            ubicacionDao.Create(u);

            ubicacionDao.updateInformacion(u.ubicacionId, u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero, u2.etiqueta);

            Ubicacion ubicacionActualizada = ubicacionDao.Find(u.ubicacionId);
            Assert.AreEqual(u2.localidad, ubicacionActualizada.localidad);
            Assert.AreEqual(u2.codigoPostal, ubicacionActualizada.codigoPostal);
            Assert.AreEqual(u2.calle, ubicacionActualizada.calle);
            Assert.AreEqual(u2.portal, ubicacionActualizada.portal);
            Assert.AreEqual(u2.numero, ubicacionActualizada.numero);

            //probamos los valores nulos
            u2.portal = null;
            u2.etiqueta = null;

            ubicacionDao.updateInformacion(u.ubicacionId, u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero, u2.etiqueta);

            ubicacionActualizada = ubicacionDao.Find(u.ubicacionId);
            Assert.AreEqual(u2.localidad, ubicacionActualizada.localidad);
            Assert.AreEqual(u2.codigoPostal, ubicacionActualizada.codigoPostal);
            Assert.AreEqual(u2.calle, ubicacionActualizada.calle);
            Assert.AreEqual(u2.portal, ubicacionActualizada.portal);
            Assert.AreEqual(u2.numero, ubicacionActualizada.numero);

        }

        [TestMethod()]
        public void findUbicacion()
        {
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15000;
            u.localidad = "Coruña";
            u.calle = "Calle";
            u.portal = "B";
            u.numero = 120;
            
            ubicacionDao.Create(u);

            Assert.AreEqual(u, ubicacionDao.findUbicacionExistente(u.codigoPostal, u.localidad, u.calle, u.portal, u.numero));

        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void findUbicacion2()
        {
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15000;
            u.localidad = "Coruña";
            u.calle = "Calle";
            u.portal = "B";
            u.numero = 120;

            Ubicacion u2 = new Ubicacion();
            u2.codigoPostal = 15000;
            u2.localidad = "Coruña";
            u2.calle = "Calle";
            u2.portal = "B";
            u2.numero = 121;

            ubicacionDao.Create(u);
            

            Assert.AreEqual(null, ubicacionDao.findUbicacionExistente(u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero));

        }

        [TestMethod()]
        public void ubicacionesUsuario()
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
            b.almacenajeMaximoKwH= 1000;
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
            b2.almacenajeMaximoKwH= 2000;
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
            b3.almacenajeMaximoKwH= 2000;
            b3.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            b3.marca = "MARCA 2";
            b3.modelo = "MODELO 2";
            b3.ratioCarga = 20;
            b3.ratioCompra = 20;
            b3.ratioUso = 20;
            bateriaDao.Create(b3);

            int count = 2;
            int startOfIndex = 0;

            //COMPROBAMOS
            List<Ubicacion> bat1 = ubicacionDao.ubicacionesUsuario(user.usuarioId, startOfIndex, count);
            List<Ubicacion> bat2 = ubicacionDao.ubicacionesUsuario(user2.usuarioId, startOfIndex, count);


            Assert.AreEqual(bat1[0], u);
            Assert.AreEqual(bat1.Count, 1);
            Assert.AreEqual(bat2[0], u2);
            Assert.AreEqual(bat2.Count, 1);

        }

        [TestMethod()]
        public void ubicacionesUsuarioSinUbicaciones()
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
            b.almacenajeMaximoKwH= 1000;
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
            b2.almacenajeMaximoKwH= 2000;
            b2.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            b2.marca = "MARCA 2";
            b2.modelo = "MODELO 2";
            b2.ratioCarga = 20;
            b2.ratioCompra = 20;
            b2.ratioUso = 20;
            bateriaDao.Create(b2);

            int count = 2;
            int startOfIndex = 0;

            //COMPROBAMOS
            List<Ubicacion> bat3 = ubicacionDao.ubicacionesUsuario(user3.usuarioId, startOfIndex, count);

            Assert.AreEqual(bat3.Count, 0);

        }
    }
}