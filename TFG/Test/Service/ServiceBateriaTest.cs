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
        private const double almacenajeMaximoKwH = 2000;
        private DateTime fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private const string marca = "marca";
        private const string modelo = "modelo" ;
        private const double ratioCarga = 40;
        private const double ratioCompra = 50;
        private const double ratioUso = 45;


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
            crearTarifa(0, 0, fecha);
            crearTarifa(100, 1, fecha);
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
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);


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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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

                servicio.ModificarBateria(bateriaId, ubicacionId2, usuarioId2, precioMedio2, kwHAlmacenados2, almacenajeMaximoKwH2,
                fechaDeAdquisicion2, marca2, modelo2, ratioCarga2, ratioCompra2, ratioUso2);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                long bateriaId3 = servicio.CrearBateria(ubicacionId2, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                long bateriaId4 = servicio.CrearBateria(ubicacionId3, usuarioId2, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                //Buscamos los baterias que pertenecen al usuario (dos en la misma residencia y una en otra)

                List<BateriaDTO> bateriasUsuario = servicio.VerBaterias(usuarioId, 0, 3);
                List<BateriaDTO> bateriasUsuario2 = servicio.VerBaterias(usuarioId2, 0, 3);


                //Comprobamos los cambios


                Assert.AreEqual(bateriaId, bateriasUsuario[0].bateriaId);
                Assert.AreEqual(bateriaId2, bateriasUsuario[1].bateriaId);
                Assert.AreEqual(bateriaId3, bateriasUsuario[2].bateriaId);

                Assert.AreEqual(bateriaId4, bateriasUsuario2[0].bateriaId);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
        public void BuscarCargaPorIdTest()
        {
            using (var scope = new TransactionScope())
            {
                crearEstados();
                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Bateria
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                double kwH = 0;

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                TimeSpan horaIni2 = new TimeSpan(hour1+1, minutes, seconds);
                TimeSpan horaIni3 = new TimeSpan(hour1+2, minutes, seconds);
                TimeSpan horaIni4 = new TimeSpan(hour1+3, minutes, seconds);
                TimeSpan horaIni5 = new TimeSpan(hour1+4, minutes, seconds);
                double kwH = 0;

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                double kwH = 3000;
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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId3 = servicio.CrearBateria(ubicacionId, usuarioId2, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

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
                double kwH = 3000;
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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                //comprobamos que es estado anterior es "sin actividad"
                long estadoIdSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById( bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdSA);
                        
                         
                        
                //buscamos el estadoId de "cargando"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdC);

                //cambiamos el estado:  "sin actividad" -> "cargando"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdC,0,0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                //comprobamos que es estado anterior es "sin actividad"
                long estadoIdSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdSA);



                //buscamos el estadoId de "suministrando"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdS);

                //cambiamos el estado:  "sin actividad" -> "suministrando"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdS,0,0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdS);

            }
        }

        [TestMethod()]
        public void CambiarEstadoBateria_Sin_Actividad_A_Carga_Y_SuministraTest()
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
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                //comprobamos que es estado anterior es "sin actividad"
                long estadoIdSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdSA);



                //buscamos el estadoId de "carga y suministra"
                long estadoIdCYS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                string estadoAnterior = servicioEstado.BuscarEstadoPorId(estadoIdCYS);

                //cambiamos el estado:  "sin actividad" -> "carga y suministra"
                servicio.CambiarEstadoEnBateria(bateriaId, estadoIdCYS, 0, 0);


                //Buscamos y Comprobamos
                Bateria bateriaCambiada = servicio.BuscarBateriaById(bateriaId);
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateriaCambiada.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdCYS);

            }
        }
    }
}
