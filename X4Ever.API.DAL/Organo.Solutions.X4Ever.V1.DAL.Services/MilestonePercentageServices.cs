using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class MilestonePercentageServices : IMilestonePercentageServices
    {
        private IUnitOfWork _unitOfWork;

        public MilestonePercentageServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool Delete(ref ValidationErrors validationErrors, MilestonePercentage entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MilestonePercentage> Get()
        {
            return (from p in _unitOfWork.MilestonePercentageRepository.GetMany(p => p.Active)
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
                }).ToList();
        }

        public async Task<IEnumerable<MilestonePercentage>> GetAsync()
        {
            return (from p in await _unitOfWork.MilestonePercentageRepository.GetManyAsync(p => p.Active)
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
                }).ToList();
        }

        public IEnumerable<MilestonePercentage> Get(Expression<Func<MilestonePercentage, bool>> filter = null,
            Func<IQueryable<MilestonePercentage>, IOrderedQueryable<MilestonePercentage>> orderBy = null,
            string includeProperties = "")
        {
            return (from p in _unitOfWork.MilestonePercentageRepository.GetMany(filter, orderBy, includeProperties)
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
                }).ToList();
        }

        public async Task<IEnumerable<MilestonePercentage>> GetAsync(
            Expression<Func<MilestonePercentage, bool>> filter = null,
            Func<IQueryable<MilestonePercentage>, IOrderedQueryable<MilestonePercentage>> orderBy = null,
            string includeProperties = "")
        {
            return (from p in await _unitOfWork.MilestonePercentageRepository.GetManyAsync(filter, orderBy,
                    includeProperties)
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
                }).ToList();
        }

        public MilestonePercentage Get(string ID)
        {
            throw new NotImplementedException();
        }

        public Task<MilestonePercentage> GetAsync(string ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MilestonePercentage> GetByLanguage(string languageCode)
        {
            return (from p in _unitOfWork.MilestonePercentageRepository.GetMany(p =>
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
                }).OrderByDescending(m => m.TargetPercentValue).ToList();
        }

        public async Task<IEnumerable<MilestonePercentage>> GetByLanguageAsync(string languageCode)
        {
            return (from p in await _unitOfWork.MilestonePercentageRepository.GetManyAsync(p =>
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
                }).OrderByDescending(m => m.TargetPercentValue).ToList();
        }

        public bool Insert(ref ValidationErrors validationErrors, MilestonePercentage entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(ref ValidationErrors validationErrors, MilestonePercentage entity)
        {
            throw new NotImplementedException();
        }

        public MilestonePercentage GetLast(Func<MilestonePercentage, bool> predicate = null)
        {
            return _unitOfWork.MilestonePercentageRepository.GetLast(predicate);
        }

        public async Task<MilestonePercentage> GetLastAsync(Func<MilestonePercentage, bool> predicate = null)
        {
            return await _unitOfWork.MilestonePercentageRepository.GetLastAsync(predicate);
        }
    }
}