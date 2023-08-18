using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoBateriaDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service.Estados
{
    public class ServiceEstado : IServiceEstado
    {

        [Inject]
        public IEstadoDao estadoDao { private get; set; }
        [Inject]
        public IEstadoBateriaDao estadoBateriaDao { private get; set; }
        [Inject]
        public IServiceBateria servicioBateria { private get; set; }


        [Transactional]
        public long CrearEstadoBateria( TimeSpan horaIni, DateTime fecha, long bateriaId, long estadoId)
        {
            // Se podria hacer poniendo el campo nullable pero me decante por esta forma
            int hour = 0;
            int minutes = 0;
            int seconds = 0;

            TimeSpan horaFin = new TimeSpan(hour, minutes, seconds);

            SeEncuentra b = new SeEncuentra();
            b.horaIni = horaIni;
            b.horaFin = horaFin;
            b.fecha = fecha;
            b.bateriaId = bateriaId;
            b.estadoId = estadoId;

            //cambiamos el atributo estado de la bateria
            servicioBateria.CambiarEstadoBateria(bateriaId, estadoId);

            estadoBateriaDao.Create(b);
            return b.seEncuentraId;


        }

        #region Buscar EstadoBateria por ID
        [Transactional]
        public SeEncuentraDTO BuscarEstadoBateriaById(long estadobateriaId)
        {
            SeEncuentra bateriaEstado = estadoBateriaDao.Find(estadobateriaId);
            return new SeEncuentraDTO(bateriaEstado.seEncuentraId, bateriaEstado.horaIni, bateriaEstado.horaFin, bateriaEstado.fecha, bateriaEstado.bateriaId, bateriaEstado.estadoId);
        }
        #endregion Buscar por ID

        #region mostrar todos los estados posibles
        [Transactional]
        public List<EstadoDTO> verTodosLosEstados()
        {
            try
            {
                List<EstadoDTO> estadosDTO = new List<EstadoDTO>();

                List<Estado> estados = estadoDao.FindAllEstados();

                foreach (Estado e in estados)
                {
                    estadosDTO.Add(new EstadoDTO(e.nombre));
                }
                return estadosDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        //        List<SeEncuentra> MostrarEstadoBareriaPorFecha(DateTime fecha, DateTime fecha2);
        #region cargas de una bateria
        [Transactional]
        public List<SeEncuentraDTO> MostrarEstadoBateriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            try
            {
                List<SeEncuentraDTO> seEncuentraDTO = new List<SeEncuentraDTO>();

                List<SeEncuentra> estadosBaterias = estadoBateriaDao.MostrarEstadoBateriaPorFecha(bateriaId, fecha, fecha2, startIndex, count);

                foreach (SeEncuentra eb in estadosBaterias)
                {
                    seEncuentraDTO.Add(new SeEncuentraDTO(eb.seEncuentraId, eb.horaIni, eb.horaFin, eb.fecha, eb.bateriaId, eb.estadoId));

                }
                return seEncuentraDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion


        #region Poner hora fin estado bateria
        [Transactional]
        public bool PonerHorafinEstadoBateria(long estadobateriaID, TimeSpan hora)
        {
            return estadoBateriaDao.PonerHorafinEstadoBateria(estadobateriaID, hora);
        }

        #endregion

        #region Buscar Estado por nombre
        [Transactional]
        public long BuscarEstadoPorNombre(string nombre)
        {
         
            return estadoDao.FindEstadoByName(nombre);

        }
        #endregion Buscar por nombre
    }

}
