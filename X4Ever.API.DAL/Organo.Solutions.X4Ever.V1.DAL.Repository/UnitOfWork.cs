using System.Transactions;

namespace Organo.Solutions.X4Ever.V1.DAL.Repository
{
    using Organo.Solutions.X4Ever.V1.DAL.Helper;
    using Organo.Solutions.X4Ever.V1.DAL.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Threading.Tasks;

    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly X4EverContext _context;
        private readonly IHelper _helper;
        public string LogFilePath => _helper.GetAppSetting("uowErrorLogs");
        public List<string> OutputLines { get; set; }

        public UnitOfWork()
        {
            _context = new X4EverContext();
            _helper = new Helper();
            OutputLines = new List<string>();
        }

        private IGenericRepository<User> _userRepository;
        private IGenericRepository<UserMeta> _userMetaRepository;
        private IGenericRepository<UserMetaHistory> _userMetaHistoryRepository;
        private IGenericRepository<UserToken> _userTokenRepository;
        private IGenericRepository<UserPasswordRequest> _passwordRequestRepository;
        private IGenericRepository<UserTracker> _userTrackerRepository;
        private IGenericRepository<UserPasswordHistory> _userPasswordHistoryRepository;
        private IGenericRepository<Milestone> _milestoneRepository;
        private IGenericRepository<UserMilestone> _userMilestoneRepository;
        private IGenericRepository<MediaCategory> _mediaCategoryRepository;
        private IGenericRepository<MediaContent> _mediaContentRepository;
        private IGenericRepository<MediaType> _mediaTypeRepository;
        private IGenericRepository<Testimonial> _testimonialRepository;
        private IGenericRepository<UserSetting> _userSettingRepository;
        private IGenericRepository<Country> _countryRepository;
        private IGenericRepository<Language> _languageRepository;
        private IGenericRepository<CountryLanguage> _countryLanguageRepository;
        private IGenericRepository<Application> _applicationRepository;
        private IGenericRepository<WeightVolume> _weightVolumeRepository;
        private IGenericRepository<MealPlan> _mealPlanRepository;
        private IGenericRepository<MealPlanOption> _mealPlanOptionRepository;
        private IGenericRepository<MealPlanOptionGrid> _mealPlanOptionGridRepository;
        private IGenericRepository<MealPlanOptionList> _mealPlanOptionListRepository;
        private IGenericRepository<News> _newsRepository;
        private IGenericRepository<MilestonePercentage> _milestonePercentageRepository;
        private IGenericRepository<UserPushToken> _userPushTokenRepository;
        private IGenericRepository<ApplicationCountry> _applicationCountryRepository;
        private IGenericRepository<Province> _provinceRepository;
        private IGenericRepository<Menu> _menuRepository;
        private IGenericRepository<ApplicationMenu> _applicationMenuRepository;
        private IGenericRepository<YoutubeConfiguration> _youtubeConfigurationRepository;
        private IGenericRepository<YoutubeVideoCollection> _youtubeVideoCollectionRepository;
        private IGenericRepository<UserNotification> _userNotificationRepository;
        private IGenericRepository<UserNotificationType> _userNotificationTypeRepository;
        private IGenericRepository<OpenNotificationUser> _openNotificationUserRepository;
        private IGenericRepository<OpenNotificationUserRegistration> _openNotificationUserRegistrationRepository;
        private IGenericRepository<ApplicationSetting> _applicationSettingRepository;
        private IGenericRepository<UserTrackerReportV1> _userTrackerReportV1Repository;
        private IGenericRepository<UserTrackerDetailReportV2> _userTrackerDetailReportV2ReportRepository;
        private IGenericRepository<OpenNotificationOnlyUser> _openNotificationOnlyUserRepository;
        private IGenericRepository<UserNotificationView> _userNotificationViewRepository;
        private IGenericRepository<UserTrackerRealtime> _userTrackerRealtimeRepository;
        private IGenericRepository<UserMetaPivot> _userMetaPivotRepository;
        private IGenericRepository<UserTrackerPivot> _userTrackerPivotRepository;
        private IGenericRepository<WebUserMetaPivot> _webUserMetaPivotRepository;
        private IGenericRepository<WebUserTrackerViewPivot> _webUserTrackerViewPivotRepository;

        public IGenericRepository<User> UserRepository =>
            _userRepository ?? (_userRepository = new GenericRepository<User>(_context));

        public IGenericRepository<UserMeta> UserMetaRepository =>
            _userMetaRepository ?? (_userMetaRepository = new GenericRepository<UserMeta>(_context));

        public IGenericRepository<UserMetaHistory> UserMetaHistoryRepository =>
            _userMetaHistoryRepository ??
            (_userMetaHistoryRepository = new GenericRepository<UserMetaHistory>(_context));

