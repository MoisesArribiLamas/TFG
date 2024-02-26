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
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Daos.CargaDao;
using Es.Udc.DotNet.TFG.Model.Daos.SuministraDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using System.Threading;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceUbicacionTest
    {
        private static IKernel kernel;
        private static IServiceUbicacion servicio;
        private static IServiceBateria servicioBateria;
        private static IServiceEstado servicioEstado;
        private static IServiceTarifa servicioTarifa;

        private static IUbicacionDao ubicacionDao;
        private static IUsuarioDao usuarioDao;
        private static IBateriaDao bateriaDao;
        private static IConsumoDao consumoDao;
        private static ITarifaDao tarifaDao;
        private static ICargaDao cargaDao;
        private static ISuministraDao suministraDao;
        private static IEstadoDao estadoDao;
        

        //USUARIO

        public const string contraseña = "password";
        public const string nombre = "name";
        private const string apellido1 = "lastName";
        private const string apellido2 = "lastName";
        private const string email = "user@udc.es";
        private const string telefono = "123456789";
        private const string idioma = "es-ES";
        private const string pais = "Spain";
        private const string language = "es-ES";
        private const string country = "Spain";


        public long crearUsuario(string nombre, string email, string apellido1, string apellido2, string contraseña
            , string telefono, string pais, string idioma)
        {
            Usuario user = new Usuario();
            user.nombre = nombre;
            user.email = email;
            user.apellido1 = apellido1;
            user.apellido2 = apellido2;
            user.contraseña = contraseña;
            user.telefono = telefono;
            user.pais = pais;
            user.idioma = idioma;

            usuarioDao.Create(user);

            return user.usuarioId;
        }
        /*
         CONSUMO
             */

        public Consumo crearConsumo(long ubicacionId, double consumoActual, TimeSpan horaIni, TimeSpan horaFin, DateTime fechaActual)
        {

            Consumo c = new Consumo();

            c.consumoActual = consumoActual;
            c.kwCargados = 0;
            c.kwSuministrados = 0;
            c.kwRed = 0;
            c.fecha = fechaActual;
            c.horaIni = horaIni;
            c.horaFin = horaFin;
            c.ubicacionId = ubicacionId;

            consumoDao.Create(c);
            return c;


        }

        //UBICACION
        private const long codigoPostal = 15000;
        private const string localidad = "localidad";
        private const string calle = "calle";
        private const string portal = "portal";
        private const long numero = 1;
        private const string etiqueta = "Trastero";

        public long crearUbicacion(long codigoPostal, string localidad, string calle, string portal, long numero)
        {
            Ubicacion t = new Ubicacion();
            t.codigoPostal = codigoPostal;
            t.localidad = localidad;
            t.calle = calle;
            t.portal = portal;
            t.numero = numero;

            ubicacionDao.Create(t);
            return t.ubicacionId;
        }

        // BATERIA

        private const double precioMedio = 100;
        private const double kwHAlmacenados = 1000;
        private const double almacenajeMaximoKwH = 20000;
        private DateTime fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private const string marca = "marca";
        private const string modelo = "modelo";
        private const double ratioCarga = 40;
        private const double ratioCompra = 50;
        private const double ratioUso = 45;
        private const double capacidadCargador = 10;


        // TARIFA

        public long crearTarifa(long precio, long hora, DateTime fecha)
        {
            Tarifa t = new Tarifa();
            t.precio = precio;
            t.hora = hora;
            t.fecha = fecha;
            tarifaDao.Create(t);
            return t.tarifaId;
        }

        public void crearTarifas24H(DateTime fecha)
        {
            crearTarifa(110, 0, fecha);
            crearTarifa(111, 1, fecha);
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
            crearTarifa(2300, 23, fecha);
        }

        // ESTADOS

        public void crearEstados()
        {
            Estado estado = new Estado();
            estado.nombre = "sin actividad";
            estadoDao.Create(estado);

            Estado estado2 = new Estado();
            estado2.nombre = "cargando";
            estadoDao.Create(estado2);

            Estado estado3 = new Estado();
            estado3.nombre = "suministrando";
            estadoDao.Create(estado3);

            Estado estado4 = new Estado();
            estado4.nombre = "carga y suministra";
            estadoDao.Create(estado4);

        }
        //SUMINISTRA

        #region crear Suministra

        public long IniciarSuministra(long bateriaId, long tarifaId, double ahorro,
            TimeSpan horaIni)
        {
            // Se podria hacer poniendo el campo nullable pero me decante por esta forma
            int hour = 0;
            int minutes = 0;
            int seconds = 0;

            TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);

            Suministra s = new Suministra();
            s.bateriaId = bateriaId;
            s.tarifaId = tarifaId;
            s.ahorro = ahorro;
            s.horaIni = horaIni;
            s.horaFin = horaFin;
            s.kwH = 0;

            suministraDao.Create(s);
            return s.suministraId;

        }
        #endregion

        private const long NON_EXISTENT_USER_ID = -1;

        private UserProfileDetails userDetails = new UserProfileDetails(email, nombre, apellido1, apellido2, telefono, language, country);
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
            servicio = kernel.Get<IServiceUbicacion>();
            servicioBateria = kernel.Get<IServiceBateria>();
            servicioEstado = kernel.Get<IServiceEstado>();
            servicioTarifa = kernel.Get<IServiceTarifa>();

            ubicacionDao = kernel.Get<IUbicacionDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();
            bateriaDao = kernel.Get<IBateriaDao>();
            consumoDao = kernel.Get<IConsumoDao>();
            cargaDao = kernel.Get<ICargaDao>();
            suministraDao = kernel.Get<ISuministraDao>();
            tarifaDao = kernel.Get<ITarifaDao>();
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
        public void crearUbicacionTest()
        {
            using (var scope = new TransactionScope())
            {
                var ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);

                var ubicacionProfile = ubicacionDao.Find(ubicacionId);


                Assert.AreEqual(ubicacionId, ubicacionProfile.ubicacionId);
                Assert.AreEqual(codigoPostal, ubicacionProfile.codigoPostal);
                Assert.AreEqual(localidad, ubicacionProfile.localidad);
                Assert.AreEqual(calle, ubicacionProfile.calle);
                Assert.AreEqual(portal, ubicacionProfile.portal);
                Assert.AreEqual(numero, ubicacionProfile.numero);
                Assert.AreEqual(null, ubicacionProfile.bateriaSuministradora);
                Assert.AreEqual(null, ubicacionProfile.ultimoConsumo);;

            }
        }


        [TestMethod()]

        [ExpectedException(typeof(InstanceNotFoundException))]
        public void EliminarUbicacionTest()
        {
            using (var scope = new TransactionScope())
            {
                var ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);

                // comprobamos que existe
                var ubicacionProfile = ubicacionDao.Find(ubicacionId);

                // Eliminamos
                servicio.eliminarUbicacion( ubicacionId);

                // comprobamos que no existe
                var ubicacionProfile2 = ubicacionDao.Find(ubicacionId);

            }
        }

        [TestMethod()]
        public void modificarUbicacionTest()
        {
            using (var scope = new TransactionScope())
            {

                Ubicacion u = new Ubicacion();
                u.codigoPostal = 15401;
                u.localidad = "Ferrol";
                u.calle = "Real";
                u.portal = "D";
                u.numero = 2;
                u.etiqueta = "bateria principal";

                ubicacionDao.Create(u);

                Ubicacion u2 = new Ubicacion();
                u2.codigoPostal = 15009;
                u2.localidad = "Coruña";
                u2.calle = "Real";
                u2.portal = "B";
                u2.numero = 2;
                u2.etiqueta = "bateria auxiliar";

                ubicacionDao.Create(u2);

                servicio.modificarUbicacion(u.ubicacionId, u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero, u2.etiqueta);

                var obtained =
                    ubicacionDao.Find(u.ubicacionId);

                // Check changes
                Assert.AreEqual(u, obtained);
            }
        }


        [TestMethod()]
        public void CambiarBateriaSuministradoraTest()
        {
            using (var scope = new TransactionScope())
            {

                Ubicacion u = new Ubicacion();
                u.codigoPostal = 15401;
                u.localidad = "Ferrol";
                u.calle = "Real";
                u.portal = "D";
                u.numero = 2;
                u.etiqueta = "bateria principal";

                ubicacionDao.Create(u);

                //comprobamos que no tiene ninguna bateria suministrando
                Assert.AreEqual(u.bateriaSuministradora, null);

                //ponemos una bateria
                long? bateriaSuministradora = 2;
                servicio.CambiarBateriaSuministradora(u.ubicacionId,  bateriaSuministradora);

                var obtained =
                    ubicacionDao.Find(u.ubicacionId);

                // Check changes
                Assert.AreEqual(u.bateriaSuministradora, 2);
            }
        }

        [TestMethod()]
        public void verUbicacionesDeUnUsuarioTest()
        {
            using (var scope = new TransactionScope())
            {

                // CREAMOS UBICACIONES
                Ubicacion u = new Ubicacion();
                u.codigoPostal = 15405;
                u.localidad = "Ferrol";
                u.calle = "calle de Ferrol";
                u.portal = "A";
                u.numero = 1;
                ubicacionDao.Create(u);


                Ubicacion u2 = new Ubicacion();
                u2.localidad = "A Coruña";
                u2.codigoPostal = 15005;
                u2.calle = "calle de Coruña";
                u2.portal = "B";
                u2.numero = 1;
                ubicacionDao.Create(u2);

                //CREAMOS LOS USUARIOS
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

                Usuario user2 = new Usuario();
                user2.nombre = "María";
                user2.contraseña = "nos olvidamos ups";
                user2.email = "micorreo@gmail.com";
                user2.apellido1 = "Pérez";
                user2.apellido2 = "Fernández";
                user2.telefono = "981123457";
                user2.idioma = "es-ES";
                user2.pais = "España";
                usuarioDao.Create(user2);

                Usuario user3 = new Usuario();
                user3.nombre = "María";
                user3.contraseña = "nos olvidamos ups";
                user3.email = "micorreo@gmail.com";
                user3.apellido1 = "Pérez";
                user3.apellido2 = "Fernández";
                user3.telefono = "981123457";
                user3.idioma = "es-ES";
                user3.pais = "España";
                usuarioDao.Create(user3);

                //CREAMOS LA BATERIA
                Bateria b = new Bateria();
                b.ubicacionId = u.ubicacionId;
                b.usuarioId = user.usuarioId;
                b.precioMedio = 111;
                b.kwHAlmacenados = 1000;
                b.almacenajeMaximoKwH = 1000;
                b.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b.marca = "MARCA 1";
                b.modelo = "MODELO 1";
                b.ratioCarga = 10;
                b.ratioCompra = 10;
                b.ratioUso = 10;
                bateriaDao.Create(b);

                Bateria b2 = new Bateria();
                b2.ubicacionId = u2.ubicacionId;
                b2.usuarioId = user2.usuarioId;
                b2.precioMedio = 222;
                b2.kwHAlmacenados = 2000;
                b2.almacenajeMaximoKwH = 2000;
                b2.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b2.marca = "MARCA 2";
                b2.modelo = "MODELO 2";
                b2.ratioCarga = 20;
                b2.ratioCompra = 20;
                b2.ratioUso = 20;
                bateriaDao.Create(b2);

                //MISMA UBICACION Y MISMO USUARIO QUE LA B2
                Bateria b3 = new Bateria();
                b3.ubicacionId = u2.ubicacionId;
                b3.usuarioId = user2.usuarioId;
                b3.precioMedio = 222;
                b3.kwHAlmacenados = 2000;
                b3.almacenajeMaximoKwH = 2000;
                b3.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                b3.marca = "MARCA 2";
                b3.modelo = "MODELO 2";
                b3.ratioCarga = 20;
                b3.ratioCompra = 20;
                b3.ratioUso = 20;
                bateriaDao.Create(b3);

                int count = 2;
                int startOfIndex = 0;



                List<UbicacionProfileDetails> obteined = servicio.verUbicaciones(user.usuarioId, startOfIndex, count);
                List<UbicacionProfileDetails> obteined2 = servicio.verUbicaciones(user2.usuarioId, startOfIndex, count);



                UbicacionProfileDetails o1 = new UbicacionProfileDetails(u.ubicacionId, u.codigoPostal, u.localidad, u.calle, u.portal, u.numero);
                UbicacionProfileDetails o2 = new UbicacionProfileDetails(u2.ubicacionId, u2.codigoPostal, u2.localidad, u2.calle, u2.portal, u2.numero);


                //COMPROBAMOS


                Assert.AreEqual(obteined[0], o1);
                Assert.AreEqual(obteined.Count, 1);
                Assert.AreEqual(obteined2[0], o2);
                Assert.AreEqual(obteined2.Count, 1);
            }
        }

        [TestMethod()]
        public void buscarcarUbicacionByIdTest()
        {
            using (var scope = new TransactionScope())
            {

                Ubicacion u = new Ubicacion();
                u.codigoPostal = 15401;
                u.localidad = "Ferrol";
                u.calle = "Real";
                u.portal = "D";
                u.numero = 2;
                u.etiqueta = "bateria principal";

                ubicacionDao.Create(u);

                Ubicacion u2 = new Ubicacion();
                u2.codigoPostal = 15009;
                u2.localidad = "Coruña";
                u2.calle = "Real";
                u2.portal = "B";
                u2.numero = 2;
                u2.etiqueta = "bateria auxiliar";

                ubicacionDao.Create(u2);

                Ubicacion obtained = servicio.buscarUbicacionById(u.ubicacionId);
                Ubicacion obtained2 = servicio.buscarUbicacionById(u2.ubicacionId);


                // Check changes
                Assert.AreEqual(u, obtained);
                Assert.AreEqual(u2, obtained2);
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void buscarcarUbicacionById2Test()
        {
            using (var scope = new TransactionScope())
            {

                Ubicacion u = new Ubicacion();
                u.codigoPostal = 15401;
                u.localidad = "Ferrol";
                u.calle = "Real";
                u.portal = "D";
                u.numero = 2;
                u.etiqueta = "bateria principal";

                ubicacionDao.Create(u);

                long idFalso = u.ubicacionId + 10;

                // le pasamos un ID falso para ver que falla
                Ubicacion obtained = servicio.buscarUbicacionById(idFalso);
               
            }
        }

        [TestMethod()]
        public void crearConsumoTest()
        {
            using (var scope = new TransactionScope())
            {

                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                
                double consumoActual = 1000; 
                // Fecha y hora actual
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumoActual, horaActual); 

                //buscamos el consumo creado
                Consumo consumonProfile = consumoDao.Find(consumoId);

                Assert.AreEqual(ubicacionId, consumonProfile.ubicacionId);
                Assert.AreEqual(consumoActual, consumonProfile.consumoActual);
                Assert.AreEqual(consumoId, consumonProfile.consumoId);
                Assert.AreEqual(fechaActual, consumonProfile.fecha);
                Assert.AreEqual(horaActual, consumonProfile.horaIni);
                Assert.AreEqual(null, consumonProfile.horaFin);
                Assert.AreEqual(0, consumonProfile.kwCargados);
                Assert.AreEqual(0, consumonProfile.kwSuministrados);
                Assert.AreEqual(0, consumonProfile.kwRed);

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId, u.ultimoConsumo);

            }
        }


        [TestMethod()]
        public void calcularConsumoTest()
        {
            using (var scope = new TransactionScope())
            {

                double consumoActual = 360;
                int hour = 1;
                int minutes = 0;
                int seconds = 0;

                // hora actual
                TimeSpan horaIni = new TimeSpan(hour, minutes, seconds);
                TimeSpan h2 = new TimeSpan(hour, minutes, seconds+10);
                TimeSpan h3 = new TimeSpan(hour, minutes+10, seconds);
                TimeSpan h4 = new TimeSpan(hour, minutes + 10, seconds+10);

                //calculamos el consumo

                double c2 = servicio.calcularConsumo(consumoActual, horaIni, h2);
                double c3 = servicio.calcularConsumo(consumoActual, horaIni, h3);
                double c4 = servicio.calcularConsumo(consumoActual, horaIni, h4);

                //comprobamos

                // 360*10/3600 = 1
                Assert.AreEqual(c2, 1);
                // 360*600/3600 = 60
                Assert.AreEqual(c3, 60);
                // 360*610/3600 = 61
                Assert.AreEqual(c4, 61);



            }
        }

        [TestMethod()]
        public void finalizarConsumoTest()
        {
            using (var scope = new TransactionScope())
            {

                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);

                double consumoActual = 1000;
                // Fecha y hora actual
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan horaInicio = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumoActual, horaInicio);

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId, u.ultimoConsumo);

                //finalizamos el consumo creado
                string estado = "sin actividad";
                TimeSpan horafinal = horaInicio;

                long bateriaSuministradora = 0;
                
                servicio.finalizarConsumo(ubicacionId, consumoActual, horafinal, estado, bateriaSuministradora);

                //buscamos el consumo
                Consumo consumonProfile = consumoDao.Find(consumoId);

                Assert.AreEqual(ubicacionId, consumonProfile.ubicacionId);
                Assert.AreEqual(consumoActual, consumonProfile.consumoActual);
                Assert.AreEqual(consumoId, consumonProfile.consumoId);
                Assert.AreEqual(fechaActual, consumonProfile.fecha);
                Assert.AreEqual(horaInicio, consumonProfile.horaIni);
                Assert.AreEqual(horafinal, consumonProfile.horaFin);

            }
        }

        [TestMethod()]
        public void ModificarConsumoSinActividadTest()
        {
            using (var scope = new TransactionScope())
            {

                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                double consumo = 1000;
                
                int hour1 = 1;
                //int hour2 = 1;
                int minutes = 1;
                int seconds = 1;
                //int seconds2 = 2;

                TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);
                //TimeSpan horafinal = new TimeSpan(hour2, minutes, seconds2);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);

                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual = 2000;

                TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoIdNuevo = servicio.modificarConsumoActual(ubicacionId, consumoActual);

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoIdNuevo, u.ultimoConsumo);

                // buscamos el primer consumo
                Consumo consumo1 = consumoDao.Find(consumoId);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo1.ubicacionId);
                Assert.AreEqual(consumo, consumo1.consumoActual);
                Assert.AreEqual(consumoId, consumo1.consumoId);
                Assert.AreEqual(fechaActual, consumo1.fecha);
                Assert.AreEqual(horaInicio, consumo1.horaIni);
                Assert.AreEqual(horafinal, consumo1.horaFin);

                // buscamos el consumo (entidad) actual
                Consumo consumo2 = consumoDao.UltimoConsumoUbicacion(ubicacionId);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumoActual, consumo2.consumoActual);
                Assert.AreEqual(consumoIdNuevo, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(null, consumo2.horaFin);


                // obtenemos la Carga
                Carga c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c, null);

                // obtenemos Suministra
                Suministra s = servicioBateria.UltimaSuministra(bateriaId);
                
                //comprobamos los cambios
                Assert.AreEqual(s, null);
            }
        }

        [TestMethod()]
        public void ModificarConsumoCargandoTest()
        {
            using (var scope = new TransactionScope())
            {

                //private const double precioMedio = 100;
                //private const double kwHAlmacenados = 1000;
                //private const double almacenajeMaximoKwH = 20000;
                //private const double ratioCarga = 10;
                //private const double ratioCompra = 50;
                //private const double ratioUso = 45;
                //private const double capacidadCargador = 10;
                double kwHAlmacenados = 10000;


                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                //Ponemos el estado a "Cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("Cargando");
                Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
                
                // -> cargando
                servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdC, 0, 0);

                double consumo = 1000;

                int hour1 = 1;
                //int hour2 = 1;
                int minutes = 0;
                int seconds = 0;
                //int seconds2 = 2;

                TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);
                //TimeSpan horafinal = new TimeSpan(hour2, minutes, seconds2);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);

                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual2 = 2000;

                TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoId2 = servicio.modificarConsumoActual(ubicacionId, consumoActual2);        //consumo 1000 -> 2000

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId2, u.ultimoConsumo);

                //+++++++
                double capacidadCarga = servicioBateria.capacidadCargadorBateriaSuministradora(bateriaId);
                double kwCargados = servicio.calcularConsumo(capacidadCarga, horaInicio, horafinal);
                double kwRed = servicio.calcularConsumo(consumo, horaInicio, horafinal);
                //++++++++++++

                // buscamos el primer consumo
                Consumo consumo1 = consumoDao.Find(consumoId);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo1.ubicacionId);
                Assert.AreEqual(consumo, consumo1.consumoActual);
                Assert.AreEqual(consumoId, consumo1.consumoId);
                Assert.AreEqual(fechaActual, consumo1.fecha);
                Assert.AreEqual(horaInicio, consumo1.horaIni);
                Assert.AreEqual(horafinal, consumo1.horaFin);
                Assert.AreEqual(kwCargados, consumo1.kwCargados);
                Assert.AreEqual(0, consumo1.kwSuministrados);
                Assert.AreEqual(kwRed, consumo1.kwRed);

                // buscamos el consumo (entidad) actual
                long ucId2 = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo2 = servicio.buscarConsumoById(ucId2);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumoActual2, consumo2.consumoActual);
                Assert.AreEqual(consumoId2, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(null, consumo2.horaFin);
                Assert.AreEqual(0, consumo2.kwCargados);
                Assert.AreEqual(0, consumo2.kwSuministrados);
                Assert.AreEqual(0, consumo2.kwRed);


                // obtenemos la Carga
                Carga c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c.kwH, kwCargados);

                // obtenemos Suministra
                Suministra s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s, null);

                //++++++++++++++++++++++++++++++
                // tercer consumo,
                // segundo consumo en "Cargando"
                //++++++++++++++++++++++++++++++
                Thread.Sleep(1000);
                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual3 = 1000;

                TimeSpan horafinal2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoId3 = servicio.modificarConsumoActual(ubicacionId, consumoActual3);  //consumo 2000 -> 1000

                // buscamos ubicacion
                u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId3, u.ultimoConsumo);

                //+++++++
                double kwCargados2 = servicio.calcularConsumo(capacidadCarga, horafinal, horafinal2);
                double kwRed2 = servicio.calcularConsumo(consumoActual2, horafinal, horafinal2);
                //++++++++++++

                // buscamos el segundo consumo
                consumo2 = consumoDao.Find(consumoId2);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumoActual2, consumo2.consumoActual);
                Assert.AreEqual(consumoId2, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(horafinal2, consumo2.horaFin);
                Assert.AreEqual(kwCargados2, consumo2.kwCargados);
                Assert.AreEqual(0, consumo2.kwSuministrados);
                Assert.AreEqual(kwRed2, consumo2.kwRed);

                // buscamos el consumo (entidad) actual
                long ucId3 = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo3 = servicio.buscarConsumoById(ucId3);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo3.ubicacionId);
                Assert.AreEqual(consumoActual3, consumo3.consumoActual);
                Assert.AreEqual(consumoId3, consumo3.consumoId);
                Assert.AreEqual(fechaActual, consumo3.fecha);
                Assert.AreEqual(horafinal2, consumo3.horaIni);
                Assert.AreEqual(null, consumo3.horaFin);
                Assert.AreEqual(0, consumo3.kwCargados);
                Assert.AreEqual(0, consumo3.kwSuministrados);
                Assert.AreEqual(0, consumo3.kwRed);


                // obtenemos la Carga
                c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c.kwH, kwCargados + kwCargados2);

                // obtenemos Suministra
                s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s, null);
            }
        }


        [TestMethod()]
        public void ModificarConsumoSuministrandoTest()
        {
            using (var scope = new TransactionScope())
            {

                //private const double precioMedio = 100;
                //private const double kwHAlmacenados = 1000;
                //private const double almacenajeMaximoKwH = 20000;
                //private const double ratioCarga = 10;
                //private const double ratioCompra = 50;
                //private const double ratioUso = 45;
                //private const double capacidadCargador = 10;
                double kwHAlmacenados = 10000;


                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);

                // -> cargando
                servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                double consumo = 1000;

                int hour1 = 1;
                //int hour2 = 1;
                int minutes = 0;
                int seconds = 0;
                //int seconds2 = 2;

                TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);
                //TimeSpan horafinal = new TimeSpan(hour2, minutes, seconds2);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);

                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual2 = 2000;

                TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoId2 = servicio.modificarConsumoActual(ubicacionId, consumoActual2);        //consumo 1000 -> 2000

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId2, u.ultimoConsumo);

                //+++++++
                //double capacidadCarga = servicioBateria.capacidadCargadorBateriaSuministradora(bateriaId);
                //double kwCargados = servicio.calcularConsumo(capacidadCarga, horaInicio, horafinal);
                double kwSuministrados = servicio.calcularConsumo(consumo, horaInicio, horafinal);
                //double kwRed = servicio.calcularConsumo(consumo, horaInicio, horafinal);
                //++++++++++++

                // buscamos el primer consumo
                Consumo consumo1 = consumoDao.Find(consumoId);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo1.ubicacionId);
                Assert.AreEqual(consumo, consumo1.consumoActual);
                Assert.AreEqual(consumoId, consumo1.consumoId);
                Assert.AreEqual(fechaActual, consumo1.fecha);
                Assert.AreEqual(horaInicio, consumo1.horaIni);
                Assert.AreEqual(horafinal, consumo1.horaFin);
                Assert.AreEqual(0, consumo1.kwCargados);
                Assert.AreEqual(kwSuministrados, consumo1.kwSuministrados);
                Assert.AreEqual(0, consumo1.kwRed);

                // buscamos el consumo (entidad) actual
                long ucId2 = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo2 = servicio.buscarConsumoById(ucId2);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumoActual2, consumo2.consumoActual);
                Assert.AreEqual(consumoId2, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(null, consumo2.horaFin);
                Assert.AreEqual(0, consumo2.kwCargados);
                Assert.AreEqual(0, consumo2.kwSuministrados);
                Assert.AreEqual(0, consumo2.kwRed);


                // obtenemos la Carga
                Carga c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c, null);

                // obtenemos Suministra
                Suministra s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s.kwH, kwSuministrados);

                //++++++++++++++++++++++++++++++
                // tercer consumo,
                // segundo consumo en "Cargando"
                //++++++++++++++++++++++++++++++
                Thread.Sleep(1000);
                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual3 = 1000;

                TimeSpan horafinal2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoId3 = servicio.modificarConsumoActual(ubicacionId, consumoActual3);  //consumo 2000 -> 1000

                // buscamos ubicacion
                u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId3, u.ultimoConsumo);

                //+++++++
                //double kwCargados2 = servicio.calcularConsumo(capacidadCarga, horafinal, horafinal2);
                double kwSuministrados2 = servicio.calcularConsumo(consumoActual2, horafinal, horafinal2);
                //double kwRed2 = servicio.calcularConsumo(consumoActual2, horafinal, horafinal2);
                //++++++++++++

                // buscamos el segundo consumo
                consumo2 = consumoDao.Find(consumoId2);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumoActual2, consumo2.consumoActual);
                Assert.AreEqual(consumoId2, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(horafinal2, consumo2.horaFin);
                Assert.AreEqual(0, consumo2.kwCargados);
                Assert.AreEqual(kwSuministrados2, consumo2.kwSuministrados);
                Assert.AreEqual(0, consumo2.kwRed);

                // buscamos el consumo (entidad) actual
                long ucId3 = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo3 = servicio.buscarConsumoById(ucId3);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo3.ubicacionId);
                Assert.AreEqual(consumoActual3, consumo3.consumoActual);
                Assert.AreEqual(consumoId3, consumo3.consumoId);
                Assert.AreEqual(fechaActual, consumo3.fecha);
                Assert.AreEqual(horafinal2, consumo3.horaIni);
                Assert.AreEqual(null, consumo3.horaFin);
                Assert.AreEqual(0, consumo3.kwCargados);
                Assert.AreEqual(0, consumo3.kwSuministrados);
                Assert.AreEqual(0, consumo3.kwRed);


                // obtenemos la Carga
                c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c, null);

                // obtenemos Suministra
                s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s.kwH, kwSuministrados + kwSuministrados2);
            }
        }


        [TestMethod()]
        public void ModificarConsumoCargaYSuministraTest()
        {
            using (var scope = new TransactionScope())
            {

                //private const double precioMedio = 100;
                //private const double kwHAlmacenados = 1000;
                //private const double almacenajeMaximoKwH = 20000;
                //private const double ratioCarga = 10;
                //private const double ratioCompra = 50;
                //private const double ratioUso = 45;
                //private const double capacidadCargador = 10;
                double kwHAlmacenados = 10000;


                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);

                // -> cargando
                servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                double consumo = 1000;

                int hour1 = 1;
                //int hour2 = 1;
                int minutes = 0;
                int seconds = 0;
                //int seconds2 = 2;

                TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);
                //TimeSpan horafinal = new TimeSpan(hour2, minutes, seconds2);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);

                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual2 = 2000;

                TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoId2 = servicio.modificarConsumoActual(ubicacionId, consumoActual2);        //consumo 1000 -> 2000

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId2, u.ultimoConsumo);

                //+++++++
                double capacidadCarga = servicioBateria.capacidadCargadorBateriaSuministradora(bateriaId);
                double kwCargados = servicio.calcularConsumo(capacidadCarga, horaInicio, horafinal);
                double kwSuministrados = servicio.calcularConsumo(consumo, horaInicio, horafinal);
                //double kwRed = servicio.calcularConsumo(consumo, horaInicio, horafinal);
                //++++++++++++

                // buscamos el primer consumo
                Consumo consumo1 = consumoDao.Find(consumoId);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo1.ubicacionId);
                Assert.AreEqual(consumo, consumo1.consumoActual);
                Assert.AreEqual(consumoId, consumo1.consumoId);
                Assert.AreEqual(fechaActual, consumo1.fecha);
                Assert.AreEqual(horaInicio, consumo1.horaIni);
                Assert.AreEqual(horafinal, consumo1.horaFin);
                Assert.AreEqual(kwCargados, consumo1.kwCargados);
                Assert.AreEqual(kwSuministrados, consumo1.kwSuministrados);
                Assert.AreEqual(0, consumo1.kwRed);

                // buscamos el consumo (entidad) actual
                long ucId2 = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo2 = servicio.buscarConsumoById(ucId2);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumoActual2, consumo2.consumoActual);
                Assert.AreEqual(consumoId2, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(null, consumo2.horaFin);
                Assert.AreEqual(0, consumo2.kwCargados);
                Assert.AreEqual(0, consumo2.kwSuministrados);
                Assert.AreEqual(0, consumo2.kwRed);


                // obtenemos la Carga
                Carga c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c.kwH, kwCargados);

                // obtenemos Suministra
                Suministra s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s.kwH, kwSuministrados);

                //++++++++++++++++++++++++++++++
                // tercer consumo,
                // segundo consumo en "Cargando"
                //++++++++++++++++++++++++++++++
                Thread.Sleep(1000);
                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual3 = 1000;

                TimeSpan horafinal2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoId3 = servicio.modificarConsumoActual(ubicacionId, consumoActual3);  //consumo 2000 -> 1000

                // buscamos ubicacion
                u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId3, u.ultimoConsumo);

                //+++++++
                double kwCargados2 = servicio.calcularConsumo(capacidadCarga, horafinal, horafinal2);
                double kwSuministrados2 = servicio.calcularConsumo(consumoActual2, horafinal, horafinal2);
                //double kwRed2 = servicio.calcularConsumo(consumoActual2, horafinal, horafinal2);
                //++++++++++++

                // buscamos el segundo consumo
                consumo2 = consumoDao.Find(consumoId2);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumoActual2, consumo2.consumoActual);
                Assert.AreEqual(consumoId2, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(horafinal2, consumo2.horaFin);
                Assert.AreEqual(kwCargados2, consumo2.kwCargados);
                Assert.AreEqual(kwSuministrados2, consumo2.kwSuministrados);
                Assert.AreEqual(0, consumo2.kwRed);

                // buscamos el consumo (entidad) actual
                long ucId3 = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo3 = servicio.buscarConsumoById(ucId3);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo3.ubicacionId);
                Assert.AreEqual(consumoActual3, consumo3.consumoActual);
                Assert.AreEqual(consumoId3, consumo3.consumoId);
                Assert.AreEqual(fechaActual, consumo3.fecha);
                Assert.AreEqual(horafinal2, consumo3.horaIni);
                Assert.AreEqual(null, consumo3.horaFin);
                Assert.AreEqual(0, consumo3.kwCargados);
                Assert.AreEqual(0, consumo3.kwSuministrados);
                Assert.AreEqual(0, consumo3.kwRed);


                // obtenemos la Carga
                c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c.kwH, kwCargados + kwCargados2);

                // obtenemos Suministra
                s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s.kwH, kwSuministrados + kwSuministrados2);
            }
        }


        [TestMethod()]
        public void ConsumoActualUbicacionActualTest()
        {
            using (var scope = new TransactionScope())
            {

                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                //---------------------

                //long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);

                double consumo = 1000;
                // Hora actual
                TimeSpan horaInicio = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);

                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual = 2000;
                TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoIdNuevo = servicio.modificarConsumoActual(ubicacionId, consumoActual);

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoIdNuevo, u.ultimoConsumo);

                // buscamos el primer consumo
                Consumo consumo1 = consumoDao.Find(consumoId);

                // buscamos el consumo (entidad) actual
                //Consumo consumo2 = consumoDao.UltimoConsumoUbicacion(ubicacionId);
                long ucId = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo2 = servicio.buscarConsumoById(ucId);

                // Buscamos el ultimo consumo
                long consumoNuevo = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);


                // comprobamos los cambios en consumo2
                Assert.AreEqual(consumoNuevo, consumo2.consumoId);

            }
        }

        [TestMethod()]
        public void ObtenerCapacidadCargadorBateriaSuministradoraTest()
        {
            using (var scope = new TransactionScope())
            {

                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador+2);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                //comprobamos capacidadCargador
                double capacidadC = servicio.obtenerCapacidadCargadorBateriaSuministradora(ubicacionId);
                Assert.AreEqual(capacidadC, capacidadCargador);

                // Cambiamos de bateria
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId2);

                //comprobamos capacidadCargador
                 capacidadC = servicio.obtenerCapacidadCargadorBateriaSuministradora(ubicacionId);
                Assert.AreEqual(capacidadC, capacidadCargador+2);

            }
        }

        [TestMethod()]

        [ExpectedException(typeof(InstanceNotFoundException))]
        public void EliminarUbicacion2Test()
        {
            using (var scope = new TransactionScope())
            {
                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                //--------------------------------------

                double consumo = 1000;
                // Fecha y hora actual
                //DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan horaInicio = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);

                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual = 2000;
                TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoIdNuevo = servicio.modificarConsumoActual(ubicacionId, consumoActual);

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoIdNuevo, u.ultimoConsumo);

                // buscamos el primer consumo
                Consumo consumo1 = consumoDao.Find(consumoId);

                // buscamos el consumo (entidad) actual
                Consumo consumo2 = consumoDao.UltimoConsumoUbicacion(ubicacionId);

                long consumo2Id = consumo2.consumoId;


                // Eliminamos
                servicio.eliminarUbicacion(ubicacionId);

                // comprobamos que no existe un consumo asociado previamente la a ubicacion eliminada
                var ubicacionProfile2 = ubicacionDao.Find(consumo2Id);

            }
        }


        [TestMethod()]
        public void MostrarCargasBareriaPorFechaTest()
        {
            using (var scope = new TransactionScope())
            {

                // Creamos Ubicacion
                long codigoPostal = 15000;
                string localidad = "Coruña";
                string calle = "San Juan";
                string portal = "";
                long numero = 100;
                //string etiqueta = "bichito";
                //long bateriaSuministradora = 1;
                //crearUbicacion(long codigoPostal, string localidad, string calle, string portal, long numero)
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                TimeSpan tresMinutos = new TimeSpan(0, 3, 0);
                TimeSpan cincoMinutos = new TimeSpan(0, 5, 0);

                // Creamos Consumos
                double consumoActual = 10;
                //double kwCargados = 100;
                //double kwSuministrados = 100;
                //double kwRed = 0;
                DateTime fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                TimeSpan horaFin = horaIni.Add(tresMinutos);


                Consumo c0 = crearConsumo(ubicacionId, consumoActual, horaIni, horaFin, fecha); //dia 0

                DateTime fechaBusquedaInicial = fecha = fecha.AddDays(1); // dia siguiente

                Consumo c1 = crearConsumo(ubicacionId, consumoActual, horaIni, horaFin, fecha); //dia 1

                consumoActual = 15;
                TimeSpan horaFin2 = horaIni.Add(cincoMinutos);

                // consumo 2
                Consumo c2 = crearConsumo(ubicacionId, consumoActual, horaFin, horaFin2, fecha); //dia 1


                DateTime fechaBusquedaFinal = fecha = fecha.AddDays(1); // dia siguiente

                // consumo 3
                Consumo c3 = crearConsumo(ubicacionId, consumoActual, horaIni, horaFin, fecha); //dia 2

                fecha = fecha.AddDays(1); // dia siguiente

                // consumo 4
                Consumo c4 = crearConsumo(ubicacionId, consumoActual, horaIni, horaFin, fecha); //dia 3

                fecha = fecha.AddDays(1); // dia siguiente

                // consumo 5
                Consumo c5 = crearConsumo(ubicacionId, consumoActual, horaIni, horaFin, fecha); //dia 4

                //COMPROBAMOS   
                int startIndex = 0;
                int count = 5;
                fecha = fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = fecha.AddDays(1);
                List<Consumo> consumoResult = consumoDao.MostrarConsumosUbicacionPorFecha(ubicacionId, fechaBusquedaInicial, fechaBusquedaFinal, startIndex, count);

                //public List<ConsumoDTO> MostrarCargasBareriaPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2, int startIndex, int count)

                Assert.AreEqual(consumoResult[0], c1);
                Assert.AreEqual(consumoResult[1], c2);
                Assert.AreEqual(consumoResult[2], c3);
                Assert.AreEqual(consumoResult.Count(), 3);
                

            }
        }

        //[TestMethod()]
        //public void CalcularConsumoParaCalculoRatiosSinActividadTest()
        //{
        //    using (var scope = new TransactionScope())
        //    {

        //        //private const double precioMedio = 100;
        //        //private const double kwHAlmacenados = 1000;
        //        //private const double almacenajeMaximoKwH = 20000;
        //        //private const double ratioCarga = 10;
        //        //private const double ratioCompra = 50;
        //        //private const double ratioUso = 45;
        //        //private const double capacidadCargador = 10;
        //        double kwHAlmacenados = 10000;


        //        //Creamos los estados usuario y ubicacion
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
        //        //Creamos Tarifa
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        // Ponemos la bateria en la ubicacion
        //        servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);

        //        // -> "sin actividad"
        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

        //        double consumo = 1000;

        //        int hour1 = 1;
        //        //int hour2 = 1;
        //        int minutes = 0;
        //        int seconds = 0;
        //        //int seconds2 = 2;

        //        TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);
        //        //TimeSpan horafinal = new TimeSpan(hour2, minutes, seconds2);

        //        //creamos el consumo
        //        long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);


        //        TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


        //        // buscamos el consumo
        //        double consumoN = servicio.CalcularConsumoParaCalculoRatios(ubicacionId, consumo, horafinal, "sin actividad", bateriaId);

        //        // comprobamos  "sin actividad" => 0 
        //        Assert.AreEqual(0, consumoN);

        //    }
        //}

        //[TestMethod()]
        //public void CalcularConsumoParaCalculoRatiosCargandoTest()
        //{
        //    using (var scope = new TransactionScope())
        //    {

        //        //private const double precioMedio = 100;
        //        //private const double kwHAlmacenados = 1000;
        //        //private const double almacenajeMaximoKwH = 20000;
        //        //private const double ratioCarga = 10;
        //        //private const double ratioCompra = 50;
        //        //private const double ratioUso = 45;
        //        //private const double capacidadCargador = 10;
        //        double kwHAlmacenados = 10000;


        //        //Creamos los estados usuario y ubicacion
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
        //        //Creamos Tarifa
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        // Ponemos la bateria en la ubicacion
        //        servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);

        //        // -> "cargando"
        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

        //        double consumo = 1000;

        //        int hour1 = 1;
        //        int minutes = 0;
        //        int seconds = 0;

        //        TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);

        //        //creamos el consumo
        //        long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);


        //        TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


        //        //+++++++
        //        double capacidadCarga = servicioBateria.capacidadCargadorBateriaSuministradora(bateriaId);
        //        double kwCargados = servicio.calcularConsumo(capacidadCarga, horaInicio, horafinal);
        //        double kwSuministrados = servicio.calcularConsumo(consumo, horaInicio, horafinal);
        //        //++++++++++++

        //        // buscamos el consumo
        //        double consumoC = servicio.CalcularConsumoParaCalculoRatios(ubicacionId, consumo, horafinal, "cargando", bateriaId);

        //        // comprobamos 
        //        Assert.AreEqual(kwCargados, consumoC);

        //    }
        //}

        //[TestMethod()]
        //public void CalcularConsumoParaCalculoRatiosSuministrandoTest()
        //{
        //    using (var scope = new TransactionScope())
        //    {

        //        //private const double precioMedio = 100;
        //        //private const double kwHAlmacenados = 1000;
        //        //private const double almacenajeMaximoKwH = 20000;
        //        //private const double ratioCarga = 10;
        //        //private const double ratioCompra = 50;
        //        //private const double ratioUso = 45;
        //        //private const double capacidadCargador = 10;
        //        double kwHAlmacenados = 10000;


        //        //Creamos los estados usuario y ubicacion
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
        //        //Creamos Tarifa
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        // Ponemos la bateria en la ubicacion
        //        servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);

        //        // -> "suministrando"
        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

        //        double consumo = 1000;

        //        int hour1 = 1;
        //        int minutes = 0;
        //        int seconds = 0;

        //        TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);

        //        //creamos el consumo
        //        long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);


        //        TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


        //        //+++++++
        //        double capacidadCarga = servicioBateria.capacidadCargadorBateriaSuministradora(bateriaId);
        //        double kwCargados = servicio.calcularConsumo(capacidadCarga, horaInicio, horafinal);
        //        double kwSuministrados = servicio.calcularConsumo(consumo, horaInicio, horafinal);
        //        //++++++++++++

        //        // buscamos el consumo
        //        double consumoS = servicio.CalcularConsumoParaCalculoRatios(ubicacionId, consumo, horafinal, "suministrando", bateriaId);

        //        // comprobamos 
        //        Assert.AreEqual(-kwSuministrados, consumoS);

        //    }
        //}

        //[TestMethod()]
        //public void CalcularConsumoParaCalculoRatiosCargaYSuministraTest()
        //{
        //    using (var scope = new TransactionScope())
        //    {

        //        //private const double precioMedio = 100;
        //        //private const double kwHAlmacenados = 1000;
        //        //private const double almacenajeMaximoKwH = 20000;
        //        //private const double ratioCarga = 10;
        //        //private const double ratioCompra = 50;
        //        //private const double ratioUso = 45;
        //        //private const double capacidadCargador = 10;
        //        double kwHAlmacenados = 10000;


        //        //Creamos los estados usuario y ubicacion
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
        //        //Creamos Tarifa
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        // Ponemos la bateria en la ubicacion
        //        servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);

        //        // -> "carga y suministra"
        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

        //        double consumo = 1000;

        //        int hour1 = 1;
        //        int minutes = 0;
        //        int seconds = 0;

        //        TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);

        //        //creamos el consumo
        //        long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);


        //        TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


        //        //+++++++
        //        double capacidadCarga = servicioBateria.capacidadCargadorBateriaSuministradora(bateriaId);
        //        double kwCargados = servicio.calcularConsumo(capacidadCarga, horaInicio, horafinal);
        //        double kwSuministrados = servicio.calcularConsumo(consumo, horaInicio, horafinal);
        //        //++++++++++++

        //        // buscamos el consumo
        //        double consumoS = servicio.CalcularConsumoParaCalculoRatios(ubicacionId, consumo, horafinal, "carga y suministra", bateriaId);

        //        // comprobamos 
        //        Assert.AreEqual(kwCargados - kwSuministrados, consumoS);

        //    }
        //}

        [TestMethod()]
        public void ActualizarConsumoActualSinActividadTest()
        {
            using (var scope = new TransactionScope())
            {

                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                double consumo = 1000;

                int hour1 = 1;
                int minutes = 1;
                int seconds = 1;

                TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);

                TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoIdNuevo = servicio.actualizarConsumoActual(ubicacionId);

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoIdNuevo, u.ultimoConsumo);

                // buscamos el primer consumo
                Consumo consumo1 = consumoDao.Find(consumoId);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo1.ubicacionId);
                Assert.AreEqual(consumo, consumo1.consumoActual);
                Assert.AreEqual(consumoId, consumo1.consumoId);
                Assert.AreEqual(fechaActual, consumo1.fecha);
                Assert.AreEqual(horaInicio, consumo1.horaIni);
                Assert.AreEqual(horafinal, consumo1.horaFin);

                // buscamos el consumo (entidad) actual
                Consumo consumo2 = consumoDao.UltimoConsumoUbicacion(ubicacionId);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumo, consumo2.consumoActual);
                Assert.AreEqual(consumoIdNuevo, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(null, consumo2.horaFin);


                // obtenemos la Carga
                Carga c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c, null);

                // obtenemos Suministra
                Suministra s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s, null);
            }
        }

        [TestMethod()]
        public void ActualizarConsumoActualCargandoTest()
        {
            using (var scope = new TransactionScope())
            {

                //private const double precioMedio = 100;
                //private const double kwHAlmacenados = 1000;
                //private const double almacenajeMaximoKwH = 20000;
                //private const double ratioCarga = 10;
                //private const double ratioCompra = 50;
                //private const double ratioUso = 45;
                //private const double capacidadCargador = 10;
                double kwHAlmacenados = 10000;


                //Creamos los estados usuario y ubicacion
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = servicio.crearUbicacion(codigoPostal, localidad, calle, portal, numero, etiqueta);
                //Creamos Tarifa
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                // Ponemos la bateria en la ubicacion
                servicio.CambiarBateriaSuministradora(ubicacionId, bateriaId);

                //Ponemos el estado a "Cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("Cargando");
                Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);

                // -> cargando
                servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdC, 0, 0);

                double consumo = 1000;
                int hour1 = 1;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaInicio = new TimeSpan(hour1, minutes, seconds);

                //creamos el consumo
                long consumoId = servicio.crearConsumo(ubicacionId, consumo, horaInicio);

                //modificamos el consumo ,finalizamos el consumo creado

                TimeSpan horafinal = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoId2 = servicio.actualizarConsumoActual(ubicacionId);        //consumo 1000 

                // buscamos ubicacion
                Ubicacion u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId2, u.ultimoConsumo);

                //+++++++
                double capacidadCarga = servicioBateria.capacidadCargadorBateriaSuministradora(bateriaId);
                double kwCargados = servicio.calcularConsumo(capacidadCarga, horaInicio, horafinal);
                double kwRed = servicio.calcularConsumo(consumo, horaInicio, horafinal);
                //++++++++++++

                // buscamos el primer consumo
                Consumo consumo1 = consumoDao.Find(consumoId);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo1.ubicacionId);
                Assert.AreEqual(consumo, consumo1.consumoActual);
                Assert.AreEqual(consumoId, consumo1.consumoId);
                Assert.AreEqual(fechaActual, consumo1.fecha);
                Assert.AreEqual(horaInicio, consumo1.horaIni);
                Assert.AreEqual(horafinal, consumo1.horaFin);
                Assert.AreEqual(kwCargados, consumo1.kwCargados);
                Assert.AreEqual(0, consumo1.kwSuministrados);
                Assert.AreEqual(kwRed, consumo1.kwRed);

                // buscamos el consumo (entidad) actual
                long ucId2 = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo2 = servicio.buscarConsumoById(ucId2);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumo, consumo2.consumoActual);
                Assert.AreEqual(consumoId2, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(null, consumo2.horaFin);
                Assert.AreEqual(0, consumo2.kwCargados);
                Assert.AreEqual(0, consumo2.kwSuministrados);
                Assert.AreEqual(0, consumo2.kwRed);


                // obtenemos la Carga
                Carga c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c.kwH, kwCargados);

                // obtenemos Suministra
                Suministra s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s, null);

                //++++++++++++++++++++++++++++++
                // tercer consumo,
                // segundo consumo en "Cargando"
                //++++++++++++++++++++++++++++++
                Thread.Sleep(1000);
                //modificamos el consumo ,finalizamos el consumo creado
                double consumoActual3 = 1000;

                TimeSpan horafinal2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                long consumoId3 = servicio.actualizarConsumoActual(ubicacionId);  //consumo 2000 

                // buscamos ubicacion
                u = servicio.buscarUbicacionById(ubicacionId);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Assert.AreEqual(consumoId3, u.ultimoConsumo);

                //+++++++
                double kwCargados2 = servicio.calcularConsumo(capacidadCarga, horafinal, horafinal2);
                double kwRed2 = servicio.calcularConsumo(consumo, horafinal, horafinal2);
                //++++++++++++

                // buscamos el segundo consumo
                consumo2 = consumoDao.Find(consumoId2);

                // comprobamos los cambios en consumo1
                Assert.AreEqual(ubicacionId, consumo2.ubicacionId);
                Assert.AreEqual(consumo, consumo2.consumoActual);
                Assert.AreEqual(consumoId2, consumo2.consumoId);
                Assert.AreEqual(fechaActual, consumo2.fecha);
                Assert.AreEqual(horafinal, consumo2.horaIni);
                Assert.AreEqual(horafinal2, consumo2.horaFin);
                Assert.AreEqual(kwCargados2, consumo2.kwCargados);
                Assert.AreEqual(0, consumo2.kwSuministrados);
                Assert.AreEqual(kwRed2, consumo2.kwRed);

                // buscamos el consumo (entidad) actual
                long ucId3 = (long)servicio.UltimoConsumoEnUbicacion(ubicacionId);
                Consumo consumo3 = servicio.buscarConsumoById(ucId3);

                // comprobamos los cambios en consumo2
                Assert.AreEqual(ubicacionId, consumo3.ubicacionId);
                Assert.AreEqual(consumoActual3, consumo3.consumoActual);
                Assert.AreEqual(consumoId3, consumo3.consumoId);
                Assert.AreEqual(fechaActual, consumo3.fecha);
                Assert.AreEqual(horafinal2, consumo3.horaIni);
                Assert.AreEqual(null, consumo3.horaFin);
                Assert.AreEqual(0, consumo3.kwCargados);
                Assert.AreEqual(0, consumo3.kwSuministrados);
                Assert.AreEqual(0, consumo3.kwRed);


                // obtenemos la Carga
                c = servicioBateria.UltimaCarga(bateriaId);

                //comprobamos que no hay cambios por que esta "sin actividad"
                Assert.AreEqual(c.kwH, kwCargados + kwCargados2);

                // obtenemos Suministra
                s = servicioBateria.UltimaSuministra(bateriaId);

                //comprobamos los cambios
                Assert.AreEqual(s, null);
            }
        }
    }
}
