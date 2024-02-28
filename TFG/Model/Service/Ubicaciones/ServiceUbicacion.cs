using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service.Ubicaciones
{
    public class ServiceUbicacion : IServiceUbicacion
    {

        [Inject]
        public IUbicacionDao ubicacionDao { private get; set; }
        [Inject]
        public IConsumoDao consumoDao { private get; set; }

        [Inject]
        public IServiceBateria ServicioBateria { private get; set; }


        #region crear Ubicación
        [Transactional]
        public long crearUbicacion( long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta)
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
                    ubicacionesDTO.Add(new UbicacionProfileDetails(u.ubicacionId, u.codigoPostal, u.localidad, u.calle, u.portal, u.numero));
                }
                return ubicacionesDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
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

        #region crear Consumo
        [Transactional]
        public long crearConsumo(long ubicacionId, double consumoActual, TimeSpan horaActual)
        {
            // Fecha actual
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            Consumo c = new Consumo();

            c.consumoActual = consumoActual;
            c.kwCargados = 0;
            c.kwSuministrados = 0;
            c.kwRed = 0;
            c.fecha = fechaActual;
            c.horaIni = horaActual;
            c.horaFin = null;
            c.ubicacionId = ubicacionId;

            consumoDao.Create(c);

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
            Consumo c = consumoDao.UltimoConsumoUbicacion(ubicacionId);

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
                    consumoDao.Update(c);
                }
            }
            else if (estado == "cargando") // "cargando"
            {

                // consumoAnterior => consumido por la red
                c.kwRed = calcularConsumo(consumoActual, c.horaIni, horaActual);

                // calculamos lo que ha cargado la bateria
                double capacidadCarga = ServicioBateria.capacidadCargadorBateriaSuministradora(bateriaSuministradora);
                c.kwCargados = calcularConsumo(capacidadCarga, c.horaIni, horaActual);

                //actualizamos
                consumoDao.Update(c);

                // ponemos en la entidad Carga los datos obtenidos
                gestionRatios = ServicioBateria.CargaAñadida(bateriaSuministradora, (double)c.kwCargados, 0, horaActual);


            }
            else if (estado == "suministrando") // "suministrando"
            {

                // consumoAnterior => suministra
                c.kwSuministrados = calcularConsumo(consumoActual, c.horaIni, horaActual);

                //actualizamos
                consumoDao.Update(c);

                //ponemos en la entidad Suministra los datos obtenidos
                gestionRatios = ServicioBateria.CargaAñadida(bateriaSuministradora, 0, (double)c.kwSuministrados, horaActual);

            }
            else if (estado == "carga y suministra")// "carga y suministra"
            {
                // consumoAnterior => suministra
                double capacidadCarga = ServicioBateria.capacidadCargadorBateriaSuministradora(bateriaSuministradora);
                c.kwCargados = calcularConsumo(capacidadCarga, c.horaIni, horaActual);
                c.kwSuministrados = calcularConsumo(consumoActual, c.horaIni, horaActual);

                //actualizamos
                consumoDao.Update(c);

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
            Consumo c = consumoDao.UltimoConsumoUbicacion(ubicacionId);

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


        #region modificar Consumo (cierra el consumo previo y crea uno nuevo)
        [Transactional]
        public long modificarConsumoActual(long ubicacionId, double consumoActual)
        {
            bool gestionRatios = false;

            // hora actual
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            // buscamos el consumo (entidad) actual
            Consumo c = consumoDao.UltimoConsumoUbicacion(ubicacionId);
            double consumoAnterior = c.consumoActual;

            // buscamos ubicacion
            Ubicacion u = buscarUbicacionById(ubicacionId);

            // obtenemos el estado
            string estado = ServicioBateria.EstadoDeLaBateria((long) u.bateriaSuministradora);


            // finalizar consumo
            gestionRatios = finalizarConsumo(ubicacionId, consumoAnterior, horaActual, estado, (long)u.bateriaSuministradora);

            // creamos el nuevo consumo
            long consumoNuevo = crearConsumo(ubicacionId, consumoActual, horaActual);

            if (gestionRatios)// ratio de carga >= %Bateria => gestion de ratios
            {
                double kwHcargadosFinal = 0;
                double kwhsuministradosFinal = 0;

                //obtenemos la carga
                Carga carga= ServicioBateria.UltimaCarga((long)u.bateriaSuministradora);

                if (carga != null) { // la carga que hay sin contabilizar en la bateria
                    kwHcargadosFinal = carga.kwH;
                }

                //obtenemos suministra
                Suministra suministra = ServicioBateria.UltimaSuministra((long)u.bateriaSuministradora);

                if (suministra != null) // lo suministrado que hay sin contabilizar
                {
                    kwhsuministradosFinal = suministra.kwH;
                }

                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                ServicioBateria.gestionDeRatios((long)u.bateriaSuministradora, kwHcargadosFinal, kwhsuministradosFinal, fechaActual, horaActual);
            }
            //devolvemos el id del nuevo consumo
            return consumoNuevo;
        }

        #endregion modificar Consumo

        #region actualizar los datos de Consumo (carga y suministra) 
        [Transactional]
        public long actualizarConsumoActual(long ubicacionId, TimeSpan horaActual)
        {

            // buscamos el consumo (entidad) actual
            Consumo c = consumoDao.UltimoConsumoUbicacion(ubicacionId);
            double consumoActual = c.consumoActual;

            // buscamos ubicacion
            Ubicacion u = buscarUbicacionById(ubicacionId);

            // obtenemos el estado
            string estado = ServicioBateria.EstadoDeLaBateria((long)u.bateriaSuministradora);


            // finalizar consumo
            finalizarConsumo(ubicacionId, consumoActual, horaActual, estado, (long)u.bateriaSuministradora);

            // creamos el nuevo consumo
            long consumoNuevo = crearConsumo(ubicacionId, consumoActual, horaActual);

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
                return consumoDao.Find(consumoId);


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

        #region Consumos de una ubicacion por fechas
        [Transactional]
        public List<ConsumoDTO> MostrarCargasBareriaPorFecha(long ubicacionID, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            try
            {
                List<ConsumoDTO> ConsumosDTO = new List<ConsumoDTO>();

                List<Consumo> consumos = consumoDao.MostrarConsumosUbicacionPorFecha(ubicacionID, fecha, fecha2, startIndex, count);

                foreach (Consumo c in consumos) //long consumoId, long ubicacionId, double? kwTotal, DateTime fecha, TimeSpan horaIni, TimeSpan? horaFin, double consumoActual, long ubicacion)
                {
                    ConsumosDTO.Add(new ConsumoDTO(c.consumoId, c.ubicacionId, c.kwCargados, c.kwSuministrados, c.kwRed, c.fecha, c.horaIni, c.horaFin, c.consumoActual, c.ubicacionId));

                }
                return ConsumosDTO;
                
            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }
        #endregion

    }

}
