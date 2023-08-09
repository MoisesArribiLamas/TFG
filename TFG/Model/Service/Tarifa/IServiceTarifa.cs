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
        long BuscarMejorTarifa(DateTime fecha);
        [Transactional]
        long BuscarpeorTarifa(DateTime fecha);



    }
}
