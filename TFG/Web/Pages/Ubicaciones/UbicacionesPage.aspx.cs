using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using Es.Udc.DotNet.TFG.Web.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages.Ubicaciones
{
    public partial class UbicacionesPage : SpecificCulturePage
    {
        private ObjectDataSource pbpDataSource = new ObjectDataSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsUserAuthenticated(Context))
            {
               // Response.Redirect(
               //Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }
            if (!IsPostBack)
            {

                



                
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/Ubicaciones/CrearUbicacion.aspx"));
        }
    }
}