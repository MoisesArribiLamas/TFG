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
using Es.Udc.DotNet.TFG.Model.Service.Controlador;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceControladorTest
    {
        private static IKernel kernel;
        private static IServiceControlador servicio;
        private static IServiceBateria servicioBateria;
        private static IServiceEstado servicioEstado;
        private static IServiceTarifa servicioTarifa;
        private static IServiceUbicacion servicioUbicacion;


        private static IBateriaDao bateriaDao;
        private static IUsuarioDao usuarioDao;
        private static IUbicacionDao ubicacionDao;
        private static ITarifaDao tarifaDao;
        private static ICargaDao cargaDao;
        private static ISuministraDao suministraDao;
        private static IEstadoDao estadoDao;
        private static IConsumoDao consumoDao;



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

        public long crearTarifa(double precio, long hora, DateTime fecha)
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
            servicio = kernel.Get<IServiceControlador>();
            servicioBateria = kernel.Get<IServiceBateria>();
            servicioEstado = kernel.Get<IServiceEstado>();
            servicioTarifa = kernel.Get<IServiceTarifa>();
            servicioUbicacion = kernel.Get<IServiceUbicacion>();

            bateriaDao = kernel.Get<IBateriaDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();
            ubicacionDao = kernel.Get<IUbicacionDao>();
            cargaDao = kernel.Get<ICargaDao>();
            suministraDao = kernel.Get<ISuministraDao>();
            tarifaDao = kernel.Get<ITarifaDao>();
            estadoDao = kernel.Get<IEstadoDao>();
            consumoDao = kernel.Get<IConsumoDao>();

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
        //public void gestionDeRatiosTest()
        //{
        //    // Estado: "sin actividad" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifas
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "sin actividad"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaActual);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "sin actividad"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaIni);

        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "suministrando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        // Comprobamos precio medio
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);

        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest2()
        //{
        //    // Estado: "sin actividad" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "sin actividad"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaActual);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "sin actividad"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "sin actividad"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        // Comprobamos precio medio
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest3()
        //{
        //    // Estado: "sin actividad" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "sin actividad"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaActual);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "sin actividad"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        // Comprobamos precio medio
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest4()
        //{
        //    // Estado: "sin actividad" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "sin actividad"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaActual);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "sin actividad"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        // Comprobamos precio medio
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest5()
        //{
        //    // Estado: "sin actividad" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "sin actividad"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaActual);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "sin actividad"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        // Comprobamos precio medio
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest6()
        //{
        //    // Estado: "sin actividad" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "sin actividad"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaActual);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "sin actividad"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        // Comprobamos precio medio
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest7()
        //{
        //    // Estado: "sin actividad" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "sin actividad"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaActual);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "sin actividad"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        // Comprobamos precio medio
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest8()
        //{
        //    // Estado: "sin actividad" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "sin actividad"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "sin actividad"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaActual);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "sin actividad"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        // Comprobamos precio medio
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest9()
        //{
        //    // Estado: "cargando"
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "cargando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "cargando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "cargando" -> "suministrando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, (1000*100 + (consumoCargaBateria * tarifa.precio)) / (1000 + consumoCargaBateria));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest10()
        //{
        //    // Estado: "cargando" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);


        //        //comprobamos que el estado es "cargando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "cargando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "sin actividad" -> "sin actividad"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, (1000 * 100 + (consumoCargaBateria * tarifa.precio)) / (1000 + consumoCargaBateria));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest11()
        //{
        //    // Estado: "cargando" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "cargando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 10;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "cargando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "cargando" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, (1000 * 100 + (consumoCargaBateria * tarifa.precio)) / (1000 + consumoCargaBateria));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest12()
        //{
        //    // Estado: "cargando" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "cargando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 10;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "cargando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "cargando" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000); // no se cambia de estado

        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100); // no se cambia de estado

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest13()
        //{
        //    // Estado: "cargando" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "cargando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "cargando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "cargando" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, (1000 * 100 + (consumoCargaBateria * tarifa.precio)) / (1000 + consumoCargaBateria));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest14()
        //{
        //    // Estado: "cargando" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "cargando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "cargando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "cargando"" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000 ); // no cambia de estado

        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100 ); // no cambia de estado

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest15()
        //{
        //    // Estado: "cargando"" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "cargando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "cargando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "cargando" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, (1000 * 100 + (consumoCargaBateria * tarifa.precio)) / (1000 + consumoCargaBateria));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest16()
        //{
        //    // Estado: "cargando" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "cargando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "cargando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 0;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "cargando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "cargando" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000); //no cambia de estado

        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest17()
        //{
        //    // Estado: "suministrando"
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "suministrando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "suministrando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "suministrando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000); //no cambia de estado

        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest18()
        //{
        //    // Estado: "suministrando" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "suministrando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);
        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "suministrando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "sin actividad"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 - consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest19()
        //{
        //    // Estado: "suministrando" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "suministrando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "suministrando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 - consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100 );

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest20()
        //{
        //    // Estado: "cargando" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "suministrando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "suministrando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual); 


        //        //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 - consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100 );

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest21()
        //{
        //    // Estado: "suministrando" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "suministrando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "suministrando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 - consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100 );

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest22()
        //{
        //    // Estado: "suministrando" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "suministrando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "suministrando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "suministrando"" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 - consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100 );

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest23()
        //{
        //    // Estado: "cargando"" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "suministrando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "suministrando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 - consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100 );

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest24()
        //{
        //    // Estado: "suministrando" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "suministrando"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "suministrando"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "suministrando"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "suministrando" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double consumoCargaBateria = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 - consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        Assert.AreEqual(b.precioMedio, 100 );

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);


        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest25()
        //{
        //    // Estado: "carga y suministra"
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double suministra = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);
        //        double carga = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);
               
        //        double consumoCargaBateria = carga - suministra;

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        //preciomedioNuevo = (((b.kwHAlmacenados - kwHSuministrados) * b.precioMedio) + (kwHCargados * tarifa.precio)) / (b.kwHAlmacenados + kwHCargados);
        //        Assert.AreEqual(b.precioMedio, ((1000- suministra)*100 + (carga * tarifa.precio)) / (1000 + carga));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);


        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest26()
        //{
        //    // Estado: "suministrando"
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "suministrando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double suministra = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);
        //        double carga = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        double consumoCargaBateria = carga - suministra;

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        //preciomedioNuevo = (((b.kwHAlmacenados - kwHSuministrados) * b.precioMedio) + (kwHCargados * tarifa.precio)) / (b.kwHAlmacenados + kwHCargados);
        //        Assert.AreEqual(b.precioMedio, ((1000 - suministra) * 100 + (carga * tarifa.precio)) / (1000 + carga));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest27()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "sin actividad"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double suministra = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);
        //        double carga = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        double consumoCargaBateria = carga - suministra;

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        //preciomedioNuevo = (((b.kwHAlmacenados - kwHSuministrados) * b.precioMedio) + (kwHCargados * tarifa.precio)) / (b.kwHAlmacenados + kwHCargados);
        //        Assert.AreEqual(b.precioMedio, ((1000 - suministra) * 100 + (carga * tarifa.precio)) / (1000 + carga));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest28()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 15;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //         //obtenemos la bateria
        //         b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double suministra = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);
        //        double carga = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        double consumoCargaBateria = carga - suministra;

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, 1000 );
        //        // no hay cambio de estado
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest29()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 15;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double suministra = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);
        //        double carga = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        double consumoCargaBateria = carga - suministra;

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        //preciomedioNuevo = (((b.kwHAlmacenados - kwHSuministrados) * b.precioMedio) + (kwHCargados * tarifa.precio)) / (b.kwHAlmacenados + kwHCargados);
        //        Assert.AreEqual(b.precioMedio, ((1000 - suministra) * 100 + (carga * tarifa.precio)) / (1000 + carga));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest30()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double suministra = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);
        //        double carga = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        double consumoCargaBateria = carga - suministra;

        //        // Comprobamos kwh almacenados | no hay cambio de estado
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest31()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra"" -> "cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double suministra = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);
        //        double carga = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        double consumoCargaBateria = carga - suministra;

        //        // Comprobamos kwh almacenados 
        //        Assert.AreEqual(b.kwHAlmacenados, (1000 + consumoCargaBateria));
        //        // Comprobamos precio medio   100 precio medio anterior| kwHAlmacenados Antes = 1000;
        //        //preciomedioNuevo = (((b.kwHAlmacenados - kwHSuministrados) * b.precioMedio) + (kwHCargados * tarifa.precio)) / (b.kwHAlmacenados + kwHCargados);
        //        Assert.AreEqual(b.precioMedio, ((1000 - suministra) * 100 + (carga * tarifa.precio)) / (1000 + carga));

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest32()
        //{
        //    // Estado: "carga y suministra"
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 10;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //obtenemos la bateria
        //        b = bateriaDao.Find(bateriaId);

        //        // calculamos el consumo
        //        double suministra = servicioUbicacion.calcularConsumo(consumoActual, horaIni, horaActual);
        //        double carga = servicioUbicacion.calcularConsumo(b.capacidadCargador, horaIni, horaActual);

        //        double consumoCargaBateria = carga - suministra;

        //        // Comprobamos kwh almacenados | No hay cambio de estado
        //        Assert.AreEqual(b.kwHAlmacenados, 1000);
        //        Assert.AreEqual(b.precioMedio, 100);

        //        //comprobamos el ultimo consumo
        //        u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Consumo c = consumoDao.Find((long)u.ultimoConsumo);
        //        Assert.AreEqual(consumoActual, c.consumoActual);
        //    }
        //}

        //[TestMethod()]
        //public void gestionDeRatiosTest33()
        //{
        //    // Estado: "carga y suministra"
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, almacenajeMaximoKwH-8, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 5;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 10;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "sin actividad"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);


        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest34()
        //{
        //    // Estado: "suministrando"
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, almacenajeMaximoKwH-7, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 10;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "suministrando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);


        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest35()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, almacenajeMaximoKwH-6, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 10;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "sin actividad"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest36()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, almacenajeMaximoKwH-5, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 15;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 10;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "carga y suministra"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);


        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest37()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra <  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, almacenajeMaximoKwH-4, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = null;
        //        double? ratioCargaNuevo = 15;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 10;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "sin actividad"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);


        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest38()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, almacenajeMaximoKwH -2, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 10;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> "suministrando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);


        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest39()
        //{
        //    // Estado: "carga y suministra" 
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga <  %Bateria
        //    //    ratioUso >= Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, almacenajeMaximoKwH-1, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 3;
        //        double? ratioUsoNuevo = 2500;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 10;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra"" -> "sin actividad"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("sin actividad");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //    }
        //}


        //[TestMethod()]
        //public void gestionDeRatiosTest40()
        //{
        //    // Estado: "carga y suministra"
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, almacenajeMaximoKwH - 1, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 10;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);


        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> " suministrando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("suministrando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);


        //    }
        //}


        //[TestMethod()]
        //public void cambiarRatiosBateriaTest()
        //{
        //    // Estado: "carga y suministra"
        //    // ratioCompra >=  Tarifa
        //    //  ratioCarga >=  %Bateria
        //    //    ratioUso < Tarifa 
        //    using (var scope = new TransactionScope())
        //    {
        //        //kwHAlmacenados = 1000;
        //        //almacenajeMaximoKwH = 20000;   => 5% de carga

        //        /*
        //            precioMedio = 100;
        //            kwHAlmacenados = 1000;
        //            almacenajeMaximoKwH = 20000;
        //            ratioCompra = 50;                 =>  ratioCompra <   Tarifa
        //            ratioCarga = 40;                  =>   ratioCarga >=  %Bateria
        //            ratioUso = 45;                    =>     ratioUso <   Tarifa
        //         */
        //        crearEstados();
        //        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
        //        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

        //        //Creamos Tarifaz
        //        DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        crearTarifas24H(fechaActual);

        //        //Creamos Bateria
        //        long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
        //        long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
        //        fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

        //        //Ponemos el estado a "carga y suministra"
        //        long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
        //        Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
        //        //  hora actual
        //        TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        //        servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);

        //        //comprobamos que el estado es "carga y suministra"
        //        SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

        //        //obtenemos la bateria
        //        var b = bateriaDao.Find(bateriaId);

        //        // El ratio menor valor que el precio de la tarifa
        //        double? ratioCompraNuevo = 2500;
        //        double? ratioCargaNuevo = 10;
        //        double? ratioUsoNuevo = null;


        //        servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);

        //        // hora actual
        //        horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //        // Tarifa actual
        //        int horaTarifa = horaActual.Hours;
        //        TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

        //        //ponemos la bateria suministradora
        //        servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);

        //        //creamos el consumo de la ubicacion
        //        TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
        //        double consumoActual = 1;
        //        long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);

        //        //comprobamos que la ubicacion tiene el ultimo consumo
        //        Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
        //        Assert.AreEqual(consumoId, u.ultimoConsumo);

        //        // gestion de :   Estado: "carga y suministra"
        //        servicio.gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

        //        //cambiamos ratios de bateria suministradora
        //        ratioCompraNuevo = 2500;
        //        ratioCargaNuevo = 3;
        //        ratioUsoNuevo = 2500;

        //        servicio.cambiarRatiosBateria( bateriaId, ratioCompraNuevo, ratioCargaNuevo, ratioUsoNuevo);

        //        //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> " cargando"
        //        long estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

        //        //cambiamos ratios de bateria NO suministradora
        //        servicio.cambiarRatiosBateria(bateriaId2, ratioCompra, ratioCarga, ratioUso);

        //        //comprobamos que no acectan a la ubicacion
        //        estadoIdC = servicioEstado.BuscarEstadoPorNombre("cargando");
        //        estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
        //        Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);
        //    }
        //}


        [TestMethod()]
        public void ComprobarRatiosUbicacionesTest()
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
                long ubicacionId2 = crearUbicacion(codigoPostal, localidad, calle, portal, numero);
                long ubicacionId3 = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                //Creamos Tarifaz
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                crearTarifas24H(fechaActual);

                //Creamos Bateria
                long bateriaId = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId2 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId3 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);
                long bateriaId4 = servicioBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                //Ponemos el estado a "carga y suministra"
                long estadoIdS = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                Bateria bateria = servicioBateria.BuscarBateriaById(bateriaId);
                Bateria bateria2 = servicioBateria.BuscarBateriaById(bateriaId2);
                Bateria bateria3 = servicioBateria.BuscarBateriaById(bateriaId3);
                //bateria no suministradora
                long estadoIdSA = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                Bateria bateria4 = servicioBateria.BuscarBateriaById(bateriaId3);


                //  hora actual
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                servicioBateria.CambiarEstadoEnBateria(bateriaId, estadoIdS, 0, 0, horaActual);
                servicioBateria.CambiarEstadoEnBateria(bateriaId2, estadoIdS, 0, 0, horaActual);
                servicioBateria.CambiarEstadoEnBateria(bateriaId3, estadoIdS, 0, 0, horaActual);
                servicioBateria.CambiarEstadoEnBateria(bateriaId4, estadoIdSA, 0, 0, horaActual); // bateria no suministradora


                //comprobamos que el estado es "carga y suministra"
                SeEncuentraDTO estadoBateria = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                SeEncuentraDTO estadoBateria2 = servicioEstado.BuscarEstadoBateriaById(bateria2.estadoBateria);
                SeEncuentraDTO estadoBateria3 = servicioEstado.BuscarEstadoBateriaById(bateria3.estadoBateria);
                Assert.AreEqual(estadoBateria.estadoId, estadoIdS);

                //obtenemos las baterias
                var b = bateriaDao.Find(bateriaId);
                var b2 = bateriaDao.Find(bateriaId);
                var b3 = bateriaDao.Find(bateriaId);

                // El ratio menor valor que el precio de la tarifa
                double? ratioCompraNuevo = 2500;
                double? ratioCargaNuevo = 10;
                double? ratioUsoNuevo = null;


                servicioBateria.ModificarRatios(bateriaId, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);
                servicioBateria.ModificarRatios(bateriaId2, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);
                servicioBateria.ModificarRatios(bateriaId3, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);
                servicioBateria.ModificarRatios(bateriaId4, ratioCargaNuevo, ratioCompraNuevo, ratioUsoNuevo);


                // hora actual
                horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                // Tarifa actual
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = servicioTarifa.TarifaActual(fechaActual, horaTarifa);

                //ponemos la bateria suministradora
                servicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaId);
                servicioUbicacion.CambiarBateriaSuministradora(ubicacionId2, bateriaId2);
                servicioUbicacion.CambiarBateriaSuministradora(ubicacionId3, bateriaId3);

                //creamos el consumo de la ubicacion
                TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, 0, 0); // ponemos asi para no poner un sleep
                double consumoActual = 1;
                long consumoId = servicioUbicacion.crearConsumo(ubicacionId, consumoActual, horaIni);
                long consumoId2 = servicioUbicacion.crearConsumo(ubicacionId2, consumoActual, horaIni);
                long consumoId3 = servicioUbicacion.crearConsumo(ubicacionId3, consumoActual, horaIni);

                //comprobamos que la ubicacion tiene el ultimo consumo
                Ubicacion u = servicioUbicacion.buscarUbicacionById(ubicacionId);
                Assert.AreEqual(consumoId, u.ultimoConsumo);
                Ubicacion u2 = servicioUbicacion.buscarUbicacionById(ubicacionId2);
                Assert.AreEqual(consumoId2, u2.ultimoConsumo);
                Ubicacion u3 = servicioUbicacion.buscarUbicacionById(ubicacionId3);
                Assert.AreEqual(consumoId3, u3.ultimoConsumo);

                // gestion de :   Estado: "carga y suministra"
                servicio.ComprobarRatiosUbicaciones( fechaActual, horaActual);


                //comprobamos que se ha cometido el cambio de estado: "carga y suministra" -> " carga y suministra"
                long estadoIdC = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo = servicioEstado.BuscarEstadoBateriaById(bateria.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

                long estadoIdC2 = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo2 = servicioEstado.BuscarEstadoBateriaById(bateria2.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

                long estadoIdC3 = servicioEstado.BuscarEstadoPorNombre("carga y suministra");
                SeEncuentraDTO estadoBateriaNuevo3 = servicioEstado.BuscarEstadoBateriaById(bateria3.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

                //no es bateria suministradora
                long estadoIdC4 = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                SeEncuentraDTO estadoBateriaNuevo4 = servicioEstado.BuscarEstadoBateriaById(bateria4.estadoBateria);
                Assert.AreEqual(estadoBateriaNuevo.estadoId, estadoIdC);

            }
        }
    }
}
