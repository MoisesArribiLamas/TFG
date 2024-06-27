using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages.Graficas
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string suministradoCargadoyRed()
        {
            //[
            //  ['Year', 'Sales', 'Expenses', 'Profit'],
            //  ['2014', 1000, 400, 200],
            //  ['2015', 1170, 460, 250],
            //  ['2016', 660, 1120, 300],
            //  ['2017', 1030, 540, 350]
            //]

            string strDatos;

            strDatos = "[['Dia','(€)'],";

            //foreach (AhorroPorDias dr in ahorros)
            //{
            //    string f = (dr.fecha).ToString();
            //    string fecha = f.Substring(0, f.IndexOf(" "));

            //    strDatos = strDatos + "[";
            //    strDatos = strDatos + "'" + fecha + "'" + "," + Math.Truncate(dr.Count * 1000); ;
            //    strDatos = strDatos + "],";
            //}

            //strDatos = strDatos + "]";

            return strDatos;
        }
    }
   
    
}