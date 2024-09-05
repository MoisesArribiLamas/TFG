using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Es.Udc.DotNet.TFG.Web
{
    /// <summary>
    /// Descripción breve de ServicioWeb
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ServicioWeb : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hola a todos";
        //}

        [WebMethod]
        public void ModificarConsumo(double consumo, string etiqueta)
        {
            // obtenemos el servicio Ubicacion
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion serviceUbicacion = iocManager.Resolve<IServiceUbicacion>();

            // obtenemos la ubicacion
            Ubicacion ubicacion = serviceUbicacion.buscarUbicacionByNombre(etiqueta);

            //modificamos el consumo
            serviceUbicacion.modificarConsumoActual(ubicacion.ubicacionId, consumo);


        }



        [WebMethod]
        public List<string> MostrarUsuarios()
        {
            // obtenemos el servicio Usuario
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUsuario serviceUsuario = iocManager.Resolve<IServiceUsuario>();

            List<Usuario> usuarios = serviceUsuario.ListaUsuarios();
            List<string> users = new List<string>();

            foreach (Usuario b in usuarios)
            {
                users.Add(b.email);
            
            }

            return users;

        }
        

        [WebMethod]
        public List<string> MostrarUbicacionesDelUsuario(string email)
        {
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion IServiceUbicacion = iocManager.Resolve<IServiceUbicacion>();
            IServiceUsuario serviceUsuario = iocManager.Resolve<IServiceUsuario>();

            // obtenemos el servicio Usuario
            long idUsuario = serviceUsuario.IdUsuarioPorEmail(email);

            List<UbicacionProfileDetails> ubicaciones = IServiceUbicacion.ubicacionesDelUsuario(idUsuario);

            List<string> locations = new List<string>();

            foreach (UbicacionProfileDetails l in ubicaciones)
            {
                
                locations.Add(l.etiqueta);

            }


            return locations;

        }

        //consumoEnEsteInstante

        [WebMethod]
        public double ConsumoEnEsteInstante(string etiqueta)
        {
            // obtenemos el servicio
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion IServiceUbicacion = iocManager.Resolve<IServiceUbicacion>();

            // obtenemos la ubicacion
            Ubicacion ubicacion = IServiceUbicacion.buscarUbicacionByNombre(etiqueta);
            
            return IServiceUbicacion.consumoEnEsteInstante(ubicacion.ubicacionId);

        }
    }
}
