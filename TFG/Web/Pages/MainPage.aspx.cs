using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class MainPage : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (SessionManager.IsUserAuthenticated(Context))
            {
                if (lblInicioMainPage != null)
                    lblInicioMainPage.Visible = false;
            }
        }
    }
}