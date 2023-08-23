using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using System.Linq;

namespace Es.Udc.DotNet.TFG.Model.Daos.CargaDao.Tests
{
    [TestClass()]
    public class CargaDaoEntityFrameworkTests
    {

        private static IKernel kernel;
        private static IBateriaDao bateriaDao;
        private static IUbicacionDao ubicacionDao;
        private static IUsuarioDao usuarioDao;
        private static ICargaDao cargaDao;
        private static ITarifaDao tarifaDao;


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
            cargaDao = kernel.Get<ICargaDao>();
            tarifaDao = kernel.Get<ITarifaDao>();
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
        public void getInfoCargaTest()
        {
            // CREAMOS UBICACION
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            u.portal = "B";
            u.numero = 1;
            ubicacionDao.Create(u);


            //CREAMOS LOS USUARIO
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

            //CREAMOS LA TARIFA
            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);

            //CREAMOS CARGAS
            Carga c = new Carga();
            c.kws = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            cargaDao.Create(c);

            Carga c2 = new Carga();
            c2.kws = 1000;
            c2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            c2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            c2.tarifaId = t.tarifaId;
            c2.bateriaId = b.bateriaId;
            cargaDao.Create(c2);


            //COMPROBAMOS
            Carga cargaResult = cargaDao.getInfoCarga(c.cargaId);
            Carga cargaResult2 = cargaDao.getInfoCarga(c2.cargaId);

            Assert.AreEqual(cargaResult, c);
            Assert.AreEqual(cargaResult2, c2);

        }

        [TestMethod()]
        public void MostrarCargasBareriaPorFechaTest()
        {
            // CREAMOS UBICACION
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            u.portal = "B";
            u.numero = 1;
            ubicacionDao.Create(u);


            //CREAMOS LOS USUARIO
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


            //CREAMOS LAS BATERIAS
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
            b2.ubicacionId = u.ubicacionId;
            b2.usuarioId = user.usuarioId;
            b2.precioMedio = 111;
            b2.kwAlmacenados = 1000;
            b2.almacenajeMaximoKw = 1000;
            b2.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            b2.marca = "MARCA 1";
            b2.modelo = "MODELO 1";
            b2.ratioCarga = 10;
            b2.ratioCompra = 10;
            b2.ratioUso = 10;
            bateriaDao.Create(b2);

            //CREAMOS LAS TARIFAS
            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);

            Tarifa t2 = new Tarifa();
            t2.precio = 100;
            t2.hora = 2;
            t2.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t2);

            Tarifa t3 = new Tarifa();
            t3.precio = 100;
            t3.hora = 3;
            t3.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(5).Day);
            tarifaDao.Create(t3);

            //CREAMOS CARGAS
            Carga c = new Carga();
            c.kws = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            cargaDao.Create(c);

            Carga c2 = new Carga();
            c2.kws = 2000;
            c2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            c2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            c2.tarifaId = t2.tarifaId;
            c2.bateriaId = b.bateriaId;
            cargaDao.Create(c2);

            Carga c3 = new Carga();
            c3.kws = 3000;
            c3.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c3.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c3.tarifaId = t2.tarifaId;
            c3.bateriaId = b2.bateriaId;
            cargaDao.Create(c3);

            Carga c0 = new Carga();
            c0.kws = 3000;
            c0.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c0.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c0.tarifaId = t3.tarifaId;
            c0.bateriaId = b.bateriaId;
            cargaDao.Create(c0);


            //COMPROBAMOS
            DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
            int startIndex = 0;
            int count = 3;
            List<Carga> cargaResult = cargaDao.MostrarCargasBareriaPorFecha(b.bateriaId, fecha, fecha2, startIndex, count);
            

            Assert.AreEqual(cargaResult[0], c);
            Assert.AreEqual(cargaResult[1], c2);
            Assert.AreEqual(cargaResult.Count(), 2);

        }

        [TestMethod()]
        public void FinalizarCargaTest()
        {
            // CREAMOS UBICACION
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            u.portal = "B";
            u.numero = 1;
            ubicacionDao.Create(u);


            //CREAMOS LOS USUARIO
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


            //CREAMOS LAS BATERIAS
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

            //CREAMOS LA TARIFA
            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);
            

            //CREAMOS CARGA
            Carga c = new Carga();
            c.kws = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            cargaDao.Create(c);


            //finalizamos la carga
            TimeSpan horaFin2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            double kws = 2000;
            cargaDao.FinalizarCarga(c.cargaId, horaFin2, kws);

            //COMPROBAMOS
            Carga carga = cargaDao.Find(c.cargaId);
            Assert.AreEqual(carga.horaFin, horaFin2);
            Assert.AreEqual(carga.kws, kws);


        }
    }
}
