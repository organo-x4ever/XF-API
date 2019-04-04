using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserPushTokenServices : IUserPushTokenServices, IValidation
    {
        private IUnitOfWork _unitOfWork;
        private readonly IHelper _helper;
        private readonly X4EverContext _context = new X4EverContext();
        private readonly IUserTokensServices _tokensServices;

        public UserPushTokenServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
            _tokensServices = new UserTokensServices(unitOfWork);
        }

        public bool Delete(ref ValidationErrors validationErrors, UserPushToken entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserPushToken> Get()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserPushToken>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserPushToken> Get(Expression<Func<UserPushToken, bool>> filter = null,
            Func<IQueryable<UserPushToken>, IOrderedQueryable<UserPushToken>> orderBy = null,
            string includeProperties = "")
        {
            return _unitOfWork.UserPushTokenRepository.GetMany(filter, orderBy, includeProperties);
        }

        public async Task<IEnumerable<UserPushToken>> GetAsync(Expression<Func<UserPushToken, bool>> filter = null,
            Func<IQueryable<UserPushToken>, IOrderedQueryable<UserPushToken>> orderBy = null,
            string includeProperties = "")
        {
            return await _unitOfWork.UserPushTokenRepository.GetManyAsync(filter, orderBy, includeProperties);
        }

        public UserPushToken Get(string ID)
        {
            return _unitOfWork.UserPushTokenRepository.GetByID(ID);
        }
        public async Task<UserPushToken> GetAsync(string ID)
        {
            return await _unitOfWork.UserPushTokenRepository.GetByIDAsync(ID);
        }

        public UserPushToken GetByToken(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return Get(tokenDetail?.UserID ?? 0);
        }
        
        public UserPushToken Get(long userId)
        {
            return _unitOfWork.UserPushTokenRepository.Get(u => u.UserID == userId);
        }

        public async Task<UserPushToken> GetByTokenAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetAsync(tokenDetail?.UserID ?? 0);
        }
        public async Task<UserPushToken> GetAsync(long userId)
        {
            return await _unitOfWork.UserPushTokenRepository.GetAsync(u => u.UserID == userId);
        }

        public bool Insert(ref ValidationErrors validationErrors, UserPushToken entity)
        {
            dynamic[] obj = {entity};
            if (Validate(ref validationErrors, obj))
            {
                if (Exists(ref entity))
                    _unitOfWork.UserPushTokenRepository.Update(entity);
                else
                    _unitOfWork.UserPushTokenRepository.Insert(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Insert(ref ValidationErrors validationErrors, string token, UserPushToken entity)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            if (entity == null || tokenDetail == null)
                return false;
            return Insert(ref validationErrors, tokenDetail?.UserID ?? 0, entity);
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, UserPushToken entity)
        {
            if (entity != null)
            {
                entity.UserID = userId;
            }

            return Insert(ref validationErrors, entity);
        }
        public bool Insert(ref ValidationErrors validationErrors, string token, UserPushTokenRegister entity)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return Insert(ref validationErrors, tokenDetail?.UserID ?? 0, entity);
        }
        
        public bool Insert(ref ValidationErrors validationErrors, long userId, UserPushTokenRegister entity)
        {
            if (entity != null)
                entity.UserID = userId;
            var date = DateTime.Now;
            UserPushToken userPushToken;
            if (!string.IsNullOrEmpty(entity?.OldDeviceToken))
            {
                userPushToken =
                    _unitOfWork.UserPushTokenRepository.GetLast(p => p.DeviceToken == entity.OldDeviceToken);
                if (userPushToken != null && userPushToken.DeviceToken != entity.OldDeviceToken)
                {
                    userPushToken.DeviceToken = entity?.DeviceToken;
                    return Insert(ref validationErrors, userPushToken);
                }
            }
            else if (entity != null)
            {
                userPushToken =
                    _unitOfWork.UserPushTokenRepository.GetLast(p => p.UserID == userId);
                if (userPushToken != null)
                {
                    userPushToken.DeviceToken = entity?.DeviceToken;
                    userPushToken.IssuedOn = entity?.IssuedOn ?? date;
                    return Insert(ref validationErrors, userPushToken);
                }
            }

            return Insert(ref validationErrors, new UserPushToken()
            {
                DeviceApplication = entity?.DeviceApplication,
                DeviceIdentity = entity?.DeviceIdentity,
                DeviceIdiom = entity?.DeviceIdiom,
                DevicePlatform = entity?.DevicePlatform,
                DeviceToken = entity?.DeviceToken,
                IssuedOn = entity?.IssuedOn ?? date,
                UserID = userId
            });
        }
        public bool Update(ref ValidationErrors validationErrors, UserPushToken entity)
        {
            dynamic[] obj = {entity};
            if (Validate(ref validationErrors, obj))
            {
                if (Exists(ref entity))
                    _unitOfWork.UserPushTokenRepository.Update(entity);
                else
                    _unitOfWork.UserPushTokenRepository.Insert(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, string token, UserPushToken entity)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return Update(ref validationErrors, tokenDetail?.UserID ?? 0, entity);
        }

        public bool Update(ref ValidationErrors validationErrors, long userId, UserPushToken entity)
        {
            if (entity != null)
                entity.UserID = userId;
            return Update(ref validationErrors, entity);
        }
        public bool Exists(ref UserPushToken entity)
        {
            var userPushToken = entity;
            var userPush = _unitOfWork.UserPushTokenRepository
                .GetMany(p =>
                    p.UserID == userPushToken.UserID && p.DeviceToken == userPushToken.DeviceToken)
                .OrderByDescending(p => p.IssuedOn);
            if (userPush.Any())
            {
                entity = userPush.FirstOrDefault();
                return true;
            }

            return false;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var obj = (UserPushToken) objValue[0];
            if (obj.UserID == 0)
            {
                validationErrors.Add("UserIDRequired");
            }

            if (obj.DeviceToken.Trim().Length == 0)
            {
                validationErrors.Add("MessageDeviceTokenRequired");
            }

            return validationErrors.Count() == 0;
        }

        public async Task<IEnumerable<Notification_UserTracker>> CheckTrackerDue(int userId = 0)
        {
            double.TryParse(_helper.GetAppSetting("WeightSubmitIntervalDays"), out double timeInterval);
            var date = DateTime.Today.AddDays(-timeInterval);
            var users = (from u in _unitOfWork.UserRepository.GetMany(u =>
                    u.ID == (userId == 0 ? u.ID : userId)
                    && !_unitOfWork.UserTrackerRepository
                        .GetMany(t => t.UserID == u.ID && t.ModifyDate >= date
                                                       && _unitOfWork.UserPushTokenRepository.GetMany(p =>
                                                           p.UserID == u.ID && t.UserID == p.UserID &&
                                                           (p.DevicePlatform != null &&
                                                            (p.DevicePlatform.Contains("Android") ||
                                                             p.DevicePlatform.Contains("iOS")))).Any()).Any())
                select new Notification_UserTracker()
                {
                    UserID = u.ID,
                    UserEmail = u.UserEmail,
                    UserPushToken = _unitOfWork.UserPushTokenRepository.GetMany(p =>
                            p.UserID == u.ID && p.DevicePlatform != null &&
                            (p.DevicePlatform.Contains("Android") || p.DevicePlatform.Contains("iOS")))?
                        .OrderByDescending(p => p.ID)?.FirstOrDefault(),
                    LanguageCode = _unitOfWork.UserSettingRepository.Get(s => s.UserID == u.ID)?.LanguageCode,
                }).ToList();
            return users;
        }
    }
}