        public IGenericRepository<UserToken> UserTokenRepository =>
            _userTokenRepository ?? (_userTokenRepository = new GenericRepository<UserToken>(_context));

        public IGenericRepository<UserPasswordRequest> PasswordRequestRepository =>
            _passwordRequestRepository ??
            (_passwordRequestRepository = new GenericRepository<UserPasswordRequest>(_context));

        public IGenericRepository<UserTracker> UserTrackerRepository =>
            _userTrackerRepository ?? (_userTrackerRepository = new GenericRepository<UserTracker>(_context));

        public IGenericRepository<UserPasswordHistory> UserPasswordHistoryRepository =>
            _userPasswordHistoryRepository ?? (_userPasswordHistoryRepository =
                new GenericRepository<UserPasswordHistory>(_context));

        public IGenericRepository<Milestone> MilestoneRepository =>
            _milestoneRepository ?? (_milestoneRepository = new GenericRepository<Milestone>(_context));

        public IGenericRepository<UserMilestone> UserMilestoneRepository =>
            _userMilestoneRepository ??
            (_userMilestoneRepository = new GenericRepository<UserMilestone>(_context));

        public IGenericRepository<MediaCategory> MediaCategoryRepository =>
            _mediaCategoryRepository ??
            (_mediaCategoryRepository = new GenericRepository<MediaCategory>(_context));

        public IGenericRepository<MediaContent> MediaContentRepository =>
            _mediaContentRepository ??
            (_mediaContentRepository = new GenericRepository<MediaContent>(_context));

        public IGenericRepository<MediaType> MediaTypeRepository =>
            _mediaTypeRepository ?? (_mediaTypeRepository = new GenericRepository<MediaType>(_context));

        public IGenericRepository<Testimonial> TestimonialRepository =>
            _testimonialRepository ?? (_testimonialRepository = new GenericRepository<Testimonial>(_context));

        public IGenericRepository<UserSetting> UserSettingRepository =>
            _userSettingRepository ?? (_userSettingRepository = new GenericRepository<UserSetting>(_context));

        public IGenericRepository<Country> CountryRepository =>
            _countryRepository ?? (_countryRepository = new GenericRepository<Country>(_context));

        public IGenericRepository<Language> LanguageRepository =>
            _languageRepository ?? (_languageRepository = new GenericRepository<Language>(_context));

        public IGenericRepository<CountryLanguage> CountryLanguageRepository =>
            _countryLanguageRepository ??
            (_countryLanguageRepository = new GenericRepository<CountryLanguage>(_context));

        public IGenericRepository<Application> ApplicationRepository =>
            _applicationRepository ?? (_applicationRepository = new GenericRepository<Application>(_context));

        public IGenericRepository<WeightVolume> WeightVolumeRepository =>
            _weightVolumeRepository ??
            (_weightVolumeRepository = new GenericRepository<WeightVolume>(_context));

        public IGenericRepository<MealPlan> MealPlanRepository =>
            _mealPlanRepository ?? (_mealPlanRepository = new GenericRepository<MealPlan>(_context));

        public IGenericRepository<MealPlanOption> MealPlanOptionRepository =>
            _mealPlanOptionRepository ??
            (_mealPlanOptionRepository = new GenericRepository<MealPlanOption>(_context));

        public IGenericRepository<MealPlanOptionGrid> MealPlanOptionGridRepository =>
            _mealPlanOptionGridRepository ?? (_mealPlanOptionGridRepository =
                new GenericRepository<MealPlanOptionGrid>(_context));

        public IGenericRepository<MealPlanOptionList> MealPlanOptionListRepository =>
            _mealPlanOptionListRepository ?? (_mealPlanOptionListRepository =
                new GenericRepository<MealPlanOptionList>(_context));

        public IGenericRepository<News> NewsRepository =>
            _newsRepository ?? (_newsRepository = new GenericRepository<News>(_context));

        public IGenericRepository<MilestonePercentage> MilestonePercentageRepository =>
            _milestonePercentageRepository ?? (_milestonePercentageRepository =
                new GenericRepository<MilestonePercentage>(_context));

        public IGenericRepository<UserPushToken> UserPushTokenRepository =>
            _userPushTokenRepository ??
            (_userPushTokenRepository = new GenericRepository<UserPushToken>(_context));

        public IGenericRepository<ApplicationCountry> ApplicationCountryRepository =>
            _applicationCountryRepository ??
            (_applicationCountryRepository = new GenericRepository<ApplicationCountry>(_context));

        public IGenericRepository<Province> ProvinceRepository =>
            _provinceRepository ?? (_provinceRepository = new GenericRepository<Province>(_context));

        public IGenericRepository<Menu> MenuRepository =>
            _menuRepository ?? (_menuRepository = new GenericRepository<Menu>(_context));

        public IGenericRepository<ApplicationMenu> ApplicationMenuRepository =>
            _applicationMenuRepository ??
            (_applicationMenuRepository = new GenericRepository<ApplicationMenu>(_context));

