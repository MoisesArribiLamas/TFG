using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class UserMainPage : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }
            if (!IsPostBack)
            {
                IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IServiceControlador serviceControlador = iocManager.Resolve<IServiceControlador>();
                //List<TarifaDTO> tarifasHoy = serviceControlador.TarifasDeHoy();
            }
        }
    }
}