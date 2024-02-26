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
        public IBateriaDao ubicacionDao { private get; set; }

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
                // buscamos todas las baterias vincuadas a una ubicacion
                // nuevo consumo para las baterias que no esten en "sin actividad"
                // cambio de estado para todas las baterias que no esten en "sin actividad"
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

        #region
        [Transactional]
        public void cambiarRatiosBateria(long bateriaId)
        {
            // buscamos la bateria.
            Bateria b = bateriaDao.Find(bateriaId);

            // Comprobamos si la bateria siministradora es la misma que la de los cambios.
            if ((long)b.Ubicacion.bateriaSuministradora == bateriaId)
            {

            }
            else // no es la bateria suministradora
            {

            }
            
        }
        #endregion

        #region Gestion de los ratios

        [Transactional]
        public void gestionDeRatiosBateriaSuministradora(long bateriaId, DateTime fechaActual, TimeSpan horaActual)
        {
            double kwHCargados = 0;
            double kwHSuministrados = 0;

            // buscamos la bateria
            Bateria b = bateriaDao.Find(bateriaId);
            
            //
            //obtenemos la carga
            Carga carga = ServicioBateria.UltimaCarga(bateriaId);

            if (carga != null)
            { // la carga que hay sin contabilizar en la bateria
                kwHCargados = carga.kwH;
            }

            //obtenemos suministra
            Suministra suministra = ServicioBateria.UltimaSuministra(bateriaId);

            if (suministra != null) // lo suministrado que hay sin contabilizar
            {
                kwHSuministrados = suministra.kwH;
            }
            //


            // Tarifa actual (hora)
            int horaTarifa = horaActual.Hours;

            // Buscar la tarifa actual
            TarifaDTO tarifa = TarifaEstado.TarifaActual(fechaActual, horaTarifa);

            //consumo pendiente
            string estado = ServicioBateria.EstadoDeLaBateria(bateriaId);
            double consumoPendiente = ServicioUbicacion.CalcularConsumoParaCalculoRatios(b.ubicacionId, horaActual, estado, bateriaId);

            // porcentaje de la bateria sumandole el consumo pendiente
            double porcentajeCargaConConsumo = ServicioBateria.porcentajeDeCargaConConsumo(bateriaId, consumoPendiente);

            // si el ratio de carga (minimo 10%) es menor al porcentaje de la bateria => carga
            if (b.ratioCarga >= porcentajeCargaConConsumo)
            {

                if ((b.ratioUso < tarifa.precio)) // "sin actividad" -> "cargando" 
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId)!= "carga y suministra")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId);

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("carga y suministra");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados);
                    }

                }
                else
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "cargando")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId);

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("cargando");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados);
                    }
                }
               
            }
            else
                // el ratio de compra < precio tarifa
                if (b.ratioCompra < tarifa.precio)
                {

                if ((b.ratioUso < tarifa.precio))
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "suministrando")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId);

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("suministrando");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados);
                    }

                }
                else
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "sin actividad")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId);

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("sin actividad");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados);
                    }
                }

            }

            else
                    // el ratio de compra >= precio tarifa
                    if (b.ratioCompra >= tarifa.precio)
            {

                if ((b.ratioUso < tarifa.precio))
                {
                    if (porcentajeCargaConConsumo < 100) //  si la bateria esta al 100% no puede cargar
                    {
                        if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "carga y suministra")
                        {
                            //ponemos consumo en carga y/o suministra
                            ServicioUbicacion.actualizarConsumoActual(b.ubicacionId);

                            long estadoId = ServicioEstado.BuscarEstadoPorNombre("carga y suministra");
                            ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados);
                        }
                    }
                    else
                    {
                        if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "suministrando")
                        {
                            //ponemos consumo en carga y/o suministra
                            ServicioUbicacion.actualizarConsumoActual(b.ubicacionId);

                            long estadoId = ServicioEstado.BuscarEstadoPorNombre("suministrando");
                            ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados);
                        }
                    }


                }
                else
                    if (porcentajeCargaConConsumo < 100) //  si la bateria esta al 100% no puede cargar
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "cargando")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId);

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("cargando");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados);
                    }
                }
                else
                {
                    if (ServicioBateria.EstadoDeLaBateria(bateriaId) != "sin actividad")
                    {
                        //ponemos consumo en carga y/o suministra
                        ServicioUbicacion.actualizarConsumoActual(b.ubicacionId);

                        long estadoId = ServicioEstado.BuscarEstadoPorNombre("sin actividad");
                        ServicioBateria.CambiarEstadoEnBateria(bateriaId, estadoId, kwHCargados, kwHSuministrados);
                    }
                }

            }

        }
        #endregion
    }
        
}
