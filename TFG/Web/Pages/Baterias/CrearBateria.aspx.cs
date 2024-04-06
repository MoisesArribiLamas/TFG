using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.ModelUtil.Log;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
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

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class CrearBateria : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }

            


        }
        

        

        protected void btRegistrar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                    IServiceBateria serviceBateria = iocManager.Resolve<IServiceBateria>();

                    IServiceEstado ServicioEstado = iocManager.Resolve<IServiceEstado>();

                    // Usuario
                    long usuarioId = SessionManager.GetUserSession(Context).UserProfileId;

                    //fecha
                    DateTime fecha = System.DateTime.Today;
                    TimeSpan horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                    // Obtenemos el id de la ubicacion por parametro
                    long ubicacionId = Int32.Parse(Request.Params.Get("idUbicacion"));

                    //serviceBateria.crearBateria(Convert.ToInt64(BoxCodigoPostalCrearBateria.Text), BoxLocalidadCrearBateria.Text, BoxCalleCrearBateria.Text, BoxPortalCrearBateria.Text, Convert.ToInt64(BoxNumeroCrearBateria.Text), BoxEtiquetaCrearBateria.Text, idUser);
                    //        long CrearBateria(long ubicacionId, long usuarioId, double precioMedio, double kwHAlmacenados, double almacenajeMaximoKwH,
                    //DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso, double capacidadCargador);

                    //BoxPortalCrearUbicacion.Text = ubicacion.portal;
                    // Hacemos que  kwHAlmacenados = almacenajeMaximoKwH (suponiendo que está llena) dejamos la funcion así por si en el futuro se implementan mejoras.
                    // hacemos precioMedio = 0 , dejamos la funcionalidad asi por si en el futuro se hacen mejoras.

                    string marca = BoxMarcaCrearBateria.Text;
                    string modelo = BoxModeloCrearBateria.Text;
                    string nSerie = BoxNSerieCrearBateria.Text;
                    double almacenajeMaximoKwH = Convert.ToDouble(BoxAlmacenajeMaximoCrearBateria.Text);
                    double kwHAlmacenados = almacenajeMaximoKwH;
                    double capacidadCargador = Convert.ToDouble(BoxCapacidadCargadorCrearBateria.Text);
                    double precioMedio = 0;
                    double ratioCarga = Convert.ToDouble(BoxRatioCargaCrearBateria.Text);
                    double ratioCompra = Convert.ToDouble(BoxRatioCompraCrearBateria.Text);
                    double ratioUso = Convert.ToDouble(BoxRatioUsoCrearBateria.Text);

                    long bateriaId = serviceBateria.CrearBateria(ubicacionId, usuarioId, precioMedio, kwHAlmacenados, almacenajeMaximoKwH,
                     fecha, marca, modelo, nSerie, ratioCarga, ratioCompra, ratioUso, capacidadCargador);

                    //creamos el estadoBateria inicial
                    //long estadoBateriaId = ServicioEstado.CrearEstadoBateria(horaIni, fecha, bateriaId, ServicioEstado.BuscarEstadoPorNombre("sin actividad"));

                    //IniciarEstadoEnBateria(b.bateriaId, estadoBateriaId);

                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));
                }
                catch (DuplicateInstanceException)
                {
                    lblErrorCrearBateria.Visible = true;
                }
            }
        }
    }
}