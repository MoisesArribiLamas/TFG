using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public interface IServiceBateria
    {


        #region crear bateria
        [Transactional]
        long CrearBateria(long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso);
        #endregion

        [Transactional]
        void ModificarBateria(long bateriaId, long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso);


        [Transactional]
        List<BateriaDTO> VerBaterias(long idUsuario, int startIndex, int count);

        [Transactional]
        Bateria BuscarBateriaById(long bateriaId);


        [Transactional]
        void EliminarBateria(long bateriaId);

        [Transactional]

        long CrearCarga(long bateriaId, long tarifaId,
            TimeSpan horaIni, TimeSpan horaFin, double kws);



    }
}
