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

namespace Es.Udc.DotNet.TFG.Model.Daos.SuministraDao.Tests
{
    [TestClass()]
    public class SuministraDaoEntityFrameworkTests
    {

        private static IKernel kernel;
        private static IBateriaDao bateriaDao;
        private static IUbicacionDao ubicacionDao;
        private static IUsuarioDao usuarioDao;
        private static ISuministraDao suministraDao;
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
            suministraDao = kernel.Get<ISuministraDao>();
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
        public void getInfoSuministraTest()
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
            b.almacenajeMaximoKwH = 1000 ;
            b.almacenajeMaximoKwH= 1000 ;
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

            //CREAMOS Suministrar
            Suministra c = new Suministra();
            c.kwH = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.ahorro = 100;
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            suministraDao.Create(c);

            Suministra c2 = new Suministra();
            c2.kwH = 1000;
            c2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            c2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            c.ahorro = 200;
            c2.tarifaId = t.tarifaId;
            c2.bateriaId = b.bateriaId;
            suministraDao.Create(c2);


            //COMPROBAMOS
            Suministra suministraResult = suministraDao.getInfoSuministra(c.suministraId);
            Suministra suministraResult2 = suministraDao.getInfoSuministra(c2.suministraId);

            Assert.AreEqual(suministraResult, c);
            Assert.AreEqual(suministraResult2, c2);

        }

        [TestMethod()]
        public void MostrarSuministrarBareriaPorFechaTest()
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
            b.almacenajeMaximoKwH = 1000;
            b.almacenajeMaximoKwH= 1000;
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
            b2.almacenajeMaximoKwH = 1000;
            b2.almacenajeMaximoKwH= 1000;
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
            Suministra c = new Suministra();
            c.kwH = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            c.ahorro = 0;
            suministraDao.Create(c);

            Suministra c2 = new Suministra();
            c2.kwH = 2000;
            c2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            c2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            c2.tarifaId = t2.tarifaId;
            c2.bateriaId = b.bateriaId;
            c2.ahorro = 0;
            suministraDao.Create(c2);

            Suministra c3 = new Suministra();
            c3.kwH = 3000;
            c3.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c3.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c3.tarifaId = t2.tarifaId;
            c3.bateriaId = b2.bateriaId;
            c3.ahorro = 0;
            suministraDao.Create(c3);

            Suministra c0 = new Suministra();
            c0.kwH = 3000;
            c0.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c0.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c0.tarifaId = t3.tarifaId;
            c0.bateriaId = b.bateriaId;
            c0.ahorro = 0;
            suministraDao.Create(c0);


            //COMPROBAMOS
            DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
            int startIndex = 0;
            int count = 3;
            List<Suministra> suministraResult = suministraDao.MostrarSuministrosBareriaPorFecha(b.bateriaId, fecha, fecha2, startIndex, count);


