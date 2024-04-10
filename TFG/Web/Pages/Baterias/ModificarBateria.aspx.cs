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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class ModificarBateria : SpecificCulturePage
    {
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
                String bateriaId = Request.Params.Get("idBateria");


                // obtenemos el servicio Bateria
                IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IServiceBateria servicioBateria = iocManager.Resolve<IServiceBateria>();

                Bateria bateria = servicioBateria.BuscarBateriaById(Convert.ToInt64(bateriaId));
                string estado = servicioBateria.EstadoDeLaBateria(Convert.ToInt64(bateriaId));

                BoxNSerieModificarBateria.Text = bateria.nSerie;
                BoxMarcaModificarBateria.Text = bateria.marca;
                BoxCapacidadCargadorModificarBateria.Text = bateria.capacidadCargador.ToString();
                BoxModeloModificarBateria.Text = bateria.modelo.ToString();
                BoxRatioCompra.Text = bateria.ratioCompra.ToString();
                BoxRatioCarga.Text = bateria.ratioCarga.ToString();
                BoxRatioUso.Text = bateria.ratioUso.ToString();
                lblAlmacenajeMaximoN.Text = bateria.almacenajeMaximoKwH.ToString();
                lblKwAlmacenadosNumero.Text = bateria.kwHAlmacenados.ToString();
                lblPrecioMedioNumero.Text = bateria.precioMedio.ToString();

                string idioma = SessionManager.GetUserSession(Context).Idioma;
                lblEstado.Text = MostrarEstadoIdioma(estado, idioma);

                // buscamos la ubicacion
                IServiceUbicacion servicioUbicacion = iocManager.Resolve<IServiceUbicacion>();
                Ubicacion ubicacion = servicioUbicacion.buscarUbicacionById(bateria.ubicacionId);
                lblValorBateriaSuministradora.Text = BateriaSuministradora(ubicacion, idioma, bateria.bateriaId);
            }
        }




        protected void btModificar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                    IServiceBateria servicioBateria = iocManager.Resolve<IServiceBateria>();

                    // Obtenemos el id de la ubicacion por parametro
                    long idBateria = Int32.Parse(Request.Params.Get("idBateria"));

                    servicioBateria.ModificarBateria(idBateria, null, null, null, null, null, null, BoxMarcaModificarBateria.Text,
                        BoxModeloModificarBateria.Text, null, null, null, Convert.ToDouble(BoxCapacidadCargadorModificarBateria.Text),
                        BoxNSerieModificarBateria.Text);

                    string idioma = SessionManager.GetUserSession(Context).Idioma;
                    String mensaje;
                    String operacion;

                    if (idioma == "es") // castellano
                    {
                        mensaje = "Modificado con éxito";
                        operacion = "Modificar";

                    }
                    else if (idioma == "gl") // gallego
                    {
                        mensaje = "Modificado con éxito";
                        operacion = "Modificar";
                    }
                    else // (idioma == "en") ingles
                    {
                        mensaje = "successfully modified";
                        operacion = "Modify ";

                    }

                    MessageBox.Show(mensaje, operacion, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Response.Redirect(Response.
                    //    ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));
                }
                catch (FormatException)
                {
                    lblErrorModificarBateria.Visible = true;
                }
            }
        }

        protected string MostrarEstadoIdioma(string estado, string idioma)
        {

            if (idioma == "es") // castellano
            {
                if (estado == "carga y suministra")
                {
                    estado = "Cargando y Suministrando";
                }
                return estado;
            }
            else if (idioma == "gl") // gallego
                {
                if (estado == "sin actividad")
                {
                    estado = "Sen Actividade";
                }

                if (estado == "carga y suministra")
                {
                    estado = "Cargando e Suministrando";
                }
                return estado;
            }
            else // (idioma == "en") ingles
            {
                if (estado == "sin actividad")
                {
                    estado = "No Activity";
                }

                if (estado == "cargando")
                {
                    estado = "Loading";
                }

                if (estado == "suministrando")
                {
                    estado = "Supplying";
                }

                if (estado == "carga y suministra")
                {
                    estado = "Loading & Supplying";
                }
                return estado;

            }
        }

        protected string BateriaSuministradora(Ubicacion ubicacion, string idioma, long bateriaId)
        {
            long? bSuministradora = ubicacion.bateriaSuministradora;
            bool isSupplyingBattery = (bSuministradora == bateriaId);
            if (idioma == "es") // castellano
            {
                if (isSupplyingBattery)
                {
                    return "SUMINISTRADORA";
                } else
                {
                 return "NO SUMINISTRADORA";
                }
               
            }
            else if (idioma == "gl") // gallego
            {
                if (isSupplyingBattery)
                {
                    return "SUMINISTRADORA";
                }
                else
                {
                    return "NON SUMINISTRADORA";
                }
            }
            else // (idioma == "en") ingles
            {
                if (isSupplyingBattery)
                {
                    return "SUPPLYING";
                }
                else
                {
                    return "NON-SUPPLYING";
                }

            }
        }

        protected void btModificarRatios_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                    IServiceControlador servicioControlador = iocManager.Resolve<IServiceControlador>();

                    // Obtenemos el id de la ubicacion por parametro
                    long idBateria = Int32.Parse(Request.Params.Get("idBateria"));

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

                    MessageBox.Show(mensaje,operacion,MessageBoxButtons.OK,MessageBoxIcon.Information);

      
                    //Response.Redirect(Response.
                    //    ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));
                }
                catch (FormatException)
                {
                    lblErrorModificarRatios.Visible = true;
                }
            }
        }

        protected void btnEliminarBaterias_Click(object sender, EventArgs e)
        {
            //comprobamos si es la bateria suministradora

            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceBateria servicioBateria = iocManager.Resolve<IServiceBateria>();

            //Obtenemos parametro
            String bateriaId = Request.Params.Get("idBateria");

            // obtenemos la bateria
            long IdBateria = Convert.ToInt64(bateriaId);
            Bateria bateria = servicioBateria.BuscarBateriaById(IdBateria);
           
            // buscamos la ubicacion
            IServiceUbicacion servicioUbicacion = iocManager.Resolve<IServiceUbicacion>();
            Ubicacion ubicacion = servicioUbicacion.buscarUbicacionById(bateria.ubicacionId);

            //obtenemos la bateria suministradora
            long? bSuministradora = ubicacion.bateriaSuministradora;

            string idioma = SessionManager.GetUserSession(Context).Idioma;
            String mensaje;
            String operacion;

            // si no es la bateria suministradora, se puede eliminar
            if (bSuministradora != bateria.bateriaId)
            {
                

                if (idioma == "es") // castellano
                {
                    mensaje = "¿Está seguro de querer eliminar la bateria?";
                    operacion = "eliminar bateria";

                }
                else if (idioma == "gl") // gallego
                {
                    mensaje = "Está seguro de querer eliminar a bateria?";
                    operacion = "eliminar bateria";
                }
                else // (idioma == "en") ingles
                {
                    mensaje = "Are you sure you want to remove the battery?";
                    operacion = "Remove Battery";

                }

                DialogResult dR = MessageBox.Show(mensaje, operacion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dR== DialogResult.OK) { 
                    servicioBateria.EliminarBateria(IdBateria);

                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/Baterias/BateriasPage.aspx"));
                }
            }
            else // no se puede eliminar la batería suministradora
            { 

                if (idioma == "es") // castellano
                {
                    mensaje = "No se puede eliminar la bateria suministradora";
                    operacion = "Eliminar Batería";

                }
                else if (idioma == "gl") // gallego
                {
                    mensaje = "A bateria suministradora non se pode eliminar";
                    operacion = "Eliminar Batería";
                }
                else // (idioma == "en") ingles
                {
                    mensaje = "Cannot remove the supply battery";
                    operacion = "Remove Battery";

                }

                MessageBox.Show(mensaje, operacion, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    }