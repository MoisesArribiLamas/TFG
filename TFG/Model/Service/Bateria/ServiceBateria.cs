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


        #region crear baterias
        [Transactional]
        public long CrearBateria(long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso)
        {


            Bateria b = new Bateria();
                b.ubicacionId = ubicacionId;
                b.usuarioId = usuarioId;
                b.precioMedio = precioMedio;
                b.kwAlmacenados = kwAlmacenados;
                b.almacenajeMaximoKw = almacenajeMaximoKw;
                b.fechaDeAdquisicion = fechaDeAdquisicion;
                b.marca = marca;
                b.modelo = modelo;
                b.ratioCarga = ratioCarga;
                b.ratioCompra = ratioCompra;
                b.ratioUso = ratioUso;
                
            bateriaDao.Create(b);
            return b.bateriaId;

            
        }

            #endregion crear baterias
        #region Modificacar Bateria
        [Transactional]
        public void ModificarBateria(long bateriaId, long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso)
        {
            bateriaDao.updateInformacion(bateriaId, ubicacionId, usuarioId, precioMedio, kwAlmacenados,
                almacenajeMaximoKw, fechaDeAdquisicion, marca, modelo, ratioCarga,
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
                    bateriasDTO.Add(new BateriaDTO(b.bateriaId, b.ubicacionId, b.usuarioId, b.precioMedio, b.kwAlmacenados,
                b.almacenajeMaximoKw, b.fechaDeAdquisicion, b.marca, b.modelo, b.ratioCarga,
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
        public long CrearCarga(long bateriaId, long tarifaId,
            TimeSpan horaIni, TimeSpan horaFin, double kws)
        {

            Carga c = new Carga();
            c.bateriaId = bateriaId;
            c.tarifaId = tarifaId;
            c.horaIni = horaIni;
            c.horaFin = horaFin;
            c.kws = kws;

            CargaDao.Create(c);
            return c.cargaId;


        }

        #endregion crear cargas

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
                    cargasDTO.Add(new CargaDTO(c.cargaId, c.bateriaId, c.tarifaId, c.horaIni, c.horaFin, c.kws)); 
                   
                }
                return cargasDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        [Transactional]
        public long CrearSuministra(long bateriaId, long tarifaId, double ahorro,
            TimeSpan horaIni, TimeSpan horaFin, double kws)
        {

            Suministra s = new Suministra();
            s.bateriaId = bateriaId;
            s.tarifaId = tarifaId;
            s.ahorro = ahorro;
            s.horaIni = horaIni;
            s.horaFin = horaFin;
            s.kws = kws;

            SuministroDao.Create(s);
            return s.suministraId;

        }

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
                    suministroDTO.Add(new SuministroDTO(c.suministraId, c.bateriaId, c.tarifaId, c.ahorro, c.horaIni, c.horaFin, c.kws));

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
        //        double ahorroBareriasUsuarioPorFecha(long usuarioId, DateTime fecha, DateTime fecha2);

        #region ahorro de las baterias de un usuario
        [Transactional]

        public double ahorroBareriasUsuarioPorFecha(long usuarioId, DateTime fecha, DateTime fecha2)
        {

            return SuministroDao.ahorroUsuarioPorFecha(usuarioId, fecha, fecha2);

        }

        #endregion
    }

}
