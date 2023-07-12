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
using Es.Udc.DotNet.TFG.Model.Daos.EstadoBateriaDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;

namespace Es.Udc.DotNet.TFG.Model.TarifaDao.Tests
{
    [TestClass()]
    public class EstadoBateriaEntityFrameworkTests
    {
        private static IKernel kernel;
        private static IEstadoBateriaDao estadoBateriaDao;
        private static IBateriaDao bateriaDao;
        private static IEstadoDao estadoDao;
        private static IUbicacionDao ubicacionDao;
        private static IUsuarioDao usuarioDao;

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
            estadoBateriaDao = kernel.Get<IEstadoBateriaDao>();
            bateriaDao = kernel.Get<IBateriaDao>();
            estadoDao = kernel.Get<IEstadoDao>();
            ubicacionDao = kernel.Get<IUbicacionDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();

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

            //CREAMOS LOS ESTADOS
            Estado cargando = new Estado();
            cargando.nombre = "Cargando";
            estadoDao.Create(cargando);

            Estado suministrando = new Estado();
            suministrando.nombre = "Suministrando";
            estadoDao.Create(suministrando);

            Estado sYC = new Estado();
            sYC.nombre = "Suministra y Carga";
            estadoDao.Create(sYC);

            // CREAMOS UBICACIONES
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            u.portal = 1;
            u.numero = 1;
            ubicacionDao.Create(u);


            //CREAMOS USUARIO
            Usuario user = new Usuario();
            user.nombre = "Dani";
            user.email = "micorreo@gmail.com";
            user.apellido1 = "Díaz";
            user.apellido2 = "González";
            user.contraseña = "unacontraseña";
            user.telefono = "981123456";
            user.pais = "España";
            user.idioma = "es-ES";
            usuarioDao.Create(user);


            //CREAMOS LA BATERIA
            Bateria b = new Bateria();
            b.ubicacionId = u.ubicacionId;
            b.usuarioId = user.usuarioId;
            b.precioMedio = 111;
            b.kwAlmacenados = 1000;
            b.almacenajeMaximoKw = 1000;
            b.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            b.marca = "MARCA 1";
            b.modelo = "MODELO 1";
            b.ratioCarga = 10;
            b.ratioCompra = 10;
            b.ratioUso = 10;
            bateriaDao.Create(b);

            //CREAMOS los ESTADOBATERIA
            SeEncuentra estadoBateria = new SeEncuentra();

            estadoBateria.bateriaId = b.bateriaId;
            estadoBateria.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            estadoBateria.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            estadoBateria.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            estadoBateria.estadoId = cargando.estadoId;
            estadoBateria.bateriaId = b.bateriaId;
            estadoBateriaDao.Create(estadoBateria);

            SeEncuentra estadoBateria2 = new SeEncuentra();

            estadoBateria2.bateriaId = b.bateriaId;
            estadoBateria2.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
            estadoBateria2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            estadoBateria2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            estadoBateria2.estadoId = suministrando.estadoId;
            estadoBateria2.bateriaId = b.bateriaId;
            estadoBateriaDao.Create(estadoBateria2);

            SeEncuentra estadoBateria3 = new SeEncuentra();

            estadoBateria3.bateriaId = b.bateriaId;
            estadoBateria3.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
            estadoBateria3.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            estadoBateria3.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            estadoBateria3.estadoId = sYC.estadoId;
            estadoBateria3.bateriaId = b.bateriaId;
            estadoBateriaDao.Create(estadoBateria3);

            SeEncuentra estadoBateria4 = new SeEncuentra();

            estadoBateria4.bateriaId = b.bateriaId;
            estadoBateria4.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(5).Day);
            estadoBateria4.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            estadoBateria4.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            estadoBateria4.estadoId = sYC.estadoId;
            estadoBateria4.bateriaId = b.bateriaId;
            estadoBateriaDao.Create(estadoBateria4);


            DateTime hoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime mañana = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
            DateTime pasado = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(2).Day);

            //COMPROBAMOS
            List<SeEncuentra> estados = estadoBateriaDao.MostrarEstadoBareriaPorFecha(mañana, pasado);
          
            Assert.AreEqual(estados[0].estadoId, estadoBateria2.estadoId);
            Assert.AreEqual(estados[1].estadoId, estadoBateria3.estadoId);
     
        }
        
        
    }
}