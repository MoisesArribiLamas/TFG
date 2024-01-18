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
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Es.Udc.DotNet.TFG.Model.Service.Util;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Daos.CargaDao;
using Es.Udc.DotNet.TFG.Model.Daos.SuministraDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Es.Udc.DotNet.TFG.Model.Service.Estados;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceBateriaTest
    {
        private static IKernel kernel;
        private static IServiceBateria servicio;
        private static IServiceEstado servicioEstado;
        private static IServiceTarifa servicioTarifa;


        private static IBateriaDao bateriaDao;
        private static IUsuarioDao usuarioDao;
        private static IUbicacionDao ubicacionDao;
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
         
             */

        //UBICACION

        private const long codigoPostal = 15000;
        private const string localidad = "localidad";
        private const string calle = "calle";
        private const string portal = "portal";
        private const long numero = 1;

        // constructor de Ubicaciones
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
        private const string modelo = "modelo" ;
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
            servicio = kernel.Get<IServiceBateria>();
            servicioEstado = kernel.Get<IServiceEstado>();
            servicioTarifa = kernel.Get<IServiceTarifa>();
            bateriaDao = kernel.Get<IBateriaDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();
            ubicacionDao = kernel.Get<IUbicacionDao>();
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
        public void CrearBateriaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion( codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);


                var bateriaProfile = bateriaDao.Find(bateriaId);


                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(ubicacionId, bateriaProfile.ubicacionId);
                Assert.AreEqual(usuarioId, bateriaProfile.usuarioId);
                Assert.AreEqual(precioMedio, bateriaProfile.precioMedio);
                Assert.AreEqual(kwHAlmacenados, bateriaProfile.kwHAlmacenados);
                Assert.AreEqual(almacenajeMaximoKwH, bateriaProfile.almacenajeMaximoKwH);
                Assert.AreEqual(fechaDeAdquisicion, bateriaProfile.fechaDeAdquisicion);
                Assert.AreEqual(marca, bateriaProfile.marca);
                Assert.AreEqual(modelo, bateriaProfile.modelo);
                Assert.AreEqual(ratioCarga, bateriaProfile.ratioCarga);
                Assert.AreEqual(ratioCompra, bateriaProfile.ratioCompra);
                Assert.AreEqual(ratioUso, bateriaProfile.ratioUso);

            }
        }

        
        
        [TestMethod()]
        public void ModificarBateriaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Modificamos datos

                long ubicacionId2 = crearUbicacion(codigoPostal, localidad, calle, portal, numero);
                long usuarioId2 = crearUsuario("nombre2", email, "apellido1", apellido2, contraseña, telefono, pais, idioma);
                double precioMedio2 = 2;
                double kwHAlmacenados2 = 2;
                double almacenajeMaximoKwH2 = 2;
                DateTime fechaDeAdquisicion2 = fechaDeAdquisicion.AddDays(1);

                string marca2 = "Marca2";
                string modelo2 = "Modelo2";
                double ratioCarga2 = 2;
                double ratioCompra2 = 2;
                double ratioUso2 = 2;
                double capacidadCargador2 =  10;

                servicio.ModificarBateria(bateriaId, ubicacionId2, usuarioId2, precioMedio2, kwHAlmacenados2, almacenajeMaximoKwH2,
                fechaDeAdquisicion2, marca2, modelo2, ratioCarga2, ratioCompra2, ratioUso2, capacidadCargador2);

                //Comprobamos los cambios

                var bateriaProfile = bateriaDao.Find(bateriaId);


                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(usuarioId2, bateriaProfile.usuarioId);
                Assert.AreEqual(precioMedio2, bateriaProfile.precioMedio);
                Assert.AreEqual(kwHAlmacenados2, bateriaProfile.kwHAlmacenados);
                Assert.AreEqual(almacenajeMaximoKwH2, bateriaProfile.almacenajeMaximoKwH);
                Assert.AreEqual(fechaDeAdquisicion2, bateriaProfile.fechaDeAdquisicion);
                Assert.AreEqual(marca2, bateriaProfile.marca);
                Assert.AreEqual(modelo2, bateriaProfile.modelo);
                Assert.AreEqual(ratioCarga2, bateriaProfile.ratioCarga);
                Assert.AreEqual(ratioCompra2, bateriaProfile.ratioCompra);
                Assert.AreEqual(ratioUso2, bateriaProfile.ratioUso);

            }
        }




        [TestMethod()]
        public void BuscarBateriaPorIdTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Buscamos
                var bateriaProfile = servicio.BuscarBateriaById(bateriaId);


                //Comprobamos

                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(usuarioId, bateriaProfile.usuarioId);
                Assert.AreEqual(precioMedio, bateriaProfile.precioMedio);
                Assert.AreEqual(kwHAlmacenados, bateriaProfile.kwHAlmacenados);
                Assert.AreEqual(almacenajeMaximoKwH, bateriaProfile.almacenajeMaximoKwH);
                Assert.AreEqual(fechaDeAdquisicion, bateriaProfile.fechaDeAdquisicion);
                Assert.AreEqual(marca, bateriaProfile.marca);
                Assert.AreEqual(modelo, bateriaProfile.modelo);
                Assert.AreEqual(ratioCarga, bateriaProfile.ratioCarga);
                Assert.AreEqual(ratioCompra, bateriaProfile.ratioCompra);
                Assert.AreEqual(ratioUso, bateriaProfile.ratioUso);                   

            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void EliminarBateriaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Buscamos
                var bateriaProfile = servicio.BuscarBateriaById(bateriaId);

                //Comprobamos

                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(usuarioId, bateriaProfile.usuarioId);
                Assert.AreEqual(precioMedio, bateriaProfile.precioMedio);
                Assert.AreEqual(kwHAlmacenados, bateriaProfile.kwHAlmacenados);
                Assert.AreEqual(almacenajeMaximoKwH, bateriaProfile.almacenajeMaximoKwH);
                Assert.AreEqual(fechaDeAdquisicion, bateriaProfile.fechaDeAdquisicion);
                Assert.AreEqual(marca, bateriaProfile.marca);
                Assert.AreEqual(modelo, bateriaProfile.modelo);
                Assert.AreEqual(ratioCarga, bateriaProfile.ratioCarga);
                Assert.AreEqual(ratioCompra, bateriaProfile.ratioCompra);
                Assert.AreEqual(ratioUso, bateriaProfile.ratioUso);

                //Eliminamos
                servicio.EliminarBateria(bateriaId);

                //Buscamos //Comprobamos la exception
                var b = servicio.BuscarBateriaById(bateriaId);


            }
        }

        [TestMethod()]
        public void MostrarBateriasUsuarioTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long usuarioId2 = crearUsuario("nombre2", "email2", apellido1, apellido2, "contraseña2", telefono, pais, idioma);


                // Creamos las ubicaciones
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);
                long ubicacionId2 = crearUbicacion(codigoPostal, localidad, calle, "10B", numero);
                long ubicacionId3 = crearUbicacion(codigoPostal, localidad, calle, "11B", numero);



                // creamos las baterias
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                long bateriaId3 = servicio.CrearBateria(ubicacionId2, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                long bateriaId4 = servicio.CrearBateria(ubicacionId3, usuarioId2, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Buscamos los baterias que pertenecen al usuario (dos en la misma residencia y una en otra)

                List<BateriaDTO> bateriasUsuario = servicio.VerBateriasUsuario(usuarioId, 0, 3);
                List<BateriaDTO> bateriasUsuario2 = servicio.VerBateriasUsuario(usuarioId2, 0, 3);


                //Comprobamos los cambios


                Assert.AreEqual(bateriaId, bateriasUsuario[0].bateriaId);
                Assert.AreEqual(bateriaId2, bateriasUsuario[1].bateriaId);
                Assert.AreEqual(bateriaId3, bateriasUsuario[2].bateriaId);

                Assert.AreEqual(bateriaId4, bateriasUsuario2[0].bateriaId);

            }
        }

        [TestMethod()]
        public void IniciarCargaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //bateria creada
                var bateriaProfile = servicio.BuscarBateriaById(bateriaId);

                //crear tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                long tarifaId = crearTarifa(500, 0, fecha);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 0;
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaIni =new TimeSpan(hour1,  minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                double kwH = 0;

                long cargaId = servicio.IniciarCarga(bateriaId, tarifaId, horaIni);


                //Comprobamos

                Carga c = cargaDao.Find(cargaId);


                Assert.AreEqual(bateriaId, c.bateriaId);
                Assert.AreEqual(tarifaId, c.tarifaId);
                Assert.AreEqual(horaIni, c.horaIni);
                Assert.AreEqual(horaFin, c.horaFin);
                Assert.AreEqual(kwH, c.kwH);


            }
        }

        [TestMethod()]
        public void CrearCargaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //bateria creada
                var bateriaProfile = servicio.BuscarBateriaById(bateriaId);

                //crear tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);         

                // Tarifa actual (hora)
                int horaTarifa = horaActual.Hours;
                long tarifaId = crearTarifa(500, horaTarifa, fecha);

                //creamos Carga
                int hour2 = 0;
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                double kwH = 0;

                long cargaId = servicio.CrearCargaEnBateria(bateriaId);


                //Comprobamos

                Carga c = cargaDao.Find(cargaId);


                Assert.AreEqual(bateriaId, c.bateriaId);
                Assert.AreEqual(tarifaId, c.tarifaId);
                Assert.AreEqual(horaActual, c.horaIni);
                Assert.AreEqual(horaFin, c.horaFin);
                Assert.AreEqual(kwH, c.kwH);


            }
        }

        [TestMethod()]
        public void BuscarCargaPorIdTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Creamos Tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                long tarifaId = crearTarifa(500, 0, fecha);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 0;
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                double kwH = 0;

                long cargaId = servicio.IniciarCarga(bateriaId, tarifaId, horaIni);

                //Buscamos
                var c = servicio.BuscarCargaById(cargaId);


                //Comprobamos

                Assert.AreEqual(bateriaId, c.bateriaId);
                Assert.AreEqual(tarifaId, c.tarifaId);
                Assert.AreEqual(horaIni, c.horaIni);
                Assert.AreEqual(horaFin, c.horaFin);
                Assert.AreEqual(kwH, c.kwH);


            }
        }

        [TestMethod()]
        public void MostrarCargasBareriaPorFechaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Creamos Tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
                DateTime fecha3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(2).Day);
                DateTime fecha4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(3).Day);

                long tarifaId = crearTarifa(500, 0, fecha);
                long tarifaId2 = crearTarifa(500, 0, fecha2);
                long tarifaId3 = crearTarifa(500, 0, fecha3);
                long tarifaId4 = crearTarifa(500, 0, fecha4);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 0;
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);

                long cargaId = servicio.IniciarCarga(bateriaId, tarifaId, horaIni);
                long cargaId2 = servicio.IniciarCarga(bateriaId, tarifaId2, horaIni);
                long cargaId3 = servicio.IniciarCarga(bateriaId, tarifaId3, horaIni);
                long cargaId4 = servicio.IniciarCarga(bateriaId, tarifaId4, horaIni);
                long cargaId5 = servicio.IniciarCarga(bateriaId2, tarifaId2, horaIni);

                //Buscamos
                int startIndex = 0;
                int count = 3;
                var c = servicio.MostrarCargasBareriaPorFecha(bateriaId, fecha2, fecha3, startIndex, count);


                //Comprobamos

                Assert.AreEqual(cargaId2, c[0].cargaId);
                Assert.AreEqual(cargaId3, c[1].cargaId);
                Assert.AreEqual(2, c.Count());

            }
        }

        [TestMethod()]
        public void UltimaCargaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Creamos Tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
                DateTime fecha3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(2);
                DateTime fecha4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(3);

                long tarifaId = crearTarifa(500, 0, fecha);
                long tarifaId2 = crearTarifa(500, 0, fecha2);
                long tarifaId3 = crearTarifa(500, 0, fecha3);
                long tarifaId4 = crearTarifa(500, 0, fecha4);

                //creamos Carga
                int hour1 = 1;
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaIni2 = new TimeSpan(hour1+1, minutes, seconds);
                TimeSpan horaIni3 = new TimeSpan(hour1+2, minutes, seconds);
                TimeSpan horaIni4 = new TimeSpan(hour1+3, minutes, seconds);
                TimeSpan horaIni5 = new TimeSpan(hour1+4, minutes, seconds);

                long cargaId = servicio.IniciarCarga(bateriaId, tarifaId, horaIni);
                long cargaId2 = servicio.IniciarCarga(bateriaId, tarifaId2, horaIni2);
                long cargaId3 = servicio.IniciarCarga(bateriaId, tarifaId3, horaIni3);
                long cargaId4 = servicio.IniciarCarga(bateriaId, tarifaId4, horaIni4);
                long cargaId5 = servicio.IniciarCarga(bateriaId2, tarifaId2, horaIni5);

                //Buscamos

                Carga carga = servicio.UltimaCarga(bateriaId);


                //Comprobamos

                Assert.AreEqual(carga.cargaId, cargaId4);


            }
        }

        [TestMethod()]
        public void FinalizarCargaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Creamos Tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
                DateTime fecha3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(2).Day);
                DateTime fecha4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(3).Day);

                long tarifaId = crearTarifa(500, 0, fecha);
                long tarifaId2 = crearTarifa(500, 0, fecha2);
                long tarifaId3 = crearTarifa(500, 0, fecha3);
                long tarifaId4 = crearTarifa(500, 0, fecha4);

                //creamos Carga
                int hour1 = 1;                       
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);

                long cargaId = servicio.IniciarCarga(bateriaId, tarifaId, horaIni);
                long cargaId2 = servicio.IniciarCarga(bateriaId, tarifaId2, horaIni);
                long cargaId3 = servicio.IniciarCarga(bateriaId, tarifaId3, horaIni);
                long cargaId4 = servicio.IniciarCarga(bateriaId, tarifaId4, horaIni);
                long cargaId5 = servicio.IniciarCarga(bateriaId2, tarifaId2, horaIni);

                //Finalizamos las cargas
                int hour2 = 2;
                int minutes2 = 2;
                int seconds2 = 2;
                TimeSpan horaFin = new TimeSpan(hour2, minutes2, seconds2);
                double kwH = 2222;

                servicio.FinalizarCarga(cargaId, horaFin, kwH);
                servicio.FinalizarCarga(cargaId2, horaFin, kwH);
                servicio.FinalizarCarga(cargaId3, horaFin, kwH);
                servicio.FinalizarCarga(cargaId4, horaFin, kwH);
                servicio.FinalizarCarga(cargaId5, horaFin, kwH);


                //Buscamos y Comprobamos
                Carga c1 = servicio.BuscarCargaById( cargaId);
                Assert.AreEqual(c1.horaFin, horaFin);
                Assert.AreEqual(c1.kwH, kwH);

                Carga c2 = servicio.BuscarCargaById(cargaId2);
                Assert.AreEqual(c2.horaFin, horaFin);
                Assert.AreEqual(c2.kwH, kwH);

                Carga c3 = servicio.BuscarCargaById(cargaId3);
                Assert.AreEqual(c3.horaFin, horaFin);
                Assert.AreEqual(c3.kwH, kwH);

                Carga c4 = servicio.BuscarCargaById(cargaId4);
                Assert.AreEqual(c4.horaFin, horaFin);
                Assert.AreEqual(c4.kwH, kwH);

                Carga c5 = servicio.BuscarCargaById(cargaId5);
                Assert.AreEqual(c5.horaFin, horaFin);
                Assert.AreEqual(c5.kwH, kwH);

            }
        }

        [TestMethod()]
        public void CrearSuministraTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //bateria creada
                var bateriaProfile = servicio.BuscarBateriaById(bateriaId);

                //crear tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                long tarifaId = crearTarifa(500, 0, fecha);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 0;
                int minutes = 0;
                int seconds = 0;
                double ahorro = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                double kwH = 0;

                long suministraId = servicio.IniciarSuministra(bateriaId, tarifaId, horaIni);


                //Comprobamos

                Suministra s = suministraDao.Find(suministraId);


                Assert.AreEqual(bateriaId, s.bateriaId);
                Assert.AreEqual(tarifaId, s.tarifaId);
                Assert.AreEqual(ahorro, s.ahorro);
                Assert.AreEqual(horaIni, s.horaIni);
                Assert.AreEqual(horaFin, s.horaFin);
                Assert.AreEqual(kwH, s.kwH);


            }
        }

        [TestMethod()]
        public void finalizarSuministraTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //bateria creada
                var bateriaProfile = servicio.BuscarBateriaById(bateriaId);

                //crear tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                long tarifaId = crearTarifa(500, 0, fecha);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 2;
                int minutes = 0;
                int seconds = 0;
                double ahorro = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                double kwH = 2000;

                //inicializamos la carga
                long suministraId = servicio.IniciarSuministra(bateriaId, tarifaId, horaIni);

                //Finalizamos la carga
                servicio.FinalizarSuministra(suministraId, horaFin, kwH, ahorro);

                //Comprobamos

                Suministra s = suministraDao.Find(suministraId);


                Assert.AreEqual(bateriaId, s.bateriaId);
                Assert.AreEqual(tarifaId, s.tarifaId);
                Assert.AreEqual(ahorro, s.ahorro);
                Assert.AreEqual(horaIni, s.horaIni);
                Assert.AreEqual(horaFin, s.horaFin);
                Assert.AreEqual(kwH, s.kwH);


            }
        }

        [TestMethod()]
        public void BuscarSuministraPorIdTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Creamos Tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                long tarifaId = crearTarifa(500, 0, fecha);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 0;
                int minutes = 0;
                int seconds = 0;
                double ahorro = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                double kwH = 0;

                long suministraId = IniciarSuministra(bateriaId, tarifaId, ahorro, horaIni);

                //Buscamos
                var c = servicio.BuscarsuministraById(suministraId);


                //Comprobamos

                Assert.AreEqual(bateriaId, c.bateriaId);
                Assert.AreEqual(tarifaId, c.tarifaId);
                Assert.AreEqual(horaIni, c.horaIni);
                Assert.AreEqual(horaFin, c.horaFin);
                Assert.AreEqual(kwH, c.kwH);

            }
        }

        [TestMethod()]
        public void MostrarSuministrosBareriaPorFechaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Creamos Tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
                DateTime fecha3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(2).Day);
                DateTime fecha4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(3).Day);

                long tarifaId = crearTarifa(500, 0, fecha);
                long tarifaId2 = crearTarifa(500, 0, fecha2);
                long tarifaId3 = crearTarifa(500, 0, fecha3);
                long tarifaId4 = crearTarifa(500, 0, fecha4);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 2;
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);


                long suministraId = servicio.IniciarSuministra(bateriaId, tarifaId, horaIni);
                long suministraId2 = servicio.IniciarSuministra(bateriaId, tarifaId2, horaIni);
                long suministraId3 = servicio.IniciarSuministra(bateriaId, tarifaId3, horaIni);
                long suministraId4 = servicio.IniciarSuministra(bateriaId, tarifaId4, horaIni);
                long suministraId5 = servicio.IniciarSuministra(bateriaId2, tarifaId2, horaIni);

                //Buscamos
                int startIndex = 0;
                int count = 3;
                var s = servicio.MostrarSuministraBareriaPorFecha(bateriaId, fecha2, fecha3, startIndex, count);


                //Comprobamos

                Assert.AreEqual(suministraId2, s[0].suministroId);
                Assert.AreEqual(suministraId3, s[1].suministroId);
                Assert.AreEqual(2, s.Count());


            }
        }

        [TestMethod()]
        public void ahorroBareriaPorFechaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Creamos Tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
                DateTime fecha3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(2).Day);
                DateTime fecha4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(3).Day);

                long tarifaId = crearTarifa(500, 0, fecha);
                long tarifaId2 = crearTarifa(500, 0, fecha2);
                long tarifaId3 = crearTarifa(500, 0, fecha3);
                long tarifaId4 = crearTarifa(500, 0, fecha4);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 2;
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                double ahorro = 0;
                double ahorro2 = 10;
                double ahorro3 = 100;
                double ahorro4 = 1000;
                double ahorro5 = 10000;


                long suministraId = IniciarSuministra(bateriaId, tarifaId, ahorro, horaIni);
                long suministraId2 = IniciarSuministra(bateriaId, tarifaId2, ahorro2, horaIni);
                long suministraId3 = IniciarSuministra(bateriaId, tarifaId3, ahorro3, horaIni);
                long suministraId4 = IniciarSuministra(bateriaId, tarifaId4, ahorro4, horaIni);
                long suministraId5 = IniciarSuministra(bateriaId2, tarifaId2, ahorro5, horaIni);

                //Buscamos
                double a = servicio.ahorroBareriaPorFecha(bateriaId, fecha2, fecha3);


                //Comprobamos

                Assert.AreEqual(ahorro2+ ahorro3, a);



            }
        }

        [TestMethod()]
        public void ahorroBareriasUsuarioPorFechaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long usuarioId2 = crearUsuario("pedro", email, "apellido1", "apellido2", contraseña, telefono, pais, idioma);

                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId3 = servicio.CrearBateria(ubicacionId, usuarioId2, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Creamos Tarifa
                DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
                DateTime fecha3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(2).Day);
                DateTime fecha4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(3).Day);

                long tarifaId = crearTarifa(500, 0, fecha);
                long tarifaId2 = crearTarifa(500, 0, fecha2);
                long tarifaId3 = crearTarifa(500, 0, fecha3);
                long tarifaId4 = crearTarifa(500, 0, fecha4);

                //creamos Carga
                int hour1 = 1;
                int hour2 = 2;
                int minutes = 0;
                int seconds = 0;
                TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                
                double ahorro = 1;
                double ahorro2 = 10;
                double ahorro3 = 100;
                double ahorro4 = 1000;
                double ahorro5 = 10000;


                long suministraId  = IniciarSuministra(bateriaId, tarifaId, ahorro, horaIni);
                long suministraId2 = IniciarSuministra(bateriaId, tarifaId2, ahorro2, horaIni);
                long suministraId3 = IniciarSuministra(bateriaId, tarifaId3, ahorro3, horaIni);
                long suministraId4 = IniciarSuministra(bateriaId, tarifaId4, ahorro4, horaIni);
                long suministraId5 = IniciarSuministra(bateriaId2, tarifaId2, ahorro5, horaIni);
                long suministraId6 = IniciarSuministra(bateriaId3, tarifaId2, ahorro, horaIni);

                //Buscamos
                double a = servicio.ahorroBareriasUsuarioPorFecha(usuarioId, fecha2, fecha3);


                //Comprobamos

                Assert.AreEqual(ahorro5 + ahorro2 + ahorro3, a);

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Sin_Actividad_A_Sin_ActividadTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);


                //comprobamos el estado anterior.
                long estadoIdSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    // Fecha y hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);

                //int hour = 0;
                //int minutes = 0;
                //int seconds = 0;

                //aun no tenemos fecha de finalizacion
                TimeSpan? horaFin = null;

                Assert.AreEqual(estadoBateria.estadoId, estadoIdSA);
                Assert.AreEqual(estadoBateria.horaIni, horaActual);
                Assert.AreEqual(estadoBateria.horaFin, horaFin);




                //Intentamos cambiar el estado:  "sin actividad" -> "sin actividad"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdSA, 0, 0);


                //Buscamos y Comprobamos que no se ha modificado nada.
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdSA);
                Assert.AreEqual(estadoBateriaNuevo.horaIni, horaActual);
                Assert.AreEqual(estadoBateriaNuevo.horaFin, horaFin);

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Sin_Actividad_A_CargandoTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H( fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //comprobamos que es estado anterior es "sin actividad"
                long estadoIdSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById( bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdSA);
                        
                         
                        
                //buscamos el estadoId de "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdC);

                
                // Fecha y hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "sin actividad" -> "cargando"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdC,0,0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);


                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);


                //Comprobamos
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);               


                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Sin_Actividad_A_SuministrandoTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //comprobamos que es estado anterior es "sin actividad"
                long estadoIdSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdSA);

                //buscamos el estadoId de "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdS);

                // Fecha y hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


                //cambiamos el estado:  "sin actividad" -> "suministrando"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS,0,0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdS);
 
                // Suministro actual
                Suministra suministroActual = servicio.UltimaSuministra(bateriaId);

                //Comprobamos
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministroActual.bateriaId, bateriaId);
                Assert.AreEqual(suministroActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministroActual.horaIni, horaActual);
                Assert.AreEqual(suministroActual.horaFin, horaFin);
                Assert.AreEqual(suministroActual.ahorro, 0);
                Assert.AreEqual(suministroActual.kwH, 0);
            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Sin_Actividad_A_Carga_YSuministraTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //comprobamos que es estado anterior es "sin actividad"
                long estadoIdSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdSA);



                //buscamos el estadoId de "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdS);

                // Fecha y hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                // CAMBIAMOS EL ESTADO:  "sin actividad" -> "carga y suministra"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdS);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);


                // Suministro actual
                Suministra suministroActual = servicio.UltimaSuministra(bateriaId);

                //Comprobamos

                Assert.AreEqual(suministroActual.bateriaId, bateriaId);
                Assert.AreEqual(suministroActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministroActual.horaIni, horaActual);
                Assert.AreEqual(suministroActual.horaFin, horaFin);
                Assert.AreEqual(suministroActual.ahorro, 0);
                Assert.AreEqual(suministroActual.kwH, 0);
            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Carga_A_Sin_ActividadTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "Cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("Cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    //  hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> cargando
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdC, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdC);


                //buscamos el estadoId de "sin actividad"
                long estadoIdCSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCSA);

                

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "Cargando" -> "sin actividad"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCSA, 300, 0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCSA);

                //Comprobamos
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, 300);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + 300);
                Assert.AreEqual(bateriaCambiada.precioMedio, ((kwHAlmacenados * precioMedio + 300 * tarifa.precio) / (kwHAlmacenados + 300)));

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Carga_A_CargaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "Cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("Cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    //  hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> cargando
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdC, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdC);


                //buscamos el estadoId de "Cargando"
                long estadoIdC2 = servicioEstado.BuscarEstadoPorNombre("Cargando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdC2);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "Cargando" -> "Cargando"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdC2, 300, 0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC2);

                //Comprobamos Carga Cerrada
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, 300);

                //Buscamos la carga
                Carga cargaN = servicio.UltimaCarga(bateriaId);

                //Comprobamos la carga nueva
                Assert.AreEqual(cargaN.bateriaId, bateriaId);
                Assert.AreEqual(cargaN.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(cargaN.horaIni, horaActual);
                Assert.AreEqual(cargaN.horaFin, horaFin);
                Assert.AreEqual(cargaN.kwH, 0);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados+300);
                Assert.AreEqual(bateriaCambiada.precioMedio, ((kwHAlmacenados* precioMedio + 300 * tarifa.precio) /(kwHAlmacenados+300)));

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Carga_A_SuministrandoTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);


                //Ponemos el estado anterior a "Cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("Cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    // hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> cargando
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdC, 0, 0); 

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdC);


                //buscamos el estadoId de "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdS);
            

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);

                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "Cargando" -> "suministrando"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 300, 0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdS);


                //Comprobamos Carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, 300);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + 300);
                Assert.AreEqual(bateriaCambiada.precioMedio, ((kwHAlmacenados * precioMedio + 300 * tarifa.precio) / (kwHAlmacenados + 300)));

                // Suministro actual
                Suministra suministroActual = servicio.UltimaSuministra(bateriaId);
                
                //Comprobamos
                Assert.AreEqual(suministroActual.bateriaId, bateriaId);
                Assert.AreEqual(suministroActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministroActual.horaIni, horaActual);
                Assert.AreEqual(suministroActual.horaFin, horaFin);
                Assert.AreEqual(suministroActual.ahorro, 0);
                Assert.AreEqual(suministroActual.kwH, 0);

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Carga_A_Carga_Y_SuministraTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "Cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("Cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    //  hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> cargando
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdC, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdC);


                //buscamos el estadoId de "carga y suministra"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);
              
                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos la carga
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);

                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
               
                //cambiamos el estado:  "Cargando" -> "carga y suministra"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, 300, 0);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, 300);


                // Suministro actual
                Suministra suministroActual = servicio.UltimaSuministra(bateriaId);

                //Comprobamos
                Assert.AreEqual(suministroActual.bateriaId, bateriaId);
                Assert.AreEqual(suministroActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministroActual.horaIni, horaActual);
                Assert.AreEqual(suministroActual.horaFin, horaFin);
                Assert.AreEqual(suministroActual.ahorro, 0);
                Assert.AreEqual(suministroActual.kwH, 0);

                //Buscamos la carga
                Carga cargaN = servicio.UltimaCarga(bateriaId);

                //Comprobamos la carga nueva
                Assert.AreEqual(cargaN.bateriaId, bateriaId);
                Assert.AreEqual(cargaN.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(cargaN.horaIni, horaActual);
                Assert.AreEqual(cargaN.horaFin, horaFin);
                Assert.AreEqual(cargaN.kwH, 0);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + 300);
                Assert.AreEqual(bateriaCambiada.precioMedio, ((kwHAlmacenados * precioMedio + 300 * tarifa.precio) / (kwHAlmacenados + 300)));

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Suministrando_A_Sin_ActividadTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    //  hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> cargando
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "sin actividad"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Comprobamos el suministro
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);

                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                
                //cambiamos el estado:  "suministrando" -> "sin actividad"
                int suministrados = 300;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, 0, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, suministrados * (tarifa.precio - precioMedio)); // kwHSuministrados * (tarifa.precio - b.precioMedio)

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Suministrando_A_CargandoTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    //  hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> cargando
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "Cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("Cargando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdC);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Comprobamos el suministro
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);


                //hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "suministrando" -> "Cargando
                int suministrados = 300;        
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdC, 0, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, suministrados * (tarifa.precio - precioMedio));


                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos la carga

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Suministrando_A_Suministrando()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    //  hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> cargando
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "suministrando"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);
               

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Comprobamos el suministro
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);
                

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "suministrando" -> "suministrando"
                int suministrados = 300;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, 0, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, suministrados * (tarifa.precio - precioMedio)); // kwHSuministrados * (tarifa.precio - b.precioMedio)


                // Suministro actual
                Suministra suministroActual = servicio.UltimaSuministra(bateriaId);

                //Comprobamos el suministro actual

                Assert.AreEqual(suministroActual.bateriaId, bateriaId);
                Assert.AreEqual(suministroActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministroActual.horaIni, horaActual);
                Assert.AreEqual(suministroActual.horaFin, horaFin);
                Assert.AreEqual(suministroActual.ahorro, 0);
                Assert.AreEqual(suministroActual.kwH, 0);
            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Suministrando_A_Carga_Y_SuministraTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    //  hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> cargando
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "carga y suministra"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Comprobamos el suministro
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);


                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "suministrando" -> "carga y suministra"
                int suministrados = 300;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, 0, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, suministrados * (tarifa.precio - precioMedio));


                // Suministro actual
                Suministra suministroActual = servicio.UltimaSuministra(bateriaId);

                //Comprobamos el suministro actual

                Assert.AreEqual(suministroActual.bateriaId, bateriaId);
                Assert.AreEqual(suministroActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministroActual.horaIni, horaActual);
                Assert.AreEqual(suministroActual.horaFin, horaFin);
                Assert.AreEqual(suministroActual.ahorro, 0);
                Assert.AreEqual(suministroActual.kwH, 0);


                //Buscamos la carga actual
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos la carga

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);
            }
        }

        [TestMethod()] // Precio medio ALMACENADO <= Tarifa actual && kwHAlmacenados >= kwHSuministrados
        public void CambiarEstadoBateria_Carga_Y_Suministra_A_Sin_ActividadTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                    //  hora actual
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> carga y suministra
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "sin actividad"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos el suministro y la carga
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);


                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "suministrando" -> "sin actividad"
                int suministrados = 300;
                int cargados = 1000;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, cargados, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, suministrados * (tarifa.precio - precioMedio)); // kwHSuministrados * (tarifa.precio - b.precioMedio)

                //Comprobamoss los cambios en la carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, cargados);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + cargados - suministrados);
                Assert.AreEqual(bateriaCambiada.precioMedio, (((kwHAlmacenados - suministrados) * precioMedio) + (cargados * tarifa.precio)) / (kwHAlmacenados + cargados));

            }
        }

        [TestMethod()] // Precio medio almacenado <= TARIFA actual && kwHAlmacenados < KWHSUMINISTRADOS
        public void CambiarEstadoBateria_Carga_Y_Suministra_A_Sin_ActividadTest2()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> carga y suministra
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "sin actividad"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos el suministro y la carga
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);


                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "suministrando" -> "sin actividad"
                int suministrados = 1300;
                int cargados = 14000;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, cargados, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, kwHAlmacenados * (tarifa.precio - precioMedio));

                //Comprobamoss los cambios en la carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, cargados);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + cargados - suministrados);
                Assert.AreEqual(bateriaCambiada.precioMedio, tarifa.precio);

            }
        }

        [TestMethod()] // Precio medio almacenado > TARIFA actual && kwHCargados >= KWHSUMINISTRADOS
        public void CambiarEstadoBateria_Carga_Y_Suministra_A_Sin_ActividadTest3()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                double precioMedioNuevo = precioMedio + 2300;
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedioNuevo, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedioNuevo, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> carga y suministra
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "sin actividad"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos el suministro y la carga
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);


                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "suministrando" -> "sin actividad"
                int suministrados = 1300;
                int cargados = 14000;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, cargados, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, 0);

                //Comprobamoss los cambios en la carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, cargados);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + cargados - suministrados);
                Assert.AreEqual(bateriaCambiada.precioMedio, (kwHAlmacenados * precioMedioNuevo + (cargados - suministrados) * tarifa.precio) / (kwHAlmacenados + (cargados - suministrados)));

            }
        }


        [TestMethod()] // Precio medio almacenado > TARIFA actual && kwHCargados < KWHSUMINISTRADOS
        public void CambiarEstadoBateria_Carga_Y_Suministra_A_Sin_ActividadTest4()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                double precioMedioNuevo = precioMedio + 2300;
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedioNuevo, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedioNuevo, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> carga y suministra
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "sin actividad"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos el suministro y la carga
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);


                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "suministrando" -> "sin actividad"
                int suministrados = 1300;
                int cargados = 1000;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, cargados, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, (cargados - suministrados) * (precioMedioNuevo - tarifa.precio));
                
                //Comprobamoss los cambios en la carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, cargados);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + cargados - suministrados);
                Assert.AreEqual(bateriaCambiada.precioMedio, precioMedioNuevo);
            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Carga_Y_Suministra_A_CargandoTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> carga y suministra
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "cargando"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("cargando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos el suministro y la carga
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);


                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "suministrando" -> "sin actividad"
                int suministrados = 300;
                int cargados = 1000;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, cargados, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, suministrados * (tarifa.precio - precioMedio)); // kwHSuministrados * (tarifa.precio - b.precioMedio)

                //Comprobamoss los cambios en la carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, cargados);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + cargados - suministrados);
                Assert.AreEqual(bateriaCambiada.precioMedio, (((kwHAlmacenados - suministrados) * precioMedio) + (cargados * tarifa.precio)) / (kwHAlmacenados + cargados));


                //Buscamos la carga actual
                carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos la carga

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Carga_Y_Suministra_A_SuministrandoTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> carga y suministra
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "suministrando"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos el suministro y la carga
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);


                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "carga y suministra" -> "suministrando"
                int suministrados = 300;
                int cargados = 1000;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, cargados, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, suministrados * (tarifa.precio - precioMedio)); // kwHSuministrados * (tarifa.precio - b.precioMedio)

                //Comprobamoss los cambios en la carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, cargados);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + cargados - suministrados);
                Assert.AreEqual(bateriaCambiada.precioMedio, (((kwHAlmacenados - suministrados) * precioMedio) + (cargados * tarifa.precio)) / (kwHAlmacenados + cargados));

                
               // Suministro actual
               Suministra suministroActual = servicio.UltimaSuministra(bateriaId);

                //Comprobamos el suministro actual

                Assert.AreEqual(suministroActual.bateriaId, bateriaId);
                Assert.AreEqual(suministroActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministroActual.horaIni, horaActual);
                Assert.AreEqual(suministroActual.horaFin, horaFin);
                Assert.AreEqual(suministroActual.ahorro, 0);
                Assert.AreEqual(suministroActual.kwH, 0);


            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Carga_Y_Suministra_A_Carga_Y_SuministraTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifas
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado anterior a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // -> carga y suministra
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos el estado anterior
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);


                //buscamos el estadoId de "carga y suministra"
                long estadoIdCS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCS);

                //Buscamos el suministro
                Suministra suministra = servicio.UltimaSuministra(bateriaId);

                //Buscamos la carga
                Carga carga = servicio.UltimaCarga(bateriaId);

                //Comprobamos el suministro y la carga
                int hour = 0;
                int minutes = 0;
                int seconds = 0;

                TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaIni, horaActual);
                Assert.AreEqual(suministra.horaFin, horaFin);
                Assert.AreEqual(suministra.kwH, 0);
                Assert.AreEqual(suministra.ahorro, 0);

                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaIni, horaActual);
                Assert.AreEqual(carga.horaFin, horaFin);
                Assert.AreEqual(carga.kwH, 0);


                //  hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                //cambiamos el estado:  "carga y suministra" -> "carga y suministra"
                int suministrados = 300;
                int cargados = 1000;
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCS, cargados, suministrados);


                //Buscamos y Comprobamos el estado actual de la bateria
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCS);

                // Comprobamos los cambios en el suministro
                Assert.AreEqual(suministra.bateriaId, bateriaId);
                Assert.AreEqual(suministra.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministra.horaFin, horaActual);
                Assert.AreEqual(suministra.kwH, suministrados);
                Assert.AreEqual(suministra.ahorro, suministrados * (tarifa.precio - precioMedio)); // kwHSuministrados * (tarifa.precio - b.precioMedio)

                //Comprobamoss los cambios en la carga
                Assert.AreEqual(carga.bateriaId, bateriaId);
                Assert.AreEqual(carga.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(carga.horaFin, horaActual);
                Assert.AreEqual(carga.kwH, cargados);

                //comprobamos bateria
                Assert.AreEqual(bateriaCambiada.kwHAlmacenados, kwHAlmacenados + cargados - suministrados);
                Assert.AreEqual(bateriaCambiada.precioMedio, (((kwHAlmacenados - suministrados) * precioMedio) + (cargados * tarifa.precio)) / (kwHAlmacenados + cargados));


                // Suministro actual
                Suministra suministroActual = servicio.UltimaSuministra(bateriaId);

                //Comprobamos el suministro actual

                Assert.AreEqual(suministroActual.bateriaId, bateriaId);
                Assert.AreEqual(suministroActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(suministroActual.horaIni, horaActual);
                Assert.AreEqual(suministroActual.horaFin, horaFin);
                Assert.AreEqual(suministroActual.ahorro, 0);
                Assert.AreEqual(suministroActual.kwH, 0);

                //Buscamos la carga actual
                Carga cargaActual = servicio.UltimaCarga(bateriaId);

                //Comprobamos la carga

                Assert.AreEqual(cargaActual.bateriaId, bateriaId);
                Assert.AreEqual(cargaActual.tarifaId, tarifa.tarifaId);
                Assert.AreEqual(cargaActual.horaIni, horaActual);
                Assert.AreEqual(cargaActual.horaFin, horaFin);
                Assert.AreEqual(cargaActual.kwH, 0);
            }
        }

        [TestMethod()]
        public void porcentajeDeCargaTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                //carculamos el porcetaje de carga que hay en la bateria
                double porcentaje = servicio.porcentajeDeCarga(bateriaId);

                Assert.AreEqual(5, porcentaje);

                //Modificamos datos

                var b = bateriaDao.Find(bateriaId);

                //cargamos la bateria a 10000 => 50% de carga
                b.kwHAlmacenados = 10000;
                bateriaDao.Update(b);

                //carculamos el porcetaje de carga que hay en la bateria
                porcentaje = servicio.porcentajeDeCarga(bateriaId);

                //comprobamos
                Assert.AreEqual(50, porcentaje);        

            }
        }

        [TestMethod()]
        public void ModificarRatiossTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);


                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                //comprobamos los ratios iniciales
                Assert.AreEqual(b.ratioCarga, ratioCarga);   // 40
                Assert.AreEqual(b.ratioCompra, ratioCompra); // 50
                Assert.AreEqual(b.ratioUso, ratioUso);       // 45

                //Modificamos los ratios
                double ratioCargaNuevo = 100;  // 40 -> 100
                double ratioCompraNuevo = 100; // 50 -> 100 
                double ratioUsoNuevo = 100;    // 45 -> 100

                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                b = servicio.BuscarBateriaById(bateriaId);

                //comprobamos los ratios nuevos
                Assert.AreEqual(b.ratioCarga, ratioCargaNuevo);
                Assert.AreEqual(b.ratioCompra, ratioCompraNuevo);
                Assert.AreEqual(b.ratioUso, ratioUsoNuevo);

                //Modificamos ratioCargaNuevo
                  // 100 -> 40                

                servicio.ModificarRatios(bateriaId, ratioCarga, null, null);

                b = servicio.BuscarBateriaById(bateriaId);

                //comprobamos los ratios nuevos
                Assert.AreEqual(b.ratioCarga, ratioCarga);
                Assert.AreEqual(b.ratioCompra, ratioCompraNuevo);
                Assert.AreEqual(b.ratioUso, ratioUsoNuevo);


                //Modificamos ratioCompra
                // 100 -> 50                

                servicio.ModificarRatios(bateriaId, null, ratioCompra, null);

                b = servicio.BuscarBateriaById(bateriaId);

                //comprobamos los ratios nuevos            
                Assert.AreEqual(b.ratioCompra, ratioCompra);


                //Modificamos ratioUso
                // 100 -> 45                

                servicio.ModificarRatios(bateriaId, null, null, ratioUso);

                b = servicio.BuscarBateriaById(bateriaId);

                //comprobamos los ratios nuevos            
                Assert.AreEqual(b.ratioUso, ratioUso);
            }
        }

        [TestMethod()]
        public void MostrarRatiosTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);


                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                //comprobamos los ratios iniciales
                Assert.AreEqual(b.ratioCarga, ratioCarga);   // 40
                Assert.AreEqual(b.ratioCompra, ratioCompra); // 50
                Assert.AreEqual(b.ratioUso, ratioUso);       // 45
                RatiosDTO ratios = servicio.MostrarRatios( bateriaId);
                Assert.AreEqual(ratios.ratioCarga, ratioCarga);   // 40
                Assert.AreEqual(ratios.ratioCompra, ratioCompra); // 50
                Assert.AreEqual(ratios.ratioUso, ratioUso);       // 45


                //Modificamos los ratios
                double ratioCargaNuevo = 100;  // 40 -> 100
                double ratioCompraNuevo = 100; // 50 -> 100 
                double ratioUsoNuevo = 100;    // 45 -> 100

                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                b = servicio.BuscarBateriaById(bateriaId);

                //comprobamos los ratios nuevos
                Assert.AreEqual(b.ratioCarga, ratioCargaNuevo);
                Assert.AreEqual(b.ratioCompra, ratioCompraNuevo);
                Assert.AreEqual(b.ratioUso, ratioUsoNuevo);
                ratios = servicio.MostrarRatios(bateriaId);
                Assert.AreEqual(ratios.ratioCarga, ratioCargaNuevo);   // 40 -> 100
                Assert.AreEqual(ratios.ratioCompra, ratioCompraNuevo); // 50 -> 100
                Assert.AreEqual(ratios.ratioUso, ratioUsoNuevo);       // 45 -> 100

                //Modificamos ratioCargaNuevo
                // 100 -> 40                

                servicio.ModificarRatios(bateriaId, ratioCarga, null, null);

                b = servicio.BuscarBateriaById(bateriaId);

                //comprobamos los ratios nuevos
                Assert.AreEqual(b.ratioCarga, ratioCarga);
                Assert.AreEqual(b.ratioCompra, ratioCompraNuevo);
                Assert.AreEqual(b.ratioUso, ratioUsoNuevo);
                ratios = servicio.MostrarRatios(bateriaId);
                Assert.AreEqual(ratios.ratioCarga, ratioCarga);        // 100 -> 40
                Assert.AreEqual(ratios.ratioCompra, ratioCompraNuevo); // 100
                Assert.AreEqual(ratios.ratioUso, ratioUsoNuevo);       // 100

                //Modificamos ratioCompra
                // 100 -> 50                

                servicio.ModificarRatios(bateriaId, null, ratioCompra, null);

                b = servicio.BuscarBateriaById(bateriaId);

                //comprobamos los ratios nuevos            
                Assert.AreEqual(b.ratioCompra, ratioCompra);
                ratios = servicio.MostrarRatios(bateriaId);
                Assert.AreEqual(ratios.ratioCarga, ratioCarga);   // 40
                Assert.AreEqual(ratios.ratioCompra, ratioCompra); // 100 -> 50
                Assert.AreEqual(ratios.ratioUso, ratioUsoNuevo);  // 100


                //Modificamos ratioUso
                // 100 -> 45                

                servicio.ModificarRatios(bateriaId, null, null, ratioUso);

                b = servicio.BuscarBateriaById(bateriaId);

                //comprobamos los ratios nuevos            
                Assert.AreEqual(b.ratioUso, ratioUso);
                ratios = servicio.MostrarRatios(bateriaId);
                Assert.AreEqual(ratios.ratioCarga, ratioCarga);   // 40
                Assert.AreEqual(ratios.ratioCompra, ratioCompra); // 50
                Assert.AreEqual(ratios.ratioUso, ratioUso);       // 100 -> 45
            }
        }

        [TestMethod()] 
        public void gestionDeRatiosTest()
        {
            // Estado: "sin actividad" 
            // ratioCompra <  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "suministrando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest2()
        {
            // Estado: "sin actividad" 
            // ratioCompra <  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "sin actividad"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest3()
        {
            // Estado: "sin actividad" 
            // ratioCompra <  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest4()
        {
            // Estado: "sin actividad" 
            // ratioCompra <  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest5()
        {
            // Estado: "sin actividad" 
            // ratioCompra >=  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest6()
        {
            // Estado: "sin actividad" 
            // ratioCompra >=  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest7()
        {
            // Estado: "sin actividad" 
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest8()
        {
            // Estado: "sin actividad" 
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest9()
        {
            // Estado: "cargando"
            // ratioCompra <  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "cargando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "cargando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "cargando" -> "suministrando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest10()
        {
            // Estado: "cargando" 
            // ratioCompra <  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "cargando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "sin actividad"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "sin actividad"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest11()
        {
            // Estado: "cargando" 
            // ratioCompra <  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "cargando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "cargando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "cargando" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest12()
        {
            // Estado: "cargando" 
            // ratioCompra <  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "cargando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "cargando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "cargando" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest13()
        {
            // Estado: "cargando" 
            // ratioCompra >=  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "cargando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "cargando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "cargando" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest14()
        {
            // Estado: "cargando" 
            // ratioCompra >=  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "cargando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "cargando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "cargando"" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest15()
        {
            // Estado: "cargando"" 
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "cargando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "cargando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "cargando" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest16()
        {
            // Estado: "cargando" 
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "cargando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "cargando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "cargando" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest17()
        {
            // Estado: "suministrando"
            // ratioCompra <  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "suministrando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "suministrando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest18()
        {
            // Estado: "suministrando" 
            // ratioCompra <  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "suministrando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "sin actividad"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest19()
        {
            // Estado: "suministrando" 
            // ratioCompra <  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "suministrando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest20()
        {
            // Estado: "cargando" 
            // ratioCompra <  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "suministrando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest21()
        {
            // Estado: "suministrando" 
            // ratioCompra >=  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "suministrando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest22()
        {
            // Estado: "suministrando" 
            // ratioCompra >=  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "suministrando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "suministrando"" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest23()
        {
            // Estado: "cargando"" 
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "suministrando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest24()
        {
            // Estado: "suministrando" 
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "suministrando"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest25()
        {
            // Estado: "carga y suministra"
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest26()
        {
            // Estado: "suministrando"
            // ratioCompra <  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "suministrando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest27()
        {
            // Estado: "carga y suministra" 
            // ratioCompra <  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "sin actividad"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest28()
        {
            // Estado: "carga y suministra" 
            // ratioCompra <  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest29()
        {
            // Estado: "carga y suministra" 
            // ratioCompra <  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = null;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest30()
        {
            // Estado: "carga y suministra" 
            // ratioCompra >=  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest31()
        {
            // Estado: "carga y suministra" 
            // ratioCompra >=  Tarifa
            //  ratioCarga <  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 3;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra"" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest32()
        {
            // Estado: "carga y suministra"
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso < Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = null;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }


        [TestMethod()]
        public void gestionDeRatiosTest33()
        {
            // Estado: "carga y suministra" 
            // ratioCompra >=  Tarifa
            //  ratioCarga >=  %Bateria
            //    ratioUso >= Tarifa 
            using (var scope = new TransactionScope())
            {
                //kwHAlmacenados = 1000;
                //almacenajeMaximoKwH = 20000;   => 5% de carga

                /*
                    precioMedio = 100;
                    kwHAlmacenados = 1000;
                    almacenajeMaximoKwH = 20000;
                    ratioCompra = 50;                 =>  ratioCompra <   Tarifa
                    ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
                    ratioUso = 45;                    =>     ratioUso <   Tarifa
                 */
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos la bateria
                var b = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 5;
                double? ratioUsoNuevo = 2500;


                servicio.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // gestion de :   Estado: "carga y suministra"
                servicio.gestionDeRatios(bateriaId, kwHCargados, kwHSuministrados, fechaActual, horaActual);

                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
            }
        }

        [TestMethod()]
        public void estadoDeLaBateriaTest()
        {
            using (var scope = new TransactionScope())
            {
                
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a"sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);
                
                //comprobamos que devuelve bien el estado actual de la bateria
                string estado = servicio.EstadoDeLaBateria(bateria.bateriaId);
                Assert.AreEqual(estado, "sin actividad");


                //Ponemos el estado a "cargando"
                estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //comprobamos que devuelve bien el estado actual de la bateria
                estado = servicio.EstadoDeLaBateria(bateria.bateriaId);
                Assert.AreEqual(estado, "cargando");


                //Ponemos el estado a "suministrando"
                estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //comprobamos que devuelve bien el estado actual de la bateria
                estado = servicio.EstadoDeLaBateria(bateria.bateriaId);
                Assert.AreEqual(estado, "suministrando");


                //Ponemos el estado a "carga y suministra"
                estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //comprobamos que devuelve bien el estado actual de la bateria
                estado = servicio.EstadoDeLaBateria(bateria.bateriaId);
                Assert.AreEqual(estado, "carga y suministra");
            }
        }


        [TestMethod()]
        public void CargaAñadidaTest()
        {
            using (var scope = new TransactionScope())
            {

                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a"sin actividad"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);

                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "sin actividad"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //comprobamos que devuelve bien el estado actual de la bateria
                string estado = servicio.EstadoDeLaBateria(bateria.bateriaId);
                Assert.AreEqual(estado, "sin actividad");


                //Ponemos el estado a "cargando"
                estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "cargando"
                estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //comprobamos que devuelve bien el estado actual de la bateria
                estado = servicio.EstadoDeLaBateria(bateria.bateriaId);
                Assert.AreEqual(estado, "cargando");


                //Ponemos el estado a "suministrando"
                estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "suministrando"
                estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //comprobamos que devuelve bien el estado actual de la bateria
                estado = servicio.EstadoDeLaBateria(bateria.bateriaId);
                Assert.AreEqual(estado, "suministrando");


                //Ponemos el estado a "carga y suministra"
                estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0);

                //comprobamos que el estado es "carga y suministra"
                estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //comprobamos que devuelve bien el estado actual de la bateria
                estado = servicio.EstadoDeLaBateria(bateria.bateriaId);
                Assert.AreEqual(estado, "carga y suministra");
            }
        }
    }
}
