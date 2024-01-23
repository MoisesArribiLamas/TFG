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
            return c.consumoId;


        }

        #endregion crear Consumo

        #region finalizar Consumo
        [Transactional]
        public void finalizarConsumo(long ubicacionId, double consumoActual, TimeSpan horaActual, string estado)
        {
  
            //buscamos el consumo (entidad) actual
            Consumo c = consumoDao.UltimoConsumoUbicacion(ubicacionId);

            //calculamos KWTotal
            double kwTotal = calcularConsumo(consumoActual, c.horaIni, horaActual);

            //finalizar consumo
            c.horaFin = horaActual;
            if (estado == "sin actividad" || estado == "cargando")
            {
                c.kwRed = c.kwRed + kwTotal;

            } else if (estado == "suministrando" || estado == "carga y suministra")
                    {
                        c.kwSuministrados = c.kwSuministrados + kwTotal;
                    }

            //actualizamos
            consumoDao.Update(c);


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



        #region modificar Consumo
        [Transactional]
        public long modificarConsumoActual(long ubicacionId, double consumoActual)
        {
            // hora actual
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            // buscamos el consumo (entidad) actual
            Consumo c = consumoDao.UltimoConsumoUbicacion(ubicacionId);
            double consumoAnterior = c.consumoActual;

            // buscamos ubicacion
            Ubicacion u = buscarUbicacionById(ubicacionId);
            // obtenemos el estado
            string estado = ServicioBateria.EstadoDeLaBateria((long) u.bateriaSuministradora);

            //dependiendo del estado          
            if (estado == "sin actividad") // "sin actividad"
            {
                // consumoAnterior => consumido por la red
                c.kwRed = calcularConsumo(consumoAnterior, c.horaIni,  horaActual);


            } else if (estado == "cargando") // "cargando"
            {
                    
                // consumoAnterior => consumido por la red, calcular carga comprobar el ratio de carga
                c.kwRed = calcularConsumo(consumoAnterior, c.horaIni, horaActual);

                // calculamos lo que ha cargado la bateria
                double capacidadCarga = ServicioBateria.capacidadCargadorBateriaSuministradora((long)u.bateriaSuministradora);
                c.kwCargados = calcularConsumo(capacidadCarga, c.horaIni, horaActual);

                // Sumamos los que se ha cargado a la entidad Carga


            } else if (estado == "suministrando") // "suministrando"
                            {

                // consumoAnterior => suministra, calcular almacenaje en bateria

                            } else if (estado == "carga y suministra")// "carga y suministra"
                                    { 
                // consumoAnterior => suministra, calgular carga, calcular almacenaje en bateria
                                    }
                //
                // finalizar consumo
                finalizarConsumo(ubicacionId, c.consumoActual, horaActual, estado);

            // creamos el nuevo consumo
            long consumoNuevo = crearConsumo(ubicacionId, consumoActual, horaActual);

            //devolvemos el id del nuevo consumo
            return consumoNuevo;
        }

        #endregion modificar Consumo

        #region Obtener la entidad Consumo vigente
        [Transactional]
        public Consumo ConsumoActualUbicacionActual(long ubicacionId)
        {

            //buscamos el consumo (entidad) actual
            return consumoDao.UltimoConsumoUbicacion(ubicacionId);

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
