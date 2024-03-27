using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service.Tarifas
{
    public interface IServiceTarifa
    {

        [Transactional]
        List<TarifaDTO> verTarifasDelDia(DateTime fecha);

        [Transactional]
        List<TarifaDetails> TarifasDelDia(DateTime fecha);

        [Transactional]
        TarifaDTO BuscarMejorTarifa(DateTime fecha);

        [Transactional]
        TarifaDTO BuscarpeorTarifa(DateTime fecha);
        [Transactional]
        double PrecioMedioTarifasHoy();
        [Transactional]
        List<TarifaDTO> OrdenarMejorPrecioTarifasDelDia(DateTime fecha);
        [Transactional]
        List<TarifaDTO> OrdenarPeorPrecioTarifasDelDia(DateTime fecha);
        [Transactional]
        TarifaDTO TarifaActual(DateTime fecha, int hora);
        [Transactional]
        long crearTarifa(double precio, long hora, DateTime fecha);
        [Transactional]
        bool ExistenTarifasDelDia(DateTime fecha);
        [Transactional]
        void scrapyTarifas();

    }
}
