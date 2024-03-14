using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Es.Udc.DotNet.ModelUtil.IoC;
using Ninject;
using System.Configuration;
using System.Data.Entity;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
using Es.Udc.DotNet.TFG.Model.Daos.CargaDao;
using Es.Udc.DotNet.TFG.Model.Daos.SuministraDao;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoBateriaDao;

namespace Es.Udc.DotNet.TFG.Web.HTTP.Util.IoC
{
    public class IoCManagerNinject : IIoCManager
    {
        private static IKernel kernel;
        private static NinjectSettings settings;

        public void Configure()
        {
            settings = new NinjectSettings() { LoadExtensions = true };
            kernel = new StandardKernel(settings);

            kernel.Bind<IEstadoDao>().To<EstadoDaoEntitFramework>(); 

            kernel.Bind<ITarifaDao>().To<TarifaEntityFramework>(); 

            kernel.Bind<IBateriaDao>().To<BateriaEntityFramework>(); 

            kernel.Bind<IUbicacionDao>().To<UbicacionEntityFramework>(); 

            kernel.Bind<IConsumoDao>().To<ConsumoEntityFramework>(); 

            kernel.Bind<ICargaDao>().To<CargaDaoEntitFramework>(); 

            kernel.Bind<ISuministraDao>().To<SuministraDaoEntitFramework>(); 

            kernel.Bind<IEstadoBateriaDao>().To<EstadoBateriaDaoEntitFramework>();

            kernel.Bind<IServiceUsuario>().To<ServiceUsuario>();

            kernel.Bind<IServiceControlador>().To<ServiceControlador>();

            kernel.Bind<IServiceBateria>().To<ServiceBateria>(); 

            kernel.Bind<IServiceUbicacion>().To<ServiceUbicacion>(); 

            kernel.Bind<IServiceEstado>().To<ServiceEstado>(); 

            kernel.Bind<IServiceTarifa>().To<ServiceTarifa>();

            kernel.Bind<IUsuarioDao>().To<UsuarioEntityFramework>();

            string connectionString =
                ConfigurationManager.ConnectionStrings["TFGEntities"].ConnectionString;

            kernel.Bind<DbContext>().
                ToSelf().
                InSingletonScope().
                WithConstructorArgument("nameOrConnectionString", connectionString);


        }
                     
        public T Resolve<T>()
        {
            return kernel.Get<T>();
        }
    }
}