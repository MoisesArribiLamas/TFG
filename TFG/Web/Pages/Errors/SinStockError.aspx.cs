﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages.Errors
{
    public partial class SinStockError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string nombreProd= Request.Params.Get("nombreProducto");

            lblErrorTitle.Text = lblErrorTitle.Text + nombreProd;


        }
    }
}