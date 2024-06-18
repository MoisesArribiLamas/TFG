using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.ModelUtil.Log;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class EstadisticasUbicacion : SpecificCulturePage
    {
        private static readonly ArrayList baterias = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }
            if (!IsPostBack)
            {
                //Obtenemos parametro
                String ubicacionId = Request.Params.Get("idUbicacion");

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
                Console.WriteLine(dateTime);
            }

            if (txtFecha2.Text != "")
            {
                string format = "dd/mm/yyyy";

                DateTime dateTime = DateTime.ParseExact(txtFecha2.Text, format, CultureInfo.InvariantCulture);
                Console.WriteLine(dateTime);
            }

            //Obtenemos parametro
            String ubicacionId = Request.Params.Get("idUbicacion");

   

            //SuministradoXRed
            if (ddlListaCriterios.Text == "Supplied by the Network" || ddlListaCriterios.Text == "Suministrado por la Red")
            {
                String url = String.Format("~/Pages/Graficas/SuministradoXRed.aspx?idUbicacion={0}" +
                     "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //SuministradoXBateria
            if (ddlListaCriterios.Text == "Supplied" || ddlListaCriterios.Text == "Suministrado" || ddlListaCriterios.Text == "Suministrou")
            {
                String url = String.Format("~/Pages/Graficas/SuministradoXBateria.aspx?idUbicacion={0}" +
                     "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //CargasDelSistema
            if (ddlListaCriterios.Text == "Loaded" || ddlListaCriterios.Text == "Cargados" || ddlListaCriterios.Text == "Cargou")
            {
                String url = String.Format("~/Pages/Graficas/CargasDelSistema.aspx?idUbicacion={0}" +
                     "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //AhorroXDias
            if (ddlListaCriterios.Text == "Saving money" || ddlListaCriterios.Text == "Ahorro" || ddlListaCriterios.Text == "Aforro")
            {
                String url = String.Format("~/Pages/Graficas/AhorroXDias.aspx?idUbicacion={0}" +
                     "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //CargaSuministraVSRed
            if (ddlListaCriterios.Text == "Network vs Loaded vs Supplied" || ddlListaCriterios.Text == "Red vs Suministrado Cargados" || ddlListaCriterios.Text == "Red vs Suministrou Cargou")
            {
                String url = String.Format("~/Pages/Graficas/CargaSuministraVSRed.aspx?idUbicacion={0}" +
                     "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }
        }

        protected void gvUbicacionesEstadisticas_RowCommand(Object sender, GridViewCommandEventArgs e)
        {



        }
        protected void gvUbicacionesPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }
    }
}