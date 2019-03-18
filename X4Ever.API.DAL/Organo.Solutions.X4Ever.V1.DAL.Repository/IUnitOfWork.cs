using Organo.Solutions.X4Ever.V1.DAL.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<UserMeta> UserMetaRepository { get; }
        IGenericRepository<UserMetaHistory> UserMetaHistoryRepository { get; }
        IGenericRepository<UserToken> UserTokenRepository { get; }
        IGenericRepository<UserTracker> UserTrackerRepository { get; }
        IGenericRepository<UserPasswordRequest> PasswordRequestRepository { get; }
        IGenericRepository<UserPasswordHistory> UserPasswordHistoryRepository { get; }
        IGenericRepository<Milestone> MilestoneRepository { get; }
        IGenericRepository<UserMilestone> UserMilestoneRepository { get; }
        IGenericRepository<MediaCategory> MediaCategoryRepository { get; }
        IGenericRepository<MediaContent> MediaContentRepository { get; }
        IGenericRepository<MediaType> MediaTypeRepository { get; }
        IGenericRepository<Testimonial> TestimonialRepository { get; }
        IGenericRepository<UserSetting> UserSettingRepository { get; }
        IGenericRepository<Country> CountryRepository { get; }
        IGenericRepository<Language> LanguageRepository { get; }
        IGenericRepository<CountryLanguage> CountryLanguageRepository { get; }
        IGenericRepository<Application> ApplicationRepository { get; }
        IGenericRepository<WeightVolume> WeightVolumeRepository { get; }

        IGenericRepository<MealPlan> MealPlanRepository { get; }
        IGenericRepository<MealPlanOption> MealPlanOptionRepository { get; }
        IGenericRepository<MealPlanOptionGrid> MealPlanOptionGridRepository { get; }
        IGenericRepository<MealPlanOptionList> MealPlanOptionListRepository { get; }
        IGenericRepository<News> NewsRepository { get; }
        IGenericRepository<MilestonePercentage> MilestonePercentageRepository { get; }
        IGenericRepository<UserPushToken> UserPushTokenRepository { get; }
        IGenericRepository<ApplicationCountry> ApplicationCountryRepository { get; }
        IGenericRepository<Province> ProvinceRepository { get; }

        IGenericRepository<Menu> MenuRepository { get; }
        IGenericRepository<ApplicationMenu> ApplicationMenuRepository { get; }

        IGenericRepository<YoutubeConfiguration> YoutubeConfigurationRepository { get; }
        IGenericRepository<YoutubeVideoCollection> YoutubeVideoCollectionRepository { get; }

        IGenericRepository<UserNotification> UserNotificationRepository { get; }
        IGenericRepository<UserNotificationType> UserNotificationTypeRepository { get; }

        IGenericRepository<OpenNotificationUser> OpenNotificationUserRepository { get; }

        IGenericRepository<ApplicationSetting> ApplicationSettingRepository { get; }
        IGenericRepository<UserTrackerReportV1> UserTrackerReportV1Repository { get; }

        IGenericRepository<UserTrackerDetailReportV2> UserTrackerDetailReportV2ReporRepository { get; }

        IGenericRepository<OpenNotificationUserRegistration> OpenNotificationUserRegistrationRepository { get; }
        IGenericRepository<OpenNotificationOnlyUser> OpenNotificationOnlyUserRepository { get; }

        IGenericRepository<UserNotificationView> UserNotificationViewRepository { get; }

        IGenericRepository<UserTrackerRealtime> UserTrackerRealtimeRepository { get; }

        IGenericRepository<UserMetaPivot> UserMetaPivotRepository { get; }
        IGenericRepository<UserTrackerPivot> UserTrackerPivotRepository { get; }
        IGenericRepository<WebUserMetaPivot> WebUserMetaPivotRepository { get; }
        IGenericRepository<WebUserTrackerViewPivot> WebUserTrackerViewPivotRepository { get; }
        bool Commit();
        Task<bool> CommitAsync();

        string LogFilePath { get; }
        List<string> OutputLines { get; set; }
    }
}