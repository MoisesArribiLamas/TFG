using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Timers;

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

                IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();
                IServiceBateria serviceBateria = iocManager.Resolve<IServiceBateria>();
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

                // creamos la cabecera donde ponemos las ubicaciones

                // obtenemos el USUARIO
                long idUser = SessionManager.GetUserSession(Context).UserProfileId;

                
                // ubicaciones del usuario
                List<UbicacionProfileDetails> ubicaciones = serviceUbicacion.ubicacionesDelUsuario(idUser);

                // desplegable con las ubicaciones 
                foreach (UbicacionProfileDetails u in ubicaciones)
                {
                    this.ListaUbicaciones.Items.Add(u.etiqueta);
                }

                // obtenemos la ubicacion que se muestra en el desplegble 
                Ubicacion ubicacion = serviceUbicacion.primeraUbicacionDelUsuario(idUser);

                //long ubicacionId = Convert.ToInt64(ubicacionMorstrada);
                hlUbicacion.NavigateUrl = "~/Pages/Ubicaciones/ModificarUbicacion.aspx?idUbicacion=" + ubicacion.ubicacionId;

                // El consumo que tiene la ubicacion en este instante
                double cons = serviceUbicacion.consumoEnEsteInstante(ubicacion.ubicacionId);
                this.lblConsumo.Text = cons.ToString();

                // Tarifa actual
                DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan horaActual = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                int horaTarifa = horaActual.Hours;
                TarifaDTO tarifa = serviceTarifa.TarifaActual(fechaActual, horaTarifa);

                this.lblPrecioActual.Text = Convert.ToString(tarifa.precio);

                if (tarifa.precio < verdeAmarillo)
                {
                    this.lblPrecioActual.BackColor = ColorTranslator.FromHtml("#07DC10");

                }
                else
                {
                    if (tarifa.precio < amarilloRojo)
                    {
                        this.lblPrecioActual.BackColor = Color.Yellow;
                    }
                    else
                    {
                        this.lblPrecioActual.BackColor = ColorTranslator.FromHtml("#FE2E2E");
                    }
                }

                // Bateria principal de la ubicacion
                if (ubicacion.bateriaSuministradora != null)
                {
                    Bateria bateria = serviceBateria.BuscarBateriaById((long)ubicacion.bateriaSuministradora);
                    this.hlsuministrador.Text = bateria.nSerie;
                    this.hlsuministrador.NavigateUrl = "~/Pages/Baterias/ModificarBateria.aspx?idBateria=" + bateria.bateriaId;

                    // estado de la Bateria
                    string estado = serviceBateria.EstadoDeLaBateria((long)ubicacion.bateriaSuministradora);
                    this.lblEstado.Text = estado;
                    string idioma = SessionManager.GetUserSession(Context).Idioma;

                    if (idioma == "es") // castellano
                    {
                        this.lblEstado.Text = estado;


                    }
                    else if (idioma == "gl") // gallego
                    {
                        if (estado == "sin actividad")
                        {
                            this.lblEstado.Text = "sen actividade";
                        }
                        else
                        {
                            if (estado == "carga y suministra")
                            {
                                this.lblEstado.Text = "carga e suministra";
                            }
                            else // cargando , Suministrando
                            {
                                this.lblEstado.Text = estado;
                            }
                        }
                    }
                    else // (idioma == "en") ingles
                    {
                        if (estado == "sin actividad")
                        {
                            this.lblEstado.Text = "Without activity";
                        }
                        else
                        {
                            if (estado == "carga y suministra")
                            {
                                this.lblEstado.Text = "loads and supplies";
                            }
                            else
                            {
                                if (estado == "cargando")
                                {
                                    this.lblEstado.Text = "charging";
                                }
                                else
                                {
                                    if (estado == "suministrando")
                                    {
                                        this.lblEstado.Text = "supplying";
                                    }
                                }
                            } 
                        }

                    }

                    this.BoxRatioCompra.Text = bateria.ratioCompra.ToString();
                    this.BoxRatioCarga.Text = bateria.ratioCarga.ToString();
                    this.BoxRatioUso.Text = bateria.ratioUso.ToString();
                }
                else
                {
                    if (cons == 0)
                    {   // Sin Bateria suministradora y sin consumo
                        Localize1Suministrando.Visible = false;
                        hlsuministrador.Visible = false;
                    }
                    else
                    { // Sin Bateria suministradora y con consumo Mains

                        string idioma = SessionManager.GetUserSession(Context).Idioma;

                        if (idioma == "es") // castellano
                        {
                            hlsuministrador.Text = "Red Electrica";
                            

                        }
                        else if (idioma == "gl") // gallego
                        {
                            hlsuministrador.Text = "Red Electrica";
                        }
                        else // (idioma == "en") ingles
                        {
                            hlsuministrador.Text = "Mains";

                        }
                        
                    }

                }
                //var time = new System.Threading.Timer(obj => 
                //{ this.lblhora.Text = DateTime.Now.ToLongTimeString(); },null,TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                
                //Reloj();

            }

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ListaUbicaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            string etiqueta = ListaUbicaciones.SelectedValue;

            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];

            IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();
            IServiceBateria serviceBateria = iocManager.Resolve<IServiceBateria>();


            // obtenemos la ubicacion seleccionada
            Ubicacion ubicacion = serviceUbicacion.buscarUbicacionByNombre(etiqueta);

            // Bateria principal de la ubicacion
            if (ubicacion.bateriaSuministradora != null)
            {
                Localize1Suministrando.Visible = true;
                hlsuministrador.Visible = true;
                Localize1EstadoBateria.Visible = true;
                lblEstado.Visible = true;
                Localize1ratioCompra.Visible = true;
                BoxRatioCompra.Visible = true;
                Localize1ratioCarga.Visible = true;
                BoxRatioCarga.Visible = true;
                Localize1ratioUso.Visible = true;
                BoxRatioUso.Visible = true;
                btModificarRatios.Visible = true;

                Bateria bateria = serviceBateria.BuscarBateriaById((long)ubicacion.bateriaSuministradora);
                this.hlsuministrador.Text = bateria.nSerie;
                this.hlsuministrador.NavigateUrl = "~/Pages/Baterias/ModificarBateria.aspx?idBateria=" + bateria.bateriaId;


                // estado de la Bateria
                string estado = serviceBateria.EstadoDeLaBateria((long)ubicacion.bateriaSuministradora);
                this.lblEstado.Text = estado;
                this.BoxRatioCompra.Text = bateria.ratioCompra.ToString();
                this.BoxRatioCarga.Text = bateria.ratioCarga.ToString();
                this.BoxRatioUso.Text = bateria.ratioUso.ToString();
            }
            else
            {
                // El consumo que tiene la ubicacion en este instante
                double cons = serviceUbicacion.consumoEnEsteInstante(ubicacion.ubicacionId);
                this.lblConsumo.Text = cons.ToString();

                if (cons == 0)
                {   // Sin Bateria suministradora y sin consumo
                    Localize1Suministrando.Visible = false;
                    hlsuministrador.Visible = false;
                }
                else
                { // Sin Bateria suministradora y con consumo Mains

                    string idioma = SessionManager.GetUserSession(Context).Idioma;

                    if (idioma == "es") // castellano
                    {
                        hlsuministrador.Text = "Red Electrica";


                    }
                    else if (idioma == "gl") // gallego
                    {
                        hlsuministrador.Text = "Red Electrica";
                    }
                    else // (idioma == "en") ingles
                    {
                        hlsuministrador.Text = "Mains";

                    }

                }

                Localize1EstadoBateria.Visible = false;
                lblEstado.Visible = false;
                Localize1ratioCompra.Visible = false;
                BoxRatioCompra.Visible = false;
                Localize1ratioCarga.Visible = false;
                BoxRatioCarga.Visible = false;
                Localize1ratioUso.Visible = false;
                BoxRatioUso.Visible = false;
                btModificarRatios.Visible = false;
            }
        }

        protected void btModificarRatios_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    lblErrorModificarRatios.Visible = false;

                    IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];

                    IServiceControlador servicioControlador = iocManager.Resolve<IServiceControlador>();
                    IServiceBateria serviceBateria = iocManager.Resolve<IServiceBateria>();
                    IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

                    // obtenemos la etiqueta de la ubicacion que esta seleccionada
                    string etiqueta = ListaUbicaciones.SelectedValue;

                    // obtenemos la ubicacion seleccionada
                    Ubicacion ubicacion = serviceUbicacion.buscarUbicacionByNombre(etiqueta);

                    Bateria bateria = serviceBateria.BuscarBateriaById((long)ubicacion.bateriaSuministradora);

                    // Obtenemos el id de la bateria
                    long idBateria = bateria.bateriaId;

                    double rCarga = Convert.ToDouble(BoxRatioCarga.Text);

                    if (rCarga > 100 || rCarga < 10)
                    {
                        throw new FormatException();
                    }
                    // Cambiamos de forma manual los ratios
                    servicioControlador.cambiarRatiosBateria(idBateria, Convert.ToDouble(BoxRatioCarga.Text), Convert.ToDouble(BoxRatioCompra.Text), Convert.ToDouble(BoxRatioUso.Text));

                    string idioma = SessionManager.GetUserSession(Context).Idioma;
                    String mensaje;
                    String operacion;

                    if (idioma == "es") // castellano
                    {
                        mensaje = "Ratios Modificados";
                        operacion = "Modificar Ratios";

                    }
                    else if (idioma == "gl") // gallego
                    {
                        mensaje = "Ratios Mdificados";
                        operacion = "Modificar Ratios";
                    }
                    else // (idioma == "en") ingles
                    {
                        mensaje = "Modified Ratios";
                        operacion = "Modify Ratios";

                    }

                    MessageBox.Show(mensaje, operacion, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (FormatException)
                {
                    lblErrorModificarRatios.Visible = true;
                }
            }
        }




        //public System.Timers.Timer timer = new System.Timers.Timer(200);
        //private void Reloj()
        //{
        //    timer.Enabled = true;
        //    timer.Elapsed += (s_, e_) => send();
        //    timer.AutoReset = true;
        //}

        //public void send()
        //{
        //    this.lblhora.Text = DateTime.Now.ToLongTimeString();
        //}
    }
}