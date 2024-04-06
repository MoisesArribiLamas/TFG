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
                this.ListaBateriasUbicacion.Items.Insert(0, " -- NO -- ");
            }
            foreach (BateriaDTO b in bateriasDTO)
            {
                this.ListaBateriasUbicacion.Items.Add(b.nSerie);
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

                    //obtenemos el id de la bateria suministradora
                    long batSum = serviceBateria.getBateriaIdByNSerie(ListaBateriasUbicacion.Text);

                    serviceUbicacion.modificarUbicacion(idUbicacion, Convert.ToInt64(BoxCodigoPostalCrearUbicacion.Text), BoxLocalidadCrearUbicacion.Text, BoxCalleCrearUbicacion.Text, BoxPortalCrearUbicacion.Text, Convert.ToInt64(BoxNumeroCrearUbicacion.Text), BoxEtiquetaCrearUbicacion.Text);

                    Trace.Warn("Bateria Suministradora", Convert.ToString(batSum));

                    serviceControlador.CambiarBateriaSuministradora(idUbicacion, batSum);
                    //CambiarBateriaSuministradora(long ubicacionId, long? bateriaSuministradora);
                    //long getBateriaIdByNSerie(string nserie)
                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));
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
    }
}