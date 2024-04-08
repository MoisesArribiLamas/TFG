using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.ModelUtil.Log;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class ModificarBateria : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }

            //Obtenemos parametro
            String bateriaId = Request.Params.Get("idBateria");


            // obtenemos el servicio Bateria
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceBateria servicioBateria = iocManager.Resolve<IServiceBateria>();

            Bateria bateria = servicioBateria.BuscarBateriaById(Convert.ToInt64(bateriaId));
            string estado = servicioBateria.EstadoDeLaBateria(Convert.ToInt64(bateriaId));

            BoxNSerieModificarBateria.Text = bateria.nSerie;
            BoxMarcaModificarBateria.Text = bateria.marca;
            BoxCapacidadCargadorModificarBateria.Text = bateria.capacidadCargador.ToString();
            BoxModeloModificarBateria.Text = bateria.modelo.ToString();
            BoxRatioCompra.Text = bateria.ratioCompra.ToString();
            BoxRatioCarga.Text = bateria.ratioCarga.ToString();
            BoxRatioUso.Text = bateria.ratioUso.ToString();
            lblAlmacenajeMaximoN.Text = bateria.almacenajeMaximoKwH.ToString();
            lblKwAlmacenadosNumero.Text = bateria.kwHAlmacenados.ToString();
            lblPrecioMedioNumero.Text = bateria.precioMedio.ToString();

            string idioma = SessionManager.GetUserSession(Context).Idioma;            
            lblEstado.Text = MostrarEstadoIdioma(estado, idioma);

            // buscamos la ubicacion
            IServiceUbicacion servicioUbicacion = iocManager.Resolve<IServiceUbicacion>();
            Ubicacion ubicacion = servicioUbicacion.buscarUbicacionById(bateria.ubicacionId);
            lblValorBateriaSuministradora.Text = BateriaSuministradora(ubicacion, idioma, bateria.bateriaId);

        }




        protected void btModificar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                    IServiceBateria servicioBateria = iocManager.Resolve<IServiceBateria>();

                    // Obtenemos el id de la ubicacion por parametro
                    long idBateria = Int32.Parse(Request.Params.Get("idBateria"));


                    // serviceUbicacion.modificarUbicacion(idUbicacion, Convert.ToInt64(BoxCodigoPostalCrearUbicacion.Text), BoxLocalidadCrearUbicacion.Text, BoxCalleCrearUbicacion.Text, BoxPortalCrearUbicacion.Text, Convert.ToInt64(BoxNumeroCrearUbicacion.Text), BoxEtiquetaCrearUbicacion.Text);

            //        servicioBateria.ModificarBateria(idBateria, null , null , null, null, null, null, BoxMarcaModificarBateria.Text,
            //string modelo, double ? ratioCarga, double ? ratioCompra, double ? ratioUso, double ? capacidadCargador);

                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));
                }
                catch (DuplicateInstanceException)
                {
                    //lblErrorModificarUbicacion.Visible = true;
                }
            }
        }

        protected string MostrarEstadoIdioma(string estado, string idioma)
        {

            if (idioma == "es") // castellano
            {
                if (estado == "carga y suministra")
                {
                    estado = "Cargando y Suministrando";
                }
                return estado;
            }
            else if (idioma == "gl") // gallego
                {
                if (estado == "sin actividad")
                {
                    estado = "Sen Actividade";
                }

                if (estado == "carga y suministra")
                {
                    estado = "Cargando e Suministrando";
                }
                return estado;
            }
            else // (idioma == "en") ingles
            {
                if (estado == "sin actividad")
                {
                    estado = "No Activity";
                }

                if (estado == "cargando")
                {
                    estado = "Loading";
                }

                if (estado == "suministrando")
                {
                    estado = "Supplying";
                }

                if (estado == "carga y suministra")
                {
                    estado = "Loading & Supplying";
                }
                return estado;

            }
        }

        protected string BateriaSuministradora(Ubicacion Ubicacion, string idioma, long bateriaId)
        {
            long? bSuministradora = Ubicacion.bateriaSuministradora;
            bool isSupplyingBattery = (bSuministradora == bateriaId);
            if (idioma == "es") // castellano
            {
                if (isSupplyingBattery)
                {
                    return "SUMINISTRADORA";
                } else
                {
                 return "NO SUMINISTRADORA";
                }
               
            }
            else if (idioma == "gl") // gallego
            {
                if (isSupplyingBattery)
                {
                    return "SUMINISTRADORA";
                }
                else
                {
                    return "NON SUMINISTRADORA";
                }
            }
            else // (idioma == "en") ingles
            {
                if (isSupplyingBattery)
                {
                    return "SUPPLYING";
                }
                else
                {
                    return "NON-SUPPLYING";
                }

            }
        }

        protected void btModificarRatios_Click(object sender, EventArgs e)
        {

        }
    }
}