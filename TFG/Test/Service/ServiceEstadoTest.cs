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
using Es.Udc.DotNet.TFG.Model.Daos.EstadoBateriaDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Es.Udc.DotNet.TFG.Model.Service.Estados;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceEstadoTest
    {
       private static IKernel kernel;
        private static IServiceBateria servicio;
        private static IServiceTarifa servicioTarifa;
        private static IServiceEstado servicioEstado;

        private static IBateriaDao bateriaDao;
        private static IUsuarioDao usuarioDao;
        private static IUbicacionDao ubicacionDao;
        private static ITarifaDao tarifaDao;
        private static ICargaDao cargaDao;
        private static ISuministraDao suministraDao;
        private static IEstadoBateriaDao estadoBateriaDao;
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

        //Crear estado
        public long crearEstado(string nombre)
        {
            Estado estado = new Estado();
            estado.nombre = nombre;
            estadoDao.Create(estado);

            return estado.estadoId;

        }
        
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
        private const double kwAlmacenados = 1000;
        private const double almacenajeMaximoKw = 2000;
        private DateTime fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private const string marca = "marca";
        private const string modelo = "modelo";
        private const double ratioCarga = 40;
        private const double ratioCompra = 50;
        private const double ratioUso = 45;


        // TARIFA

        public long CrearTarifa(long precio, long hora, DateTime fecha)
        {
            Tarifa t = new Tarifa();
            t.precio = precio;
            t.hora = hora;
            t.fecha = fecha;
            tarifaDao.Create(t);
            return t.tarifaId;
        }

        public void Crearestados()
        {
            long estadoId = crearEstado("sin actividad");
            long estadoId2 = crearEstado("cargando");
            long estadoId3 = crearEstado("suministrando");
            long estadoId4 = crearEstado("carga y suministra");
        }

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
            bateriaDao = kernel.Get<IBateriaDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();
            ubicacionDao = kernel.Get<IUbicacionDao>();
            cargaDao = kernel.Get<ICargaDao>();
            suministraDao = kernel.Get<ISuministraDao>();
            tarifaDao = kernel.Get<ITarifaDao>();
            estadoBateriaDao = kernel.Get<IEstadoBateriaDao>();
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
                public void verTodosLosEstadosTest()
                {
                    using (var scope = new TransactionScope())
                    {
                        Crearestados();
                        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                        long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);



                        var estadoResult = servicioEstado.verTodosLosEstados();

                        //buscamos el estado creado


                        Assert.AreEqual(estadoResult[0].nombre, "sin actividad");
                        Assert.AreEqual(estadoResult[1].nombre, "cargando");
                        Assert.AreEqual(estadoResult[2].nombre, "suministrando");
                        Assert.AreEqual(estadoResult[3].nombre, "carga y suministra");
                        Assert.AreEqual(estadoResult.Count(), 4);



                    } 

        }
        

                [TestMethod()]
                public void CrearEstadoBateriaTest()
                {
                    using (var scope = new TransactionScope())
                    {
                        //creamos los estados
                        long estadoId = crearEstado( "sin actividad");
                        long estadoId2 = crearEstado("cargando");
                        long estadoId3 = crearEstado("suministrando");
                        long estadoId4 = crearEstado("c y suministrando");

                        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                        long ubicacionId = crearUbicacion( codigoPostal, localidad, calle, portal, numero);

                        long bateriaId = servicio.CrearBateria( ubicacionId,  usuarioId,  precioMedio,  kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion,  marca,  modelo,  ratioCarga,  ratioCompra,  ratioUso);


                        int hour1 = 1;
                        int hour2 = 0;
                        int minutes = 0;
                        int seconds = 0;
                        TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                        TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                        DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


                        var estadoResult = servicioEstado.CrearEstadoBateria( horaIni,  fecha,  bateriaId, estadoId);

                        //buscamos el estado creado
                        var estadoBateria = estadoBateriaDao.Find(estadoResult);

                        //buscamos bateria
                        Bateria bateria = servicio.BuscarBateriaById(bateriaId);

                        //comprobamos el atributo de estado en la bateria
                        Assert.AreEqual(bateria.estadoBateria, estadoBateria.seEncuentraId);

                        //comprobaciones
                        Assert.AreEqual(estadoId, estadoBateria.estadoId);
                        Assert.AreEqual(horaIni, estadoBateria.horaIni);
                        Assert.AreEqual(horaFin, estadoBateria.horaFin);
                        Assert.AreEqual(fecha, estadoBateria.fecha);
                        Assert.AreEqual(bateriaId, estadoBateria.bateriaId);
                        Assert.AreEqual(estadoId, estadoBateria.estadoId);


                    }
                }
        
                [TestMethod()]
                public void BuscarEstadoBateriaPorIdTest()
                {
                    using (var scope = new TransactionScope())
                    {
                
                long estadoId = crearEstado("sin actividad");
                long estadoId2 = crearEstado("cargando");
                long estadoId3 = crearEstado("suministrando");
                long estadoId4 = crearEstado("carga y suministra");



                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                        long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);


                        int hour1 = 1;
                        int hour2 = 0;
                        int minutes = 0;
                        int seconds = 0;
                        TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                        TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                        DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


                        var estadoResult = servicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, estadoId);

                        //buscamos el estado creado

                        var estadoBateria = servicioEstado.BuscarEstadoBateriaById(estadoResult);


                        Assert.AreEqual(estadoId, estadoBateria.estadoId);
                        Assert.AreEqual(horaIni, estadoBateria.horaIni);
                        Assert.AreEqual(horaFin, estadoBateria.horaFin);
                        Assert.AreEqual(fecha, estadoBateria.fecha);
                        Assert.AreEqual(bateriaId, estadoBateria.bateriaId);
                        Assert.AreEqual(estadoId, estadoBateria.estadoId);


                    }
                }

                [TestMethod()]
                public void NombreEstadoEnEstadoBateriaByIdTest()
                {
                    using (var scope = new TransactionScope())
                    {

                        long estadoId = crearEstado("sin actividad");
                        long estadoId2 = crearEstado("cargando");
                        long estadoId3 = crearEstado("suministrando");
                        long estadoId4 = crearEstado("carga y suministra");



                        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                        long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);


                        int hour1 = 1;
                        int hour2 = 0;
                        int minutes = 0;
                        int seconds = 0;
                        TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                        TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                        DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


                        var estadoResult = servicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, estadoId2);

                        //buscamos el estado creado

                        var nombreEstado = servicioEstado.NombreEstadoEnEstadoBateriaById(estadoResult);


                        Assert.AreEqual("cargando", nombreEstado);



                    }
                }

                [TestMethod()]
                public void MostrarEstadoBateriaPorFechaTest()
                {
                    using (var scope = new TransactionScope())
                    {

                        //Creamos los estados
                        long estadoId = crearEstado("sin actividad");
                        long estadoId2 = crearEstado("cargando");
                        long estadoId3 = crearEstado("suministrando");
                        long estadoId4 = crearEstado("carga y suministra");

                        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                        //Creamos baterias

                        long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                        long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);




                        int hour1 = 1;
                        int hour2 = 2;
                        int minutes = 0;
                        int seconds = 0;
                        TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                        TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                        DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
                        DateTime fecha3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(2).Day);
                        DateTime fecha4 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(3).Day);

                        //creamos estadosBateria



                        long estadoBateriaId = servicioEstado.CrearEstadoBateria( horaIni, fecha2, bateriaId, estadoId);

                        long estadoBateriaId2 = servicioEstado.CrearEstadoBateria( horaIni, fecha2, bateriaId, estadoId);

                        long estadoBateriaId3 = servicioEstado.CrearEstadoBateria( horaIni, fecha2, bateriaId, estadoId);

                        long estadoBateriaId4 = servicioEstado.CrearEstadoBateria( horaIni, fecha2, bateriaId, estadoId);

                        long estadoBateriaId5 = servicioEstado.CrearEstadoBateria( horaIni, fecha2, bateriaId, estadoId);

                        long estadoBateriaId6 = servicioEstado.CrearEstadoBateria( horaIni, fecha4, bateriaId, estadoId);


                        //buscamos el estado creado
                        int startIndex = 0;
                        int count = 10;
                        var estadoBateria = servicioEstado.MostrarEstadoBateriaPorFecha(bateriaId, fecha2, fecha3, startIndex, count);


                        Assert.AreEqual(estadoBateriaId, estadoBateria[0].seEncuentraId);
                        Assert.AreEqual(estadoBateriaId2, estadoBateria[1].seEncuentraId);
                        Assert.AreEqual(estadoBateriaId3, estadoBateria[2].seEncuentraId);
                        Assert.AreEqual(estadoBateriaId4, estadoBateria[3].seEncuentraId);
                        Assert.AreEqual(estadoBateriaId5, estadoBateria[4].seEncuentraId);
                        Assert.AreEqual(5, estadoBateria.Count());


                    }
                }


                [TestMethod()]
                public void PonerHorafinEstadoBateriaTest()
                {
                    using (var scope = new TransactionScope())
                    {
                        //Creamos los estados
                        long estadoId = crearEstado("sin actividad");
                        long estadoId2 = crearEstado("cargando");
                        long estadoId3 = crearEstado("suministrando");
                        long estadoId4 = crearEstado("carga y suministra");

                        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                        //Creamos baterias
                        long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);
                        long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);


                        int hour1 = 1;
                        int hour2 = 2;
                        int minutes = 0;
                        int seconds = 0;
                        TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                        TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                        DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        DateTime fecha2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
                        DateTime fecha3 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(2).Day);
                        //creamos estadosBateria



                        long estadoBateriaId = servicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, estadoId);
                        hour1++;
                        hour2++;
                        long estadoBateriaId2 = servicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, estadoId);
                        hour1++;
                        hour2++;
                        long estadoBateriaId3 = servicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, estadoId);
                        hour1++;
                        hour2++;
                        long estadoBateriaId4 = servicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, estadoId);
                        hour1++;
                        hour2++;
                        long estadoBateriaId5 = servicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, estadoId);
                        hour1++;
                        hour2++;
                        long estadoBateriaId6 = servicioEstado.CrearEstadoBateria(horaIni, fecha3, bateriaId, estadoId);


                        //buscamos el estado creado

                        int hour3 = 3;
                        int minutes10 = 10;
                        int seconds10 = 10;

                        TimeSpan horaFin2 = new TimeSpan(hour3, minutes10, seconds10);
                        var estadoBateria = servicioEstado.PonerHorafinEstadoBateria(estadoBateriaId, horaFin2);
                        var estadoBateria2 = servicioEstado.PonerHorafinEstadoBateria(estadoBateriaId2, horaFin2);
                        var estadoBateria3 = servicioEstado.PonerHorafinEstadoBateria(estadoBateriaId3, horaFin2);
                        var estadoBateria4 = servicioEstado.PonerHorafinEstadoBateria(estadoBateriaId4, horaFin2);
                        var estadoBateria5 = servicioEstado.PonerHorafinEstadoBateria(estadoBateriaId5, horaFin2);
                        var estadoBateria6 = servicioEstado.PonerHorafinEstadoBateria(estadoBateriaId6, horaFin2);

                        Assert.AreEqual(estadoBateria, true);
                        Assert.AreEqual(estadoBateria2, true);
                        Assert.AreEqual(estadoBateria3, true);
                        Assert.AreEqual(estadoBateria4, true);
                        Assert.AreEqual(estadoBateria5, true);
                        Assert.AreEqual(estadoBateria6, true);

                        // comprobamos que se ha modificado horafinal
                        SeEncuentraDTO estadobateria1 = servicioEstado.BuscarEstadoBateriaById(estadoBateriaId);
                        Assert.AreEqual(horaFin2, estadobateria1.horaFin);

                        SeEncuentraDTO estadobateria2 = servicioEstado.BuscarEstadoBateriaById(estadoBateriaId2);
                        Assert.AreEqual(horaFin2, estadobateria2.horaFin);

                        SeEncuentraDTO estadobateria3 = servicioEstado.BuscarEstadoBateriaById(estadoBateriaId3);
                        Assert.AreEqual(horaFin2, estadobateria1.horaFin);

                        SeEncuentraDTO estadobateria4 = servicioEstado.BuscarEstadoBateriaById(estadoBateriaId4);
                        Assert.AreEqual(horaFin2, estadobateria1.horaFin);

                        SeEncuentraDTO estadobateria5 = servicioEstado.BuscarEstadoBateriaById(estadoBateriaId5);
                        Assert.AreEqual(horaFin2, estadobateria1.horaFin);

                        SeEncuentraDTO estadobateria6 = servicioEstado.BuscarEstadoBateriaById(estadoBateriaId6);
                        Assert.AreEqual(horaFin2, estadobateria1.horaFin);


                    }
                }

                public void BuscarEstadoBateriaPorNombreTest()
                {
                    using (var scope = new TransactionScope())
                    {

                        long estadoId = crearEstado("sin actividad");
                        long estadoId2 = crearEstado("cargando");
                        long estadoId3 = crearEstado("suministrando");
                        long estadoId4 = crearEstado("carga y suministra");

                        long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                        long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                        long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
                     fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);


                        int hour1 = 1;
                        int hour2 = 0;
                        int minutes = 0;
                        int seconds = 0;
                        TimeSpan horaIni = new TimeSpan(hour1, minutes, seconds);
                        TimeSpan horaFin = new TimeSpan(hour2, minutes, seconds);
                        DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


                        var estadoResult = servicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, estadoId);

                        //buscamos el estado creado

                        var estadoid = servicioEstado.BuscarEstadoPorNombre("sin actividad");
                        var estadoid2 = servicioEstado.BuscarEstadoPorNombre("cargando");
                        var estadoid3 = servicioEstado.BuscarEstadoPorNombre("suministrando");
                        var estadoid4 = servicioEstado.BuscarEstadoPorNombre("carga y suministra");

                        Assert.AreEqual("sin actividad", estadoDao.Find(estadoid).nombre);
                        Assert.AreEqual("cargando", estadoDao.Find(estadoid2).nombre);
                        Assert.AreEqual("suministrando", estadoDao.Find(estadoid3).nombre);
                        Assert.AreEqual("carga y suministra", estadoDao.Find(estadoid4).nombre);


                    }
                }
        
        [TestMethod()]
        public void BuscarEstadoPorIdTest()
        {
                       using (var scope = new TransactionScope())
                       {

                           long estadoId = crearEstado("sin actividad");
                           long estadoId2 = crearEstado("cargando");
                           long estadoId3 = crearEstado("suministrando");
                           long estadoId4 = crearEstado("carga y suministra");



                           //buscamos el estado creado

                           var estadoid = servicioEstado.BuscarEstadoPorId(estadoId);
                           var estadoid2 = servicioEstado.BuscarEstadoPorId(estadoId2);
                           var estadoid3 = servicioEstado.BuscarEstadoPorId(estadoId3);
                           var estadoid4 = servicioEstado.BuscarEstadoPorId(estadoId4);

                           Assert.AreEqual("sin actividad", estadoid);
                           Assert.AreEqual("cargando", estadoid2);
                           Assert.AreEqual("suministrando", estadoid3);
                           Assert.AreEqual("carga y suministra", estadoid4);


                       }

        }

    }
}
