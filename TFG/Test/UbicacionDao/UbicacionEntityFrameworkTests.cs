using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ninject;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;

namespace Es.Udc.DotNet.TFG.Model.UbicacionDao.Tests
{
    [TestClass()]
    public class UbicacionEntityFrameworkTests
    {
        private static IKernel kernel;
        private static IUbicacionDao ubicacionDao;

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
         
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            u.portal = 1;
            u.numero = 1;
            ubicacionDao.Create(u);


            Ubicacion u2 = new Ubicacion();

            u2.localidad = "A Coruña";
            u2.codigoPostal = 15005;
            u2.calle = "calle de Ferrol";
            u2.portal = 2;
            u2.numero = 2;
            ubicacionDao.Create(u);

            ubicacionDao.updateInformacion(u.ubicacionId, u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero);

            Ubicacion ubicacionActualizada = ubicacionDao.Find(u.ubicacionId);
            Assert.AreEqual(u2.localidad, ubicacionActualizada.localidad);
            Assert.AreEqual(u2.codigoPostal, ubicacionActualizada.codigoPostal);
            Assert.AreEqual(u2.calle, ubicacionActualizada.calle);
            Assert.AreEqual(u2.portal, ubicacionActualizada.portal);
            Assert.AreEqual(u2.numero, ubicacionActualizada.numero);



        }

        [TestMethod()]
        public void findUbicacion()
        {
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15000;
            u.localidad = "Coruña";
            u.calle = "Calle";
            u.portal = 1;
            u.numero = 120;
            
            //ubicacionDao.Create(u);

            Assert.AreEqual(true, ubicacionDao.findUbicacionExistente(u.codigoPostal, u.localidad, u.calle, u.portal, u.numero));

        }

        [TestMethod()]
        public void findUbicacion2()
        {
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15000;
            u.localidad = "Coruña";
            u.calle = "Calle";
            u.portal = 1;
            u.numero = 120;

            Ubicacion u2 = new Ubicacion();
            u2.codigoPostal = 15000;
            u2.localidad = "Coruña";
            u2.calle = "Calle";
            u2.portal = 1;
            u2.numero = 121;

            ubicacionDao.Create(u);
            

            Assert.AreEqual(true, ubicacionDao.findUbicacionExistente(u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero));

        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateInstanceException))]
        
        public void findUbicacionFallo()
        {
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15000;
            u.localidad = "Coruña";
            u.calle = "Calle";
            u.portal = 1;
            u.numero = 120;

            Ubicacion u2 = new Ubicacion();
            u2.codigoPostal = 15000;
            u2.localidad = "Coruña";
            u2.calle = "Calle";
            u2.portal = 1;
            u2.numero = 121;

            ubicacionDao.Create(u);
            Assert.AreEqual(true, ubicacionDao.findUbicacionExistente(u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u.numero));



        }
    }
}