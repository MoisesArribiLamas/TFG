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
    public partial class ModificarUbicacion : SpecificCulturePage
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

                BoxEtiquetaCrearUbicacion.Text = ubicacion.etiqueta;
                BoxLocalidadCrearUbicacion.Text = ubicacion.localidad;
                BoxCalleCrearUbicacion.Text = ubicacion.calle;
                BoxNumeroCrearUbicacion.Text = ubicacion.numero.ToString();
                BoxPortalCrearUbicacion.Text = ubicacion.portal;
                BoxCodigoPostalCrearUbicacion.Text = ubicacion.codigoPostal.ToString();


                //// Obtenemos el id de la ubicacion por parametro
                long idUbicacion = Int32.Parse(Request.Params.Get("idUbicacion"));

                List<BateriaDTO> bateriasDTO = serviceUbicacion.bateriasDeUnaUbicacion(idUbicacion);

                if (ubicacion.bateriaSuministradora != null) //hay bateria suministradora
                {
                    IServiceBateria serviceBateria = iocManager.Resolve<IServiceBateria>();
                    Bateria bSuministradora = serviceBateria.BuscarBateriaById((long)(ubicacion.bateriaSuministradora));

                    this.ListaBateriasUbicacion.Items.Insert(0, bSuministradora.nSerie);

                }
                else // no hay bateria suministradora
                {
                    this.ListaBateriasUbicacion.Items.Insert(0, "-- NO --");
                }
                foreach (BateriaDTO b in bateriasDTO)
                {
                    this.ListaBateriasUbicacion.Items.Add(b.nSerie);
                }

                
            }
        }




        protected void btModificar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                    IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();
                    IServiceControlador serviceControlador = iocManager.Resolve<IServiceControlador>();
                    IServiceBateria serviceBateria = iocManager.Resolve<IServiceBateria>();

                    // Obtenemos el id de la ubicacion por parametro
                    long idUbicacion = Int32.Parse(Request.Params.Get("idUbicacion"));

                    serviceUbicacion.modificarUbicacion(idUbicacion, Convert.ToInt64(BoxCodigoPostalCrearUbicacion.Text), BoxLocalidadCrearUbicacion.Text, BoxCalleCrearUbicacion.Text, BoxPortalCrearUbicacion.Text, Convert.ToInt64(BoxNumeroCrearUbicacion.Text), BoxEtiquetaCrearUbicacion.Text);
                   
                    if (ListaBateriasUbicacion.Text != "-- NO --")
                    { 
                        //obtenemos el id de la bateria suministradora
                        long batSum = serviceBateria.getBateriaIdByNSerie(ListaBateriasUbicacion.Text);

                        Trace.Warn("Bateria Suministradora", ListaBateriasUbicacion.Text);

                        serviceControlador.CambiarBateriaSuministradora(idUbicacion, batSum);
                    }

                    string idioma = SessionManager.GetUserSession(Context).Idioma;
                    String mensaje;
                    String operacion;

                    if (idioma == "es") // castellano
                    {
                        mensaje = "Ubicación Modificada";
                        operacion = "Modificar Ubicacion";

                    }
                    else if (idioma == "gl") // gallego
                    {
                        mensaje = "Ubicación Modificada";
                        operacion = "Modificar Ubicacion";
                    }
                    else // (idioma == "en") ingles
                    {
                        mensaje = "Modified Location";
                        operacion = "Modify Location";

                    }

                    MessageBox.Show(mensaje, operacion, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Response.Redirect(Response.
                    //    ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));

                }
                catch (DuplicateInstanceException)
                {
                    lblErrorModificarUbicacion.Visible = true;
                }
            }
        }

        protected void btCrearBateria_Click(object sender, EventArgs e)
        {

            // Obtenemos el id de la ubicacion por parametro
            long idUbicacion = Int32.Parse(Request.Params.Get("idUbicacion"));

            Response.Redirect(Response.
                ApplyAppPathModifier("~/Pages/Baterias/CrearBateria.aspx?idUbicacion="+ idUbicacion));
        }

        protected void ListaBateriasUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnEliminarUbicacion_Click(object sender, EventArgs e)
        {
            // comprobar si hay consumo

            //Obtenemos parametro
            String ubicacionId = Request.Params.Get("idUbicacion");
            long IdUbicacion = Convert.ToInt64(ubicacionId);

            // obtenemos el servicio Ubicacion
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

            // obtenemos la Ubicacion
            Ubicacion ubicacion = serviceUbicacion.buscarUbicacionById(IdUbicacion);

            long? consumoU = serviceUbicacion.UltimoConsumoEnUbicacion(IdUbicacion);

            string idioma = SessionManager.GetUserSession(Context).Idioma;
            String mensaje;
            String operacion;

            if (consumoU == null)
            { // No Hubo consumo

                if (idioma == "es") // castellano
                {
                    mensaje = "¿Está seguro de querer eliminar la Ubicación?";
                    operacion = "Eliminar Ubicación";

                }
                else if (idioma == "gl") // gallego
                {
                    mensaje = "Está seguro de querer eliminar a Ubicación?";
                    operacion = "Eliminar Ubicación";
                }
                else // (idioma == "en") ingles
                {
                    mensaje = "Are you sure you want to remove the Location?";
                    operacion = "Remove Location";

                }

                DialogResult dR = MessageBox.Show(mensaje, operacion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dR == DialogResult.OK)
                {
                    //eliminar ubicacion
                    serviceUbicacion.eliminarUbicacion(IdUbicacion);

                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/Ubicaciones/UbicacionesPage.aspx"));
                }
                
            }
            else
            {   // buscamos el ultimo consumo
                Consumo consumoUltimo = serviceUbicacion.buscarConsumoById((long)consumoU);
                if (consumoUltimo.consumoActual == 0)
                {
                    if (idioma == "es") // castellano
                    {
                        mensaje = "¿Está seguro de querer eliminar la Ubicación?";
                        operacion = "Eliminar Ubicación";

                    }
                    else if (idioma == "gl") // gallego
                    {
                        mensaje = "Está seguro de querer eliminar a Ubicación?";
                        operacion = "Eliminar Ubicación";
                    }
                    else // (idioma == "en") ingles
                    {
                        mensaje = "Are you sure you want to remove the Location?";
                        operacion = "Remove Location";

                    }

                    DialogResult dR = MessageBox.Show(mensaje, operacion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (dR == DialogResult.OK)
                    {
                        //eliminar ubicacion
                        serviceUbicacion.eliminarUbicacion(IdUbicacion);

                        Response.Redirect(Response.
                            ApplyAppPathModifier("~/Pages/Baterias/UbicacionesPage.aspx"));
                    }
                    
                }
                else
                {
                    if (idioma == "es") // castellano
                    {
                        mensaje = "No se puede eliminar una Ubicación que está consumiendo";
                        operacion = "Modificar Ubicacion";

                    }
                    else if (idioma == "gl") // gallego
                    {
                        mensaje = "No se pode eliminar una Ubicación que está consumindo";
                        operacion = "Modificar Ubicacion";
                    }
                    else // (idioma == "en") ingles
                    {
                        mensaje = "Cannot delete a Location that is consuming";
                        operacion = "Modify Location";

                    }

                    MessageBox.Show(mensaje, operacion, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        protected void BtnQuitarSuministradora_Click(object sender, EventArgs e)
        {

            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();
            IServiceControlador serviceControlador = iocManager.Resolve<IServiceControlador>();
            IServiceBateria serviceBateria = iocManager.Resolve<IServiceBateria>();

            // Obtenemos el id de la ubicacion por parametro
            long idUbicacion = Int32.Parse(Request.Params.Get("idUbicacion"));


            string idioma = SessionManager.GetUserSession(Context).Idioma;
            String mensaje;
            String operacion;

            if (ListaBateriasUbicacion.Text != "-- NO --")
            {
                // consumoId
                long? consumoU = serviceUbicacion.UltimoConsumoEnUbicacion(idUbicacion);
                

                // buscamos el ultimo consumo
                Consumo consumoUltimo = serviceUbicacion.buscarConsumoById((long)consumoU);
                if (consumoUltimo.consumoActual == 0)  // no hay consumo activo
                {
                    if (idioma == "es") // castellano
                    {
                        mensaje = "¿Está seguro de dejar la ubicación sin batería suministradora?";
                        operacion = "Ubicación sin batería suministradora";

                    }
                    else if (idioma == "gl") // gallego
                    {
                        mensaje = "Está seguro de deixar la ubicación sen batería suministradora?";
                        operacion = "Ubicación sen batería suministradora";
                    }
                    else // (idioma == "en") ingles
                    {
                        mensaje = "Are you sure to leave the location without a supply battery?";
                        operacion = "Location without supply battery";

                    }

                    DialogResult dR = MessageBox.Show(mensaje, operacion, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (dR == DialogResult.OK)
                    {
                        //eliminar ubicacion
                        
                        serviceControlador.CambiarBateriaSuministradora(idUbicacion, null);

                        ListaBateriasUbicacion.Text = "-- NO --";
                        //Response.Redirect(Response.
                        //    ApplyAppPathModifier("~/Pages/Ubicaciones/UbicacionesPage.aspx"));
                    }

                }
                else
                {
                    if (idioma == "es") // castellano
                    {
                        mensaje = "No se puede quitar, la Ubicación tiene consumo activo";
                        operacion = "Ubicación sin batería suministradora";

                    }
                    else if (idioma == "gl") // gallego
                    {
                        mensaje = "Non se pode quitar, a Ubicación ten consumo activo";
                        operacion = "Ubicación sen batería suministradora";
                    }
                    else // (idioma == "en") ingles
                    {
                        mensaje = "Cannot be removed, Location has active consumption";
                        operacion = "Location without supply battery";

                    }

                    MessageBox.Show(mensaje, operacion, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            else { // no hay bateria suministradora


                if (idioma == "es") // castellano
                {
                    mensaje = "La Ubicacion no tiene Batería Suministradora ";
                    operacion = "Ubicación sin batería suministradora";

                }
                else if (idioma == "gl") // gallego
                {
                    mensaje = "A Ubicacion carece de Batería Suministradora";
                    operacion = "Ubicación sen batería suministradora";
                }
                else // (idioma == "en") ingles
                {
                    mensaje = "The Location does not have a Supply Battery";
                    operacion = "Location without supply battery";

                }

                MessageBox.Show(mensaje, operacion, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}