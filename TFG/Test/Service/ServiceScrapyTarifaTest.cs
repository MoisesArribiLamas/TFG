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
using Es.Udc.DotNet.TFG.Model.Service.Consumos;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceScrapyTarifaTest
    {
        private static IKernel kernel;
        private static IServiceConsumo servicio;

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
            servicio = kernel.Get<IServiceConsumo>();


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

       

       // [TestMethod()]
        public void TarifaActualTest()
        {
            using (var scope = new TransactionScope())
            {

                servicio.scrapyTarifas();

            }
        }
    }
}
