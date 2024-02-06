using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Es.Udc.DotNet.TFG.Model.Daos.CargaDao;
using Es.Udc.DotNet.TFG.Model.Daos.SuministraDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Ninject;

namespace Es.Udc.DotNet.TFG.Model.Service.Controlador
{
    public class ServiceControlador : IServiceControlador
    {

        [Inject]
        public IBateriaDao bateriaDao { private get; set; }

        [Inject]
        public IUsuarioDao UsuarioDao { private get; set; }

        [Inject]
        public ICargaDao CargaDao { private get; set; }

        [Inject]
        public ISuministraDao SuministroDao { private get; set; }

        [Inject]
        public IServiceBateria ServicioBateria { private get; set; }

        [Inject]
        public IServiceUbicacion ServicioUbicacion { private get; set; }

        [Inject]
        public IServiceEstado ServicioEstado { private get; set; }

        [Inject]
        public IServiceTarifa TarifaEstado { private get; set; }

        //[Inject]
        //public IServiceUbicacion ServicioUbicacion { private get; set; } 


        #region Parte Asincrona

        [Transactional]
        public void Asincrono(long bateriaId, long estadoId, double kwHCargados, double kwHSuministrados)
        {
            // Fecha y hora actual
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second); 


            //el cambio de hora (las que tienen estado distinto a "sin actividad")
            if (horaActual.Minutes == 0)
            {
                //cambio de estado para todas las baterias que no esten en "sin actividad"
                    // buscamos la bateria
                    Bateria b = bateriaDao.Find(bateriaId);
//List<BateriaDTO> Baterias = VerBaterias(b.usuarioId, startIndex, count); --------------------------------------
                    //bucle cambiando todos los estados que haya que cambiar

            }
            //no exceder del maximo de capacidad de la bateria

            //gestion de ratios
                //buscamos todas las baterias
                

                // Tarifa actual (hora)
                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = TarifaEstado.TarifaActual(fechaActual, horaTarifa);

                //bucle con todas las baterias gestion de ratios
            //alertas de si se esta agotando la bateria en caso de que se suministre mas de lo que se carga.
        }
        #endregion


    }

}
