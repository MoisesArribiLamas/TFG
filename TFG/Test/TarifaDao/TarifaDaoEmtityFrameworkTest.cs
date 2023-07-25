using Microsoft.VisualStudio.TestTools.UnitTesting;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ninject;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.ModelUtil.Exceptions;
namespace Es.Udc.DotNet.TFG.Model.TarifaDao.Tests
{
    [TestClass()]
    public class TarifaEntityFrameworkTests
    {
        private static IKernel kernel;
        private static ITarifaDao tarifaDao;

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

            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = DateTime.Now;
            tarifaDao.Create(t);


            Tarifa t2 = new Tarifa();
            t2.precio = 105;
            t2.hora = 5;
            t2.fecha = DateTime.Now;
            tarifaDao.Create(t2);

            tarifaDao.updateInformacion(t.tarifaId, t2.precio, t2.hora, t2.fecha);

            Tarifa tarifaActualizada = tarifaDao.Find(t.tarifaId);
            Assert.AreEqual(t2.precio, tarifaActualizada.precio);
            Assert.AreEqual(t2.hora, tarifaActualizada.hora);
            Assert.AreEqual(t2.fecha, tarifaActualizada.fecha);




        }

        [TestMethod()]
        public void BuscarMejorTarifaTest()
        {

            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);


            Tarifa t2 = new Tarifa();
            t2.precio = 105;
            t2.hora = 5;
            t2.fecha = t.fecha;
            tarifaDao.Create(t2);

            Tarifa mtarifa = tarifaDao.BuscarMejorTarifa(t2.fecha);

            Assert.AreEqual(t.precio, mtarifa.precio);
        }

        [TestMethod()]
        public void BuscarPeorTarifaTest()
        {

            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);


            Tarifa t2 = new Tarifa();
            t2.precio = 105;
            t2.hora = 5;
            t2.fecha = t.fecha;
            tarifaDao.Create(t2);

            Tarifa mtarifa = tarifaDao.BuscarPeorTarifa(t2.fecha);

            Assert.AreEqual(t2.precio, mtarifa.precio);
        }

        [TestMethod()]
        public void CalcularMediaTarifaTest()
        {

            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);


            Tarifa t2 = new Tarifa();
            t2.precio = 110;
            t2.hora = 5;
            t2.fecha = t.fecha.AddDays(5);
            tarifaDao.Create(t2);

            Tarifa t3 = new Tarifa();
            t3.precio = 120;
            t3.hora = 5;
            t3.fecha = t.fecha.AddDays(5);
            tarifaDao.Create(t3);

            Tarifa t4 = new Tarifa();
            t4.precio = 310;
            t4.hora = 5;
            t4.fecha = t.fecha.AddDays(20);
            tarifaDao.Create(t4);

            Assert.AreEqual(115, tarifaDao.CalcularMediaTarifa(t2.fecha, t3.fecha));
            Assert.AreEqual(160, tarifaDao.CalcularMediaTarifa(t.fecha, t4.fecha));

        }

        [TestMethod()]
        public void VerFarifasDelDiaTest()
        {

            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);


            Tarifa t2 = new Tarifa();
            t2.precio = 110;
            t2.hora = 4;
            t2.fecha = t.fecha.AddDays(5);
            tarifaDao.Create(t2);

            Tarifa t3 = new Tarifa();
            t3.precio = 120;
            t3.hora = 5;
            t3.fecha = t.fecha.AddDays(5);
            tarifaDao.Create(t3);

            Tarifa t4 = new Tarifa();
            t4.precio = 310;
            t4.hora = 5;
            t4.fecha = t.fecha.AddDays(20);
            tarifaDao.Create(t4);

            List<Tarifa> ta = tarifaDao.verTarifasDelDia(t.fecha);
            List<Tarifa> ta2 = tarifaDao.verTarifasDelDia(t2.fecha);


            Assert.AreEqual(ta.Count, 1);
            Assert.AreEqual(ta2.Count, 2);
            Assert.AreEqual(ta2[0], t2);
            Assert.AreEqual(ta2[1], t3);

        }

        public void crearTarifa(long precio, long hora, DateTime fecha )
        {
        Tarifa t = new Tarifa();
            t.precio = precio;
            t.hora = hora;
            t.fecha = fecha;
            tarifaDao.Create(t);
        }


    [TestMethod()]
        public void VerFarifasDelDiaTest2()
        {

            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);

            crearTarifa(200, 2, t.fecha);
            crearTarifa(300, 3, t.fecha);
            crearTarifa(400, 4, t.fecha);
            crearTarifa(500, 5, t.fecha);
            crearTarifa(600, 6, t.fecha);
            crearTarifa(700, 7, t.fecha);
            crearTarifa(800, 8, t.fecha);
            crearTarifa(900, 9, t.fecha);
            crearTarifa(100, 10, t.fecha);
            crearTarifa(200, 11, t.fecha);
            crearTarifa(300, 12, t.fecha);
            crearTarifa(400, 13, t.fecha);
            crearTarifa(500, 14, t.fecha);
            crearTarifa(600, 15, t.fecha);
            crearTarifa(700, 16, t.fecha);
            crearTarifa(800, 17, t.fecha);
            crearTarifa(900, 18, t.fecha);
            crearTarifa(100, 19, t.fecha);
            crearTarifa(200, 20, t.fecha);
            crearTarifa(200, 21, t.fecha);
            crearTarifa(300, 22, t.fecha);
            crearTarifa(400, 23, t.fecha);
            crearTarifa(500, 0, t.fecha);

            List<Tarifa> ta = tarifaDao.verTarifasDelDia(t.fecha);


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
}