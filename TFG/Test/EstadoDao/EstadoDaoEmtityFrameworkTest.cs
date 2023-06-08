using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Es.Udc.DotNet.TFG.Test;

namespace Es.Udc.DotNet.TFG.Model.Daos.EstadoDao.Tests
{
    [TestClass()]
    public class EstadoDaoEntityFrameworkTests
    {

        private static IKernel kernel;
        private static IEstadoDao estadoDao;


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
            estadoDao = kernel.Get<IEstadoDao>();
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
        public void findEstadoTest()
        {
            //CREAMOS LA CATEGORIA
            Estado est = new Estado();
            est.nombre = "Cargando";
            estadoDao.Create(est);

            List<Estado> estadoObtenido = estadoDao.FindAllEstados();

            Assert.AreEqual("Cargando", estadoObtenido[0].nombre);
        }
    }
}
