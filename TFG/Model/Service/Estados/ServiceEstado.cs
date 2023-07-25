using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service.Estados
{
    public class ServiceEstado : IServiceEstado
    {

        [Inject]
        public IEstadoDao estadoDao { private get; set; }


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


    }

}