        public IGenericRepository<YoutubeConfiguration> YoutubeConfigurationRepository =>
            _youtubeConfigurationRepository ??
            (_youtubeConfigurationRepository = new GenericRepository<YoutubeConfiguration>(_context));

        public IGenericRepository<YoutubeVideoCollection> YoutubeVideoCollectionRepository =>
            _youtubeVideoCollectionRepository ?? (_youtubeVideoCollectionRepository =
                new GenericRepository<YoutubeVideoCollection>(_context));

        public IGenericRepository<UserNotification> UserNotificationRepository =>
            _userNotificationRepository ??
            (_userNotificationRepository = new GenericRepository<UserNotification>(_context));

        public IGenericRepository<UserNotificationType> UserNotificationTypeRepository =>
            _userNotificationTypeRepository ??
            (_userNotificationTypeRepository = new GenericRepository<UserNotificationType>(_context));

        public IGenericRepository<OpenNotificationUser> OpenNotificationUserRepository =>
            _openNotificationUserRepository ??
            (_openNotificationUserRepository = new GenericRepository<OpenNotificationUser>(_context));

        public IGenericRepository<ApplicationSetting> ApplicationSettingRepository =>
            _applicationSettingRepository ??
            (_applicationSettingRepository = new GenericRepository<ApplicationSetting>(_context));

        public IGenericRepository<UserTrackerReportV1> UserTrackerReportV1Repository =>
            _userTrackerReportV1Repository ??
            (_userTrackerReportV1Repository = new GenericRepository<UserTrackerReportV1>(_context));

        public IGenericRepository<UserTrackerDetailReportV2> UserTrackerDetailReportV2ReporRepository =>
            _userTrackerDetailReportV2ReportRepository ?? (_userTrackerDetailReportV2ReportRepository =
                new GenericRepository<UserTrackerDetailReportV2>(_context));

        public IGenericRepository<OpenNotificationUserRegistration> OpenNotificationUserRegistrationRepository =>
            _openNotificationUserRegistrationRepository ?? (_openNotificationUserRegistrationRepository =
                new GenericRepository<OpenNotificationUserRegistration>(_context));

        public IGenericRepository<OpenNotificationOnlyUser> OpenNotificationOnlyUserRepository =>
            _openNotificationOnlyUserRepository ?? (_openNotificationOnlyUserRepository =
                new GenericRepository<OpenNotificationOnlyUser>(_context));

        public IGenericRepository<UserNotificationView> UserNotificationViewRepository =>
            _userNotificationViewRepository ??
            (_userNotificationViewRepository = new GenericRepository<UserNotificationView>(_context));

        public IGenericRepository<UserTrackerRealtime> UserTrackerRealtimeRepository =>
            _userTrackerRealtimeRepository ??
            (_userTrackerRealtimeRepository = new GenericRepository<UserTrackerRealtime>(_context));

        public IGenericRepository<UserMetaPivot> UserMetaPivotRepository =>
            _userMetaPivotRepository ?? (_userMetaPivotRepository = new GenericRepository<UserMetaPivot>(_context));

        public IGenericRepository<UserTrackerPivot> UserTrackerPivotRepository =>
            _userTrackerPivotRepository ??
            (_userTrackerPivotRepository = new GenericRepository<UserTrackerPivot>(_context));

        public IGenericRepository<WebUserMetaPivot> WebUserMetaPivotRepository =>
            _webUserMetaPivotRepository ??
            (_webUserMetaPivotRepository = new GenericRepository<WebUserMetaPivot>(_context));

        public IGenericRepository<WebUserTrackerViewPivot> WebUserTrackerViewPivotRepository =>
            _webUserTrackerViewPivotRepository ?? (_webUserTrackerViewPivotRepository =
                new GenericRepository<WebUserTrackerViewPivot>(_context));

        public bool Commit()
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    using (var dbContextTransaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.SaveChanges();
                            dbContextTransaction.Commit();
                            transactionScope.Complete();
                            return true;
                        }
                        catch (DbEntityValidationException e)
                        {
                            dbContextTransaction.Rollback();
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                _helper.SaveLog(eve);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _helper.SaveLog(ex, "UnitOfWork", "Commit()");
                }

                transactionScope.Complete();
            }

            return false;
        }

        public async Task<bool> CommitAsync()
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    using (var dbContextTransaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            await _context.SaveChangesAsync();
                            dbContextTransaction.Commit();
                            transactionScope.Complete();
                            return true;
                        }
                        catch (DbEntityValidationException e)
                        {
                            dbContextTransaction.Rollback();
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                await _helper.SaveLogAsync(eve);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await _helper.SaveLogAsync(ex, "UnitOfWork", "Commit()");
                }

                transactionScope.Complete();
            }

            return false;
        }

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}