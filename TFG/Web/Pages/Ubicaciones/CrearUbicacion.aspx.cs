using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.ModelUtil.Log;
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
    public partial class CrearUbicacion : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }

            
        }
        

        

        protected void btRegistrar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                    IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

                    // Usuario
                    long idUser = SessionManager.GetUserSession(Context).UserProfileId;

                    serviceUbicacion.crearUbicacion(Convert.ToInt64(BoxCodigoPostalCrearUbicacion.Text), BoxLocalidadCrearUbicacion.Text, BoxCalleCrearUbicacion.Text, BoxPortalCrearUbicacion.Text, Convert.ToInt64(BoxNumeroCrearUbicacion.Text), BoxEtiquetaCrearUbicacion.Text, idUser);

                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));
                }
                catch (DuplicateInstanceException)
                {
                    lblErrorCrearUbicacion.Visible = true;
                }
            }
        }
       
    }
}