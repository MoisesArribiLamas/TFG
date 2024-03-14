using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service
{
    public interface IServiceControlador
    {
        [Transactional]
        void gestionDeRatiosBateriaSuministradora(long bateriaId, DateTime fechaActual, TimeSpan horaActual);


        [Transactional]
        void cambiarRatiosBateria(long bateriaId, double? ratioCarga, double? ratioCompra, double? ratioUso);

        [Transactional]
        void ComprobarRatiosUbicaciones(DateTime fechaActual, TimeSpan horaActual);
        [Transactional]
        void CambiarBateriaSuministradora(long ubicacionId, long? bateriaSuministradora);
        [Transactional]
        void CrearConsumoInicial(long ubicacionId, double consumoActual);

        [Transactional]
        List<TarifaDetails> TarifasDeHoy();

    }
}
