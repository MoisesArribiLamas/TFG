using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service.Ubicaciones
{
    public class ServiceUbicacion : IServiceUbicacion
    {

        [Inject]
        public IUbicacionDao ubicacionDao { private get; set; }
        [Inject]
        public IConsumoDao ConsumoDao { private get; set; }
        [Inject]
        public ITarifaDao TarifaDao { private get; set; }

        [Inject]
        public IServiceBateria ServicioBateria { private get; set; }

        [Inject]
        public IServiceTarifa ServicioTarifa { private get; set; }


        #region crear Ubicación
        [Transactional]
        public long crearUbicacion( long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta, long usuarioId)
        {
            try
            {
                ubicacionDao.findUbicacionExistente(codigoPostal, localidad, calle, portal, numero, etiqueta);

                throw new DuplicateInstanceException(localidad,
                    typeof(Ubicacion).FullName);
            }
            catch (InstanceNotFoundException)
            {
                Ubicacion u = new Ubicacion();

                u.codigoPostal = codigoPostal;
                u.localidad = localidad;
                u.calle = calle;
                u.portal = portal;
                u.numero = numero;
                u.etiqueta = etiqueta;
                u.bateriaSuministradora = null;
                u.ultimoConsumo = null;
                u.usuario = usuarioId;

                ubicacionDao.Create(u);
                return u.ubicacionId;

            }
        }

        #endregion 

        #region Obtener  Bateria suministradora
        [Transactional]
        public double obtenerCapacidadCargadorBateriaSuministradora(long ubicacionId)
        {

            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);

            if (ubicacion.bateriaSuministradora != null)
            {
                // obtenemos la bateria
                return ServicioBateria.capacidadCargadorBateriaSuministradora((long)ubicacion.bateriaSuministradora);
            }
            else
            { // no tiene bateria asociada
                throw new InstanceNotFoundException(ubicacionId,
                    typeof(Ubicacion).FullName);
            }
        }

        #endregion 


        #region Modificacar Ubicacion
        [Transactional]
        public void modificarUbicacion(long ubicacionId, long? codigoPostal, string localidad, string calle, string portal, long? numero, string etiqueta)
        {

            ubicacionDao.updateInformacion(ubicacionId, codigoPostal, localidad, calle, portal, numero, etiqueta);
        }
        #endregion Modificar

        #region Cambiar bateria suministradora
        [Transactional]
        public void CambiarBateriaSuministradora(long ubicacionId, long? bateriaSuministradora)
        {
            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);

            ubicacion.bateriaSuministradora = bateriaSuministradora;

            ubicacionDao.Update(ubicacion);
        }
                
        #endregion Cambiar bateria

        #region ubicaciones del Usuario
        [Transactional]
        public List<UbicacionProfileDetails> verUbicaciones(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<UbicacionProfileDetails> ubicacionesDTO = new List<UbicacionProfileDetails>();

                List<Ubicacion> ubicaciones = ubicacionDao.ubicacionesUsuario(idUsuario, startIndex, count);

                foreach (Ubicacion u in ubicaciones)
                {
                    ubicacionesDTO.Add(new UbicacionProfileDetails(u.ubicacionId, u.codigoPostal, u.localidad, u.calle, u.portal, u.numero, u.etiqueta));
                }
                return ubicacionesDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region ubicaciones del Usuario con paginación
        [Transactional]
        public List<UbicacionProfileDetails> ubicacionesPertenecientesAlUsuario(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<UbicacionProfileDetails> ubicacionesDTO = new List<UbicacionProfileDetails>();

                List<Ubicacion> ubicaciones = ubicacionDao.ubicacionesPertenecientesAlUsuario(idUsuario, startIndex, count);

                foreach (Ubicacion u in ubicaciones)
                {
                    ubicacionesDTO.Add(new UbicacionProfileDetails(u.ubicacionId, u.codigoPostal, u.localidad, u.calle, u.portal, u.numero, u.etiqueta));
                }
                return ubicacionesDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region ubicaciones del Usuario con paginación En mostrar estadísticas.
        [Transactional]
        public List<UbicacionProfileDetails> ubicacionesPertenecientesAlUsuarioEstadisticas(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<UbicacionProfileDetails> ubicacionesDTO = new List<UbicacionProfileDetails>();

                List<Ubicacion> ubicaciones = ubicacionDao.ubicacionesPertenecientesAlUsuario(idUsuario, startIndex, count);

                foreach (Ubicacion u in ubicaciones) //double consumoActual, string bateriaSuministradora
                {
                    // obtenemos el consumo actual de la ubicacion.
                    double? consumoActual;
                    string estado;
                    string porcentaje;


                    if (u.ultimoConsumo == null)
                    {
                        // no tiene asociado un consumo
                        consumoActual = null;

                        //si no hay asociado un consumo tampoco hay bateria suministradora
                        estado = null;
                        porcentaje = null;

                    }
                    else
                    {
                        // obtenemos el consumo
                        Consumo c = buscarConsumoById((long)u.ultimoConsumo);
                        consumoActual = c.consumoActual;

                        if (u.bateriaSuministradora != null)
                        {
                            estado = ServicioBateria.EstadoDeLaBateria((long)u.bateriaSuministradora);
                            porcentaje = ServicioBateria.porcentajeDeCarga((long)u.bateriaSuministradora).ToString();
                        }
                        else
                        {
                            estado = "";
                            porcentaje = "";
                        }
  

                    }

                    ubicacionesDTO.Add(new UbicacionProfileDetails(u.ubicacionId, u.etiqueta, consumoActual, estado, porcentaje));

                }
                return ubicacionesDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region ubicaciones del Usuario sin paginación.
        [Transactional]
        public List<UbicacionProfileDetails> ubicacionesDelUsuario(long idUsuario)
        {
            try
            {
                List<UbicacionProfileDetails> ubicacionesDTO = new List<UbicacionProfileDetails>();

                List<Ubicacion> ubicaciones = ubicacionDao.ubicacionesDelUsuario(idUsuario);

                foreach (Ubicacion u in ubicaciones)
                {
                    ubicacionesDTO.Add(new UbicacionProfileDetails(u.ubicacionId, u.codigoPostal, u.localidad, u.calle, u.portal, u.numero, u.etiqueta));
                }
                return ubicacionesDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }
        #endregion

        //primeraUbicacionDelUsuario

        #region ubicaciones del Usuario sin paginación.
        [Transactional]
        public Ubicacion primeraUbicacionDelUsuario(long idUsuario)
        {

            List<UbicacionProfileDetails> ubicacionesDTO = new List<UbicacionProfileDetails>();

            return ubicacionDao.primeraUbicacionDelUsuario(idUsuario);

        }
        #endregion

        #region Todas las baterias suministradodas.
        [Transactional]
        public List<long?> todasLasBateriasSuministradoras()
        {
            //List<long> longs = ubicacionDao.todasLasBateriasSuministradoras().ConvertAll(i => (long)i);

            return ubicacionDao.todasLasBateriasSuministradoras();

        }

        #endregion

        #region Baterias de la ubicacion
        [Transactional]
        public List<BateriaDTO> bateriasDeUnaUbicacion(long idUbicacion)
        {

            List<BateriaDTO> ubicacionesDTO = new List<BateriaDTO>();

            List<Bateria> ubicaciones = ubicacionDao.bateriasDeUnaUbicacion(idUbicacion);

            foreach (Bateria u in ubicaciones)
            {
                ubicacionesDTO.Add(new BateriaDTO( u.bateriaId, u.ubicacionId, u.usuarioId, u.precioMedio, u.kwHAlmacenados, u.almacenajeMaximoKwH,
                u.fechaDeAdquisicion, u.marca, u.modelo, u.nSerie, u.ratioCarga, u.ratioCompra, u.ratioUso, u.capacidadCargador));
            } 
            return ubicacionesDTO;


        }

        #endregion

        #region ubicaciones del Usuario
        [Transactional]
        public int numeroUbicacionesUsuario(long idUsuario)
        {
            return ubicacionDao.numeroUbicacionesUsuario(idUsuario);
        }

        #endregion

        #region Buscar Ubicación
        [Transactional]
        public Ubicacion buscarUbicacionById(long ubicacionId)
        {
            try
            {
                return ubicacionDao.Find(ubicacionId);

                
            }
            catch (InstanceNotFoundException)
            {
                throw new InstanceNotFoundException(ubicacionId,
                    typeof(Ubicacion).FullName);

            }
        }

        #endregion crear Ubicación

        #region Buscar Ubicación por nombre
        [Transactional]
        public Ubicacion buscarUbicacionByNombre(string nombre)
        {
                return ubicacionDao.findUbicacionByName(nombre);

        }

        #endregion crear Ubicación

        #region crear Consumo
        [Transactional]
        public long crearConsumo(long ubicacionId, double consumoActual, DateTime fechaActual, TimeSpan horaActual)
        {
            // Comprobar si las tarifas son las de hoy
            if (!TarifaDao.ExistenTarifasDelDia(fechaActual))
            {
                // actualizamos las tarifas
                ServicioTarifa.scrapyTarifas();
            }

            Consumo c = new Consumo();

            c.consumoActual = consumoActual;
            c.kwCargados = 0;
            c.kwSuministrados = 0;
            c.kwRed = 0;
            c.fecha = fechaActual;
            c.horaIni = horaActual;
            c.horaFin = null;
            c.ubicacionId = ubicacionId;

            ConsumoDao.Create(c);

            // siempre que se crea un consumo se indica en la ubicacion a la que pertenece
            ponerUltimoConsumoEnUbicacion(ubicacionId, c.consumoId);

            return c.consumoId;


        }

        #endregion crear Consumo


        #region poner ultimo consumo en la ubicacion
        [Transactional]
        public void ponerUltimoConsumoEnUbicacion(long ubicacionId, long ultimoConsumo)
        {
            // buscamos la ubicacions
            Ubicacion u = buscarUbicacionById(ubicacionId);

            // ponemos el ultimo consumo de la ubicacion
            u.ultimoConsumo = ultimoConsumo;

            // actualizamos
            ubicacionDao.Update(u);
        }

        #endregion 

        #region finalizar Consumo
        [Transactional]
        public bool finalizarConsumo(long ubicacionId, double consumoActual, TimeSpan horaActual, string estado, long bateriaSuministradora)
        {
            bool gestionRatios = false;

            //buscamos el consumo (entidad) actual
            Consumo c = ConsumoDao.UltimoConsumoUbicacion(ubicacionId);

            //finalizar consumo
            c.horaFin = horaActual;

            //-----------------------------------
            //dependiendo del estado          
            if (estado == "sin actividad") // "sin actividad"
            {   if (consumoActual > 0)
                {
                    // consumoAnterior => consumido por la red
                    c.kwRed = calcularConsumo(consumoActual, c.horaIni, horaActual);

                    //actualizamos
                    ConsumoDao.Update(c);
                }
            }
            else if (estado == "cargando") // "cargando"
            {

                // consumoAnterior => consumido por la red
                c.kwRed = calcularConsumo(consumoActual, c.horaIni, horaActual);

                // calculamos lo que ha cargado la bateria
                double capacidadCarga = ServicioBateria.capacidadCargadorBateriaSuministradora(bateriaSuministradora);
                c.kwCargados = c.kwCargados + calcularConsumo(capacidadCarga, c.horaIni, horaActual);

                //actualizamos
                ConsumoDao.Update(c);

                // ponemos en la entidad Carga los datos obtenidos
                gestionRatios = ServicioBateria.CargaAñadida(bateriaSuministradora, (double)c.kwCargados, 0, horaActual);


            }
            else if (estado == "suministrando") // "suministrando"
            {

                // consumoAnterior => suministra
                c.kwSuministrados = calcularConsumo(consumoActual, c.horaIni, horaActual);

                //actualizamos
                ConsumoDao.Update(c);

                //ponemos en la entidad Suministra los datos obtenidos
                gestionRatios = ServicioBateria.CargaAñadida(bateriaSuministradora, 0, (double)c.kwSuministrados, horaActual);

            }
            else if (estado == "carga y suministra")// "carga y suministra"
            {
                // consumoAnterior => suministra
                double capacidadCarga = ServicioBateria.capacidadCargadorBateriaSuministradora(bateriaSuministradora);
                c.kwCargados = c.kwCargados + calcularConsumo(capacidadCarga, c.horaIni, horaActual);
                c.kwSuministrados = calcularConsumo(consumoActual, c.horaIni, horaActual);

                //actualizamos
                ConsumoDao.Update(c);

                // ponemos en la entidad Carga los datos obtenidos
                gestionRatios = ServicioBateria.CargaAñadida(bateriaSuministradora, (double)c.kwCargados, (double)c.kwSuministrados, horaActual);
            }

            return gestionRatios;
        }

        #endregion finalizar Consumo


        #region calcular el Consumo entre dos horas
        [Transactional]
        public double calcularConsumo(double consumoActual, TimeSpan fechaIni, TimeSpan fechaFin)
        {
            // calculamos el tiempo transcurrido

            double minutos =  fechaFin.Minutes - fechaIni.Minutes;
            double segundos = fechaFin.Seconds - fechaIni.Seconds;

            //1h = 3600 segundos
            double consumido = consumoActual * (minutos * 60 + segundos) / 3600;

            //calculamos KWTotal

            return consumido;


        }

        #endregion


        #region Calcula el consumo que hay (sin pasar a carga o suministra) segun el estado 
        [Transactional]
        public double CalcularConsumoParaCalculoRatios(long ubicacionId, TimeSpan horaActual, string estado, long bateriaSuministradora)
        {

            //buscamos el consumo (entidad) actual
            Consumo c = ConsumoDao.UltimoConsumoUbicacion(ubicacionId);

            //finalizar consumo
            c.horaFin = horaActual;

            //-----------------------------------
            //dependiendo del estado          
            if (estado == "sin actividad") // "sin actividad"
            {
                return 0;
            }
            else if (estado == "cargando") // "cargando"
            {


                // calculamos lo que ha cargado la bateria
                double capacidadCarga = ServicioBateria.capacidadCargadorBateriaSuministradora(bateriaSuministradora);
                return calcularConsumo(capacidadCarga, c.horaIni, horaActual);

              

            }
            else if (estado == "suministrando") // "suministrando"
            {

                // consumoAnterior => suministra
                return -(calcularConsumo(c.consumoActual, c.horaIni, horaActual)); //negativo lo que se ha perdido de la bateria


            }
            else if (estado == "carga y suministra")// "carga y suministra"
            {
                // consumoAnterior => suministra
                double capacidadCarga = ServicioBateria.capacidadCargadorBateriaSuministradora(bateriaSuministradora);
                //c.kwCargados = calcularConsumo(capacidadCarga, c.horaIni, horaActual);
                //c.kwSuministrados = calcularConsumo(consumoActual, c.horaIni, horaActual);

                return calcularConsumo(capacidadCarga, c.horaIni, horaActual) - calcularConsumo(c.consumoActual, c.horaIni, horaActual);
            }

            return 0;
        }

        #endregion


        #region modificar Consumo (cierra el consumo previo y crea uno nuevo) con gestion de ratios
        [Transactional]
        public long modificarConsumoActual(long ubicacionId, double consumoActual)
        {
            bool gestionRatios = false;

            // hora actual
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // buscamos el consumo (entidad) actual
            Consumo c = ConsumoDao.UltimoConsumoUbicacion(ubicacionId);
            double consumoAnterior = c.consumoActual;

            // buscamos ubicacion
            Ubicacion u = buscarUbicacionById(ubicacionId);

            // obtenemos el estado
            string estado = ServicioBateria.EstadoDeLaBateria((long) u.bateriaSuministradora);


            // finalizar consumo
            gestionRatios = finalizarConsumo(ubicacionId, consumoAnterior, horaActual, estado, (long)u.bateriaSuministradora);

            // creamos el nuevo consumo
            long consumoNuevo = crearConsumo(ubicacionId, consumoActual, fechaActual, horaActual);

            if (gestionRatios)// ratio de carga >= %Bateria => gestion de ratios
            {
                double kwHcargadosFinal = 0;
                double kwhsuministradosFinal = 0;

                //obtenemos la carga
                Carga carga = ServicioBateria.UltimaCarga((long)u.bateriaSuministradora);

                if (carga != null)
                { // la carga que hay sin contabilizar en la bateria
                    kwHcargadosFinal = carga.kwH;
                }

                //obtenemos suministra
                Suministra suministra = ServicioBateria.UltimaSuministra((long)u.bateriaSuministradora);

                if (suministra != null) // lo suministrado que hay sin contabilizar
                {
                    kwhsuministradosFinal = suministra.kwH;
                }

                ServicioBateria.gestionDeRatios((long)u.bateriaSuministradora, kwHcargadosFinal, kwhsuministradosFinal, fechaActual, horaActual);
        }
            //devolvemos el id del nuevo consumo
            return consumoNuevo;
        }

        #endregion modificar Consumo

        #region actualizar los datos de Consumo (carga y suministra) sin cambio del consumoActual
        [Transactional]
        public long actualizarConsumoActual(long ubicacionId, TimeSpan horaActual)
        {
            long consumoNuevo = 0;

            // buscamos el consumo (entidad) actual
            Consumo c = ConsumoDao.UltimoConsumoUbicacion(ubicacionId);
            double consumoActual = c.consumoActual;

            // buscamos ubicacion
            Ubicacion u = buscarUbicacionById(ubicacionId);

            // obtenemos el estado
            string estado = ServicioBateria.EstadoDeLaBateria((long)u.bateriaSuministradora);


            // finalizar consumo
            finalizarConsumo(ubicacionId, consumoActual, horaActual, estado, (long)u.bateriaSuministradora);


            // creamos el nuevo consumo
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            consumoNuevo = crearConsumo(ubicacionId, consumoActual, fechaActual, horaActual);
            

            //devolvemos el id del nuevo consumo
            return consumoNuevo;
        }

        #endregion actuallizar Consumo

        #region actualizar los datos de Consumo (carga y suministra) sin cambio del consumoActual
        [Transactional]
        public long actualizarConsumoActual(long ubicacionId, TimeSpan horaActual, bool isBateriaSuministradoraNull)
        {
            long consumoNuevo = 0;

            // buscamos el consumo (entidad) actual
            Consumo c = ConsumoDao.UltimoConsumoUbicacion(ubicacionId);
            double consumoActual = c.consumoActual;

            // buscamos ubicacion
            Ubicacion u = buscarUbicacionById(ubicacionId);

            // obtenemos el estado
            string estado = ServicioBateria.EstadoDeLaBateria((long)u.bateriaSuministradora);


            // finalizar consumo
            finalizarConsumo(ubicacionId, consumoActual, horaActual, estado, (long)u.bateriaSuministradora);

            if (!isBateriaSuministradoraNull) // en el caso de que se quite la bateria suministradora por null
            {
                // creamos el nuevo consumo
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                consumoNuevo = crearConsumo(ubicacionId, consumoActual, fechaActual, horaActual);
            }
            else // en el caso de que se quite la bateria suministradora y no se ponga otra. quitamos el ultimo consumo.
            {
                u.ultimoConsumo = null;
                ubicacionDao.Update(u);
            }

            //devolvemos el id del nuevo consumo
            return consumoNuevo;
        }

        #endregion actuallizar Consumo

        #region Obtener la entidad Consumo vigente
        [Transactional]
        public long? UltimoConsumoEnUbicacion(long ubicacionId)
        {
            // buscamos ubicacion
            Ubicacion u = buscarUbicacionById(ubicacionId);
            //buscamos el consumo (entidad) actual
            return u.ultimoConsumo;

        }

        #endregion

        #region Buscar Consumo por ID
        [Transactional]
        public Consumo buscarConsumoById(long consumoId)
        {
            try
            {
                return ConsumoDao.Find(consumoId);


            }
            catch (InstanceNotFoundException)
            {
                throw new InstanceNotFoundException(consumoId,
                    typeof(Ubicacion).FullName);

            }
        }


        #endregion  Consumo

        //#region Obtener la entidad Consumo vigente
        //[Transactional]
        //public Consumo ConsumoMostrarKwAlmacenadosYSuministrados(long ubicacionId)
        //{

        //    //buscamos el consumo (entidad) actual
        //    return consumoDao.UltimoConsumoUbicacion(ubicacionId);

        //}

        //#endregion  Consumo

        #region Eliminar Ubicacion
        [Transactional]
        public void eliminarUbicacion(long ubicacionId)
        {

            Ubicacion ubicacion = ubicacionDao.Find(ubicacionId);

            
            ubicacionDao.Remove(ubicacion.ubicacionId);
        }
        #endregion Modificar

        #region Consumos directamente de la red una ubicacion por fechas
        [Transactional]
        public List<ConsumoDTO> MostrarProporcionadoPorRedPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {

            List<ConsumoDTO> ConsumosDTO = new List<ConsumoDTO>();

            List<Consumo> consumos = ConsumoDao.MostrarConsumosUbicacionPorFecha(ubicacionID, fecha, fecha2, startIndex, count);

            foreach (Consumo c in consumos) 
            {
                ConsumosDTO.Add(new ConsumoDTO(c.consumoId, c.ubicacionId, c.kwRed, c.fecha, c.horaIni, c.horaFin, c.consumoActual, c.ubicacionId));

            }
            return ConsumosDTO;

        }
        #endregion


        #region Consumos directamente de la red una ubicacion por fechas por dias
        [Transactional]
        public List<ConsumoPorDias> MostrarSuministradoXUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2)
        {

            return ConsumoDao.MostrarSuministradoXUbicacionPorFechaEnDias(ubicacionID, fecha, fecha2);

        }
        #endregion


        #region kw cargados en el sistema por dias
        [Transactional]
        public List<ConsumoPorDias> MostrarLoCargadoPorElSistemaPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2)
        {

            return ConsumoDao.MostrarLoCargadoPorElSistemaPorFechaEnDias(ubicacionID, fecha, fecha2);

        }
        #endregion

        #region kw suministrados en un día
        [Transactional]
        public List<ConsumoDiaConcreto> MostrarLoSuministradoPorElSistemaDiaConcreto(long ubicacionID, DateTime fecha)
        {

            return ConsumoDao.MostrarLoSuministradoPorElSistemaDiaConcreto(ubicacionID, fecha);

        }
        #endregion

        #region kw cargados en un día
        [Transactional]
        public List<ConsumoDiaConcreto> MostrarLoCargadoPorElSistemaDiaConcreto(long ubicacionID, DateTime fecha)
        {

            return ConsumoDao.MostrarLoCargadoPorElSistemaDiaConcreto(ubicacionID, fecha);

        }
        #endregion


        #region Consumos directamente de la red una ubicacion por fechas por dias
        [Transactional]
        public List<ConsumoPorDias> MostrarConsumosRedElectricaUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2)
        {

            return ConsumoDao.MostrarConsumosRedElectricaUbicacionPorFechaEnDias(ubicacionID, fecha, fecha2);

        }
        #endregion


        #region Consumos directamente de la red una ubicacion En una fecha concreta
        [Transactional]
        public List<ConsumoDiaConcreto> MostrarConsumosRedElectricaUbicacionDiaConcreto(long ubicacionID, DateTime fecha)
        {

            return ConsumoDao.MostrarConsumosRedElectricaUbicacionDiaConcreto(ubicacionID, fecha);

        }
        #endregion


        #region  suministros cargas del sistema y consumo por la red en un periodo de tiempo por dias
        [Transactional]
        public List<CargaSuministraYRed> MostrarSuministradoCargadoYRedXUbicacionPorFechaEnDias(long ubicacionID, DateTime fecha, DateTime fecha2)
        {

            return ConsumoDao.MostrarSuministradoCargadoYRedXUbicacionPorFechaEnDias(ubicacionID, fecha, fecha2);

        }
        #endregion

        #region  suministros cargas del sistema y consumo por la red en un día
        [Transactional]
        public List<CargaSuministraYRedDiaConcreto> MostrarSuministradoCargadoYRedXUbicacionDiaConcreto(long ubicacionID, DateTime fecha)
        {

            return ConsumoDao.MostrarSuministradoCargadoYRedXUbicacionDiaConcreto(ubicacionID, fecha);

        }
        #endregion

        #region Consumos directamente de la Bateria una ubicacion por fechas
        [Transactional]
        public List<ConsumoDTO> MostrarProporcionadoPorPareriaPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {

            List<ConsumoDTO> ConsumosDTO = new List<ConsumoDTO>();

            List<Consumo> consumos = ConsumoDao.MostrarConsumosUbicacionPorFecha(ubicacionID, fecha, fecha2, startIndex, count);

            foreach (Consumo c in consumos) 
            {
                ConsumosDTO.Add(new ConsumoDTO(c.consumoId, c.ubicacionId, c.kwSuministrados, c.fecha, c.horaIni, c.horaFin, c.consumoActual, c.ubicacionId));

            }
            return ConsumosDTO;

        }
        #endregion

        #region Consumos directamente de la Bateria una ubicacion por fechas
        [Transactional]
        public List<ConsumoDTO> MostrarLoCargadoUbicacionPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {

            List<ConsumoDTO> ConsumosDTO = new List<ConsumoDTO>();

            List<Consumo> consumos = ConsumoDao.MostrarConsumosUbicacionPorFecha(ubicacionID, fecha, fecha2, startIndex, count);

            foreach (Consumo c in consumos) 
            {
                ConsumosDTO.Add(new ConsumoDTO(c.consumoId, c.ubicacionId, c.kwCargados, c.fecha, c.horaIni, c.horaFin, c.consumoActual, c.ubicacionId));

            }
            return ConsumosDTO;

        }
        #endregion


        #region numero de Consumos de una ubicacion por fechas
        [Transactional]
        public int numeroCargasBareriaPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2)
        {

            return ConsumoDao.numeroConsumosUbicacionPorFecha(ubicacionID, fecha, fecha2); 

         
        }
        #endregion

        #region actualizar los datos de Consumo (carga y suministra) sin cambio del consumoActual
        [Transactional]
        public double consumoEnEsteInstante(long ubicacionId)
        {

            // buscamos el consumo (entidad) actual
            Consumo c = ConsumoDao.UltimoConsumoUbicacion(ubicacionId);

            if (c == null)
            {
                return 0;   
            }
            else { 
                //devolvemos el consumo actual
                return c.consumoActual;
            }
        }

        #endregion 
    }

}
