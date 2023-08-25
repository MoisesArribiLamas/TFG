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
using Es.Udc.DotNet.TFG.Model.Daos.SuministraDao;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public class ServiceBateria : IServiceBateria
    {

        [Inject]
        public IBateriaDao bateriaDao { private get; set; }
        [Inject]
        public IUsuarioDao UsuarioDao { private get; set; }

        [Inject]
        public ICargaDao CargaDao { private get; set; }

        [Inject]
        public ISuministraDao SuministroDao { private get; set; }


        [Inject]
        public IServiceEstado ServicioEstado { private get; set; }

        [Inject]
        public IServiceTarifa TarifaEstado { private get; set; }


        #region iniciar estado en bateria
        [Transactional]
        public void IniciarEstadoEnBateria(long bateriaId, long estadoId)
        {
            
            //buscamos la bateria
            Bateria b = bateriaDao.Find(bateriaId);

            //modificamos el estado actual
            b.estadoBateria = estadoId;

            bateriaDao.Update(b);


        }
        #endregion

        #region cambiar estado bateria
        [Transactional]
        public void CambiarEstadoEnBateria(long bateriaId, long estadoId, double kwHCargados, double kwHSuministrados)
        {
            //Fecha y hora actual
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            int horaTarifa = horaActual.Hours;

            //buscamos la bateria
            Bateria b = bateriaDao.Find(bateriaId);

            //recuperamos el estado anterior
            string estadoAnterior = ServicioEstado.NombreEstadoEnEstadoBateriaById(b.estadoBateria);

            //cerrar estadoBateria anterior
            ServicioEstado.PonerHorafinEstadoBateria((long)b.estadoBateria, horaActual);

            //nombre del estado posterior
            string estadoPosterior = ServicioEstado.BuscarEstadoPorId(estadoId);

            //Creamos EstadoBateria y cambiamos el estado actual en la bateria
            long estadoBateriaIdActual = ServicioEstado.CrearEstadoBateria(horaActual, fechaActual, bateriaId, estadoId);

            //Buscar la tarifa actual
            TarifaDTO tarifa = TarifaEstado.TarifaActual(fechaActual, horaTarifa);

            if ("sin actividad" == estadoAnterior) // "sin actividad" ->
            {               

                if ("cargando" == estadoPosterior)
                {                    
                    //Creamos la carga nueva
                    IniciarCarga(bateriaId, tarifa.tarifaId, horaActual);
                }

                if ("suministrando" == estadoPosterior)
                {                    
                    // Creamos el nuevo suministrando
                    IniciarSuministra(bateriaId, tarifa.tarifaId, horaActual);

                }

                if ("carga y suministra" == estadoPosterior)
                {
                    //Creamos la carga nueva
                    IniciarCarga(bateriaId, tarifa.tarifaId, horaActual);

                    //Creamoscuministrando nuevo
                    IniciarSuministra(bateriaId, tarifa.tarifaId, horaActual);
                }

            }
            if ("cargando" == estadoAnterior) // "cargando" ->
            {
                // Carga actual
                Carga cargaActual = UltimaCarga(bateriaId);

                //cerramos carga
                FinalizarCarga(cargaActual.cargaId, horaActual, kwHCargados);

                //Calculamos los kwH almacenados
                double almacenados = b.kwHAlmacenados + kwHCargados;

                //calculamos la media del precio
                double preciomedioNuevo = ((b.kwHAlmacenados * b.precioMedio) + (kwHCargados * tarifa.precio)) / (b.kwHAlmacenados + kwHCargados);

                //ponemos el total almacenado y precio medioNuevo
                b.kwHAlmacenados = almacenados;
                b.precioMedio = preciomedioNuevo;


                if ("cargando" == estadoPosterior)
                {
                    //Creamos la carga nueva
                    IniciarCarga(bateriaId, tarifa.tarifaId, horaActual);
                }

                if ("suministrando" == estadoPosterior)
                {
                    // Creamos el nuevo suministrando
                    IniciarSuministra(bateriaId, tarifa.tarifaId, horaActual);

                }

                if ("carga y suministra" == estadoPosterior)
                {
                    
                }
            }
            if ("suministrando" == estadoAnterior) // "suministrando" ->
            {
                if ("sin actividad" == estadoAnterior)
                {

                }

                if ("cargando" == estadoPosterior)
                {

                }

                if ("suministrando" == estadoPosterior)
                {


                }

                if ("carga y suministra" == estadoPosterior)
                {

                }
            }
            if ("carga y suministra" == estadoAnterior) // "carga y suministra" ->
            {
                if ("sin actividad" == estadoAnterior)
                {

                }

                if ("cargando" == estadoPosterior)
                {

                }

                if ("suministrando" == estadoPosterior)
                {


                }

                if ("carga y suministra" == estadoPosterior)
                {

                }
            }

            bateriaDao.Update(b);


        }

        #endregion
        //#region calcular porcentaje de la bateria
        //[Transactional]
        //public double porcentajeDeCarga(long bateriaId)
        //{
        //    //buscamos la bateria
        //    Bateria b = bateriaDao.Find(bateriaId);

        //    return (b.kwHAlmacenados * 100 / b.almacenajeMaximoKwH);
        //}
        //#endregion
        #region crear baterias
        [Transactional]
        public long CrearBateria(long ubicacionId, long usuarioId, double precioMedio, double kwHAlmacenados, double almacenajeMaximoKwH,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso)
        {


            Bateria b = new Bateria();
            b.ubicacionId = ubicacionId;
            b.usuarioId = usuarioId;
            b.precioMedio = precioMedio;
            b.kwHAlmacenados = kwHAlmacenados;
            b.almacenajeMaximoKwH = almacenajeMaximoKwH;
            b.fechaDeAdquisicion = fechaDeAdquisicion;
            b.marca = marca;
            b.modelo = modelo;
            b.ratioCarga = ratioCarga;
            b.ratioCompra = ratioCompra;
            b.ratioUso = ratioUso;
            b.estadoBateria = 0;

            bateriaDao.Create(b);

            // creamos el estado de la bateria inicial => "sin actividad" y en la fecha que se crea
            TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            DateTime fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            //creamos el estadoBateria inicial
            long estadoBateriaId = ServicioEstado.CrearEstadoBateria(horaIni, fecha, b.bateriaId, ServicioEstado.BuscarEstadoPorNombre("sin actividad"));

            IniciarEstadoEnBateria(b.bateriaId, estadoBateriaId);

            return b.bateriaId;


        }

        #endregion crear baterias

        #region Modificacar Bateria
        [Transactional]
        public void ModificarBateria(long bateriaId, long ubicacionId, long usuarioId, double precioMedio, double kwHAlmacenados, double almacenajeMaximoKwH,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso)
        {
            bateriaDao.updateInformacion(bateriaId, ubicacionId, usuarioId, precioMedio, kwHAlmacenados,
                almacenajeMaximoKwH, fechaDeAdquisicion, marca, modelo, ratioCarga,
                ratioCompra, ratioUso);
        }
        #endregion Modificar

        #region baterias del Usuario
        [Transactional]
        public List<BateriaDTO> VerBaterias(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<BateriaDTO> bateriasDTO = new List<BateriaDTO>();

                List<Bateria> baterias = bateriaDao.findBateriaByUser(idUsuario, startIndex, count);

                foreach (Bateria b in baterias)
                {
                    bateriasDTO.Add(new BateriaDTO(b.bateriaId, b.ubicacionId, b.usuarioId, b.precioMedio, b.kwHAlmacenados,
                b.almacenajeMaximoKwH, b.fechaDeAdquisicion, b.marca, b.modelo, b.ratioCarga,
                b.ratioCompra, b.ratioUso));
                }
                return bateriasDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region Buscar Bateria por ID
        [Transactional]
        public Bateria BuscarBateriaById(long bateriaId)
        {

            return bateriaDao.Find(bateriaId);
            
        }
        #endregion Buscar por ID

        #region Eliminar Bateria
        [Transactional]
        public void EliminarBateria(long bateriaId)
        {       
            bateriaDao.Remove(bateriaId);
        }
        #endregion Eliminar

        #region crear Carga
        [Transactional]
        public long IniciarCarga(long bateriaId, long tarifaId,
            TimeSpan horaIni)
        {
            // Se podria hacer poniendo el campo nullable pero me decante por esta forma
            int hour = 0;
            int minutes = 0;
            int seconds = 0;

            TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);

            Carga c = new Carga();
            c.bateriaId = bateriaId;
            c.tarifaId = tarifaId;
            c.horaIni = horaIni;
            c.horaFin = horaFin;
            c.kwH = 0;

            CargaDao.Create(c);
            return c.cargaId;


        }

        #endregion

        #region Poner hora fin estado bateria
        [Transactional]
        public bool FinalizarCarga(long cargaID, TimeSpan horaFin, double kwH)
        {
            return CargaDao.FinalizarCarga(cargaID, horaFin, kwH);
        }

        #endregion

        #region Buscar Carga por ID
        [Transactional]
        public Carga BuscarCargaById(long cargaId)
        {

            return CargaDao.Find(cargaId);

        }
        #endregion Buscar carga por ID
        
        #region cargas de una bateria
        [Transactional]
        public List<CargaDTO> MostrarCargasBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            try
            {
                List<CargaDTO> cargasDTO = new List<CargaDTO>();

                List<Carga> cargas = CargaDao.MostrarCargasBareriaPorFecha(bateriaId, fecha, fecha2 , startIndex, count);

                foreach (Carga c in cargas)
                {
                    cargasDTO.Add(new CargaDTO(c.cargaId, c.bateriaId, c.tarifaId, c.horaIni, c.horaFin, c.kwH)); 
                   
                }
                return cargasDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }
        #endregion

        #region Ultima Carga de una bateria
        [Transactional]
        public Carga UltimaCarga(long bateriaId)
        {
            return CargaDao.UltimaCargaBareria(bateriaId);
        }

        
        #endregion

        //#region cargas de una bateria
        //[Transactional]
        //public List<CargaDTO> UltimaCargaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count)
        //{
        //    try
        //    {
        //        List<CargaDTO> cargasDTO = new List<CargaDTO>();

        //        List<Carga> cargas = CargaDao.MostrarCargasBareriaPorFecha(bateriaId, fecha, fecha2, startIndex, count);

        //        foreach (Carga c in cargas)
        //        {
        //            cargasDTO.Add(new CargaDTO(c.cargaId, c.bateriaId, c.tarifaId, c.horaIni, c.horaFin, c.kwH));

        //        }
        //        return cargasDTO;

        //    }
        //    catch (InstanceNotFoundException)
        //    {
        //        return null;
        //    }
        //}

        //#endregion

        #region crear Suministra
        [Transactional]
        public long IniciarSuministra(long bateriaId, long tarifaId, TimeSpan horaIni)
        {
            // Se podria hacer poniendo el campo nullable pero me decante por esta forma
            int hour = 0;
            int minutes = 0;
            int seconds = 0;

            TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);

            Suministra s = new Suministra();
            s.bateriaId = bateriaId;
            s.tarifaId = tarifaId;
            s.ahorro = 0;
            s.horaIni = horaIni;
            s.horaFin = horaFin;
            s.kwH = 0;

            SuministroDao.Create(s);
            return s.suministraId;

        }
        #endregion

        #region Poner hora fin Suministra bateria
        [Transactional]
        public bool FinalizarSuministra(long suministraID, TimeSpan horaFin, double kwH, double ahorro)
        {
            return SuministroDao.FinalizarSuministra(suministraID, horaFin, kwH, ahorro);
        }

        #region Ultimo suministra de una bateria
        [Transactional]
        public Suministra UltimaSuministra(long bateriaId)
        {
            return SuministroDao.UltimaSuministraBareria(bateriaId);
        }


        #endregion

        #endregion
        #region Buscar Suministra por ID 
        [Transactional]

        public Suministra BuscarsuministraById(long suministraId)
        {

            return SuministroDao.Find(suministraId);

        }
        #endregion Buscar carga por ID

        #region suministros de una bateria
        [Transactional]

        public List<SuministroDTO> MostrarSuministraBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            try
            {
                List<SuministroDTO> suministroDTO = new List<SuministroDTO>();

                List<Suministra> cargas = SuministroDao.MostrarSuministrosBareriaPorFecha(bateriaId, fecha, fecha2, startIndex, count);

                foreach (Suministra c in cargas)
                {
                    suministroDTO.Add(new SuministroDTO(c.suministraId, c.bateriaId, c.tarifaId, c.ahorro, c.horaIni, c.horaFin, c.kwH));

                }
                return suministroDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region ahorro de una bateria
        [Transactional]

        public double ahorroBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2)
        {
                
            return SuministroDao.ahorroBareriaPorFecha(bateriaId, fecha, fecha2);

        }

        #endregion

        #region ahorro de las baterias de un usuario
        [Transactional]

        public double ahorroBareriasUsuarioPorFecha(long usuarioId, DateTime fecha, DateTime fecha2)
        {

            return SuministroDao.ahorroUsuarioPorFecha(usuarioId, fecha, fecha2);

        }

        #endregion
    }

}
