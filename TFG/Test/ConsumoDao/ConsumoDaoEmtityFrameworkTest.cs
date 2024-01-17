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

namespace Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao.Tests
{
    [TestClass()]
    public class ConsumoDaoEntityFrameworkTests
    {

        private static IKernel kernel;
        private static IUbicacionDao ubicacionDao;
        private static IConsumoDao consumoDao;



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
            ubicacionDao = kernel.Get<IUbicacionDao>();
            consumoDao = kernel.Get<IConsumoDao>();

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

        //creamos ubicacion
        public Ubicacion crearUbicacion(long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta, long bateriaSuministradora)
        {
            Ubicacion u = new Ubicacion();
            u.codigoPostal = codigoPostal;
            u.localidad = localidad;
            u.calle = calle;
            u.portal = portal;
            u.numero = numero;
            u.etiqueta = etiqueta;
            u.bateriaSuministradora = bateriaSuministradora;

            ubicacionDao.Create(u);

            return u;
        }

        //creamos consumo asociado a una ubicacion
        public Consumo crearConsumoUbicacion(double consumoActual, double kwCargados, double kwSuministrados, double kwRed, DateTime fecha, TimeSpan horaIni, TimeSpan horaFin, long ubicacionId)
        {
            Consumo c = new Consumo();
            c.consumoActual = consumoActual;
            c.kwCargados = kwCargados;
            c.kwSuministrados = kwSuministrados;
            c.kwRed = kwRed;
            c.fecha = fecha;
            c.horaIni = horaIni;
            c.horaFin = horaFin;
            c.ubicacionId = ubicacionId;           

            consumoDao.Create(c);

            return c;
        }


        [TestMethod()]
        public void MostrarConsumosUbicacionPorFechaTest()
        {
            // Creamos Ubicacion
            long codigoPostal = 15000 ; 
            string localidad = "Coruña";
            string calle = "San Juan";
            string portal = "";
            long numero = 100;
            string etiqueta = "bichito" ;
            long bateriaSuministradora = 1;

            Ubicacion u = crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta, bateriaSuministradora);

            // Creamos Consumos
            double consumoActual = 10;
            double kwCargados = 100;
            double kwSuministrados = 100;
            double kwRed = 0;
            DateTime fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            long ubicacionId = u.ubicacionId;

                // consumo 1
                Consumo c1 =crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);

            consumoActual = 15;
            TimeSpan horaFin2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);

                // consumo 2
                Consumo c2 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaFin, horaFin2, ubicacionId);


            fecha = fecha.AddDays(1); // dia siguiente

                // consumo 3
                Consumo c3 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);


            //COMPROBAMOS   
            int startIndex = 0;
            int count = 3;
            fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fecha2 = fecha.AddDays(1);
            List<Consumo> consumoResult = consumoDao.MostrarConsumosUbicacionPorFecha(ubicacionId, fecha, fecha2, startIndex, count);


            Assert.AreEqual(consumoResult[0], c1);
            Assert.AreEqual(consumoResult[1], c2);
            Assert.AreEqual(consumoResult[2], c3);
            Assert.AreEqual(consumoResult.Count(), 3);

        }

        [TestMethod()]
        public void UltimoConsumoUbicacionTest()
        {
            // Creamos Ubicacion
            long codigoPostal = 15000;
            string localidad = "Coruña";
            string calle = "San Juan";
            string portal = "";
            long numero = 100;
            string etiqueta = "bichito";
            long bateriaSuministradora = 1;

            Ubicacion u = crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta, bateriaSuministradora);

            // Creamos Consumos
            double consumoActual = 10;
            double kwCargados = 100;
            double kwSuministrados = 100;
            double kwRed = 0;
            DateTime fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            long ubicacionId = u.ubicacionId;

            // consumo 1
            Consumo c1 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);

            consumoActual = 15;
            TimeSpan horaFin2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);

            // consumo 2
            Consumo c2 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaFin, horaFin2, ubicacionId);


            fecha = fecha.AddDays(1); // dia siguiente

            // consumo 3
            Consumo c3 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);


            //COMPROBAMOS   
            Consumo consumoResult = consumoDao.UltimoConsumoUbicacion(ubicacionId);

            Assert.AreEqual(consumoResult, c3);

        }

        [TestMethod()]
        public void ConsumoUbicacionActualTest()
        {
            // Creamos Ubicacion
            long codigoPostal = 15000;
            string localidad = "Coruña";
            string calle = "San Juan";
            string portal = "";
            long numero = 100;
            string etiqueta = "bichito";
            long bateriaSuministradora = 1;

            Ubicacion u = crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta, bateriaSuministradora);

            // Creamos Consumos
            double consumoActual = 10;
            double kwCargados = 100;
            double kwSuministrados = 100;
            double kwRed = 0;
            DateTime fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            long ubicacionId = u.ubicacionId;

            // consumo 1
            Consumo c1 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);

            consumoActual = 15;
            TimeSpan horaFin2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(5).Minute, DateTime.Now.Second);

            // consumo 2
            Consumo c2 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaFin, horaFin2, ubicacionId);


            fecha = fecha.AddDays(1); // dia siguiente

            // consumo 3
            Consumo c3 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);


            //COMPROBAMOS   
            double consumoUActual = consumoDao.ConsumoUbicacionActual(ubicacionId);
            Consumo consumoResult = consumoDao.UltimoConsumoUbicacion(ubicacionId);

            Assert.AreEqual(consumoResult.consumoActual, consumoUActual);

        }

        
    }
}
