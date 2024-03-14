using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web
{
    public partial class TFG : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                if (lblWelcome != null)
                    lblWelcome.Visible = false;
                if (lnkHome != null)
                    lnkHome.Visible = false;
                if (lnkLogout != null)
                    lnkLogout.Visible = false;
                if (lnkModifyUser != null)
                    lnkModifyUser.Visible = false;

            }
            else
            {
                if (lblWelcome != null)
                    lblWelcome.Text =
                        GetLocalResourceObject("lblWelcome.Text").ToString()
                        + " " + SessionManager.GetUserSession(Context).FirstName;
                if (lnkCreateAccount != null)
                    lnkCreateAccount.Visible = false;
                if (lnkLogUser != null)
                    lnkLogUser.Visible = false;
                if (lnkLogUser != null)
                    lnkLogUser.Visible = false;
            }

        }
    }
}