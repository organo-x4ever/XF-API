using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserPivotServices : IUserPivotServices, IValidation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Helper.IHelper _helper;
        private readonly IUserMetaPivotServices _userMetaPivotServices;
        private readonly IUserTrackerPivotServices _userTrackerPivotServices;
        private readonly IUserMilestoneServices _userMilestoneServices;
        private readonly IMilestonePercentageServices _milestonePercentageServices;
        private readonly IUserTokensServices _tokensServices;
        private readonly IUserSettingServices _userSettingServices;

        public UserPivotServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
            _userMetaPivotServices = new UserMetaPivotServices(unitOfWork);
            _userTrackerPivotServices = new UserTrackerPivotServices(unitOfWork);
            _tokensServices = new UserTokensServices(unitOfWork);
            _userMilestoneServices = new UserMilestoneServices(unitOfWork);
            _milestonePercentageServices = new MilestonePercentageServices(unitOfWork);
            _userSettingServices = new UserSettingServices(unitOfWork);
        }

        public long Authenticate(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.UserLogin.ToLower().Trim() == userName.ToLower().Trim());
            if (!(user is null))
            {
                _unitOfWork.UserRepository.Reload(user);
                if (VerifySha512(user.UserPassword, password))
                    return user.ID;

                if (VerifyHash(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Commit();
                    return user.ID;
                }

                if (VerifySha512_WrongEncryption(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Commit();
                    return user.ID;
                }
            }

            return 0;
        }

        public bool ChangePassword(ref ValidationErrors validationErrors, string token, string currentPassword,
            string password)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return ChangePassword(ref validationErrors, tokenDetail?.UserID ?? 0, currentPassword, password);

        }

        public bool ChangePassword(ref ValidationErrors validationErrors, long id, string currentPassword,
            string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.ID == id);
            if (!(user is null))
            {
                if (VerifyPassword(user.UserPassword, currentPassword))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    if (_unitOfWork.Commit())
                        return SavePasswordHistory(user.ID, user.UserPassword);
                }
                else if (VerifySha512_WrongEncryption(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    if (_unitOfWork.Commit())
                        return SavePasswordHistory(user.ID, user.UserPassword);
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public async Task<UserPivot> GetFullUserAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetFullUserAsync(tokenDetail?.UserID ?? 0);
        }

        public async Task<UserPivot> GetFullUserAsync(long id)
        {
            var user = new UserPivot();
            var userData = await _unitOfWork.UserRepository.GetByIDAsync(id);
            if (userData != null)
            {
                user = new UserPivot()
                {
                    ID = userData.ID,
                    UserFirstName = userData.UserFirstName,
                    UserEmail = userData.UserEmail,
                    UserLastName = userData.UserLastName,
                    UserRegistered = userData.UserRegistered,
                    IsTrackerViewAllowed = (bool) _helper.GetAppSetting(CommonConstants.TrackerViewAllowed,
                        typeof(bool)),
                    IsDownloadAllowed = (bool) _helper.GetAppSetting(CommonConstants.TrackerDownloadAllowed,
                        typeof(bool)),
                    IsTrackerRequiredAfterDelete = (bool) _helper.GetAppSetting(CommonConstants.IsRequireDeleted,
                        typeof(bool))
                };

                double.TryParse(_helper.GetAppSetting(CommonConstants.WeightSubmitIntervalDays), out double weightSubmitIntervalDays);
                var userMilestones =
                    await _userMilestoneServices.GetAsync(um => um.UserID == user.ID && um.IsPercentage);

                user.Achievement = await _milestonePercentageServices.GetLastAsync(mp =>
                    userMilestones.Any(um => um.MilestonePercentageId == mp.MilestonePercentageId));

                if (user.Achievement != null)
                    user.Achievement.ID = user.Achievement.MilestonePercentageId;

                var userSetting = await _userSettingServices.GetAsync(s => s.UserID == userData.ID);
                user.LanguageCode = userSetting?.LanguageCode ?? CommonConstants.EnglishUS;

                user.MetaPivot = await _userMetaPivotServices.GetMetaAsync(user.ID, userSetting?.WeightVolumeType);
                user.TrackerPivot =
                    (await _userTrackerPivotServices.GetTrackersAsync(user.ID, userSetting?.WeightVolumeType,weightSubmitIntervalDays)).OrderBy(
                        t => t.RevisionNumber);

                double.TryParse(user.MetaPivot?.WeightLossGoal, out double yourGoal);
                var noOfDays = (yourGoal / CommonConstants.TargetDateCalculation);
                user.TargetDate = string.Format(CommonConstants.DATE_FORMAT_MMM_d_yyyy,
                    user.UserRegistered.AddDays(noOfDays)); // "Sunday, March 9, 2008"

                bool matched = false;
                var strEmail = _helper.GetAppSetting(CommonConstants.ExcludingSubmitCurrentWeight);
                if (!string.IsNullOrEmpty(strEmail))
                {
                    var strEmails = strEmail.Split(';');
                    foreach (var str in strEmails)
                    {
                        if (user.UserEmail.Trim().ToLower() == str.Trim().ToLower())
                            matched = true;
                    }
                }

                if (matched)
                    user.IsWeightSubmissionRequired = false;
                else
                {
                    var lastTracker = user.TrackerPivot.OrderBy(t => t.ModifyDate).LastOrDefault();
                    DateTime.TryParse(
                        lastTracker?.ModifyDate
                            .AddDays(weightSubmitIntervalDays).ToShortDateString(),
                        out DateTime submitDate);
                    user.IsWeightSubmissionRequired = DateTime.Compare(submitDate, DateTime.Today) != 1;
                }
            }

            return user;
        }

        public UserAuthentication GetUser(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return GetUser(tokenDetail?.UserID ?? 0);
        }

        public UserAuthentication GetUser(long id)
        {
            var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == id)
                select new UserAuthentication()
                {
                    ID = u.ID,
                    UserLogin = u.UserLogin,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserApplication = u.UserApplication,
                })?.FirstOrDefault();
            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == id);
                user.LanguageCode = settings?.LanguageCode ?? CommonConstants.EnglishUS;
                user.WeightVolumeType = settings?.WeightVolumeType ?? CommonConstants.kilogram;
                var meta = _userMetaPivotServices.GetMeta(user.ID);
                user.IsMetaExists = meta != null;
                user.IsAddressExists = meta?.Address?.Trim().Length > 0;
                var trackers = _userTrackerPivotServices.GetTrackers(user.ID);
                user.IsTrackerExists = trackers?.FirstOrDefault(t => t.FrontImage?.Trim().Length > 0) != null;
            }

            return user;
        }

        public async Task<UserAuthentication> GetUserAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetUserAsync(tokenDetail?.UserID ?? 0);
        }

        public async Task<UserAuthentication> GetUserAsync(long id)
        {
            var user = (from u in await _unitOfWork.UserRepository.GetManyAsync(u => u.ID == id)
                select new UserAuthentication()
                {
                    ID = u.ID,
                    UserLogin = u.UserLogin,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserApplication = u.UserApplication,
                })?.FirstOrDefault();
            if (user != null)
            {
                var settings = await _unitOfWork.UserSettingRepository.GetFirstAsync(s => s.UserID == id);
                user.LanguageCode = settings?.LanguageCode ?? CommonConstants.EnglishUS;
                user.WeightVolumeType = settings?.WeightVolumeType ?? CommonConstants.kilogram;
                var meta = await _userMetaPivotServices.GetMetaAsync(user.ID);
                user.IsMetaExists = meta != null;
                user.IsAddressExists = meta?.Address?.Trim().Length > 0;
                var trackers = await _userTrackerPivotServices.GetTrackersAsync(user.ID);
                user.IsTrackerExists = trackers?.FirstOrDefault(t => t.FrontImage?.Trim().Length > 0) != null;
            }

            return user;
        }

        public bool Update(ref ValidationErrors validationErrors, string token, User entity)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return Update(ref validationErrors, tokenDetail?.UserID ?? 0, entity);
        }

        public bool Update(ref ValidationErrors validationErrors, long id, User entity)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.ID == id);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                dynamic[] obj = {user, false, true, false, false, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, string token, string application)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return Update(ref validationErrors, tokenDetail?.UserID ?? 0, application);
        }

        public bool Update(ref ValidationErrors validationErrors, long id, string application)
        {
            if (!string.IsNullOrEmpty(application))
            {
                var user = _unitOfWork.UserRepository.Get(u => u.ID == id);
                if (!(user is null))
                {
                    user.UserApplication = application;
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }

            return false;
        }

        private bool SavePasswordHistory(Int64 userId, string encryptedPassword)
        {
            var date = DateTime.Now;
            var expires = date;
            var days = (int) _helper.GetAppSetting("passwordExpireMinutes", typeof(System.Int32));
            if (days > 0)
                expires = date.AddDays(days);
            var history = new UserPasswordHistory()
            {
                ChangeDate = date,
                ExpireDate = expires,
                UserID = userId,
                UserPassword = encryptedPassword
            };

            _unitOfWork.UserPasswordHistoryRepository.Insert(history);
            return _unitOfWork.Commit();
        }

        private User GetUserByToken(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return _unitOfWork.UserRepository.Get(u => u.ID == (tokenDetail?.UserID ?? 0));
        }

        private bool EmailExists(Int64 userId, string email)
        {
            return _unitOfWork.UserRepository.GetAll()
                .Any(u => u.ID != userId && u.UserEmail.Trim().ToLower() == email.Trim().ToLower());
        }

        private bool VerifyPassword(string encrypted, string plain)
        {
            return VerifySha512(encrypted, plain) || VerifyHash(encrypted, plain);
        }

        private bool VerifySha512(string encrypted, string plain)
        {
            return _helper.CompareSHA512(encrypted, plain);
        }

        private bool VerifySha512_WrongEncryption(string encrypted, string plain)
        {
            return _helper.CompareSHA512_WrongEncryption(encrypted, plain);
        }

        private bool VerifyHash(string encrypted, string plain)
        {
            return _helper.VerifyPassword(plain, encrypted);
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var user = (User) objValue[0];
            var basic = (bool) objValue[1];
            var basicUpdate = (bool) objValue[2];
            var status = (bool) objValue[3];
            var type = (bool) objValue[4];
            var basicLastName = (bool) objValue[5];
            if (basic)
            {
                if (user.UserLogin == null || user.UserLogin.Trim().Length == 0)
                    validationErrors.Add("MessageUsernameRequired");
                else
                {
                    int userLengthMin = (Int16) _helper.GetAppSetting(CommonConstants.UsernameLengthMin, typeof(Int16));
                    int userLengthMax = (Int16) _helper.GetAppSetting(CommonConstants.UsernameLengthMax, typeof(Int16));
                    if (user.UserLogin.Trim().Length < userLengthMin || user.UserLogin.Trim().Length > userLengthMax)
                        validationErrors.Add("MessageUsernameLength");
                    else if (_unitOfWork.UserRepository.GetAll().Any(u =>
                        u.UserLogin.ToLower() == user.UserLogin.ToLower() && u.ID != user.ID))
                        validationErrors.Add("MessageUsernameExists");
                }

                if (user.UserPassword == null || user.UserPassword.Trim().Length == 0)
                    validationErrors.Add("MessagePasswordRequired");
                else
                {
                    int passLengthMin = (Int16) _helper.GetAppSetting(CommonConstants.PasswordLengthMin, typeof(Int16));
                    int passLengthMax = (Int16) _helper.GetAppSetting(CommonConstants.PasswordLengthMax, typeof(Int16));
                    if (user.UserPassword.Trim().Length < passLengthMin ||
                        user.UserPassword.Trim().Length > passLengthMax)
                        validationErrors.Add("MessagePasswordLength");
                }
            }

            if (basicUpdate)
            {
                if (user.UserFirstName == null || user.UserFirstName.Trim().Length == 0)
                    validationErrors.Add("MessageFirstNameRequired");
                if (basicLastName)
                    if (user.UserLastName == null || user.UserLastName.Trim().Length == 0)
                        validationErrors.Add("MessageLastNameRequired");
                if (user.UserEmail == null || user.UserEmail.Trim().Length == 0)
                    validationErrors.Add("MessageUserEmailRequired");
                else if (!_helper.IsEmailAddressValid(user.UserEmail.Trim().ToLower()))
                    validationErrors.Add("MessageEmailInvalid");
                else if (EmailExists(user.ID, user.UserEmail))
                    validationErrors.Add("MessageEmailExists");
            }

            if (status)
                if (user.UserStatus == null || user.UserStatus.Trim().Length == 0)
                    validationErrors.Add("MessageUserStatusRequired");

            if (type)
                if (user.UserType == null || user.UserType.Trim().Length == 0)
                    validationErrors.Add("MessageUserTypeRequired");

            return validationErrors.Count() == 0;
        }
    }
}