            Assert.AreEqual(suministraResult[0], c);
            Assert.AreEqual(suministraResult[1], c2);
            Assert.AreEqual(suministraResult.Count(), 2);

        }

        [TestMethod()]
        public void MostrarUltimaCargaBareriaTest()
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
            b.almacenajeMaximoKwH = 1000;
            b.almacenajeMaximoKwH= 1000;
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
            b2.almacenajeMaximoKwH = 1000;
            b2.almacenajeMaximoKwH= 1000;
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
            Suministra c = new Suministra();
            c.kwH = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            c.ahorro = 0;
            suministraDao.Create(c);

            Suministra c2 = new Suministra();
            c2.kwH = 2000;
            c2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            c2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            c2.tarifaId = t2.tarifaId;
            c2.bateriaId = b.bateriaId;
            c2.ahorro = 0;
            suministraDao.Create(c2);

            Suministra c3 = new Suministra();
            c3.kwH = 3000;
            c3.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c3.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c3.tarifaId = t2.tarifaId;
            c3.bateriaId = b2.bateriaId;
            c3.ahorro = 0;
            suministraDao.Create(c3);

            Suministra c0 = new Suministra();
            c0.kwH = 3000;
            c0.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c0.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c0.tarifaId = t3.tarifaId;
            c0.bateriaId = b.bateriaId;
            c0.ahorro = 0;
            suministraDao.Create(c0);


            //COMPROBAMOS
            Suministra cargaResult = suministraDao.UltimaSuministraBareria(b.bateriaId);


            Assert.AreEqual(cargaResult.tarifaId, t3.tarifaId);
            Assert.AreEqual(cargaResult.kwH, c0.kwH);
            Assert.AreEqual(cargaResult.horaIni, c0.horaIni);
            Assert.AreEqual(cargaResult.horaFin, c0.horaFin);
            Assert.AreEqual(cargaResult.bateriaId, b.bateriaId);

        }

        [TestMethod()]
        public void ahorroBareriaPorFechaTest()
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
            b.almacenajeMaximoKwH = 1000;
            b.almacenajeMaximoKwH= 1000;
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
            b2.almacenajeMaximoKwH = 1000;
            b2.almacenajeMaximoKwH= 1000;
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
            Suministra c = new Suministra();
            c.kwH = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            c.ahorro = 10;
            suministraDao.Create(c);

            Suministra c2 = new Suministra();
            c2.kwH = 2000;
            c2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            c2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            c2.tarifaId = t2.tarifaId;
            c2.bateriaId = b.bateriaId;
            c2.ahorro = 200;
            suministraDao.Create(c2);

            Suministra c3 = new Suministra();
            c3.kwH = 3000;
            c3.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c3.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c3.tarifaId = t2.tarifaId;
            c3.bateriaId = b2.bateriaId;
            c3.ahorro = 3000;
            suministraDao.Create(c3);

            Suministra c0 = new Suministra();
            c0.kwH = 3000;
            c0.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c0.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c0.tarifaId = t3.tarifaId;
            c0.bateriaId = b.bateriaId;
            c0.ahorro = 1;
            suministraDao.Create(c0);


            //COMPROBAMOS
            DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
            
            double ahorroResult = suministraDao.ahorroBareriaPorFecha(b.bateriaId, fecha, fecha2);


            Assert.AreEqual(ahorroResult, 210);
            

        }


        [TestMethod()]
        public void ahorroUsuarioPorFechaTest()
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

            //CREAMOS LOS USUARIO
            Usuario user2 = new Usuario();
            user2.nombre = "Paula Erica";
            user2.email = "pecorreo@gmail.com";
            user2.apellido1 = "Resmond";
            user2.apellido2 = "González";
            user2.contraseña = "unacontraseña";
            user2.telefono = "981123457";
            user2.pais = "España";
            user2.idioma = "es-ES";
            usuarioDao.Create(user2);


            //CREAMOS LAS BATERIAS
            Bateria b = new Bateria();
            b.ubicacionId = u.ubicacionId;
            b.usuarioId = user.usuarioId;
            b.precioMedio = 111;
            b.almacenajeMaximoKwH = 1000;
            b.almacenajeMaximoKwH = 1000;
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
            b2.almacenajeMaximoKwH = 1000;
            b2.almacenajeMaximoKwH = 1000;
            b2.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            b2.marca = "MARCA 1";
            b2.modelo = "MODELO 1";
            b2.ratioCarga = 10;
            b2.ratioCompra = 10;
            b2.ratioUso = 10;
            bateriaDao.Create(b2);

            Bateria b3 = new Bateria();
            b3.ubicacionId = u.ubicacionId;
            b3.usuarioId = user2.usuarioId;
            b3.precioMedio = 111;
            b3.almacenajeMaximoKwH = 1000;
            b3.almacenajeMaximoKwH = 1000;
            b3.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            b3.marca = "MARCA 1";
            b3.modelo = "MODELO 1";
            b3.ratioCarga = 10;
            b3.ratioCompra = 10;
            b3.ratioUso = 10;
            bateriaDao.Create(b3);

            //CREAMOS LAS TARIFAS
            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);

            Tarifa t2 = new Tarifa();
            t2.precio = 200;
            t2.hora = 2;
            t2.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t2);

            Tarifa t3 = new Tarifa();
            t3.precio = 300;
            t3.hora = 3;
            t3.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(5).Day);
            tarifaDao.Create(t3);

            //CREAMOS CARGAS
            Suministra c = new Suministra(); // bateria 1   usuario 1
            c.kwH = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            c.ahorro = 10;
            suministraDao.Create(c);

            Suministra c2 = new Suministra(); // bateria 1  usuario 1
            c2.kwH = 2000;
            c2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            c2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            c2.tarifaId = t2.tarifaId;
            c2.bateriaId = b.bateriaId;
            c2.ahorro = 100;
            suministraDao.Create(c2);

            Suministra c3 = new Suministra(); // bateria 2  usuario 1
            c3.kwH = 3000;
            c3.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c3.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c3.tarifaId = t2.tarifaId;
            c3.bateriaId = b2.bateriaId;
            c3.ahorro = 1000;
            suministraDao.Create(c3);

            Suministra c4 = new Suministra(); // bateria 3  usuario 2 No es el usuario de la busqueda
            c4.kwH = 3000;
            c4.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c4.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c4.tarifaId = t2.tarifaId;
            c4.bateriaId = b3.bateriaId;
            c4.ahorro = 1;
            suministraDao.Create(c4);

            Suministra c0 = new Suministra(); // bateria 1 fuera de tiempo
            c0.kwH = 3000;
            c0.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, DateTime.Now.Second);
            c0.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);
            c0.tarifaId = t3.tarifaId;
            c0.bateriaId = b.bateriaId;
            c0.ahorro = 222;
            suministraDao.Create(c0);


            //COMPROBAMOS
            DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);

            double ahorroResult = suministraDao.ahorroUsuarioPorFecha(user.usuarioId, fecha, fecha2);


            Assert.AreEqual(ahorroResult, c.ahorro + c2.ahorro + c3.ahorro);


        }


    }
}
