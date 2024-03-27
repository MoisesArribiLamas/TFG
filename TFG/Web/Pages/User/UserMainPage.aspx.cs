using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class UserMainPage : SpecificCulturePage
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            //ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }
            if (!IsPostBack)
            {
                IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IServiceControlador serviceControlador = iocManager.Resolve<IServiceControlador>();
                List<TarifaDetails> tarifasHoy = serviceControlador.TarifasDeHoy();

                DateTime fecha = System.DateTime.Today;
                string año = fecha.Year.ToString();
                string mes = fecha.Month.ToString();
                string dia = fecha.Day.ToString();
                Locale locale = SessionManager.GetLocale(Context);
                
                //ponemos la fecha en el formato del pais
                if (locale.Country != "US") {

                    lblFechaTarifas.Text = dia + "/"+mes + "/" +año;// españa galicia reino unido
                        
                }
                else // USA
                {
                    lblFechaTarifas.Text = mes + "/" + dia + "/" + año;
                }

                //obtenemos el precio maximo medio y minimo
                IServiceTarifa serviceTarifa = iocManager.Resolve<IServiceTarifa>();

                TarifaDTO mejorTarifa = serviceTarifa.BuscarMejorTarifa(fecha);
                TarifaDTO peorTarifa = serviceTarifa.BuscarpeorTarifa(fecha); 
                double mediaTarifas = serviceTarifa.PrecioMedioTarifasHoy();

                lblInicioAveragePricePrecio.Text = mediaTarifas.ToString("N5") + " €/kWh";
                lblInicioHighestPricePrecio.Text = peorTarifa.precio.ToString() + " €/kWh";
                lblInicioLowestPricePrecio.Text = mejorTarifa.precio.ToString()+ " €/kWh";

                long hora = mejorTarifa.hora;
                long horaSiguiente = hora + 1;
                lblInicioLowestPriceHora.Text = (hora.ToString() + "-" + horaSiguiente.ToString() +"H");
                hora = peorTarifa.hora;
                horaSiguiente = hora + 1;
                lblInicioHighestPriceHora.Text = (hora.ToString() + "-" + horaSiguiente.ToString() + "H");

                // Mostramos las tarifas
                double verdeAmarillo = (mediaTarifas + mejorTarifa.precio) / 2; // Calculamos el limite de los dos colores
                double amarilloRojo = (mediaTarifas + peorTarifa.precio) / 2; // Calculamos el limite de los dos colores

                //Tabla de precios
                this.GridView1.DataSource = tarifasHoy;
                this.GridView1.DataBind();

                foreach (GridViewRow row in GridView1.Rows)
                {
                    if ((double)Convert.ToDouble(row.Cells[0].Text) < verdeAmarillo)
                    {
                        row.BackColor = Color.Green;
                        
                    }
                    else {
                        if ((double)Convert.ToDouble(row.Cells[0].Text) < amarilloRojo)
                        {
                            row.BackColor = Color.Yellow;
                        }
                        else
                        {
                            row.BackColor = Color.Red;
                        }
                    }   
                          
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}