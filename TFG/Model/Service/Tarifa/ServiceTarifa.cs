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
                    tarifasDTO.Add(new TarifaDTO(t.precio, t.hora, t.fecha));
                }
                return tarifasDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        } /*
        public List<UbicacionProfileDetails> verUbicaciones(long idUsuario, int startIndex, int count)
        {
            try
            {
                List<UbicacionProfileDetails> ubicacionesDTO = new List<UbicacionProfileDetails>();

                List<Ubicacion> ubicaciones = ubicacionDao.ubicacionesUsuario(idUsuario, startIndex, count);

                foreach (Ubicacion u in ubicaciones)
                {
                    ubicacionesDTO.Add(new UbicacionProfileDetails(u.codigoPostal, u.localidad, u.calle, u.portal, u.numero));
                }
                return ubicacionesDTO;

            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
         */

        #endregion


    }

}
