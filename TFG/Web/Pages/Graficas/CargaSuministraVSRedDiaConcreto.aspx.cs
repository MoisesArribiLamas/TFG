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
    public partial class CargaSuministraVSRedDiaConcreto : System.Web.UI.Page
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

            //ahorroDiaConcreto
            if (ddlListaCriterios2.Text == "Saving money" || ddlListaCriterios2.Text == "Ahorro" || ddlListaCriterios2.Text == "Aforro")
            {
                String url = String.Format("~/Pages/Graficas/ahorroDiaConcreto.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //CargaSuministraVSRedDiaConcreto
            if (ddlListaCriterios2.Text == "Network vs Loaded vs Supplied" || ddlListaCriterios2.Text == "Red vs Suministrado Cargados" || ddlListaCriterios2.Text == "Red vs Suministrou Cargou")
            {
                String url = String.Format("~/Pages/Graficas/CargaSuministraVSRed.aspx?idUbicacion={0}" +
                     "&fecha={1}"+ "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
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

            //ahorroDiaConcreto
            if (ddlListaCriterios2.Text == "Saving money" || ddlListaCriterios2.Text == "Ahorro" || ddlListaCriterios2.Text == "Aforro")
            {
                String url = String.Format("~/Pages/Graficas/ahorroDiaConcreto.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }

            //CargaSuministraVSRedDiaConcreto
            if (ddlListaCriterios2.Text == "Network vs Loaded vs Supplied" || ddlListaCriterios2.Text == "Red vs Suministrado Cargados" || ddlListaCriterios2.Text == "Red vs Suministrou Cargou")
            {
                String url = String.Format("~/Pages/Graficas/CargaSuministraVSRed.aspx?idUbicacion={0}" +
                     "&fecha={1}" + "&criterio={2}", ubicacionId, txtFecha3.Text, ddlListaCriterios2.Text);
                Response.Redirect(Response.ApplyAppPathModifier(url));
            }
        }
        protected string dia()
        {

            string idioma = SessionManager.GetUserSession(Context).Idioma;

            if (idioma == "es") // castellano
            {
                return "Día";


            }
            else if (idioma == "gl") // gallego
            {
                return "Día";
            }
            else // (idioma == "en") ingles
            {
                return "Day";

            }

        }
        protected string titulo()
        {

            string idioma = SessionManager.GetUserSession(Context).Idioma;

            if (idioma == "es") // castellano
            {
                return "Comparación de Watios";


            }
            else if (idioma == "gl") // gallego
            {
                return "Comparación de Watios";
            }
            else // (idioma == "en") ingles
            {
                return "Watt Comparison";

            }

        }

        protected string subtitulo()
        {

            string idioma = SessionManager.GetUserSession(Context).Idioma;

            if (idioma == "es") // castellano
            {
                return "consumidos y suministrados por el sistema y lo consumido directamente de la red";


            }
            else if (idioma == "gl") // gallego
            {
                return "W consumidos e suministrados polo sistema e o consumido directamente da rede";
            }
            else // (idioma == "en") ingles
            {
                return "W consumed and supplied by the system and consumed Mains";

            }

        }



        protected string carga()
        {

            string idioma = SessionManager.GetUserSession(Context).Idioma;

            if (idioma == "es") // castellano
            {
                return "Cargado";


            }
            else if (idioma == "gl") // gallego
            {
                return "cargado";
            }
            else // (idioma == "en") ingles
            {
                return "Loaded";

            }

        }

        protected string suministra()
        {

            string idioma = SessionManager.GetUserSession(Context).Idioma;

            if (idioma == "es") // castellano
            {
                return "Suministrado";


            }
            else if (idioma == "gl") // gallego
            {
                return "Suministrado";
            }
            else // (idioma == "en") ingles
            {
                return "supplied";

            }

        }

        protected string red()
        {

            string idioma = SessionManager.GetUserSession(Context).Idioma;

            if (idioma == "es") // castellano
            {
                return "Red eléctrica";


            }
            else if (idioma == "gl") // gallego
            {
                return "Rede eléctrica";
            }
            else // (idioma == "en") ingles
            {
                return "Mains";

            }

        }

        protected string suministradoCargadoyRed()
        {

            //Obtenemos parametro
            String ubicacionId = Request.Params.Get("idUbicacion");

            // obtenemos el servicio Ubicacion
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();
            IServiceBateria serviceBateria = iocManager.Resolve<IServiceBateria>();

            Ubicacion ubicacion = serviceUbicacion.buscarUbicacionById(Convert.ToInt64(ubicacionId));



            DateTime fecha = Convert.ToDateTime(txtFecha3.Text);

            


            List<CargaSuministraYRedDiaConcreto> cargasSuministrosYRed = serviceUbicacion.MostrarSuministradoCargadoYRedXUbicacionDiaConcreto(ubicacion.ubicacionId, fecha);


            string strDatos;

            strDatos = "";

            foreach (CargaSuministraYRedDiaConcreto dr in cargasSuministrosYRed)
            {
  

                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr.Hora + "H" + "'" + "," + Math.Truncate(dr.Cargado * 1000);
                strDatos = strDatos + "," + Math.Truncate(dr.Suministrado * 1000);
                strDatos = strDatos + "," + Math.Truncate(dr.Red * 1000);
                strDatos = strDatos + "],";
            }



            strDatos = strDatos + "]";

            return strDatos;
        }
    }
}