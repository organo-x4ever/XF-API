using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class NewsServices : INewsServices, IValidation
    {
        private IUnitOfWork _unitOfWork;

        public NewsServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Delete(ref ValidationErrors validationErrors, News entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<News> Get()
        {
            return _unitOfWork.NewsRepository.GetAll();
        }

        public async Task<IEnumerable<News>> GetAsync()
        {
            return await _unitOfWork.NewsRepository.GetAllAsync();
        }

        public IEnumerable<News> Get(Expression<Func<News, bool>> filter = null,
            Func<IQueryable<News>, IOrderedQueryable<News>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.NewsRepository.GetMany(filter, orderBy, includeProperties);
        }

        public async Task<IEnumerable<News>> GetAsync(Expression<Func<News, bool>> filter = null, Func<IQueryable<News>, IOrderedQueryable<News>> orderBy = null, string includeProperties = "")
        {
            return await _unitOfWork.NewsRepository.GetManyAsync(filter, orderBy, includeProperties);
        }

        public News Get(string ID)
        {
            return _unitOfWork.NewsRepository.GetByID(ID);
        }

        public async Task<News> GetAsync(string ID)
        {
            return await _unitOfWork.NewsRepository.GetByIDAsync(ID);
        }

        public List<News> GetByLanguageAndByDate(string applicationKey, string languageCode, bool active)
        {
            return _unitOfWork.NewsRepository.GetMany(n =>
                _unitOfWork.ApplicationRepository
                    .GetMany(a => a.ID == n.ApplicationId && a.ApplicationKey == applicationKey).Any()
                && n.LanguageCode.Trim().ToLower() == languageCode.Trim().ToLower()
                && n.Active == active).OrderByDescending(n => n.ModifyDate).ToList();
        }

        public async Task<List<News>> GetByLanguageAndByDateAsync(string applicationKey, string languageCode,
            bool active)
        {
            return (await _unitOfWork.NewsRepository.GetManyAsync(n =>
                _unitOfWork.ApplicationRepository
                    .GetMany(a => a.ID == n.ApplicationId && a.ApplicationKey == applicationKey).Any()
                && n.LanguageCode.Trim().ToLower() == languageCode.Trim().ToLower()
                && n.Active == active)).OrderByDescending(n => n.ModifyDate).ToList();
        }

        public IEnumerable<News> GetOrderByDate(bool active)
        {
            return _unitOfWork.NewsRepository.GetMany(n => n.Active == active).OrderByDescending(n => n.ModifyDate);
        }

        public async Task<IEnumerable<News>> GetOrderByDateAsync(bool active)
        {
            return (await _unitOfWork.NewsRepository.GetManyAsync(n => n.Active == active)).OrderByDescending(n => n.ModifyDate);
        }

        public bool Insert(ref ValidationErrors validationErrors, News entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(ref ValidationErrors validationErrors, News entity)
        {
            throw new NotImplementedException();
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            throw new NotImplementedException();
        }
    }
}