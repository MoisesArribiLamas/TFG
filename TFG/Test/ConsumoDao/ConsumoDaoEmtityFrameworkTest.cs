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
            TimeSpan dosMinutos = new TimeSpan(0, 2, 0);
            TimeSpan tresMinutos = new TimeSpan(0, 3, 0);
            TimeSpan horaFin = horaIni.Add(tresMinutos); 

            long ubicacionId = u.ubicacionId;

                // consumo 1
                Consumo c1 =crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);

            consumoActual = 15;
            TimeSpan horaFin2 = horaFin.Add(dosMinutos);

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
        public void UltimoConsumoUbicacionTest2()
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

            TimeSpan dosMinutos = new TimeSpan(0, 2, 0);
            TimeSpan tresMinutos = new TimeSpan(0, 3, 0);

            // Creamos Consumos
            double consumoActual = 10;
            double kwCargados = 100;
            double kwSuministrados = 100;
            double kwRed = 0;
            DateTime fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan horaFin = horaIni.Add(tresMinutos);
            
            long ubicacionId = u.ubicacionId;

            // consumo 1
            Consumo c1 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);

            consumoActual = 15;
            TimeSpan horaFin2 = horaFin.Add(dosMinutos);
            // consumo 2
            Consumo c2 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaFin, horaFin2, ubicacionId);


            //COMPROBAMOS   
            Consumo consumoResult = consumoDao.UltimoConsumoUbicacion(ubicacionId);

            Assert.AreEqual(consumoResult, c2);

        }

        [TestMethod()]
        public void UltimoConsumoUbicacionSinConsumoTest()
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

            long ubicacionId = u.ubicacionId;

            
            //COMPROBAMOS   
            Consumo consumoResult = consumoDao.UltimoConsumoUbicacion(ubicacionId);

            Assert.AreEqual(consumoResult, null);

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

        [TestMethod()]
        public void FechaUltimoConsumoSistemaTest()
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
            DateTime consumoResult = consumoDao.FechaUltimoConsumoSistema();

            Assert.AreEqual(consumoResult, fecha);

        }

        [TestMethod()]
        public void MostrarConsumosEnUnIntervaloTest()
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

            TimeSpan dosMinutos = new TimeSpan(0, 2, 0);
            TimeSpan tresMinutos = new TimeSpan(0, 3, 0);

            // Creamos Consumos
            double consumoActual = 10;
            double kwCargados = 100;
            double kwSuministrados = 100;
            double kwRed = 0;
            DateTime fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime fecha2  = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            DateTime fecha3  = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(2);
            TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan horaFin = horaIni.Add(tresMinutos);

            long ubicacionId = u.ubicacionId;

            // consumo 1
            Consumo c1 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);

            consumoActual = 15;
            TimeSpan horaFin2 = horaFin.Add(dosMinutos);
            // consumo 2
            Consumo c2 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaFin, horaFin2, ubicacionId);

            // consumo 3
            Consumo c3 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha3, horaFin, horaFin2, ubicacionId);

            //COMPROBAMOS   
            List<Consumo> consumoResult = consumoDao.MostrarConsumosUbicacionPorFecha(ubicacionId, fecha, fecha2, 0, 5);

            Assert.AreEqual(consumoResult.Count(), 2);
            Assert.AreEqual(consumoResult[0], c1);
            Assert.AreEqual(consumoResult[1], c2);


        }

        [TestMethod()]
        public void MostrarConsumosUbicacionPorFechaEnDiasTest()
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
            TimeSpan dosMinutos = new TimeSpan(0, 2, 0);
            TimeSpan tresMinutos = new TimeSpan(0, 3, 0);
            TimeSpan horaFin = horaIni.Add(tresMinutos);

            long ubicacionId = u.ubicacionId;

            // consumo 0
            Consumo c0 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);

            DateTime fecha1 = fecha.AddDays(1); // dia siguiente

            //-------------------------------- fecha 1

            // consumo 1
            Consumo c1 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha1, horaIni, horaFin, ubicacionId);

            consumoActual = 1;
            TimeSpan horaFin2 = horaFin.Add(dosMinutos);

            // consumo 2
            Consumo c2 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha1, horaFin, horaFin2, ubicacionId);


            DateTime fecha2 = fecha1.AddDays(1); // dia siguiente

            //-------------------------------- fecha 2

            // consumo 3
            Consumo c3 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha2, horaIni, horaFin, ubicacionId);

            DateTime fecha3 = fecha2.AddDays(1); // dia siguiente

            //-------------------------------- 

            // consumo 3
            Consumo c4 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha3, horaIni, horaFin, ubicacionId);


            //COMPROBAMOS   

            fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            List<ConsumoPorDias> consumoResult = consumoDao.MostrarConsumosUbicacionPorFechaEnDias(ubicacionId, fecha1, fecha2);


            Assert.AreEqual(consumoResult[0].Count, 10+1); //c1+c2
            Assert.AreEqual(consumoResult[0].fecha, fecha1);

            Assert.AreEqual(consumoResult[1].Count, consumoActual ); //c3
            Assert.AreEqual(consumoResult[1].fecha, fecha2);
            Assert.AreEqual(consumoResult.Count(), 2);

        }


        [TestMethod()]
        public void MostrarConsumosRedElectricaUbicacionPorFechaEnDiasTest()
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
            TimeSpan dosMinutos = new TimeSpan(0, 2, 0);
            TimeSpan tresMinutos = new TimeSpan(0, 3, 0);
            TimeSpan horaFin = horaIni.Add(tresMinutos);

            long ubicacionId = u.ubicacionId;

            // consumo 0
            Consumo c0 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha, horaIni, horaFin, ubicacionId);

            DateTime fecha1 = fecha.AddDays(1); // dia siguiente

            // ponemos lo consumido de la red
            c0.kwRed = 1;
            //-------------------------------- fecha 1

            // consumo 1
            Consumo c1 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha1, horaIni, horaFin, ubicacionId);

            // ponemos lo consumido de la red
            c1.kwRed = 10;

            consumoActual = 1;

            TimeSpan horaFin2 = horaFin.Add(dosMinutos);

            // consumo 2
            Consumo c2 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha1, horaFin, horaFin2, ubicacionId);

            // ponemos lo consumido de la red
            c2.kwRed = 100;

            DateTime fecha2 = fecha1.AddDays(1); // dia siguiente

            //-------------------------------- fecha 2

            // consumo 3
            Consumo c3 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha2, horaIni, horaFin, ubicacionId);

            // ponemos lo consumido de la red
            c3.kwRed = 1000;

            DateTime fecha3 = fecha2.AddDays(1); // dia siguiente

            //-------------------------------- 

            // consumo 3
            Consumo c4 = crearConsumoUbicacion(consumoActual, kwCargados, kwSuministrados, kwRed, fecha3, horaIni, horaFin, ubicacionId);

            // ponemos lo consumido de la red
            c4.kwRed = 10000;

            //COMPROBAMOS   

            fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            List<ConsumoPorDias> consumoResult = consumoDao.MostrarConsumosRedElectricaUbicacionPorFechaEnDias(ubicacionId, fecha1, fecha2);


            Assert.AreEqual(consumoResult[0].Count, 10 + 100); //c1+c2
            Assert.AreEqual(consumoResult[0].fecha, fecha1);

            Assert.AreEqual(consumoResult[1].Count, 1000); //c3
            Assert.AreEqual(consumoResult[1].fecha, fecha2);
            Assert.AreEqual(consumoResult.Count(), 2);

        }
    }
}
