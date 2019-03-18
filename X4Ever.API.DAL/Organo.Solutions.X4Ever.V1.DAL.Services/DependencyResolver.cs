namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    using Organo.Solutions.X4Ever.V1.DAL.Resolver;

    //using System.ComponentModel;
    using System.ComponentModel.Composition;

    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IApplicationServices, ApplicationServices>();
            registerComponent.RegisterType<IApplicationSettingServices, ApplicationSettingServices>();
            registerComponent.RegisterType<ICountryServices, CountryServices>();
            registerComponent.RegisterType<ILanguageServices, LanguageServices>();
            registerComponent.RegisterType<IMealPlanServices, MealPlanServices>();
            registerComponent.RegisterType<IMediaCategoryServices, MediaCategoryServices>();
            registerComponent.RegisterType<IMediaContentServices, MediaContentServices>();
            registerComponent.RegisterType<IMediaTypeServices, MediaTypeServices>();
            registerComponent.RegisterType<IMenuServices, MenuServices>();
            registerComponent.RegisterType<IMilestonePercentageServices, MilestonePercentageServices>();
            registerComponent.RegisterType<IMilestoneServices, MilestoneServices>();
            registerComponent.RegisterType<INewsServices, NewsServices>();
            registerComponent.RegisterType<IOpenNotificationUserServices, OpenNotificationUserServices>();
            registerComponent.RegisterType<IPasswordHistoryServices, PasswordHistoryServices>();
            registerComponent.RegisterType<IPasswordRequestServices, PasswordRequestServices>();
            registerComponent.RegisterType<IProvinceServices, ProvinceServices>();
            registerComponent.RegisterType<ITestimonialServices, TestimonialServices>();
            registerComponent.RegisterType<IUserMetaServices, UserMetaServices>();
            registerComponent.RegisterType<IUserMilestoneServices, UserMilestoneServices>();
            registerComponent.RegisterType<IUserNotificationServices, UserNotificationServices>();
            registerComponent.RegisterType<IUserPushTokenServices, UserPushTokenServices>();
            registerComponent.RegisterType<IUserServices, UserServices>();
            registerComponent.RegisterType<IUserSettingServices, UserSettingServices>();
            registerComponent.RegisterType<IUserTokensServices, UserTokensServices>();
            registerComponent.RegisterType<IUserTrackerRealtimeServices, UserTrackerRealtimeServices>();
            registerComponent.RegisterType<IUserTrackerServices, UserTrackerServices>();
            registerComponent.RegisterType<IUserTrackerReportServices, UserTrackerReportServices>();
            registerComponent.RegisterType<IWebUserMetaRealtimeServices, WebUserMetaRealtimeServices>();
            registerComponent.RegisterType<IWeightVolumeServices, WeightVolumeServices>();
            registerComponent.RegisterType<IYoutubeVideoServices, YoutubeVideoServices>();
        }
    }
}