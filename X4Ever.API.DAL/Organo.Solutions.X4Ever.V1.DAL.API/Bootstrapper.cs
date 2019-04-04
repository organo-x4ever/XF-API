using Microsoft.Practices.Unity;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using Organo.Solutions.X4Ever.V1.DAL.Resolver;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Web.Http;
using Unity.Mvc5;

namespace Organo.Solutions.X4Ever.V1.DAL.API
{
    public class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            System.Web.Mvc.DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // register dependency resolver for WebAPI RC
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here it is NOT necessary to register
            // your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IUserPivotServices, UserPivotServices>()
                .RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());

            // Not Mentioned in tutorial
            container.RegisterType<IUserTokensServices, UserTokensServices>()
                .RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            //Component initialization via MEF
            ComponentLoader.LoadContainer(container, ".\\bin", "Organo.Solutions.X4Ever.V1.DAL.API");
            ComponentLoader.LoadContainer(container, ".\\bin", "Organo.Solutions.X4Ever.V1.DAL.Services.dll");
            //ComponentLoader.LoadContainer(container, ".\\bin", "Organo.Solutions.X4Ever.Repository.dll");
            //ComponentLoader.LoadContainer(container, ".\\bin", "Organo.Solutions.X4Ever.Model.dll");
            //ComponentLoader.LoadContainer(container, ".\\bin", "Organo.Solutions.X4Ever.Web.Security.dll");
        }
    }
}