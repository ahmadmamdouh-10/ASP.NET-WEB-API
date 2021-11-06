using Autofac;
using Autofac.Integration.WebApi;
using Data;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace API.App_Start
{
    public class IoCConfiguration
    { //  IoCConfiguration.Configure()
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            // You can register controllers all at once using assembly scanning...
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
                .InstancePerRequest();
            builder.RegisterGeneric(typeof(ModelRepository<>))
                .As(typeof(IModelRepository<>)).InstancePerRequest(); ;
            builder.RegisterType<DBContextFactory>().As<IDBContextFactory>()
                 .InstancePerRequest();


            // ...or you can register individual controllers manually.
            //builder.RegisterType<userController>().InstancePerRequest();
            // builder.RegisterApiControllers(user, Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}