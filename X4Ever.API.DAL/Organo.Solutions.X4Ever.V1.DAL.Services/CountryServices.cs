using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class CountryServices : ICountryServices, IValidation
    {
        private IUnitOfWork _unitOfWork;

        public CountryServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool Delete(ref ValidationErrors validationErrors, Country entity)
        {
            this._unitOfWork.CountryRepository.Delete(entity);
            return this._unitOfWork.Commit();
        }

        public IEnumerable<Country> Get()
        {
            return this._unitOfWork.CountryRepository.GetAll();
        }

        public async Task<IEnumerable<Country>> GetAsync()
        {
            return await this._unitOfWork.CountryRepository.GetAllAsync();
        }

        public IEnumerable<Country> Get(Expression<Func<Country, bool>> filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>> orderBy = null, string includeProperties = "")
        {
            return this._unitOfWork.CountryRepository.GetMany(filter, orderBy, includeProperties);
        }

        public async Task<IEnumerable<Country>> GetAsync(Expression<Func<Country, bool>> filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>> orderBy = null, string includeProperties = "")
        {
            return await this._unitOfWork.CountryRepository.GetManyAsync(filter, orderBy, includeProperties);
        }

        public Country Get(string ID)
        {
            return this._unitOfWork.CountryRepository.GetByID(ID);
        }

        public async Task<Country> GetAsync(string ID)
        {
            return await this._unitOfWork.CountryRepository.GetByIDAsync(ID);
        }

        public Country Get(int ID)
        {
            return this._unitOfWork.CountryRepository.GetByID(ID);
        }

        public async Task<Country> GetAsync(int ID)
        {
            return await this._unitOfWork.CountryRepository.GetByIDAsync(ID);
        }


        public bool Insert(ref ValidationErrors validationErrors, Country entity)
        {
            var success = false;
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.CountryRepository.Insert(entity);
                bool result = _unitOfWork.Commit();
                if (result)
                {
                    success = true;
                }
            }

            return success;
        }

        public bool Update(ref ValidationErrors validationErrors, Country entity)
        {
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.CountryRepository.Update(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            Country obj = (Country) objValue[0];
            if (obj.CountryCode.Trim().Length == 0)
                validationErrors.Add("CountryCodeRequired");
            if (obj.CountryName.Trim().Length == 0)
                validationErrors.Add("CountryNameRequired");
            return validationErrors.Count() == 0;
        }
    }
}