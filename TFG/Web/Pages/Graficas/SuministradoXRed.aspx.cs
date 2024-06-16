using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages.Graficas
{
    public partial class SuministradoXRed : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Obtenemos parametro
                String ubicacionId = Request.Params.Get("idUbicacion");
                txtFecha.Text = Request.Params.Get("fechaIni");
                txtFecha2.Text = Request.Params.Get("fechaFin");
                ddlListaCriterios.Text = Request.Params.Get("criterio");


                // obtenemos el servicio Ubicacion
                IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

                Ubicacion ubicacion = serviceUbicacion.buscarUbicacionById(Convert.ToInt64(ubicacionId));

                // ponemos el nombre de la ubicacion
                lblNombreUbicacion.Text = ubicacion.etiqueta;

                // ocultamos los calendarios
                Calendar1.Visible = false;
                Calendar2.Visible = false;

                // Desplegable con los criterios
                string idioma = SessionManager.GetUserSession(Context).Idioma;
                this.ddlListaCriterios.DataSource = Statistics.GetStatistics(idioma);
                this.ddlListaCriterios.DataBind();
            }
        }

        protected void btnCalendario_Click(object sender, EventArgs e)
        {
            // Muestra u oculta el calendario
            Calendar1.Visible = !Calendar1.Visible;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            txtFecha.Text = Calendar1.SelectedDate.ToShortDateString();
            Calendar1.Visible = !Calendar1.Visible;

        }

        protected void ddlListaCriterios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnBuscar2_Click(object sender, EventArgs e)
        {
            // obtenemso el datetime de los calendarios

            if (txtFecha.Text != "")
            {
                string format = "dd/mm/yyyy";

                DateTime dateTime = DateTime.ParseExact(txtFecha.Text, format, CultureInfo.InvariantCulture);

            }

            if (txtFecha2.Text != "")
            {
                string format = "dd/mm/yyyy";

                DateTime dateTime = DateTime.ParseExact(txtFecha2.Text, format, CultureInfo.InvariantCulture);
            }

            //Obtenemos parametro
            String ubicacionId = Request.Params.Get("idUbicacion");

            //Response.Redirect(Response.
            //    ApplyAppPathModifier("~/Pages/Graficas/WebForm1.aspx?idUbicacion=" + ubicacionId));
            if (ddlListaCriterios.Text == "Electricity consumption" || ddlListaCriterios.Text == "Consumo")
            {
                String url = String.Format("~/Pages/Graficas/WebForm1.aspx?idUbicacion={0}" +
                     "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            if (ddlListaCriterios.Text == "Supplied by the Network" || ddlListaCriterios.Text == "Suministrado por la Red")
            {
                String url = String.Format("~/Pages/Graficas/SuministradoXRed.aspx?idUbicacion={0}" +
                     "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }
        }

        protected void btnCalendario2_Click(object sender, EventArgs e)
        {
            // Muestra u oculta el calendario
            Calendar2.Visible = !Calendar2.Visible;
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            txtFecha2.Text = Calendar2.SelectedDate.ToShortDateString();
            Calendar2.Visible = !Calendar2.Visible;
        }



        protected string obtenerDatosXRed()
        {

            //Obtenemos parametro
            String ubicacionId = Request.Params.Get("idUbicacion");

            // obtenemos el servicio Ubicacion
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

            Ubicacion ubicacion = serviceUbicacion.buscarUbicacionById(Convert.ToInt64(ubicacionId));

            //string format = "dd/mm/yyyy";

            //DateTime fechaIni = DateTime.ParseExact(txtFecha.Text, format, CultureInfo.InvariantCulture).Date;

            //DateTime fechaFin = DateTime.ParseExact(txtFecha2.Text, format, CultureInfo.InvariantCulture).Date;

            DateTime fechaIni = Convert.ToDateTime(txtFecha.Text);

            DateTime fechaFin = Convert.ToDateTime(txtFecha2.Text);


            List<ConsumoPorDias> consumoRedDias = serviceUbicacion.MostrarConsumosRedElectricaUbicacionPorFechaEnDias(ubicacion.ubicacionId, fechaIni, fechaFin);

            Trace.Warn("consumoRedDias", consumoRedDias.Count().ToString());

            Trace.Warn("consumoRedDias", consumoRedDias[0].Count.ToString());
            foreach (ConsumoPorDias dr in consumoRedDias)
            {
                string f = (dr.fecha).ToString();
                string fecha = f.Substring(0, f.IndexOf(" "));
                Trace.Warn("fecha", fecha); Trace.Warn("fecha", Math.Truncate(dr.Count*1000).ToString());
            }

            string strDatos;

            strDatos = "[['Dia','(W)'],";

            foreach (ConsumoPorDias dr in consumoRedDias)
            {
                string f = (dr.fecha).ToString();
                string fecha = f.Substring(0, f.IndexOf(" "));

                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + fecha + "'" + "," + Math.Truncate(dr.Count*1000); ;
                strDatos = strDatos + "],";
            }

            strDatos = strDatos + "]";

            return strDatos;
        }
    }
}