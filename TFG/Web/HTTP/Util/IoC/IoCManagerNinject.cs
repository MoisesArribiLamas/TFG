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

            kernel.Bind<IServiceUsuario>().To<ServiceUsuario>();

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