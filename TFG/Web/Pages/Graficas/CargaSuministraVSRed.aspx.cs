using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Daos.AhorroDao;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
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
    public partial class CargaSuministraVSRed : System.Web.UI.Page
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

            

            DateTime fechaIni = Convert.ToDateTime(txtFecha.Text);

            DateTime fechaFin = Convert.ToDateTime(txtFecha2.Text);

            
            List<CargaSuministraYRed> cargasSuministrosYRed = serviceUbicacion.MostrarSuministradoCargadoYRedXUbicacionPorFechaEnDias(ubicacion.ubicacionId, fechaIni, fechaFin);


            string strDatos;

            strDatos = "";

            foreach (CargaSuministraYRed dr in cargasSuministrosYRed)
            {
                string f = (dr.fecha).ToString();
                string fecha = f.Substring(0, f.IndexOf(" "));

                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + fecha + "'" + "," + Math.Truncate(dr.Cargado * 1000);
                strDatos = strDatos + "," + Math.Truncate(dr.Suministrado * 1000);
                strDatos = strDatos + "," + Math.Truncate(dr.Red * 1000);
                strDatos = strDatos + "],";
            }

           

            strDatos = strDatos + "]";


            //string falso = "['2014', 1000, 400, 200],['2015', 1170, 460, 250],['2016', 660, 1120, 300],['2017', 1030, 540, 350]]";

            return strDatos;
        }
    }
}