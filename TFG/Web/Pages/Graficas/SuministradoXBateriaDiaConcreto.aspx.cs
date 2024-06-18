using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.ModelUtil.Log;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
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

namespace Es.Udc.DotNet.TFG.Web.Pages.Graficas
{
    public partial class SuministradoXBateriaDiaConcreto : System.Web.UI.Page
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
                txtFecha3.Text = Request.Params.Get("fecha");
                ddlListaCriterios2.Text = Request.Params.Get("criterio");

                // obtenemos el servicio Ubicacion
                IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

                Ubicacion ubicacion = serviceUbicacion.buscarUbicacionById(Convert.ToInt64(ubicacionId));

                // ocultamos los calendarios
                Calendar3.Visible = false;

                // Desplegables con los criterios
                string idioma = SessionManager.GetUserSession(Context).Idioma;

                this.ddlListaCriterios2.DataSource = Statistics.GetStatistics(idioma);
                this.ddlListaCriterios2.DataBind();

            }



        }



        protected void btnCalendario3_Click(object sender, EventArgs e)
        {
            // Muestra u oculta el calendario
            Calendar3.Visible = !Calendar3.Visible;
        }

        protected void btnBuscar2_Click(object sender, EventArgs e)
        {

            //Obtenemos parametro
            String ubicacionId = Request.Params.Get("idUbicacion");



            //SuministradoRedDiaConcreto
            if (ddlListaCriterios2.Text == "Supplied by the Network" || ddlListaCriterios2.Text == "Suministrado por la Red")
            {
                String url = String.Format("~/Pages/Graficas/SuministradoRedDiaConcreto.aspx?idUbicacion={0}" +
                        "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //SuministradoXBateriaDiaConcreto
            if (ddlListaCriterios2.Text == "Supplied" || ddlListaCriterios2.Text == "Suministrado" || ddlListaCriterios2.Text == "Suministrou")
            {
                String url = String.Format("~/Pages/Graficas/SuministradoXBateriaDiaConcreto.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //CargasDelSistemaDiaConcreto
            if (ddlListaCriterios2.Text == "Loaded" || ddlListaCriterios2.Text == "Cargados" || ddlListaCriterios2.Text == "Cargou")
            {
                String url = String.Format("~/Pages/Graficas/CargasDelSistemaDiaConcreto.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //AhorroXDias
            //if (ddlListaCriterios.Text == "Saving money" || ddlListaCriterios.Text == "Ahorro" || ddlListaCriterios.Text == "Aforro")
            //{
            //    String url = String.Format("~/Pages/Graficas/AhorroXDias.aspx?idUbicacion={0}" +
            //         "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
            //    Response.Redirect(Response.ApplyAppPathModifier(url));
            //}

            //CargaSuministraVSRedDiaConcreto
            if (ddlListaCriterios2.Text == "Network vs Loaded vs Supplied" || ddlListaCriterios2.Text == "Red vs Suministrado Cargados" || ddlListaCriterios2.Text == "Red vs Suministrou Cargou")
            {
                String url = String.Format("~/Pages/Graficas/CargaSuministraVSRed.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

        }

        protected void Calendar3_SelectionChanged(object sender, EventArgs e)
        {
            txtFecha3.Text = Calendar3.SelectedDate.ToShortDateString();
            Calendar3.Visible = !Calendar3.Visible;
            // quitamos el error
            lblFecha3Error.Visible = false;
        }

        protected void ddlListaCriterios2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obtenemos parametro
            String ubicacionId = Request.Params.Get("idUbicacion");



            //SuministradoRedDiaConcreto
            if (ddlListaCriterios2.Text == "Supplied by the Network" || ddlListaCriterios2.Text == "Suministrado por la Red")
            {
                String url = String.Format("~/Pages/Graficas/SuministradoRedDiaConcreto.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //SuministradoXBateriaDiaConcreto
            if (ddlListaCriterios2.Text == "Supplied" || ddlListaCriterios2.Text == "Suministrado" || ddlListaCriterios2.Text == "Suministrou")
            {
                String url = String.Format("~/Pages/Graficas/SuministradoXBateriaDiaConcreto.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //CargasDelSistemaDiaConcreto
            if (ddlListaCriterios2.Text == "Loaded" || ddlListaCriterios2.Text == "Cargados" || ddlListaCriterios2.Text == "Cargou")
            {
                String url = String.Format("~/Pages/Graficas/CargasDelSistemaDiaConcreto.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //AhorroXDias
            //if (ddlListaCriterios.Text == "Saving money" || ddlListaCriterios.Text == "Ahorro" || ddlListaCriterios.Text == "Aforro")
            //{
            //    String url = String.Format("~/Pages/Graficas/AhorroXDias.aspx?idUbicacion={0}" +
            //         "&fechaIni={1}" + "&fechaFin={2}" + "&criterio={3}", ubicacionId, txtFecha.Text, txtFecha2.Text, ddlListaCriterios.Text);
            //    Response.Redirect(Response.ApplyAppPathModifier(url));
            //}

            //CargaSuministraVSRedDiaConcreto
            if (ddlListaCriterios2.Text == "Network vs Loaded vs Supplied" || ddlListaCriterios2.Text == "Red vs Suministrado Cargados" || ddlListaCriterios2.Text == "Red vs Suministrou Cargou")
            {
                String url = String.Format("~/Pages/Graficas/CargaSuministraVSRed.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }
        }

        protected string titulo()
        {

            string idioma = SessionManager.GetUserSession(Context).Idioma;

            if (idioma == "es") // castellano
            {
                return "Watios Suministrados por el sistema";


            }
            else if (idioma == "gl") // gallego
            {
                return "Watios Suministrados polo sistema";
            }
            else // (idioma == "en") ingles
            {
                return "Watts Supplied by the system";

            }

        }

        protected string obtenerDatos()
        {

            //Obtenemos parametro
            String ubicacionId = Request.Params.Get("idUbicacion");

            // obtenemos el servicio Ubicacion
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

            Ubicacion ubicacion = serviceUbicacion.buscarUbicacionById(Convert.ToInt64(ubicacionId));

            //Obtenemos el dia 
            DateTime fecha = Convert.ToDateTime(txtFecha3.Text);

            List<ConsumoDiaConcreto> suministradoSistema = serviceUbicacion.MostrarLoSuministradoPorElSistemaDiaConcreto(ubicacion.ubicacionId, fecha);


            string strDatos;

            strDatos = "[['Hora','(W)'],";

            foreach (ConsumoDiaConcreto dr in suministradoSistema)
            {

                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr.Hora + "H" + "'" + "," + Math.Truncate(dr.Count * 1000); ;
                strDatos = strDatos + "],";
            }

            strDatos = strDatos + "]";

            return strDatos;
        }
    }
}