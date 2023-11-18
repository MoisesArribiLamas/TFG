using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Estado cargando = new Estado();
            cargando.nombre = "Cargando";
            estadoDao.Create(cargando);

            //CREAMOS LAS OTRAS DOS CATEGORIAS
            Estado suministrando = new Estado();
            suministrando.nombre = "Suministrando";
            estadoDao.Create(suministrando);

            Estado sYC = new Estado();
            sYC.nombre = "Suministra y Carga";
            estadoDao.Create(sYC);

            //COMPROBAMOS
            List<Estado> estadoObtenido = estadoDao.FindAllEstados();

            Assert.AreEqual("Cargando", estadoObtenido[0].nombre);
            Assert.AreEqual("Suministrando", estadoObtenido[1].nombre);
            Assert.AreEqual("Suministra y Carga", estadoObtenido[2].nombre);

        }

        [TestMethod()]
        public void FindEstadoByNameTest()
        {
            //CREAMOS LA CATEGORIA
            Estado cargando = new Estado();
            cargando.nombre = "Cargando";
            estadoDao.Create(cargando);

            //CREAMOS LAS OTRAS TRES CATEGORIAS
            Estado suministrando = new Estado();
            suministrando.nombre = "Suministrando";
            estadoDao.Create(suministrando);

            Estado sYC = new Estado();
            sYC.nombre = "Suministra y Carga";
            estadoDao.Create(sYC);

            Estado sinActividad = new Estado();
            sinActividad.nombre = "sin actividad";
            estadoDao.Create(sinActividad);

            //COMPROBAMOS
            long estadoObtenido = estadoDao.FindEstadoByName(cargando.nombre);
            long estadoObtenido2 = estadoDao.FindEstadoByName(suministrando.nombre);
            long estadoObtenido3 = estadoDao.FindEstadoByName(sYC.nombre);
            long estadoObtenido4 = estadoDao.FindEstadoByName(sinActividad.nombre);

            Assert.AreEqual("Cargando", estadoDao.Find(estadoObtenido).nombre);
            Assert.AreEqual("Suministrando", estadoDao.Find(estadoObtenido2).nombre);
            Assert.AreEqual("Suministra y Carga", estadoDao.Find(estadoObtenido3).nombre);
            Assert.AreEqual("sin actividad", estadoDao.Find(estadoObtenido4).nombre);

        }
    }
}
