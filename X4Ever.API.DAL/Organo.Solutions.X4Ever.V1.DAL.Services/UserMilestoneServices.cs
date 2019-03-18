using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserMilestoneServices : IUserMilestoneServices, IValidation
    {
        private IUnitOfWork _unitOfWork;
        private readonly IUserTokensServices _tokensServices;
        public UserMilestoneServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokensServices = new UserTokensServices(unitOfWork);
        }

        public bool Delete(ref ValidationErrors validationErrors, UserMilestone entity)
        {
            this._unitOfWork.UserMilestoneRepository.Delete(entity);
            return this._unitOfWork.Commit();
        }

        public IEnumerable<UserMilestone> Get()
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<UserMilestone>> GetAsync()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<UserMilestone> Get(Expression<Func<UserMilestone, bool>> filter = null,
            Func<IQueryable<UserMilestone>, IOrderedQueryable<UserMilestone>> orderBy = null,
            string includeProperties = "")
        {
            return _unitOfWork.UserMilestoneRepository.GetMany(filter, orderBy, includeProperties);
        }

        public async Task<IEnumerable<UserMilestone>> GetAsync(Expression<Func<UserMilestone, bool>> filter = null,
            Func<IQueryable<UserMilestone>, IOrderedQueryable<UserMilestone>> orderBy = null,
            string includeProperties = "")
        {
            return await _unitOfWork.UserMilestoneRepository.GetManyAsync(filter, orderBy, includeProperties);
        }

        public UserMilestone Get(string ID)
        {
            throw new NotImplementedException();
        }
        public Task<UserMilestone> GetAsync(string ID)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> GetDetailByUser(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return GetDetailByUser(tokenDetail?.UserID ?? 0);
        }
        
        public IDictionary<string, object> GetDetailByUser(long userId)
        {
            var dictionaryValues = new Dictionary<string, object>();
            var userMilestones = _unitOfWork.UserMilestoneRepository
                .GetMany(u => u.UserID == userId).ToList();
            dictionaryValues.Add("UserMilestone", userMilestones);
            var milestones = (from m in _unitOfWork.MilestoneRepository.GetMany(m => m.Active)
                select new Milestone()
                {
                    ID = m.MilestoneId,
                    MilestoneId = m.MilestoneId,
                    LanguageCode = m.LanguageCode,
                    Active = m.Active,
                    AchievedMessage = m.AchievedMessage,
                    MilestoneSubTitle = m.MilestoneSubTitle,
                    MilestoneTitle = m.MilestoneTitle,
                    TargetValue = m.TargetValue
                }).ToList();
            dictionaryValues.Add("Milestone", milestones);
            return dictionaryValues;
        }

        public async Task<IDictionary<string, object>> GetDetailByUserAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetDetailByUserAsync(tokenDetail?.UserID ?? 0);
        }
        
        public async Task<IDictionary<string, object>> GetDetailByUserAsync(long userId)
        {
            var dictionaryValues = new Dictionary<string, object>();
            var userMilestones = (await _unitOfWork.UserMilestoneRepository
                .GetManyAsync(u => u.UserID == userId)).ToList();
            dictionaryValues.Add("UserMilestone", userMilestones);
            var milestones = (from m in await _unitOfWork.MilestoneRepository.GetManyAsync(m => m.Active)
                select new Milestone()
                {
                    ID = m.MilestoneId,
                    MilestoneId = m.MilestoneId,
                    LanguageCode = m.LanguageCode,
                    Active = m.Active,
                    AchievedMessage = m.AchievedMessage,
                    MilestoneSubTitle = m.MilestoneSubTitle,
                    MilestoneTitle = m.MilestoneTitle,
                    TargetValue = m.TargetValue
                }).ToList();
            dictionaryValues.Add("Milestone", milestones);
            return dictionaryValues;
        }

        public IEnumerable<UserMilestone> GetByUser(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return GetByUser(tokenDetail?.UserID ?? 0);
        }

        public IEnumerable<UserMilestone> GetByUser(long userId)
        {
            return _unitOfWork.UserMilestoneRepository.GetMany(u => u.UserID == userId);
        }

        public async Task<IEnumerable<UserMilestone>> GetByUserAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetByUserAsync(tokenDetail?.UserID ?? 0);
        }

        public async Task<IEnumerable<UserMilestone>> GetByUserAsync(long userId)
        {
            return await _unitOfWork.UserMilestoneRepository.GetManyAsync(u => u.UserID == userId);
        }

        public UserMilestoneExtended GetExtendedByUser(string token)
        {
            return GetExtendedByUser(_unitOfWork.UserTokenRepository.GetFirst(t => t.AuthToken == token)?.UserID ?? 0);
        }

        public UserMilestoneExtended GetExtendedByUser(long userId)
        {
            return new UserMilestoneExtended()
            {
                UserMilestones = _unitOfWork.UserMilestoneRepository.GetMany(u => u.UserID == userId),
                MilestonePercentages = (from p in _unitOfWork.MilestonePercentageRepository.GetMany(p => p.Active)
                    select new MilestonePercentage()
                    {
                        ID = p.MilestonePercentageId,
                        MilestonePercentageId = p.MilestonePercentageId,
                        LanguageCode = p.LanguageCode,
                        Active = p.Active,
                        MilestoneSubTitle = p.MilestoneSubTitle,
                        AchievedMessage = p.AchievedMessage,
                        MilestoneTitle = p.MilestoneTitle,
                        AchievementGiftImage = p.AchievementGiftImage,
                        AchievementIcon = p.AchievementIcon,
                        IsPercent = p.IsPercent,
                        TargetPercentValue = p.TargetPercentValue
                    }).ToList(),
                Milestones = (from m in _unitOfWork.MilestoneRepository.GetMany(m => m.Active)
                    select new Milestone()
                    {
                        ID = m.MilestoneId,
                        MilestoneId = m.MilestoneId,
                        LanguageCode = m.LanguageCode,
                        Active = m.Active,
                        AchievedMessage = m.AchievedMessage,
                        MilestoneSubTitle = m.MilestoneSubTitle,
                        MilestoneTitle = m.MilestoneTitle,
                        TargetValue = m.TargetValue
                    }).ToList()
            };
        }

        public async Task<UserMilestoneExtended> GetExtendedByUserAsync(string token)
        {
            return await GetExtendedByUserAsync(
                (await _unitOfWork.UserTokenRepository.GetFirstAsync(t => t.AuthToken == token))?.UserID ?? 0);
        }

        public async Task<UserMilestoneExtended> GetExtendedByUserAsync(long userId)
        {
            return new UserMilestoneExtended()
            {
                UserMilestones = await _unitOfWork.UserMilestoneRepository.GetManyAsync(u => u.UserID == userId),
                MilestonePercentages = (from p in _unitOfWork.MilestonePercentageRepository.GetMany(p => p.Active)
                    select new MilestonePercentage()
                    {
                        ID = p.MilestonePercentageId,
                        MilestonePercentageId = p.MilestonePercentageId,
                        LanguageCode = p.LanguageCode,
                        Active = p.Active,
                        MilestoneSubTitle = p.MilestoneSubTitle,
                        AchievedMessage = p.AchievedMessage,
                        MilestoneTitle = p.MilestoneTitle,
                        AchievementGiftImage = p.AchievementGiftImage,
                        AchievementIcon = p.AchievementIcon,
                        IsPercent = p.IsPercent,
                        TargetPercentValue = p.TargetPercentValue
                    }).ToList(),
                Milestones = (from m in _unitOfWork.MilestoneRepository.GetMany(m => m.Active)
                    select new Milestone()
                    {
                        ID = m.MilestoneId,
                        MilestoneId = m.MilestoneId,
                        LanguageCode = m.LanguageCode,
                        Active = m.Active,
                        AchievedMessage = m.AchievedMessage,
                        MilestoneSubTitle = m.MilestoneSubTitle,
                        MilestoneTitle = m.MilestoneTitle,
                        TargetValue = m.TargetValue
                    }).ToList()
            };
        }

        public UserMilestoneExtended GetExtendedByUser(string token, string languageCode)
        {
            return GetExtendedByUser(_unitOfWork.UserTokenRepository.GetFirst(t => t.AuthToken == token)?.UserID ?? 0);
        }

        public UserMilestoneExtended GetExtendedByUser(long userId, string languageCode)
        {
            return new UserMilestoneExtended()
            {
                UserMilestones = _unitOfWork.UserMilestoneRepository.GetMany(u => u.UserID == userId),
                MilestonePercentages = (from p in _unitOfWork.MilestonePercentageRepository.GetMany(p =>
                        p.Active && p.LanguageCode.Trim().ToLower() == languageCode.Trim().ToLower())
                    select new MilestonePercentage()
                    {
                        ID = p.MilestonePercentageId,
                        MilestonePercentageId = p.MilestonePercentageId,
                        LanguageCode = p.LanguageCode,
                        Active = p.Active,
                        MilestoneSubTitle = p.MilestoneSubTitle,
                        AchievedMessage = p.AchievedMessage,
                        MilestoneTitle = p.MilestoneTitle,
                        AchievementGiftImage = p.AchievementGiftImage,
                        AchievementIcon = p.AchievementIcon,
                        IsPercent = p.IsPercent,
                        TargetPercentValue = p.TargetPercentValue
                    }).ToList(),
                Milestones = (from m in _unitOfWork.MilestoneRepository.GetMany(m =>
                        m.Active && m.LanguageCode.Trim().ToLower() == languageCode.Trim().ToLower())
                    select new Milestone()
                    {
                        ID = m.MilestoneId,
                        MilestoneId = m.MilestoneId,
                        LanguageCode = m.LanguageCode,
                        Active = m.Active,
                        AchievedMessage = m.AchievedMessage,
                        MilestoneSubTitle = m.MilestoneSubTitle,
                        MilestoneTitle = m.MilestoneTitle,
                        TargetValue = m.TargetValue
                    }).ToList()
            };
        }

        public async Task<UserMilestoneExtended> GetExtendedByUserAsync(string token, string languageCode)
        {
            return await GetExtendedByUserAsync(
                (await _unitOfWork.UserTokenRepository.GetFirstAsync(t => t.AuthToken == token))?.UserID ?? 0);
        }

        public async Task<UserMilestoneExtended> GetExtendedByUserAsync(long userId, string languageCode)
        {
            return new UserMilestoneExtended()
            {
                UserMilestones = await _unitOfWork.UserMilestoneRepository.GetManyAsync(u => u.UserID == userId),
                MilestonePercentages = (from p in _unitOfWork.MilestonePercentageRepository.GetMany(p =>
                        p.Active && p.LanguageCode.Trim().ToLower() == languageCode.Trim().ToLower())
                    select new MilestonePercentage()
                    {
                        ID = p.MilestonePercentageId,
                        MilestonePercentageId = p.MilestonePercentageId,
                        LanguageCode = p.LanguageCode,
                        Active = p.Active,
                        MilestoneSubTitle = p.MilestoneSubTitle,
                        AchievedMessage = p.AchievedMessage,
                        MilestoneTitle = p.MilestoneTitle,
                        AchievementGiftImage = p.AchievementGiftImage,
                        AchievementIcon = p.AchievementIcon,
                        IsPercent = p.IsPercent,
                        TargetPercentValue = p.TargetPercentValue
                    }).ToList(),
                Milestones = (from m in _unitOfWork.MilestoneRepository.GetMany(m =>
                        m.Active && m.LanguageCode.Trim().ToLower() == languageCode.Trim().ToLower())
                    select new Milestone()
                    {
                        ID = m.MilestoneId,
                        MilestoneId = m.MilestoneId,
                        LanguageCode = m.LanguageCode,
                        Active = m.Active,
                        AchievedMessage = m.AchievedMessage,
                        MilestoneSubTitle = m.MilestoneSubTitle,
                        MilestoneTitle = m.MilestoneTitle,
                        TargetValue = m.TargetValue
                    }).ToList()
            };
        }

        public bool Insert(ref ValidationErrors validationErrors, UserMilestone entity)
        {
            throw new NotImplementedException();
        }

        public bool Insert(ref ValidationErrors validationErrors, string token, UserMilestone entity)
        {
            var user = _tokensServices.GetDetailByToken(token);
            if (user != null)
            {
                return Insert(ref validationErrors, user.UserID, entity);
            }

            return false;
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, UserMilestone entity)
        {
            entity.UserID = userId;
            entity.AchieveDate = DateTime.Now;
            dynamic[] obj = {entity, true};
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.UserMilestoneRepository.Insert(entity);
                return _unitOfWork.Commit();
            }
            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, UserMilestone entity)
        {
            throw new NotImplementedException();
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var required = (bool) objValue[1];
            var obj = (UserMilestone) objValue[0];

            if (required)
            {
                if (obj.IsPercentage && obj.MilestonePercentageId == 0)
                {
                    validationErrors.Add("MilestonePercentageIDRequired");
                }
                else if (!obj.IsPercentage && obj.MilestoneID == 0)
                {
                    validationErrors.Add("MilestoneIDRequired");
                }
                else if (obj.IsPercentage && _unitOfWork.UserMilestoneRepository.GetMany(m =>
                             m.UserID == obj.UserID && m.MilestonePercentageId == obj.MilestonePercentageId).Count() >
                         0)
                {
                    validationErrors.Add("MilestonePercentageIDExists");
                }
                else if (!obj.IsPercentage && _unitOfWork.UserMilestoneRepository
                             .GetMany(m => m.UserID == obj.UserID && m.MilestoneID == obj.MilestoneID).Count() > 0)
                {
                    validationErrors.Add("MilestoneIDExists");
                }
            }

            return validationErrors.Count() == 0;
        }
    }
}