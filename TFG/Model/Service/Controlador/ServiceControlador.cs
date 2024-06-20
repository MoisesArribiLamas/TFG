using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Es.Udc.DotNet.TFG.Model.Daos.CargaDao;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
using Es.Udc.DotNet.TFG.Model.Daos.SuministraDao;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Ninject;
using System.Net;
using System.Net.Sockets;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class ServiceControlador : IServiceControlador
    {

        [Inject]
        public IBateriaDao bateriaDao { private get; set; }


        [Inject]
        public ITarifaDao TarifaDao { private get; set; }

        [Inject]
        public IUbicacionDao ubicacionDao { private get; set; }

        [Inject]
        public IUsuarioDao UsuarioDao { private get; set; }

        [Inject]
        public IConsumoDao ConsumoDao { private get; set; }

        [Inject]
        public ICargaDao CargaDao { private get; set; }

        [Inject]
        public ISuministraDao SuministroDao { private get; set; }

        [Inject]
        public IServiceBateria ServicioBateria { private get; set; }

        [Inject]
        public IServiceUbicacion ServicioUbicacion { private get; set; }

        [Inject]
        public IServiceEstado ServicioEstado { private get; set; }

        [Inject]
        public IServiceTarifa ServicioTarifa { private get; set; }

        #region

        public void ControlCambioHoraODia() 
        {
            //ThreadStaticAttribute 
            while (true) {
                int milisegundos = Asincrono();
                Thread.Sleep(milisegundos);
            };
        }
        #endregion

        #region sockets

        Socket conexion; // s_client
        Socket listen;   // s_server

        public void ConexionClientes()
        {

            listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Socket conexion;
            IPEndPoint connect = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6500);

            listen.Bind(connect);

            // aceptamos hasta 10 conexiones
            listen.Listen(10);

            // conexion con el cliente
            conexion = listen.Accept();

            Console.WriteLine(" Conexion aceptada ");
            while (true)
            {
                // almacena la informacion recibida
                byte[] recibir_info = new byte[100];

                string data = "";
                int array_size = 0;

                // guardamos el nº de bytes que envio (donde se almacena la informacion, desde donde va a guardar la informacion, hasta donde)
                array_size = conexion.Receive(recibir_info, 0, recibir_info.Length, 0);

                // nos quedamos sólo con la información quitamos los blancos sobrantes
                Array.Resize(ref recibir_info, array_size);

                data = Encoding.Default.GetString(recibir_info);
                //if (data != "")
                //{
                double consumo = Convert.ToDouble(data);
                ServicioUbicacion.modificarConsumoActual(704, consumo);
                //}
                //Console.WriteLine("La informacion recibida {0}", data);

                //byte[] msg = Encoding.ASCII.GetBytes("Recibido");
                //listen.Send(msg);
                //Console.ReadKey();
            }

        }
        #endregion

        #region
        public void Start()
        {
            Thread t;
            while (true)
            {
                Console.Write("Esperando Conexion");
                conexion = listen.Accept();
                t = new Thread(clientConnection);
                t.Start(conexion);
                Console.WriteLine("Conectado");
            }
        }


        #endregion

        #region
        public void clientConnection(object s)
        {
            Socket conexion = (Socket)s;
            while (true)
            {
                // almacena la informacion recibida
                byte[] recibir_info = new byte[100];

                string data = "";
                int array_size = 0;

                // guardamos el nº de bytes que envio (donde se almacena la informacion, desde donde va a guardar la informacion, hasta donde)
                array_size = conexion.Receive(recibir_info, 0, recibir_info.Length, 0);

                // nos quedamos sólo con la información quitamos los blancos sobrantes
                Array.Resize(ref recibir_info, array_size);

                data = Encoding.Default.GetString(recibir_info);
                //if (data != "")
                //{
                double consumo = Convert.ToDouble(data);
                ServicioUbicacion.modificarConsumoActual(704, consumo);
                //}
                //Console.WriteLine("La informacion recibida {0}", data);

                //byte[] msg = Encoding.ASCII.GetBytes("Recibido");
                //listen.Send(msg);
                //Console.ReadKey();
            }
        }
        #endregion

        #region Parte Controla los cambios de hora para los calculos internos

        [Transactional]
        public int Asincrono()
        {
            // Fecha y hora actual
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            int hora = horaActual.Hours;
            int minutos = horaActual.Minutes;
            int segundos = horaActual.Seconds;

            

            // Si cambiamos de hora
            if (minutos == 0 || minutos == 1) {

            }
            // Si cambiamos de dia, nuevas tarifas
            if (hora == 0 && (minutos == 0 || minutos == 1))
            {
                CrearTarifasDeHoy(fechaActual);
            }
           

            // obtenemos todas las baterias suministradoras
            List<long?> bS = ServicioUbicacion.todasLasBateriasSuministradoras();
            bool corto = false;

            foreach (long? bateriaId in bS)
            {
                Bateria b = ServicioBateria.BuscarBateriaById((long)bateriaId);

                if (!corto)
                { // si hay baterias con poca energia, hacemos un timer mas corto
                    if (ServicioBateria.porcentajeDeCarga((long)bateriaId)-b.ratioCarga < 7) {
                        corto = true;
                    }
                }
                // Si cambiamos de hora
                if (minutos == 0 || minutos == 1)
                {
                    // buscamos el consumo (entidad) actual
                    Consumo c = ConsumoDao.UltimoConsumoUbicacion(b.bateriaId);
                    double consumoActual = c.consumoActual;
                    // en caso de que exista consumo
                    //pasar los datos de consumo a carga y suministra
                    //crear un nuevo consumo
                    // crear un nuevo estado actualizando los datos
                }
                

                gestionDeRatiosBateriaSuministradora( (long)bateriaId, fechaActual, horaActual);
            }

            if (corto)
            {
                int cambioHora = 59 - minutos;

                if ((cambioHora) < 10)
                {
                    return ((cambioHora+1)*60000)+((segundos+1)*1000); // un segundo despues del cambio de hora

                } else {
                    return (600000); // 10 minutos
                }
                
            }
            else // no hay bateria cerca de acabarse
            {
                int cambioHora = 59 - minutos;

                if ((cambioHora) < 30)
                {
                    return ((cambioHora + 1) * 60000) + ((segundos + 1) * 1000); // un segundo despues del cambio de hora

                }
                else
                {
                    return (1800000); // 30 minutos
                }
            }
        }
        #endregion



        #region cambio hora comprobar ratios de todas las ubicaciones
        [Transactional]
        public void ComprobarRatiosUbicaciones(DateTime fechaActual, TimeSpan horaActual)
        {
            // buscar todas la ubicaciones
            List<Ubicacion> ubicaciones = ubicacionDao.TodasLasUbicaciones();


            // obtener baterias suministradoras 
            foreach (Ubicacion u in ubicaciones)
            {
                // comprobar los ratios
                if (u.bateriaSuministradora != null) // Ubicaciones que tienen bateria suministradora
                {
                    gestionDeRatiosBateriaSuministradora((long)u.bateriaSuministradora, fechaActual, horaActual);
                }


            }

        }
        #endregion


        #region cambio manual de los ratios
        [Transactional]
        public void cambiarRatiosBateria(long bateriaId, double? ratioCarga, double? ratioCompra, double? ratioUso)
        {
            // buscamos la bateria.
            Bateria b = bateriaDao.Find(bateriaId);

            // Comprobamos si la bateria siministradora es la misma que la de los cambios.
            if (b.Ubicacion.bateriaSuministradora == null) {
                ServicioBateria.ModificarRatios(bateriaId, ratioCarga, ratioCompra, ratioUso);
            }
            else
            { 
                // Comprobamos si la bateria siministradora es la misma que la de los cambios.
                if ((long)b.Ubicacion.bateriaSuministradora == bateriaId)
                {
                    ServicioBateria.ModificarRatios(bateriaId, ratioCarga, ratioCompra, ratioUso);


                    DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                    gestionDeRatiosBateriaSuministradora(bateriaId, fechaActual, horaActual);

                }
                else // no es la bateria suministradora
                {
                    ServicioBateria.ModificarRatios(bateriaId, ratioCarga, ratioCompra, ratioUso);

                }
            }
        }
        #endregion

        #region Cambiar bateria suministradora
        [Transactional]
        public void CambiarBateriaSuministradora(long ubicacionId, long? bateriaSuministradora)
        {
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            // obtenemos la ubicacion
            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);

            // Obtenemos el consumo
            Consumo consumo = ConsumoDao.UltimoConsumoUbicacion(ubicacionId);

            // si hay bateriaSuministradora previa
            long? bateriaSuministradoraPrevia = ubicacion.bateriaSuministradora;
            if (bateriaSuministradoraPrevia != null)
            {
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // Obtenemos el estado
                String estado = ServicioBateria.EstadoDeLaBateria((long)bateriaSuministradoraPrevia);

                // Cerramos el consumo

                if (ubicacion.ultimoConsumo != null)
                {  //en el caso de que no exista consumo, no hace falta cerrarlo
                    //ServicioUbicacion.finalizarConsumo(ubicacionId, consumo.consumoActual, horaActual, estado, (long)bateriaSuministradoraPrevia);
                    ServicioUbicacion.actualizarConsumoActual(ubicacionId, horaActual, (bateriaSuministradora==null));
                }
                if (estado != "sin actividad")
                {
                    //ponemos el estado a "sin actividad"
                    //obtenemos la carga
                    Carga carga = ServicioBateria.UltimaCarga((long)bateriaSuministradoraPrevia);

                    if (carga != null)
                    { // la carga que hay sin contabilizar en la bateria
                        kwHCargados = carga.kwH;
                    }

                    //obtenemos suministra
                    Suministra suministra = ServicioBateria.UltimaSuministra((long)bateriaSuministradoraPrevia);

                    if (suministra != null) // lo suministrado que hay sin contabilizar
                    {
                        kwHSuministrados = suministra.kwH;
                    }

                    long estadoId = ServicioEstado.BuscarEstadoPorNombre("sin actividad");
                    ServicioBateria.CambiarEstadoEnBateria((long)bateriaSuministradoraPrevia, estadoId, kwHCargados, kwHSuministrados, horaActual);

                }
            }
            else
            {
                if (bateriaSuministradora != null)
                {
                    // cuando se pone la primera bateria y no hay un consumo, se crea un consumo inicial a 0
                    double consumoinicial = 0;
                    CrearConsumoInicial(ubicacionId, consumoinicial);
                }
            }

            ServicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaSuministradora);

            if (bateriaSuministradora != null)
            { 
                //comprobar los ratios con la nueva bateriaSuministradora
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                gestionDeRatiosBateriaSuministradora((long)bateriaSuministradora, fechaActual, horaActual);
            }

        }

        #endregion Cambiar bateria

        #region crear Consumo inicial
        [Transactional]
        public void CrearConsumoInicial(long ubicacionId, double consumoActual)
        {
            // obtenemos la hora y la fecha 
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Creamos el consumo
            long consumoId = ServicioUbicacion.crearConsumo(ubicacionId, consumoActual, fechaActual, horaActual);

            ////Comprobamos los ratios en la bateriasuministradora de la ubicacion
            //Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);
            //if (ubicacion.bateriaSuministradora != null)
            //{
            //    gestionDeRatiosBateriaSuministradora((long)ubicacion.bateriaSuministradora, fechaActual, horaActual);
            //}


        }

        #endregion crear Consumo

        #region Gestion de los ratios en una bateria suministradora

        [Transactional]
        public void gestionDeRatiosBateriaSuministradora(long bateriaId, DateTime fechaActual, TimeSpan horaActual)
        {
            double kwHCargados = 0;
            double kwHSuministrados = 0;
            long? consumo = null;

            // buscamos la bateria
            Bateria b = bateriaDao.Find(bateriaId);

            // Tarifa actual (hora)
            int horaTarifa = horaActual.Hours;

            // Buscar la tarifa actual
            TarifaDTO tarifa = ServicioTarifa.TarifaActual(fechaActual, horaTarifa);

            //consumo pendiente
            string estado = ServicioBateria.EstadoDeLaBateria(bateriaId);
            double consumoPendiente = ServicioUbicacion.CalcularConsumoParaCalculoRatios(b.ubicacionId, horaActual, estado, bateriaId);

            // porcentaje de la bateria sumandole el consumo pendiente
            double porcentajeCargaConConsumo = ServicioBateria.porcentajeDeCargaConConsumo(bateriaId, consumoPendiente);

            if (porcentajeCargaConConsumo > 100)
            {
                // Buscamos ultimo consumo, para modificarlo en caso de que se pase
                Ubicacion u = ServicioUbicacion.buscarUbicacionById(b.ubicacionId);
                consumo = (long)ServicioUbicacion.UltimoConsumoEnUbicacion(u.ubicacionId);
            }

            // si el ratio de carga (minimo 10%) es menor al porcentaje de la bateria => carga
            if (b.ratioCarga >= porcentajeCargaConConsumo)
            {

                if ((b.ratioUso < tarifa.precio)) // "sin actividad" -> "cargando" 
                {
                    if (porcentajeCargaConConsumo < 99) //  si la bateria esta al 100% no puede cargar
                    {
                        if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "carga y suministra")
                        {
                            //ponemos consumo en carga y/o suministra
                            ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                            //obtenemos la carga
                            Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                            if (carga != null)
                            { // la carga que hay sin contabilizar en la bateria
                                kwHCargados = carga.kwH;
                            }

                            //obtenemos suministra
                            Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                            if (suministra != null) // lo suministrado que hay sin contabilizar
                            {
                                kwHSuministrados = suministra.kwH;
                            }

                            long estadoId = ServicioEstado.BuscarEstadoPorNombre("carga y suministra");
                            ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                        }
                    }
                    //--------------------------------------------------------------------------------------------------------------
                    else
                    {
                        if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "suministrando")
                        {
                            //ponemos consumo en carga y/o suministra
                            ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                            //obtenemos la carga
                            Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                            if (carga != null)
                            { // la carga que hay sin contabilizar en la bateria
                                kwHCargados = carga.kwH;
                            }

                            //obtenemos suministra
                            Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                            if (suministra != null) // lo suministrado que hay sin contabilizar
                            {
                                kwHSuministrados = suministra.kwH;
                            }

                            long estadoId = ServicioEstado.BuscarEstadoPorNombre("suministrando");
                            ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                        }
                    }
                    //--------------------------------------------------------------------------------------------------------------------

                }
                else
                {
                    if (porcentajeCargaConConsumo < 99) //  si la bateria esta al 100% no puede cargar
                    {
                        if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "cargando")
                        {
                            //ponemos consumo en carga y/o suministra
                            ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                            //obtenemos la carga
                            Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                            if (carga != null)
                            { // la carga que hay sin contabilizar en la bateria
                                kwHCargados = carga.kwH;
                            }

                            //obtenemos suministra
                            Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                            if (suministra != null) // lo suministrado que hay sin contabilizar
                            {
                                kwHSuministrados = suministra.kwH;
                            }

                            long estadoId = ServicioEstado.BuscarEstadoPorNombre("cargando");
                            ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                        }
                    }
                    //--------------------------------------------------------------------------------------------------------------
                    else
                    {
                        if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "sin actividad")
                        {
                            //ponemos consumo en carga y/o suministra
                            ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                            //obtenemos la carga
                            Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                            if (carga != null)
                            { // la carga que hay sin contabilizar en la bateria
                                kwHCargados = carga.kwH;
                            }

                            //obtenemos suministra
                            Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                            if (suministra != null) // lo suministrado que hay sin contabilizar
                            {
                                kwHSuministrados = suministra.kwH;
                            }

                            long estadoId = ServicioEstado.BuscarEstadoPorNombre("sin actividad");
                            ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                        }
                    }
                    //--------------------------------------------------------------------------------------------------------------------
                }

            }
            else
                // el ratio de compra < precio tarifa
                if (b.ratioCompra < tarifa.precio)
            {

                if ((b.ratioUso < tarifa.precio))
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "suministrando")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                        //obtenemos la carga
                        Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                        if (carga != null)
                        { // la carga que hay sin contabilizar en la bateria
                            kwHCargados = carga.kwH;
                        }

                        //obtenemos suministra
                        Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                        if (suministra != null) // lo suministrado que hay sin contabilizar
                        {
                            kwHSuministrados = suministra.kwH;
                        }

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("suministrando");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                    }

                }
                else
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "sin actividad")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                        //obtenemos la carga
                        Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                        if (carga != null)
                        { // la carga que hay sin contabilizar en la bateria
                            kwHCargados = carga.kwH;
                        }

                        //obtenemos suministra
                        Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                        if (suministra != null) // lo suministrado que hay sin contabilizar
                        {
                            kwHSuministrados = suministra.kwH;
                        }

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("sin actividad");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                    }
                }

            }

            else
                // el ratio de compra >= precio tarifa
                //        if (b.ratioCompra >= tarifa.precio)
                //{

                if ((b.ratioUso < tarifa.precio))
            {
                if (porcentajeCargaConConsumo < 99) //  si la bateria esta al 100% no puede cargar
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "carga y suministra")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                        //obtenemos la carga
                        Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                        if (carga != null)
                        { // la carga que hay sin contabilizar en la bateria
                            kwHCargados = carga.kwH;
                        }

                        //obtenemos suministra
                        Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                        if (suministra != null) // lo suministrado que hay sin contabilizar
                        {
                            kwHSuministrados = suministra.kwH;
                        }

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("carga y suministra");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                    }
                }
                //--------------------------------------------------------------------------------------------------------------
                else
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "suministrando")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                        //obtenemos la carga
                        Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                        if (carga != null)
                        { // la carga que hay sin contabilizar en la bateria
                            kwHCargados = carga.kwH;
                        }

                        //obtenemos suministra
                        Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                        if (suministra != null) // lo suministrado que hay sin contabilizar
                        {
                            kwHSuministrados = suministra.kwH;
                        }

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("suministrando");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                    }
                }
                //--------------------------------------------------------------------------------------------------------------------
            }
            else
            {
                if (porcentajeCargaConConsumo < 99) //  si la bateria esta al 100% no puede cargar
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "cargando")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                        //obtenemos la carga
                        Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                        if (carga != null)
                        { // la carga que hay sin contabilizar en la bateria
                            kwHCargados = carga.kwH;
                        }

                        //obtenemos suministra
                        Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                        if (suministra != null) // lo suministrado que hay sin contabilizar
                        {
                            kwHSuministrados = suministra.kwH;
                        }

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("cargando");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                    }
                }
                //--------------------------------------------------------------------------------------------------------------
                else
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "sin actividad")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

                        //obtenemos la carga
                        Carga carga = ServicioBateria.UltimaCarga(bateriaId);

                        if (carga != null)
                        { // la carga que hay sin contabilizar en la bateria
                            kwHCargados = carga.kwH;
                        }

                        //obtenemos suministra
                        Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

                        if (suministra != null) // lo suministrado que hay sin contabilizar
                        {
                            kwHSuministrados = suministra.kwH;
                        }

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("sin actividad");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
                    }
                }
                //--------------------------------------------------------------------------------------------------------------------
            }


            if (porcentajeCargaConConsumo > 100)
            {
                correccionConsumoBateria(bateriaId, (long)consumo, consumoPendiente);

            }

            //}

        }
        #endregion


        #region Mostrar Tarifas
        [Transactional]
        public List<TarifaDetails> TarifasDeHoy()
        {
            // obtenemos la hora y la fecha 
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Comprobar si las tarifas son las de hoy
            if (!TarifaDao.ExistenTarifasDelDia(fechaActual))
            {
                // actualizamos las tarifas
                ServicioTarifa.scrapyTarifas();

            }

            return ServicioTarifa.TarifasDelDia(fechaActual);
        }

        #endregion

        #region Crear Tarifas si es necesario pasandole la fecha
        [Transactional]
        public void CrearTarifasDeHoy(DateTime fechaActual)
        {

            // Comprobar si las tarifas son las de hoy
            if (!TarifaDao.ExistenTarifasDelDia(fechaActual))
            {
                // actualizamos las tarifas
                ServicioTarifa.scrapyTarifas();

            }

        }

        #endregion

        #region baterias del Usuario (muestra la ubicacion por la etiqueta)
        [Transactional]
        public List<BateriaDTOEtiquetaUbicacion> VerBateriasUsuarioConEtiquetaUbicacion(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<BateriaDTOEtiquetaUbicacion> bateriaDTOEtiquetaUbicacion = new List<BateriaDTOEtiquetaUbicacion>();

                List<Bateria> baterias = bateriaDao.findBateriaByUser(idUsuario, startIndex, count);

                foreach (Bateria b in baterias)
                {
                    //Obtenemos la etiquetad de la ubicacion
                    Ubicacion u = ServicioUbicacion.buscarUbicacionById(b.ubicacionId);
                    double porcentajeCarga = 100 * b.kwHAlmacenados / b.almacenajeMaximoKwH;
                    bateriaDTOEtiquetaUbicacion.Add(new BateriaDTOEtiquetaUbicacion(b.bateriaId, u.etiqueta, b.precioMedio, porcentajeCarga,
                        b.nSerie, b.ratioCarga, b.ratioCompra, b.ratioUso));
                }
                return bateriaDTOEtiquetaUbicacion;
                
            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region baterias del Usuario (número)

        public int numeroBateriasUsuario(long idUsuario)
        {
            return ServicioBateria.numeroBateriasUsuario(idUsuario);
        }

        #endregion

        //#region Gestion de los ratios en una bateria NO suministradora

        //[Transactional]
        //public void gestionDeRatiosBateriaNOSuministradora(long bateriaId, DateTime fechaActual, TimeSpan horaActual)
        //{
        //    double kwHCargados = 0;
        //    double kwHSuministrados = 0;

        //    // buscamos la bateria
        //    Bateria b = bateriaDao.Find(bateriaId);

        //    // Tarifa actual (hora)
        //    int horaTarifa = horaActual.Hours;

        //    // Buscar la tarifa actual
        //    TarifaDTO tarifa = TarifaEstado.TarifaActual(fechaActual, horaTarifa);

        //    //consumo pendiente
        //    string estado = ServicioBateria.EstadoDeLaBateria(bateriaId);
        //    double consumoPendiente = ServicioUbicacion.CalcularConsumoParaCalculoRatios(b.ubicacionId, horaActual, estado, bateriaId);

        //    // porcentaje de la bateria sumandole el consumo pendiente
        //    double porcentajeCargaConConsumo = ServicioBateria.porcentajeDeCargaConConsumo(bateriaId, consumoPendiente);

        //    // si el ratio de carga (minimo 10%) es menor al porcentaje de la bateria => carga
        //    if (b.ratioCarga >= porcentajeCargaConConsumo)
        //    {

        //        if (porcentajeCargaConConsumo < 99) //  si la bateria esta al 100% no puede cargar
        //        {
        //            if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "cargando")
        //            {
        //                //ponemos consumo en carga y/o suministra
        //                ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

        //                //obtenemos la carga
        //                Carga carga = ServicioBateria.UltimaCarga(bateriaId);

        //                if (carga != null)
        //                { // la carga que hay sin contabilizar en la bateria
        //                    kwHCargados = carga.kwH;
        //                }

        //                //obtenemos suministra
        //                Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

        //                if (suministra != null) // lo suministrado que hay sin contabilizar
        //                {
        //                    kwHSuministrados = suministra.kwH;
        //                }

        //                long estadoId = ServicioEstado.BuscarEstadoPorNombre("cargando");
        //                ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
        //            }
        //        }
        //        else
        //        {
        //            if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "sin actividad")
        //            {
        //                //ponemos consumo en carga y/o suministra
        //                ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

        //                //obtenemos la carga
        //                Carga carga = ServicioBateria.UltimaCarga(bateriaId);

        //                if (carga != null)
        //                { // la carga que hay sin contabilizar en la bateria
        //                    kwHCargados = carga.kwH;
        //                }

        //                //obtenemos suministra
        //                Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

        //                if (suministra != null) // lo suministrado que hay sin contabilizar
        //                {
        //                    kwHSuministrados = suministra.kwH;
        //                }

        //                long estadoId = ServicioEstado.BuscarEstadoPorNombre("sin actividad");
        //                ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
        //            }
        //        }
        //    }
        //    else
        //        if (porcentajeCargaConConsumo < 99)
        //        {
        //            if ((b.ratioCompra >= tarifa.precio))
        //            {
        //                if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "cargando")
        //                {
        //                    //ponemos consumo en carga y/o suministra
        //                    ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

        //                    //obtenemos la carga
        //                    Carga carga = ServicioBateria.UltimaCarga(bateriaId);

        //                    if (carga != null)
        //                    { // la carga que hay sin contabilizar en la bateria
        //                        kwHCargados = carga.kwH;
        //                    }

        //                    //obtenemos suministra
        //                    Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

        //                    if (suministra != null) // lo suministrado que hay sin contabilizar
        //                    {
        //                        kwHSuministrados = suministra.kwH;
        //                    }

        //                    long estadoId = ServicioEstado.BuscarEstadoPorNombre("cargando");
        //                    ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
        //                }
        //            }

        //        }
        //        else {
        //            if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "sin actividad")
        //            {
        //                //ponemos consumo en carga y/o suministra
        //                ServicioUbicacion.actualizarConsumoActual(b.ubicacionId, horaActual);

        //                //obtenemos la carga
        //                Carga carga = ServicioBateria.UltimaCarga(bateriaId);

        //                if (carga != null)
        //                { // la carga que hay sin contabilizar en la bateria
        //                    kwHCargados = carga.kwH;
        //                }

        //                //obtenemos suministra
        //                Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

        //                if (suministra != null) // lo suministrado que hay sin contabilizar
        //                {
        //                    kwHSuministrados = suministra.kwH;
        //                }

        //                long estadoId = ServicioEstado.BuscarEstadoPorNombre("sin actividad");
        //                ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados, horaActual);
        //            }
        //        }

        //}
        //#endregion

        #region %bateria con Correccion del consumo en caso de desbordamiento

        [Transactional]
        public void correccionConsumoBateria(long bateriaId, long consumo, double consumoPendiente)
        {
            //buscamos la bateria
            Bateria b = bateriaDao.Find(bateriaId);
            double total = b.almacenajeMaximoKwH;
            double almacenados = b.kwHAlmacenados;

            if (total < almacenados + consumoPendiente)
            {
                // Buscamos ultimo consumo
                Consumo c = ConsumoDao.Find(consumo);

                //calculamos lo que no se ha cargado
                double noCargado = almacenados + consumoPendiente - total;

                c.kwCargados = c.kwCargados - noCargado;
                ConsumoDao.Update(c);

                // buscamos carga 
                Carga ca = ServicioBateria.UltimaCarga(bateriaId);

                if (ca != null)
                {
                    ca.kwH = ca.kwH - noCargado;
                    CargaDao.Update(ca);
                }

            }


        }
        #endregion

    }



}
