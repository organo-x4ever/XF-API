using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserTrackerServices : IUserTrackerServices, IValidation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokensServices _tokensServices;

        public UserTrackerServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokensServices = new UserTokensServices(unitOfWork);
        }

        public bool Delete(ref ValidationErrors validationErrors, UserTracker entity)
        {
            _unitOfWork.UserTrackerRepository.Delete(entity);
            return _unitOfWork.Commit();
        }

        public bool Delete(ref ValidationErrors validationErrors, string token)
        {
            dynamic[] obj = {token, true, false};
            if (Validate(ref validationErrors, obj))
            {
                foreach (var entity in Get(token))
                {
                    _unitOfWork.UserTrackerRepository.Delete(entity);
                }

                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Delete(ref ValidationErrors validationErrors, long userId)
        {
            foreach (var entity in Get(userId))
            {
                _unitOfWork.UserTrackerRepository.Delete(entity);
            }

            return _unitOfWork.Commit();
        }

        public bool Delete(ref ValidationErrors validationErrors, string token, ICollection<UserTracker> entity)
        {
            dynamic[] obj = {token, true, false};
            if (Validate(ref validationErrors, obj))
            {
                var trackers = Get(token).ToList();
                if (trackers.Count > 0)
                {
                    foreach (var tracker in entity)
                    {
                        if (trackers.Any(t =>
                            t.AttributeName == tracker.AttributeName && t.AttributeLabel == tracker.AttributeLabel &&
                            t.UserID == tracker.UserID))
                            _unitOfWork.UserTrackerRepository.Delete(tracker);
                    }
                }

                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Delete(ref ValidationErrors validationErrors, long userId, ICollection<UserTracker> entity)
        {
            var trackers = Get(userId).ToList();
            if (trackers.Count > 0)
            {
                foreach (var tracker in entity)
                {
                    if (trackers.Any(t =>
                        t.AttributeName == tracker.AttributeName && t.AttributeLabel == tracker.AttributeLabel &&
                        t.UserID == tracker.UserID))
                        _unitOfWork.UserTrackerRepository.Delete(tracker);
                }
            }

            return _unitOfWork.Commit();
        }

        public IEnumerable<UserTracker> Get()
        {
            throw new NotImplementedException();
            //return _unitOfWork.UserTrackerRepository.GetAll();
        }

        public IEnumerable<UserTracker> Get(Expression<Func<UserTracker, bool>> filter = null,
            Func<IQueryable<UserTracker>, IOrderedQueryable<UserTracker>> orderBy = null, string includeProperties = "")
        {
            var tracker= _unitOfWork.UserTrackerRepository.GetMany(filter, orderBy, includeProperties);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public IEnumerable<UserTracker> Get(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var tracker= Get(tokenDetail?.UserID ?? 0);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }
        
        public IEnumerable<UserTracker> Get(long userId)
        {
            var tracker= _unitOfWork.UserTrackerRepository.GetMany(u => u.UserID == userId);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }
        
        public async Task<IEnumerable<UserTracker>> GetAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var tracker= await GetAsync(tokenDetail?.UserID ?? 0);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public async Task<IEnumerable<UserTracker>> GetAsync(long userId)
        {
            var tracker= await _unitOfWork.UserTrackerRepository.GetManyAsync(u => u.UserID == userId);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public IEnumerable<UserTracker> GetByAndAttribute(string token, string attributeName)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var tracker= _unitOfWork.UserTrackerRepository
                .GetMany(u => u.UserID == (tokenDetail?.UserID ?? 0) && u.AttributeName == attributeName)
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }


        public IEnumerable<UserTracker> GetLatest(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var tracker= GetLatest(tokenDetail?.UserID ?? 0);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }
        
        public IEnumerable<UserTracker> GetLatest(long userId)
        {
            var tracker= _unitOfWork.UserTrackerRepository.GetMany(u => u.UserID == userId
                                                                  && _unitOfWork.UserTrackerRepository
                                                                      .GetMany(ut => ut.UserID == u.UserID)
                                                                      .OrderByDescending(ut => ut.ID)
                                                                      .Take(1).Any(ut =>
                                                                          ut.RevisionNumber ==
                                                                          u.RevisionNumber))
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public async Task<IEnumerable<UserTracker>> GetLatestAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var tracker= await GetLatestAsync(tokenDetail?.UserID ?? 0);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public async Task<IEnumerable<UserTracker>> GetLatestAsync(long userId)
        {
            var tracker= (await _unitOfWork.UserTrackerRepository.GetManyAsync(u => u.UserID == userId
                                                                              && _unitOfWork.UserTrackerRepository
                                                                                  .GetMany(ut => ut.UserID == u.UserID)
                                                                                  .OrderByDescending(ut => ut.ID)
                                                                                  .Take(1).Any(ut =>
                                                                                      ut.RevisionNumber ==
                                                                                      u.RevisionNumber)))
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }


        public bool Insert(ref ValidationErrors validationErrors, UserTracker entity)
        {
            dynamic[] obj = {entity, false, true, false};
            if (Validate(ref validationErrors, obj))
            {
                    entity.AttributeValue = entity.AttributeValue.Clean();
                _unitOfWork.UserTrackerRepository.Insert(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Insert(ref ValidationErrors validationErrors, ICollection<UserTracker> entity)
        {
            bool success = false;
            foreach (var tracker in entity)
            {
                dynamic[] obj = {tracker, false, true, false};
                if (Validate(ref validationErrors, obj))
                {
                    tracker.AttributeValue = tracker.AttributeValue.Clean();
                    _unitOfWork.UserTrackerRepository.Insert(tracker);
                }

                success = _unitOfWork.Commit();
            }

            return success;
        }

        public bool Insert(ref ValidationErrors validationErrors, string token, ICollection<UserTracker> entity)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return Insert(ref validationErrors, tokenDetail?.UserID ?? 0, entity);
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, ICollection<UserTracker> entity)
        {
            var success = false;
            short revisionNumber = 0;
            var latestTracker = _unitOfWork.UserTrackerRepository.GetMany(t => t.UserID == userId)
                .OrderByDescending(t => t.ModifyDate).FirstOrDefault();
            int.TryParse(entity?.FirstOrDefault()?.RevisionNumber ?? "0", out int rn);
            if (latestTracker != null)
            {
                double.TryParse(new Helper.Helper().GetAppSetting("WeightSubmitIntervalDays"), out double interval);
                DateTime.TryParse(latestTracker.ModifyDate.AddDays(interval).ToShortDateString(),
                    out DateTime submitDate);
                if (rn != 10000 && DateTime.Compare(submitDate, DateTime.Today) == 1)
                {
                    validationErrors.Add("MessageInvalidObject");
                    return false;
                }

                short.TryParse(latestTracker.RevisionNumber, out revisionNumber);
            }

            if (rn == 10000)
                revisionNumber = 0;

            revisionNumber++;
            foreach (var record in entity)
            {
                success = false;
                record.RevisionNumber = revisionNumber.ToString();
                record.UserID = userId;
                record.ModifyDate = DateTime.Now;
                    record.AttributeValue = record.AttributeValue.Clean();
                dynamic[] obj = {record, false, true, false};
                if (Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserTrackerRepository.Insert(record);
                    bool result = _unitOfWork.Commit();
                    if (result)
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        public bool Insert(ref ValidationErrors validationErrors, string token, UserTracker entity)
        {
            var user = _tokensServices.GetDetailByToken(token);
            if (user != null && entity != null)
            {
                return Insert(ref validationErrors, user.UserID, entity);
            }

            return false;
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, UserTracker entity)
        {
            entity.UserID = userId;
            dynamic[] obj = {entity, false, true, false};
            if (Validate(ref validationErrors, obj))
            {
                    entity.AttributeValue = entity.AttributeValue.Clean();
                _unitOfWork.UserTrackerRepository.Insert(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, UserTracker entity)
        {
            dynamic[] obj = {entity, false, true, false};
            if (Validate(ref validationErrors, obj))
            {
                    entity.AttributeValue = entity.AttributeValue.Clean();
                _unitOfWork.UserTrackerRepository.Update(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, ICollection<UserTracker> entity)
        {
            dynamic[] obj = {entity, false, true, false};
            if (Validate(ref validationErrors, obj))
            {
                foreach (var tracker in entity)
                {
                    tracker.AttributeValue = tracker.AttributeValue.Clean();
                    _unitOfWork.UserTrackerRepository.Update(tracker);
                }

                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var userTokenRequired = (bool) objValue[1];
            string token = "";
            UserTracker obj = new UserTracker();
            if (userTokenRequired)
                token = objValue[0];
            else
                obj = (UserTracker) objValue[0];
            var basic = (bool) objValue[2];
            var basicValue = (bool) objValue[3];
            if (userTokenRequired)
                if (token.Trim().Length == 0)
                    validationErrors.Add("UserTokenRequired");
            if (basic)
                if (obj.AttributeName.Trim().Length == 0)
                    validationErrors.Add("AttributeNameRequired");
            if (basicValue)
                if (obj.AttributeValue.Trim().Length == 0)
                    validationErrors.Add("AttributeValueRequired");
            return validationErrors.Count() == 0;
        }

        public IEnumerable<UserTrackerMedia> GetImages(long userId)
        {
            return (from u in _unitOfWork.UserTrackerRepository.GetMany(u =>
                        u.UserID == (userId == 0 ? u.UserID : userId) &&
                        u.AttributeName.Contains("frontimage") ||
                        u.AttributeName.Contains("sideimage"))
                    .OrderBy(u => u.UserID).ThenBy(u => u.ID)
                select new UserTrackerMedia()
                {
                    AttributeValue = u.AttributeValue.Clean()
                }).Distinct();
        }

        public IEnumerable<UserTrackerMedia> GetSpecificImages(long userId)
        {
            return (from t in _unitOfWork.UserTrackerRepository.GetMany(t =>
                        (t.MediaLink == null || t.MediaLink != "inactive")
                        && t.UserID == (userId == 0 ? t.UserID : userId)
                        && t.AttributeName.Contains("frontimage") ||
                        t.AttributeName.Contains("sideimage"))
                    .OrderBy(u => u.UserID).ThenBy(u => u.ID)
                select new UserTrackerMedia()
                {
                    AttributeValue = t.AttributeValue.Clean()
                }).Distinct();
        }

        public IEnumerable<UserTrackerMedia> GetSpecificImages(string key)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.UserKey == (key == "X" ? u.UserKey : key));
            return (from t in _unitOfWork.UserTrackerRepository.GetMany(t =>
                        (t.MediaLink == null || t.MediaLink != "inactive")
                        && t.UserID == (user?.ID ?? t.UserID)
                        && t.AttributeName.Contains("frontimage") ||
                        t.AttributeName.Contains("sideimage"))
                    .OrderBy(u => u.UserID).ThenBy(u => u.ID)
                select new UserTrackerMedia()
                {
                    AttributeValue = t.AttributeValue.Clean()
                }).Distinct();
        }

        public async Task<IEnumerable<UserTrackerPivot>> GetUserRowAsync(long userId)
        {
            return await Task.Factory.StartNew(() =>{
               var tracker= _unitOfWork.UserTrackerPivotRepository.SqlQuery(
                    "x4ever.x4_user_tracker_select_by_userid @user_id",
                    new SqlParameter("user_id", SqlDbType.BigInt) {Value = userId});
                return from t in tracker
                    select new UserTrackerPivot()
                    {
                        user_id=t.user_id,
                        rev_number=t.rev_number,
                        ID=t.ID,
                    ModifyDate = t.ModifyDate,
                    CurrentWeight =t.CurrentWeight,
                    ShirtSize = t.ShirtSize,
                    FrontImage =t.FrontImage.Clean(),
                    SideImage = t.SideImage.Clean(),
                    AboutJourney =t.AboutJourney
                    };
               });
        }

        public async Task<IEnumerable<UserTrackerMedia>> GetImagesAsync(long userId)
        {
            var tracker= (from u in (await _unitOfWork.UserTrackerRepository.GetManyAsync(u =>
                        u.UserID == (userId == 0 ? u.UserID : userId) &&
                        u.AttributeName.Contains("frontimage") ||
                        u.AttributeName.Contains("sideimage")))
                    .OrderBy(u => u.UserID).ThenBy(u => u.ID)
                select new UserTrackerMedia()
                {
                    AttributeValue = u.AttributeValue
                }).Distinct();
            return from t in tracker
                   select new UserTrackerMedia()
                   {
                       AttributeValue=t.AttributeValue.Clean()
                   };
        }

        public async Task<IEnumerable<UserTrackerMedia>> GetSpecificImagesAsync(long userId)
        {
            var tracker= (from t in (await _unitOfWork.UserTrackerRepository.GetManyAsync(t =>
                        (t.MediaLink == null || t.MediaLink != "inactive")
                        && t.UserID == (userId == 0 ? t.UserID : userId)
                        && t.AttributeName.Contains("frontimage") ||
                        t.AttributeName.Contains("sideimage")))
                    .OrderBy(u => u.UserID).ThenBy(u => u.ID)
                select new UserTrackerMedia()
                {
                    AttributeValue = t.AttributeValue
                }).Distinct();
            return from t in tracker
                   select new UserTrackerMedia()
                   {
                       AttributeValue=t.AttributeValue.Clean()
                   };
        }

        public async Task<IEnumerable<UserTrackerMedia>> GetSpecificImagesAsync(string key)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u => u.UserKey == (key == "X" ? u.UserKey : key));
            return (from t in (await _unitOfWork.UserTrackerRepository.GetManyAsync(t =>
                        (t.MediaLink == null || t.MediaLink != "inactive")
                        && t.UserID == (user?.ID ?? t.UserID)
                        && t.AttributeName.Contains("frontimage") ||
                        t.AttributeName.Contains("sideimage")))
                    .OrderBy(u => u.UserID).ThenBy(u => u.ID)
                select new UserTrackerMedia()
                {
                    AttributeValue = t.AttributeValue.Clean()
                }).Distinct();
        }

        public async Task<IEnumerable<UserTracker>> GetFirstAndLastAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var tracker= await GetFirstAndLastAsync(tokenDetail?.UserID ?? 0);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }
        
        public async Task<IEnumerable<UserTracker>> GetFirstAndLastAsync(long userId)
        {
            var tracker = (await _unitOfWork.UserTrackerRepository.GetManyAsync(u =>
                    u.UserID == userId
                    && (_unitOfWork.UserTrackerRepository
                        .GetMany(ut => ut.UserID == u.UserID)
                        .OrderByDescending(ut => ut.ID).Take(1)
                        .Any(ut =>
                            ut.RevisionNumber == u.RevisionNumber)
                    )
                    || _unitOfWork.UserTrackerRepository
                        .GetMany(ut => ut.UserID == u.UserID)
                        .OrderBy(ut => ut.ID).Take(1)
                        .Any(ut =>
                            ut.RevisionNumber ==
                            u.RevisionNumber)))
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };;
        }
        
        public IEnumerable<UserTracker> GetFirstAndLast(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var tracker= GetFirstAndLast(tokenDetail?.UserID ?? 0);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public IEnumerable<UserTracker> GetFirstAndLast(long userId)
        {
            var trackers = _unitOfWork.UserTrackerRepository.GetMany(u => u.UserID == userId
                                                                          && (_unitOfWork.UserTrackerRepository
                                                                              .GetMany(ut => ut.UserID == u.UserID)
                                                                              .OrderByDescending(ut => ut.ID).Take(1)
                                                                              .Any(ut =>
                                                                                  ut.RevisionNumber == u.RevisionNumber)
                                                                          )
                                                                          || _unitOfWork.UserTrackerRepository
                                                                              .GetMany(ut => ut.UserID == u.UserID)
                                                                              .OrderBy(ut => ut.ID).Take(1)
                                                                              .Any(ut =>
                                                                                  ut.RevisionNumber ==
                                                                                  u.RevisionNumber))
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in trackers
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }
        
        public IEnumerable<UserTracker> GetByAttribute(string token, string attributeName)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var tracker= GetByAttribute(tokenDetail?.UserID ?? 0, attributeName);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public IEnumerable<UserTracker> GetByAttribute(long userId, string attributeName)
        {
            var tracker= _unitOfWork.UserTrackerRepository
                .GetMany(u => u.UserID == userId && u.AttributeName == attributeName)
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public async Task<IEnumerable<UserTracker>> GetByAttributeAsync(string token, string attributeName)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var tracker= await GetByAttributeAsync(tokenDetail?.UserID ?? 0, attributeName);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }
        
        public async Task<IEnumerable<UserTracker>> GetByAttributeAsync(long userId, string attributeName)
        {
            var tracker= (await _unitOfWork.UserTrackerRepository
                    .GetManyAsync(u => u.UserID == userId && u.AttributeName == attributeName))
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public async Task<IEnumerable<UserTracker>> GetByAttributeLabelAsync(string token, string attributeLabel)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var tracker= await GetByAttributeLabelAsync(tokenDetail?.UserID ?? 0, attributeLabel);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public async Task<IEnumerable<UserTracker>> GetByAttributeLabelAsync(long userId, string attributeLabel)
        {
            var tracker= (await _unitOfWork.UserTrackerRepository
                    .GetManyAsync(u => u.UserID == userId && u.AttributeLabel == attributeLabel))
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public IEnumerable<UserTracker> GetByAttributeLabel(string token, string attributeLabel)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var tracker= GetByAttributeLabel(tokenDetail?.UserID ?? 0, attributeLabel);
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public IEnumerable<UserTracker> GetByAttributeLabel(long userId, string attributeLabel)
        {
            var tracker= _unitOfWork.UserTrackerRepository
                .GetMany(u => u.UserID == userId && u.AttributeLabel == attributeLabel)
                .OrderBy(u => u.ModifyDate).ToList();
            return from t in tracker
                    select new UserTracker()
                    {
                        UserID=t.UserID,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    AttributeLabel=t.AttributeLabel,
                    ID=t.ID,
                    AttributeValue=t.AttributeValue.Clean(),
                    AttributeName=t.AttributeName,
                    MediaLink=t.MediaLink
                    };
        }

        public Task<IEnumerable<UserTracker>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserTracker>> GetAsync(Expression<Func<UserTracker, bool>> filter = null,
            Func<IQueryable<UserTracker>, IOrderedQueryable<UserTracker>> orderBy = null, string includeProperties = "")
        {
            return await _unitOfWork.UserTrackerRepository.GetManyAsync(filter, orderBy, includeProperties);
        }

        public IEnumerable<TrackerPivot> GetAsPivot(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return GetAsPivot(tokenDetail.UserID);
        }
        
        public IEnumerable<TrackerPivot> GetAsPivot(long userId)
        {
            var tracker = _unitOfWork.UserTrackerRepository.GetMany(ut => ut.UserID == userId)
                .GroupBy(ut => new
                    {ut.UserID, ut.RevisionNumber, ut.ModifyDate.Year, ut.ModifyDate.Month, ut.ModifyDate.Day}).Select(
                    ut => new TrackerPivot
                    {
                        UserId = ut.Key.UserID,
                        RevisionNumber = ut.Key.RevisionNumber,
                        ModifyDate = new DateTime(ut.Key.Year, ut.Key.Month, ut.Key.Day),
                        WeightVolumeType =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "weightvolumetype")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        CurrentWeight =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "currentweight")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        CurrentWeightUI =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "currentweight_ui")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        ShirtSize = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "shirtsize")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        FrontImage =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "frontimage")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        SideImage = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "sideimage")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        AboutJourney =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "aboutjourney")
                                ?.FirstOrDefault()?.AttributeValue ?? ""
                    });
            return from t in tracker
                    select new TrackerPivot()
                    {
                        UserId=t.UserId,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    WeightVolumeType =t.WeightVolumeType,
                    CurrentWeight =t.CurrentWeight,
                    CurrentWeightUI =t.CurrentWeightUI,
                    ShirtSize = t.ShirtSize,
                    FrontImage =t.FrontImage.Clean(),
                    SideImage = t.SideImage.Clean(),
                    AboutJourney =t.AboutJourney
                    };
        }

        public async Task<IEnumerable<TrackerPivot>> GetAsPivotAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetAsPivotAsync(tokenDetail.UserID);
        }
        
        public async Task<IEnumerable<TrackerPivot>> GetAsPivotAsync(long userId)
        {
            var tracker = (await _unitOfWork.UserTrackerRepository.GetManyAsync(ut => ut.UserID == userId))
                .GroupBy(ut => new {ut.UserID, ut.RevisionNumber, ut.ModifyDate}).Select(ut => new TrackerPivot
                {
                    UserId = ut.Key.UserID,
                    RevisionNumber = ut.Key.RevisionNumber,
                    ModifyDate = ut.Key.ModifyDate,
                    WeightVolumeType =
                        ut.Where(u =>
                            u.RevisionNumber == ut.Key.RevisionNumber &&
                            u.AttributeName.ToLower() == "weightvolumetype")?.FirstOrDefault()?.AttributeValue ?? "",
                    CurrentWeight =
                        ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "currentweight")
                            ?.FirstOrDefault()?.AttributeValue ?? "",
                    CurrentWeightUI =
                        ut.Where(u =>
                            u.RevisionNumber == ut.Key.RevisionNumber &&
                            u.AttributeName.ToLower() == "currentweight_ui")?.FirstOrDefault()?.AttributeValue ?? "",
                    ShirtSize = ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "shirtsize")?.FirstOrDefault()?.AttributeValue ?? "",
                    FrontImage =
                        ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber && u.AttributeName.ToLower() == "frontimage")
                            ?.FirstOrDefault()?.AttributeValue ?? "",
                    SideImage = ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "sideimage")?.FirstOrDefault()?.AttributeValue ?? "",
                    AboutJourney =
                        ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "aboutjourney")
                            ?.FirstOrDefault()?.AttributeValue ?? ""
                });
            return from t in tracker
                    select new TrackerPivot(){
                        UserId=t.UserId,
                    RevisionNumber = t.RevisionNumber,
                    ModifyDate = t.ModifyDate,
                    WeightVolumeType =t.WeightVolumeType,
                    CurrentWeight =t.CurrentWeight,
                    CurrentWeightUI =t.CurrentWeightUI,
                    ShirtSize = t.ShirtSize,
                    FrontImage =t.FrontImage.Clean(),
                    SideImage = t.SideImage.Clean(),
                    AboutJourney =t.AboutJourney
                    };
        }

        
        //public UserTracker GetTracker(long Id)
        //{
        //    return _unitOfWork.UserTrackerRepository.GetByID(Id);
        //}
        //UserTracker IDefaultServices<UserTracker>.Get(string ID)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<UserTracker> IDefaultServices<UserTracker>.GetAsync(string ID)
        //{
        //    throw new NotImplementedException();
        //}
    }
}