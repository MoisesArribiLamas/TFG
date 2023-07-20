using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;

namespace Es.Udc.DotNet.TFG.Model.Daos.BateriaDao.Tests
{
    [TestClass()]
    public class BateriaDaoEntityFrameworkTests
    {

        private static IKernel kernel;
        private static IBateriaDao bateriaDao;
        private static IUbicacionDao ubicacionDao;
        private static IUsuarioDao usuarioDao;


        // Variables used in several tests are initialized here

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
            bateriaDao = kernel.Get<IBateriaDao>();
            ubicacionDao = kernel.Get<IUbicacionDao>();
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
        public void findBateriaTest()
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
            user2.usuarioId = user.usuarioId;
            user2.nombre = "María";
            user2.contraseña = "nos olvidamos ups";
            user2.email = "micorreo@gmail.com";
            user2.apellido1 = "Pérez";
            user2.apellido2 = "Fernández";
            user2.telefono = "981123457";
            user2.idioma = "es-ES";
            user2.pais = "España";
            usuarioDao.Create(user2);

            //CREAMOS LA BATERIA
            Bateria b = new Bateria();
            b.ubicacionId = u.ubicacionId;
            b.usuarioId = user.usuarioId ;
            b.precioMedio = 111 ;
            b.kwAlmacenados = 1000 ;
            b.almacenajeMaximoKw = 1000 ;
            b.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            b.marca = "MARCA 1" ;
            b.modelo = "MODELO 1" ;
            b.ratioCarga = 10 ;
            b.ratioCompra = 10 ;
            b.ratioUso = 10;
            bateriaDao.Create(b);

            Bateria b2 = new Bateria();
            b2.ubicacionId = u2.ubicacionId;
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



            //COMPROBAMOS
            bateriaDao.updateInformacion(b.bateriaId, b2.ubicacionId, b2.usuarioId, b2.precioMedio, b2.kwAlmacenados, b2.almacenajeMaximoKw,
                b2.fechaDeAdquisicion, b2.marca, b2.modelo, b2.ratioCarga, b2.ratioCompra, b2.ratioUso);

            Bateria b1 = bateriaDao.Find(b.bateriaId);

            Assert.AreEqual(b2.ubicacionId, b1.ubicacionId);
            Assert.AreEqual(b2.usuarioId, b1.usuarioId);
            Assert.AreEqual(b2.precioMedio, b1.precioMedio);
            Assert.AreEqual(b2.kwAlmacenados, b1.kwAlmacenados);
            Assert.AreEqual(b2.almacenajeMaximoKw, b1.almacenajeMaximoKw);
            Assert.AreEqual(b2.fechaDeAdquisicion, b1.fechaDeAdquisicion);
            Assert.AreEqual(b2.marca, b1.marca);
            Assert.AreEqual(b2.modelo, b1.modelo);
            Assert.AreEqual(b2.ratioCarga, b1.ratioCarga);
            Assert.AreEqual(b2.ratioCompra, b1.ratioCompra);
            Assert.AreEqual(b2.ratioUso, b1.ratioUso);

        }


        //----------------------------------------------------------

        public void findBateriaByUserTest()
        {
            // CREAMOS UBICACIONES
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            u.portal = "a";
            u.numero = 1;
            ubicacionDao.Create(u);


            Ubicacion u2 = new Ubicacion();

            u2.localidad = "A Coruña";
            u2.codigoPostal = 15005;
            u2.calle = "calle de Coruña";
            u2.portal = "B";
            u2.numero = 2;
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
            user2.usuarioId = user.usuarioId;
            user2.nombre = "María";
            user2.contraseña = "nos olvidamos ups";
            user2.email = "micorreo@gmail.com";
            user2.apellido1 = "Pérez";
            user2.apellido2 = "Fernández";
            user2.telefono = "981123457";
            user2.idioma = "es-ES";
            user2.pais = "España";
            usuarioDao.Create(user2);

            //CREAMOS LA BATERIA
            Bateria b = new Bateria();
            b.ubicacionId = u.ubicacionId;
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
            b2.ubicacionId = u2.ubicacionId;
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

            Bateria b3 = new Bateria();
            b3.ubicacionId = u2.ubicacionId;
            b3.usuarioId = user2.usuarioId;
            b3.precioMedio = 333;
            b3.kwAlmacenados = 3000;
            b3.almacenajeMaximoKw = 3000;
            b3.fechaDeAdquisicion = new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.Month, DateTime.Now.Day);
            b3.marca = "MARCA 3";
            b3.modelo = "MODELO 3";
            b3.ratioCarga = 30;
            b3.ratioCompra = 30;
            b3.ratioUso = 30;
            bateriaDao.Create(b3);

            int count = 2;
            int startOfIndex = 0;


            //COMPROBAMOS
            List<Bateria> bat1 = bateriaDao.findBateriaByUser(user.usuarioId, startOfIndex, count);
            List<Bateria> bat2 = bateriaDao.findBateriaByUser(user2.usuarioId, startOfIndex, count);


            Assert.AreEqual(bat1[0], b);
            Assert.AreEqual(bat2[0], b2);
            Assert.AreEqual(bat2[1], b3);
        }

        public void findBateriaByUbicacionTest()
        {
            // CREAMOS UBICACIONES
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            //u.portal = "A";
            u.numero = 1;
            ubicacionDao.Create(u);


            Ubicacion u2 = new Ubicacion();

            u2.localidad = "A Coruña";
            u2.codigoPostal = 15005;
            u2.calle = "calle de Coruña";
            //u2.portal = "B";
            u2.numero = 2;
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
            user2.usuarioId = user.usuarioId;
            user2.nombre = "María";
            user2.contraseña = "nos olvidamos ups";
            user2.email = "micorreo@gmail.com";
            user2.apellido1 = "Pérez";
            user2.apellido2 = "Fernández";
            user2.telefono = "981123457";
            user2.idioma = "es-ES";
            user2.pais = "España";
            usuarioDao.Create(user2);

            //CREAMOS LA BATERIA
            Bateria b = new Bateria();
            b.ubicacionId = u.ubicacionId;
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
            b2.ubicacionId = u2.ubicacionId;
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

            Bateria b3 = new Bateria();
            b3.ubicacionId = u2.ubicacionId;
            b3.usuarioId = user2.usuarioId;
            b3.precioMedio = 333;
            b3.kwAlmacenados = 3000;
            b3.almacenajeMaximoKw = 3000;
            b3.fechaDeAdquisicion = new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.Month, DateTime.Now.Day);
            b3.marca = "MARCA 3";
            b3.modelo = "MODELO 3";
            b3.ratioCarga = 30;
            b3.ratioCompra = 30;
            b3.ratioUso = 30;
            bateriaDao.Create(b3);

            int count = 2;
            int startOfIndex = 0;


            //COMPROBAMOS
            List<Bateria> bat1 = bateriaDao.findBateriaByUbicacion(user.usuarioId, startOfIndex, count);
            List<Bateria> bat2 = bateriaDao.findBateriaByUbicacion(user2.usuarioId, startOfIndex, count);

            Assert.AreEqual(bat1[0], b);
            Assert.AreEqual(bat2[0], b2);
            Assert.AreEqual(bat2[1], b3);
        }
    }
}
