using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Data.Entity;

namespace Organo.Solutions.X4Ever.V1.DAL.Repository
{
    public class X4EverContext : DbContext
    {
        public X4EverContext()
            : base()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<UserPasswordRequest> ForgotPasswordRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<UserMeta> UserMetas { get; set; }
        public DbSet<UserMetaHistory> UserMetaHistories { get; set; }
        public DbSet<UserPasswordHistory> UserSearchKeys { get; set; }
        public DbSet<UserTracker> UserTrackers { get; set; }
        public DbSet<UserTrackerDeleted> UserTrackerDeleted { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<UserMilestone> UserMilestones { get; set; }
        public DbSet<MediaCategory> MediaCategories { get; set; }
        public DbSet<MediaContent> MediaContents { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<CountryLanguage> CountryLanguages { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<WeightVolume> WeightVolumes { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<MealPlanOption> MealPlanOptions { get; set; }
        public DbSet<MealPlanOptionGrid> MealPlanOptionGrids { get; set; }
        public DbSet<MealPlanOptionList> MealPlanOptionLists { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<MilestonePercentage> MilestonePercentages { get; set; }
        public DbSet<UserPushToken> UserPushTokens { get; set; }
        public DbSet<ApplicationCountry> ApplicationCountries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<ApplicationMenu> ApplicationMenus { get; set; }

        public DbSet<YoutubeConfiguration> YoutubeConfigurations { get; set; }
        public DbSet<YoutubeVideoCollection> YoutubeVideoCollections { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<UserNotificationType> UserNotificationTypes { get; set; }

        public DbSet<OpenNotificationUser> OpenNotificationUsers { get; set; }
        public DbSet<OpenNotificationUserRegistration> OpenNotificationUserRegistrations { get; }
        public DbSet<OpenNotificationOnlyUser> OpenNotificationOnlyUsers { get; set; }


        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }

        public DbSet<UserTrackerReportV1> UserTrackerReportV1 { get; set; }
        public DbSet<UserTrackerDetailReportV2> UserTrackerDetailReportV2 { get; set; }

        public DbSet<UserNotificationView> UserNotificationViews { get; set; }
        public DbSet<UserNotificationSettingsView> UserNotificationSettingsViews { get; set; }

        public DbSet<UserTrackerRealtime> UserTrackerRealtimes { get; set; }

        public DbSet<UserMetaPivot> UserMetaPivots { get; set; }
        public DbSet<UserTrackerPivot> UserTrackerPivots { get; set; }
        public DbSet<WebUserMetaPivot> WebUserMetaPivots { get; set; }

        public DbSet<WebUserTrackerViewPivot> WebUserTrackerViewPivots { get; set; }

        public DbSet<UserNotificationSetting> UserNotificationSettings {get;set;}
    }
}