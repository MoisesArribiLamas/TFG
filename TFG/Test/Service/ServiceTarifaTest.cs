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
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceTarifaTest
    {
        private static IKernel kernel;
        private static IServiceTarifa servicio;

        private static ITarifaDao tarifaDao;
        

        private const long precio = 15000;
        private const long hora = 1;


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
            tarifaDao = kernel.Get<ITarifaDao>();
            servicio = kernel.Get<IServiceTarifa>();


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

        public void crearTarifa(long precio, long hora, DateTime fecha)
        {
            Tarifa t = new Tarifa();
            t.precio = precio;
            t.hora = hora;
            t.fecha = fecha;
            tarifaDao.Create(t);
        }

        [TestMethod()]
        public void verTarifasDelDiaTest()
        {
            using (var scope = new TransactionScope())
            {
                
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                crearTarifa(500, 0, fecha);
                crearTarifa(100, 1, fecha);
                crearTarifa(200, 2, fecha);
                crearTarifa(300, 3, fecha);
                crearTarifa(400, 4, fecha);
                crearTarifa(500, 5, fecha);
                crearTarifa(600, 6, fecha);
                crearTarifa(700, 7, fecha);
                crearTarifa(800, 8, fecha);
                crearTarifa(900, 9, fecha);
                crearTarifa(100, 10, fecha);
                crearTarifa(200, 11, fecha);
                crearTarifa(300, 12, fecha);
                crearTarifa(400, 13, fecha);
                crearTarifa(500, 14, fecha);
                crearTarifa(600, 15, fecha);
                crearTarifa(700, 16, fecha);
                crearTarifa(800, 17, fecha);
                crearTarifa(900, 18, fecha);
                crearTarifa(100, 19, fecha);
                crearTarifa(200, 20, fecha);
                crearTarifa(200, 21, fecha);
                crearTarifa(300, 22, fecha);
                crearTarifa(400, 23, fecha);
                

                List<TarifaDTO> ta = servicio.verTarifasDelDia(fecha);


                Assert.AreEqual(ta.Count, 24);
                Assert.AreEqual(ta[0].hora, 0);
                Assert.AreEqual(ta[1].hora, 1);
                Assert.AreEqual(ta[2].hora, 2);
                Assert.AreEqual(ta[3].hora, 3);
                Assert.AreEqual(ta[4].hora, 4);
                Assert.AreEqual(ta[5].hora, 5);
                Assert.AreEqual(ta[6].hora, 6);
                Assert.AreEqual(ta[7].hora, 7);
                Assert.AreEqual(ta[8].hora, 8);
                Assert.AreEqual(ta[9].hora, 9);
                Assert.AreEqual(ta[10].hora, 10);
                Assert.AreEqual(ta[11].hora, 11);
                Assert.AreEqual(ta[12].hora, 12);
                Assert.AreEqual(ta[13].hora, 13);
                Assert.AreEqual(ta[14].hora, 14);
                Assert.AreEqual(ta[15].hora, 15);
                Assert.AreEqual(ta[16].hora, 16);
                Assert.AreEqual(ta[17].hora, 17);
                Assert.AreEqual(ta[18].hora, 18);
                Assert.AreEqual(ta[19].hora, 19);
                Assert.AreEqual(ta[20].hora, 20);
                Assert.AreEqual(ta[21].hora, 21);

            }
        }

        [TestMethod()]
        public void BuscarMejorTarifaTest()
        {
            using (var scope = new TransactionScope())
            {

                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                crearTarifa(500, 0, fecha);
                crearTarifa(100, 1, fecha); // mejor tarifa
                crearTarifa(200, 2, fecha);
                crearTarifa(300, 3, fecha);
                crearTarifa(400, 4, fecha);
                crearTarifa(500, 5, fecha);
                crearTarifa(600, 6, fecha);
                crearTarifa(700, 7, fecha);
                crearTarifa(800, 8, fecha);
                crearTarifa(900, 9, fecha);
                crearTarifa(100, 10, fecha);
                crearTarifa(200, 11, fecha);
                crearTarifa(300, 12, fecha);
                crearTarifa(400, 13, fecha);
                crearTarifa(500, 14, fecha);
                crearTarifa(600, 15, fecha);
                crearTarifa(700, 16, fecha);
                crearTarifa(800, 17, fecha);
                crearTarifa(900, 18, fecha);
                crearTarifa(400, 19, fecha);
                crearTarifa(200, 20, fecha);
                crearTarifa(200, 21, fecha);
                crearTarifa(300, 22, fecha);
                crearTarifa(400, 23, fecha);


                long ta = servicio.BuscarMejorTarifa(fecha);


                
                Assert.AreEqual(ta, 100);


            }
        }

        [TestMethod()]
        public void BuscarPeorTarifaTest()
        {
            using (var scope = new TransactionScope())
            {

                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                crearTarifa(500, 0, fecha);
                crearTarifa(100, 1, fecha); 
                crearTarifa(200, 2, fecha);
                crearTarifa(300, 3, fecha);
                crearTarifa(400, 4, fecha);
                crearTarifa(500, 5, fecha);
                crearTarifa(600, 6, fecha);
                crearTarifa(700, 7, fecha);
                crearTarifa(800, 8, fecha);
                crearTarifa(900, 9, fecha);// Peor tarifa
                crearTarifa(100, 10, fecha);
                crearTarifa(200, 11, fecha);
                crearTarifa(300, 12, fecha);
                crearTarifa(400, 13, fecha);
                crearTarifa(500, 14, fecha);
                crearTarifa(600, 15, fecha);
                crearTarifa(700, 16, fecha);
                crearTarifa(800, 17, fecha);
                crearTarifa(900, 18, fecha);
                crearTarifa(400, 19, fecha);
                crearTarifa(200, 20, fecha);
                crearTarifa(200, 21, fecha);
                crearTarifa(300, 22, fecha);
                crearTarifa(400, 23, fecha);


                long ta = servicio.BuscarpeorTarifa(fecha);


             
                Assert.AreEqual(ta, 900);


            }
        }

        [TestMethod()]
        public void OrdenarMejorPrecioTarifasDelDiaTest()
        {
            using (var scope = new TransactionScope())
            {

                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);

                crearTarifa(1, 0, fecha);// mejor tarifa
                crearTarifa(100, 1, fecha);
                crearTarifa(200, 2, fecha);
                crearTarifa(300, 3, fecha);
                crearTarifa(400, 4, fecha);
                crearTarifa(500, 5, fecha);
                crearTarifa(600, 6, fecha);
                crearTarifa(700, 7, fecha);
                crearTarifa(800, 8, fecha);
                crearTarifa(900, 9, fecha);
                crearTarifa(1000, 10, fecha);
                crearTarifa(1100, 11, fecha);
                crearTarifa(1200, 12, fecha);
                crearTarifa(1300, 13, fecha);
                crearTarifa(1400, 14, fecha);
                crearTarifa(1500, 15, fecha);
                crearTarifa(1600, 16, fecha);
                crearTarifa(1700, 17, fecha);
                crearTarifa(1800, 18, fecha);
                crearTarifa(1900, 19, fecha);
                crearTarifa(2000, 20, fecha);
                crearTarifa(2100, 21, fecha);
                crearTarifa(2200, 22, fecha);
                crearTarifa(2300, 23, fecha);// Peor tarifa
                crearTarifa(4000, 0, fecha2);// esta fuera de fecha

                List<TarifaDTO> ta = servicio.OrdenarMejorPrecioTarifasDelDia(fecha);

                Assert.AreEqual(ta[0].precio, 1);
                Assert.AreEqual(ta[0].hora, 0);
                Assert.AreEqual(ta[1].hora, 1);
                Assert.AreEqual(ta[2].hora, 2);
                Assert.AreEqual(ta[3].hora, 3);
                Assert.AreEqual(ta[4].hora, 4);
                Assert.AreEqual(ta[5].hora, 5);
                Assert.AreEqual(ta[6].hora, 6);

            }
        }

        [TestMethod()]
        public void OrdenarPeorPrecioTarifasDelDiaTest()
        {
            using (var scope = new TransactionScope())
            {

                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);

                crearTarifa(1, 0, fecha);// mejor tarifa
                crearTarifa(100, 1, fecha);
                crearTarifa(200, 2, fecha);
                crearTarifa(300, 3, fecha);
                crearTarifa(400, 4, fecha);
                crearTarifa(500, 5, fecha);
                crearTarifa(600, 6, fecha);
                crearTarifa(700, 7, fecha);
                crearTarifa(800, 8, fecha);
                crearTarifa(900, 9, fecha);
                crearTarifa(1000, 10, fecha);
                crearTarifa(1100, 11, fecha);
                crearTarifa(1200, 12, fecha);
                crearTarifa(1300, 13, fecha);
                crearTarifa(1400, 14, fecha);
                crearTarifa(1500, 15, fecha);
                crearTarifa(1600, 16, fecha);
                crearTarifa(1700, 17, fecha);
                crearTarifa(1800, 18, fecha);
                crearTarifa(1900, 19, fecha);
                crearTarifa(2000, 20, fecha);
                crearTarifa(2100, 21, fecha);
                crearTarifa(2200, 22, fecha);
                crearTarifa(2300, 23, fecha);// Peor tarifa
                crearTarifa(4000, 0, fecha2);// esta fuera de fecha

                List<TarifaDTO> ta = servicio.OrdenarPeorPrecioTarifasDelDia(fecha);

                Assert.AreEqual(ta[0].precio, 2300);
                Assert.AreEqual(ta[0].hora, 23);
                Assert.AreEqual(ta[1].hora, 22);
                Assert.AreEqual(ta[2].hora, 21);
                Assert.AreEqual(ta[3].hora, 20);
                Assert.AreEqual(ta[4].hora, 19);
                Assert.AreEqual(ta[5].hora, 18);
                Assert.AreEqual(ta[6].hora, 17);

            }
        }
    }
}
