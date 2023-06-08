using Es.Udc.DotNet.TFG.Model.Service.Exceptions;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages.User
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }
            lblOldPasswordErrorChangePassword.Visible = false;

        }

        protected void BtnChangePasswordClick(object sender, EventArgs e)
        {
            
            if (Page.IsValid)
            {
                try
                {

                    SessionManager.ChangePassword(Context, txtOldPasswordChangePassword.Text,
                        txtNewPasswordChangePassword.Text);

                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));

                }
                catch (IncorrectPasswordException)
                {
                    lblOldPasswordErrorChangePassword.Visible = true;
                }
            }
            
        }
    }
}
