using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.ModelUtil.Log;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service;
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
    public partial class ModificarUbicacion : SpecificCulturePage
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
            String ubicacionId = Request.Params.Get("idUbicacion");

            // obtenemos el servicio Ubicacion
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion pedidoUbicacion = iocManager.Resolve<IServiceUbicacion>();

            Ubicacion ubicacion = pedidoUbicacion.buscarUbicacionById(Convert.ToInt64(ubicacionId));

            BoxEtiquetaCrearUbicacion.Text = ubicacion.etiqueta;
            BoxLocalidadCrearUbicacion.Text = ubicacion.localidad;
            BoxCalleCrearUbicacion.Text = ubicacion.calle;
            BoxNumeroCrearUbicacion.Text = ubicacion.numero.ToString();
            BoxPortalCrearUbicacion.Text = ubicacion.portal;
            BoxCodigoPostalCrearUbicacion.Text = ubicacion.codigoPostal.ToString();


            // Usuario
            long idUser = SessionManager.GetUserSession(Context).UserProfileId;
            Trace.Warn("Usuario", Convert.ToString(idUser));
            Trace.Warn("Etiqueta", Convert.ToString(BoxEtiquetaCrearUbicacion.Text));
            Trace.Warn("Localidad", Convert.ToString(BoxLocalidadCrearUbicacion.Text));
            Trace.Warn("Calle", Convert.ToString(BoxCalleCrearUbicacion.Text));
            Trace.Warn("Numero", Convert.ToString(BoxNumeroCrearUbicacion.Text));
            Trace.Warn("Portal", Convert.ToString(BoxPortalCrearUbicacion.Text));
            Trace.Warn("CodigoPostal", Convert.ToString(BoxCodigoPostalCrearUbicacion.Text));

        }




        protected void btModificar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                    IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

                    // Obtenemos el id de la ubicacion por parametro
                    long idUbicacion = Int32.Parse(Request.Params.Get("idUbicacion"));


                    serviceUbicacion.modificarUbicacion(idUbicacion, Convert.ToInt64(BoxCodigoPostalCrearUbicacion.Text), BoxLocalidadCrearUbicacion.Text, BoxCalleCrearUbicacion.Text, BoxPortalCrearUbicacion.Text, Convert.ToInt64(BoxNumeroCrearUbicacion.Text), BoxEtiquetaCrearUbicacion.Text);
                                              //        (long ubicacionId, long? codigoPostal, string localidad, string calle, string portal, long? numero, string etiqueta);

                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));
                }
                catch (DuplicateInstanceException)
                {
                    lblErrorModificarUbicacion.Visible = true;
                }
            }
        }
       
    }
}