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

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IUserPivotServices, UserPivotServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());

            // Not Mentioned in tutorial
            container.RegisterType<IUserTokensServices, UserTokensServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IApplicationServices, ApplicationServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IApplicationSettingServices, ApplicationSettingServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<ICountryServices, CountryServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<ILanguageServices, LanguageServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IMealPlanServices, MealPlanServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IMediaCategoryServices, MediaCategoryServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IMediaContentServices, MediaContentServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IMediaTypeServices, MediaTypeServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IMenuServices, MenuServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IMilestonePercentageServices, MilestonePercentageServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IMilestoneServices, MilestoneServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<INewsServices, NewsServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IOpenNotificationUserServices, OpenNotificationUserServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IPasswordHistoryServices, PasswordHistoryServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IPasswordRequestServices, PasswordRequestServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IProvinceServices, ProvinceServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<ITestimonialServices, TestimonialServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());            
            container.RegisterType<IUserMetaPivotServices, UserMetaPivotServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserMetaServices, UserMetaServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserMilestoneServices, UserMilestoneServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserNotificationServices, UserNotificationServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserPushTokenServices, UserPushTokenServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());            
            container.RegisterType<IUserServices, UserServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserSettingServices, UserSettingServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserTrackerPivotServices, UserTrackerPivotServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserTrackerRealtimeServices, UserTrackerRealtimeServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());            
            container.RegisterType<IUserTrackerServices, UserTrackerServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());            
            container.RegisterType<IUserTrackerReportServices, UserTrackerReportServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IWebUserMetaRealtimeServices, WebUserMetaRealtimeServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IWeightVolumeServices, WeightVolumeServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IYoutubeVideoServices, YoutubeVideoServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());

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