using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class MilestoneServices : IMilestoneServices
    {
        private IUnitOfWork _unitOfWork;

        public MilestoneServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Delete(ref ValidationErrors validationErrors, Milestone entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Milestone> Get()
        {
            return (from m in _unitOfWork.MilestoneRepository.GetMany(m => m.Active)
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
        }

        public async Task<IEnumerable<Milestone>> GetAsync()
        {
            return (from m in await _unitOfWork.MilestoneRepository.GetManyAsync(m => m.Active)
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
        }

        public IEnumerable<Milestone> Get(Expression<Func<Milestone, bool>> filter = null,
            Func<IQueryable<Milestone>, IOrderedQueryable<Milestone>> orderBy = null, string includeProperties = "")
        {
            return (from m in _unitOfWork.MilestoneRepository.GetMany(filter, orderBy, includeProperties)
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
        }

        public async Task<IEnumerable<Milestone>> GetAsync(Expression<Func<Milestone, bool>> filter = null,
            Func<IQueryable<Milestone>, IOrderedQueryable<Milestone>> orderBy = null, string includeProperties = "")
        {
            return (from m in await _unitOfWork.MilestoneRepository.GetManyAsync(filter, orderBy, includeProperties)
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
        }

        public Milestone Get(string ID)
        {
            throw new NotImplementedException();
        }

        public Task<Milestone> GetAsync(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Insert(ref ValidationErrors validationErrors, Milestone entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(ref ValidationErrors validationErrors, Milestone entity)
        {
            throw new NotImplementedException();
        }
    }
}