using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service.Estados
{
    public interface IServiceEstado
    {
        [Transactional]
        long CrearEstadoBateria(TimeSpan horaIni, DateTime fecha, long bateriaId, long estadoId);
        [Transactional]
        SeEncuentraDTO BuscarEstadoBateriaById(long? estadobateriaId);
        [Transactional]
        string NombreEstadoEnEstadoBateriaById(long? estadobateriaId);
        [Transactional]
        List<EstadoDTO> verTodosLosEstados();
        [Transactional]
        List<SeEncuentraDTO> MostrarEstadoBateriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count);
        [Transactional]
        bool PonerHorafinEstadoBateria(long estadobateriaID, TimeSpan hora);
        [Transactional]
        long BuscarEstadoPorNombre(string nombre);
        [Transactional]
        string BuscarEstadoPorId(long estadoId);
    }   
}
