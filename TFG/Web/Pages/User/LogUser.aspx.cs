using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.TFG.Model.Service.Exceptions;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class LogUser : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            lblPassErrorLogUser.Visible = false;
            lblUserErrorLogUser.Visible = false;
        }

        protected void btLogging_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                   
                    SessionManager.Login(Context, TBUserNameLogUser.Text,
                        TBPassLogUser.Text, CheckCookiesLogUser.Checked);

                    if (Request.Params.Get("from") == "carrito")
                    {
                        Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/VerCarrito.aspx"));
                    }
                    if(Request.Params.Get("from") == "comentario")
                    {
                            Response.Redirect(Response.
                            ApplyAppPathModifier("~/Pages/Comentarios/crearComentario.aspx?idProducto=")+ Request.Params.Get("idProducto"));
                        }
                    FormsAuthentication.
                        RedirectFromLoginPage(btLoggingLogUser.Text,
                            CheckCookiesLogUser.Checked);

                }
                catch (InstanceNotFoundException )
                {
                    lblUserErrorLogUser.Visible = true;
                }
                catch (IncorrectPasswordException )
                {
                    lblPassErrorLogUser.Visible = true;
                }
            }

        }
    }
}