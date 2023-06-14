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
    }
}