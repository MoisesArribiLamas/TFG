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

        protected void btModificarRatios_Click(object sender, EventArgs e)
        {

        }
    }
}