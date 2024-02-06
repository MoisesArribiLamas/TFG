using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class SuccesfulOperation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            imagenExito.Text += "<img src=" + "~/Img/lclSucccessSuccessfullOperation.ico" + "/>";
        }
    }
}