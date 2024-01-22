using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service.Estados
{
    public class ServiceTarifa : IServiceTarifa
    {

        [Inject]
        public ITarifaDao tarifaDao { private get; set; }


        #region mostrar las tarifas del dia
        [Transactional]
        public List<TarifaDTO> verTarifasDelDia(DateTime fecha)
        {
            try
            {
                List<TarifaDTO> tarifasDTO = new List<TarifaDTO>();

                List<Tarifa> tarifas = tarifaDao.verTarifasDelDia(fecha);

                foreach (Tarifa t in tarifas)
                {
                    tarifasDTO.Add(new TarifaDTO(t.tarifaId, t.precio, t.hora, t.fecha));
                }
                return tarifasDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region mostrar mejor precio del dia
        [Transactional]
        public long BuscarMejorTarifa(DateTime fecha)
        {
            return tarifaDao.PrecioMejorTarifa(fecha);
        }

        #endregion
        
        #region Ver las tarifas del dia ordenadas por peor precio
        [Transactional]
        public List<TarifaDTO> OrdenarMejorPrecioTarifasDelDia(DateTime fecha)
        {

            List<Tarifa> tarifas = tarifaDao.OrdenarMejorPrecioTarifasDelDia(fecha);

            List<TarifaDTO> tarifaDTO = new List<TarifaDTO>();

            foreach (Tarifa t in tarifas)
            {
                tarifaDTO.Add(new TarifaDTO(t.tarifaId, t.precio, t.hora, t.fecha));
            }
                
            return tarifaDTO;

        }

        #endregion

        #region mostrar el peor precio del dia
        [Transactional]
        public long BuscarpeorTarifa(DateTime fecha)
        {

            return tarifaDao.PrecioPeorTarifa(fecha);

        }

        #endregion
        
        #region mostrar la peor tarifa del dia
        [Transactional]
        public List<TarifaDTO> OrdenarPeorPrecioTarifasDelDia(DateTime fecha)
        {

            List<Tarifa> tarifas = tarifaDao.OrdenarPeorPrecioTarifasDelDia(fecha);

            List<TarifaDTO> tarifaDTO = new List<TarifaDTO>();

            foreach (Tarifa t in tarifas)
            {
                tarifaDTO.Add(new TarifaDTO(t.tarifaId, t.precio, t.hora, t.fecha));
            }

            return tarifaDTO;

        }

        #endregion

        #region mostrar tarifa actual
        [Transactional]
        public TarifaDTO TarifaActual(DateTime fecha, int hora)
        {
            Tarifa t = tarifaDao.TarifaActual(fecha, hora);
            
            return new TarifaDTO(t.tarifaId, t.precio, t.hora, t.fecha);

        }

        #endregion

    }

}
