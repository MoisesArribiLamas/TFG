using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Es.Udc.DotNet.TFG.Model.Service.Controlador
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

        //[Inject]
        //public IServiceUbicacion ServicioUbicacion { private get; set; } 


        #region Parte Asincrona

        [Transactional]
        public void Asincrono(long bateriaId, long estadoId, double kwHCargados, double kwHSuministrados)
        {
            // Fecha y hora actual
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


            //Mirar si hay que traer todas las tarifas del dia
            // mirar si se ha cambiado de dia
            // mirar si se ha cambiado de hora
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
                gestionDeRatiosBateriaSuministradora((long)u.bateriaSuministradora, fechaActual, horaActual);


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
        #endregion

        #region Cambiar bateria suministradora
        [Transactional]
        public void CambiarBateriaSuministradora(long ubicacionId, long? bateriaSuministradora)
        {
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);

            // si hay bateriaSuministradora previa
            long? bateriaSuministradoraPrevia = ubicacion.bateriaSuministradora;
            if (bateriaSuministradoraPrevia != null)
            {
                double kwHCargados = 0;
                double kwHSuministrados = 0;

                // Obtenemos el estado
                String estado = ServicioBateria.EstadoDeLaBateria((long)bateriaSuministradoraPrevia);
                
                // Cerramos el consumo
                Consumo consumo = ConsumoDao.UltimoConsumoUbicacion(ubicacionId);
                ServicioUbicacion.finalizarConsumo(ubicacionId, consumo.consumoActual, horaActual, estado, (long)bateriaSuministradoraPrevia);

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

            ServicioUbicacion.CambiarBateriaSuministradora(ubicacionId, bateriaSuministradora);

            //comprobar los ratios con la nueva bateriaSuministradora
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            gestionDeRatiosBateriaSuministradora((long)bateriaSuministradora, fechaActual, horaActual);


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

            //Comprobamos los ratios en la bateriasuministradora de la ubicacion
            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);
            if (ubicacion.bateriaSuministradora != null)
            {
                gestionDeRatiosBateriaSuministradora((long)ubicacion.bateriaSuministradora, fechaActual, horaActual);
            }


        }

        #endregion crear Consumo

        #region Gestion de los ratios en una bateria suministradora

        [Transactional]
        public void gestionDeRatiosBateriaSuministradora(long bateriaId, DateTime fechaActual, TimeSpan horaActual)
        {
            double kwHCargados = 0;
            double kwHSuministrados = 0;
            long? consumo =null;

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

            if (porcentajeCargaConConsumo > 100) {
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
                    else {
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
                correccionConsumoBateria( bateriaId, (long)consumo, consumoPendiente);

            }

            //}

        }
        #endregion


        #region Mostrar Tarifas
        [Transactional]
        public List<TarifaDTO> TarifasDeHoy()
        {
            // obtenemos la hora y la fecha 
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Comprobar si las tarifas son las de hoy
            if (!TarifaDao.ExistenTarifasDelDia(fechaActual))
            {
                // actualizamos las tarifas
                ServicioTarifa.scrapyTarifas();

            }

                return ServicioTarifa.verTarifasDelDia(fechaActual);
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

            if (total < almacenados + consumoPendiente) {
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
