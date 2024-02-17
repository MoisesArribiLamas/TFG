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
    public class ServiceScrapyTarifaTest
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

       

        //[TestMethod()]
        //public void TarifaActualTest()
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        // fecha
        //        DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        //        //------------------------------------------------------------
        //        // HAY QUE PONER LOS VALORES DEL DIA A MANO
        //        double p0 = 0.11497;
        //        double p1 = 0.1141;
        //        double p2 = 0.11464;
        //        double p3 = 0.11368;
        //        double p4 = 0.11285;
        //        double p5 = 0.1174;
        //        double p6 = 0.1263;
        //        double p7 = 0.13291;
        //        double p8 = 0.18838;
        //        double p9 = 0.15483;
        //        double p10 = 0.19232;
        //        double p11 = 0.17968;
        //        double p12 = 0.17709;
        //        double p13 = 0.1729;
        //        double p14 = 0.12194;
        //        double p15 = 0.12165;
        //        double p16 = 0.12346;
        //        double p17 = 0.13643;
        //        double p18 = 0.20267;
        //        double p19 = 0.23731;
        //        double p20 = 0.29742;
        //        double p21 = 0.20738;
        //        double p22 = 0.14675;
        //        double p23 = 0.13949;

        //        //------------------------------------------------------------
        //        // scrapy
        //        servicio.scrapyTarifas();

        //        // obtenemos las tarifas
        //        List<TarifaDTO> listaTarifas = servicio.verTarifasDelDia( fecha);

        //        //comprobamos
        //        Assert.AreEqual(listaTarifas.Count(), 24); //hay 24 horas
        //        // todas las fechas son la misma
        //        Assert.IsTrue((listaTarifas[0].fecha == fecha) == (listaTarifas[1].fecha == fecha) == (listaTarifas[2].fecha == fecha) == (listaTarifas[3].fecha == fecha) == (listaTarifas[4].fecha == fecha)
        //             == (listaTarifas[5].fecha == fecha) == (listaTarifas[6].fecha == fecha) == (listaTarifas[7].fecha == fecha) == (listaTarifas[8].fecha == fecha) == (listaTarifas[9].fecha == fecha)
        //              == (listaTarifas[10].fecha == fecha) == (listaTarifas[11].fecha == fecha) == (listaTarifas[12].fecha == fecha) == (listaTarifas[13].fecha == fecha) == (listaTarifas[14].fecha == fecha)
        //               == (listaTarifas[15].fecha == fecha) == (listaTarifas[16].fecha == fecha) == (listaTarifas[17].fecha == fecha) == (listaTarifas[18].fecha == fecha) == (listaTarifas[19].fecha == fecha)
        //                == (listaTarifas[20].fecha == fecha) == (listaTarifas[21].fecha == fecha) == (listaTarifas[22].fecha == fecha) == (listaTarifas[23].fecha == fecha));

        //        Assert.AreEqual(listaTarifas[0].hora, 0);
        //        Assert.AreEqual(listaTarifas[0].precio, p0);

        //        Assert.AreEqual(listaTarifas[1].hora, 1);
        //        Assert.AreEqual(listaTarifas[1].precio, p1);

        //        Assert.AreEqual(listaTarifas[2].hora, 2);
        //        Assert.AreEqual(listaTarifas[2].precio, p2);

        //        Assert.AreEqual(listaTarifas[3].hora, 3);
        //        Assert.AreEqual(listaTarifas[3].precio, p3);

        //        Assert.AreEqual(listaTarifas[4].hora, 4);
        //        Assert.AreEqual(listaTarifas[4].precio, p4);

        //        Assert.AreEqual(listaTarifas[5].hora, 5);
        //        Assert.AreEqual(listaTarifas[5].precio, p5);

        //        Assert.AreEqual(listaTarifas[6].hora, 6);
        //        Assert.AreEqual(listaTarifas[6].precio, p6);

        //        Assert.AreEqual(listaTarifas[7].hora, 7);
        //        Assert.AreEqual(listaTarifas[7].precio, p7);

        //        Assert.AreEqual(listaTarifas[8].hora, 8);
        //        Assert.AreEqual(listaTarifas[8].precio, p8);

        //        Assert.AreEqual(listaTarifas[9].hora, 9);
        //        Assert.AreEqual(listaTarifas[9].precio, p9);

        //        Assert.AreEqual(listaTarifas[10].hora, 10);
        //        Assert.AreEqual(listaTarifas[10].precio, p10);

        //        Assert.AreEqual(listaTarifas[11].hora, 11);
        //        Assert.AreEqual(listaTarifas[11].precio, p11);

        //        Assert.AreEqual(listaTarifas[12].hora, 12);
        //        Assert.AreEqual(listaTarifas[12].precio, p12);

        //        Assert.AreEqual(listaTarifas[13].hora, 13);
        //        Assert.AreEqual(listaTarifas[13].precio, p13);

        //        Assert.AreEqual(listaTarifas[14].hora, 14);
        //        Assert.AreEqual(listaTarifas[14].precio, p14);

        //        Assert.AreEqual(listaTarifas[15].hora, 15);
        //        Assert.AreEqual(listaTarifas[15].precio, p15);

        //        Assert.AreEqual(listaTarifas[16].hora, 16);
        //        Assert.AreEqual(listaTarifas[16].precio, p16);

        //        Assert.AreEqual(listaTarifas[17].hora, 17);
        //        Assert.AreEqual(listaTarifas[17].precio, p17);

        //        Assert.AreEqual(listaTarifas[18].hora, 18);
        //        Assert.AreEqual(listaTarifas[18].precio, p18);

        //        Assert.AreEqual(listaTarifas[19].hora, 19);
        //        Assert.AreEqual(listaTarifas[19].precio, p19);

        //        Assert.AreEqual(listaTarifas[20].hora, 20);
        //        Assert.AreEqual(listaTarifas[20].precio, p20);

        //        Assert.AreEqual(listaTarifas[21].hora, 21);
        //        Assert.AreEqual(listaTarifas[21].precio, p21);

        //        Assert.AreEqual(listaTarifas[22].hora, 22);
        //        Assert.AreEqual(listaTarifas[22].precio, p22);

        //        Assert.AreEqual(listaTarifas[23].hora, 23);
        //        Assert.AreEqual(listaTarifas[23].precio, p23);
        //    }
        //}
    }
}
