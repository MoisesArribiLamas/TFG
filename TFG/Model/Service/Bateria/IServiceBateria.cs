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



        [Transactional]
        long CrearBateria(long ubicacionId, long usuarioId, double precioMedio, double kwHAlmacenados, double almacenajeMaximoKwH,
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso, double capacidadCargador);

        [Transactional]
        double capacidadDelCargador(long bateriaId);

        [Transactional]
        void ModificarRatios(long bateriaId, double? ratioCarga, double? ratioCompra, double? ratioUso);

        [Transactional]
        RatiosDTO MostrarRatios(long bateriaId);

        [Transactional]
        void gestionDeRatios(long bateriaId, double kwHCargados, double kwHSuministrados, DateTime fechaActual, TimeSpan horaActual);


        [Transactional]
        void CambiarEstadoEnBateria(long bateriaId, long estadoId, double kwHCargados, double kwHSuministrados);

        [Transactional]
        void CambiarEstadoEnBateriaPorCambioDeHora(long bateriaId, long estadoId, double kwHCargados, double kwHSuministrados);


        [Transactional]
        void ModificarBateria(long bateriaId, long? ubicacionId, long? usuarioId, double? precioMedio,
            double? kwHAlmacenados, double? almacenajeMaximoKwH, DateTime fechaDeAdquisicion, string marca,
            string modelo, double? ratioCarga, double? ratioCompra, double? ratioUso, double? capacidadCargador);
        
        [Transactional]
        double porcentajeDeCarga(long bateriaId);

        [Transactional]
        double porcentajeDeCargaConConsumo(long bateriaId, double cargaEnConsumo);


        [Transactional]
        List<BateriaDTO> VerBateriasUsuario(long idUsuario, int startIndex, int count);

        [Transactional]
        Bateria BuscarBateriaById(long bateriaId);


        [Transactional]
        void EliminarBateria(long bateriaId);

        [Transactional]

        long IniciarCarga(long bateriaId, long tarifaId,
            TimeSpan horaIni);

        [Transactional]
        long CrearCargaEnBateria(long bateriaId);

        [Transactional]
        bool CargaAñadida(long bateriaId, double kwHcargados, double kwhsuministrados, TimeSpan horaActual);

        [Transactional]
        bool FinalizarCarga(long cargaID, TimeSpan horaFin, double kwH);

        [Transactional]
        Carga BuscarCargaById(long cargaId);

        [Transactional]
        List<CargaDTO> MostrarCargasBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count);

        [Transactional]
        Carga UltimaCarga(long bateriaId);

        [Transactional]
        long IniciarSuministra(long bateriaId, long tarifaId, TimeSpan horaIni);

        [Transactional]
        long CrearSuministraEnBateria(long bateriaId);
        [Transactional]
        bool FinalizarSuministra(long suministraID, TimeSpan horaFin, double kwH, double ahorro);
        [Transactional]
        Suministra UltimaSuministra(long bateriaId);
        [Transactional]
        Suministra BuscarsuministraById(long suministraId);

        [Transactional]
        List<SuministroDTO> MostrarSuministraBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count);

        [Transactional]
        double ahorroBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2);

        [Transactional]
        double ahorroBareriasUsuarioPorFecha(long usuarioId, DateTime fecha, DateTime fecha2);

        [Transactional]
        string EstadoDeLaBateria(long bateriaId);

        [Transactional]
        double capacidadCargadorBateriaSuministradora(long bateriaSuministradoraId);

    }
}
