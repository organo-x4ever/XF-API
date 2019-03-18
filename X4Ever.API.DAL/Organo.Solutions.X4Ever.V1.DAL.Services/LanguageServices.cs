using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class LanguageServices : ILanguageServices, IValidation
    {
        private IUnitOfWork _unitOfWork;

        public LanguageServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool Delete(ref ValidationErrors validationErrors, Language entity)
        {
            this._unitOfWork.LanguageRepository.Delete(entity);
            return this._unitOfWork.Commit();
        }

        public IEnumerable<Language> Get()
        {
            return this._unitOfWork.LanguageRepository.GetAll();
        }

        public async Task<IEnumerable<Language>> GetAsync()
        {
            return await this._unitOfWork.LanguageRepository.GetAllAsync();
        }

        public IEnumerable<Language> Get(Expression<Func<Language, bool>> filter = null,
            Func<IQueryable<Language>, IOrderedQueryable<Language>> orderBy = null, string includeProperties = "")
        {
            return this._unitOfWork.LanguageRepository.GetMany(filter, orderBy, includeProperties);
        }

        public async Task<IEnumerable<Language>> GetAsync(Expression<Func<Language, bool>> filter = null,
            Func<IQueryable<Language>, IOrderedQueryable<Language>> orderBy = null, string includeProperties = "")
        {
            return await this._unitOfWork.LanguageRepository.GetManyAsync(filter, orderBy, includeProperties);
        }

        public Language Get(string ID)
        {
            return this._unitOfWork.LanguageRepository.GetByID(ID);
        }

        public async Task<Language> GetAsync(string ID)
        {
            return await this._unitOfWork.LanguageRepository.GetByIDAsync(ID);
        }

        public List<ApplicationLanguage> GetLanguageByCountryID(string applicationID, int countryID)
        {
            return (from a in this._unitOfWork.ApplicationRepository.GetMany(a => a.ApplicationKey == applicationID)
                join cl in this._unitOfWork.CountryLanguageRepository.GetMany(cl => cl.CountryID == countryID) on a.ID
                    equals cl.ApplicationID
                join c in this._unitOfWork.CountryRepository.GetMany(c => c.ID == countryID) on cl.CountryID equals c.ID
                join l in this._unitOfWork.LanguageRepository.GetAll() on cl.LanguageID equals l.ID
                orderby c.CountryName, l.LanguageName
                select new ApplicationLanguage()
                {
                    CountryID = c.ID,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    CountryFlag = c.CountryFlag,
                    LanguageID = l.ID,
                    LanguageCode = l.LanguageCode,
                    LanguageName = l.LanguageName,
                    ApplicationID = a.ID,
                    LanguageNameEnglish = l.LanguageNameEnglish
                }).ToList();
        }

        public async Task<List<ApplicationLanguage>> GetLanguageByCountryIDAsync(string applicationID, int countryID)
        {
            return (from a in await this._unitOfWork.ApplicationRepository.GetManyAsync(a =>
                    a.ApplicationKey == applicationID)
                join cl in await this._unitOfWork.CountryLanguageRepository.GetManyAsync(
                        cl => cl.CountryID == countryID) on a.ID
                    equals cl.ApplicationID
                join c in await this._unitOfWork.CountryRepository.GetManyAsync(c => c.ID == countryID) on cl.CountryID
                    equals c.ID
                join l in await this._unitOfWork.LanguageRepository.GetAllAsync() on cl.LanguageID equals l.ID
                orderby c.CountryName, l.LanguageName
                select new ApplicationLanguage()
                {
                    CountryID = c.ID,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    CountryFlag = c.CountryFlag,
                    LanguageID = l.ID,
                    LanguageCode = l.LanguageCode,
                    LanguageName = l.LanguageName,
                    ApplicationID = a.ID,
                    LanguageNameEnglish = l.LanguageNameEnglish
                }).ToList();
        }

        public List<ApplicationLanguage> GetLanguageByLanguageID(string applicationID, int languageID)
        {
            return (from a in this._unitOfWork.ApplicationRepository.GetMany(a => a.ApplicationKey == applicationID)
                join cl in this._unitOfWork.CountryLanguageRepository.GetMany(cl => cl.LanguageID == languageID) on a.ID
                    equals cl.ApplicationID
                join c in this._unitOfWork.CountryRepository.GetAll() on cl.CountryID equals c.ID
                join l in this._unitOfWork.LanguageRepository.GetMany(l => l.ID == languageID) on cl.LanguageID equals
                    l.ID
                orderby c.CountryName, l.LanguageName ascending
                select new ApplicationLanguage()
                {
                    CountryID = c.ID,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    CountryFlag = c.CountryFlag,
                    LanguageID = l.ID,
                    LanguageCode = l.LanguageCode,
                    LanguageName = l.LanguageName,
                    ApplicationID = a.ID,
                    LanguageNameEnglish = l.LanguageNameEnglish
                }).ToList();
        }

        public async Task<List<ApplicationLanguage>> GetLanguageByLanguageIDAsync(string applicationID, int languageID)
        {
            return (from a in await this._unitOfWork.ApplicationRepository.GetManyAsync(a =>
                    a.ApplicationKey == applicationID)
                join cl in await this._unitOfWork.CountryLanguageRepository.GetManyAsync(cl =>
                        cl.LanguageID == languageID) on a.ID
                    equals cl.ApplicationID
                join c in await this._unitOfWork.CountryRepository.GetAllAsync() on cl.CountryID equals c.ID
                join l in await this._unitOfWork.LanguageRepository.GetManyAsync(l => l.ID == languageID) on cl
                        .LanguageID equals
                    l.ID
                orderby c.CountryName, l.LanguageName ascending
                select new ApplicationLanguage()
                {
                    CountryID = c.ID,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    CountryFlag = c.CountryFlag,
                    LanguageID = l.ID,
                    LanguageCode = l.LanguageCode,
                    LanguageName = l.LanguageName,
                    ApplicationID = a.ID,
                    LanguageNameEnglish = l.LanguageNameEnglish
                }).ToList();
        }

        public List<ApplicationLanguage> GetLanguages(string applicationID)
        {
            return (from a in this._unitOfWork.ApplicationRepository.GetMany(a => a.ApplicationKey == applicationID)
                join cl in this._unitOfWork.CountryLanguageRepository.GetAll() on a.ID equals cl.ApplicationID
                join c in this._unitOfWork.CountryRepository.GetAll() on cl.CountryID equals c.ID
                join l in this._unitOfWork.LanguageRepository.GetAll() on cl.LanguageID equals l.ID
                orderby a.ApplicationName, c.CountryName, l.LanguageName ascending
                select new ApplicationLanguage()
                {
                    LanguageID = l.ID,
                    LanguageCode = l.LanguageCode,
                    LanguageName = l.LanguageName,
                    ApplicationID = a.ID,
                    LanguageNameEnglish = l.LanguageNameEnglish
                }).ToList();
        }

        public async Task<List<ApplicationLanguage>> GetLanguagesAsync(string applicationID)
        {
            return (from a in await this._unitOfWork.ApplicationRepository.GetManyAsync(a =>
                    a.ApplicationKey == applicationID)
                join cl in await this._unitOfWork.CountryLanguageRepository.GetAllAsync() on a.ID equals cl
                    .ApplicationID
                join c in await this._unitOfWork.CountryRepository.GetAllAsync() on cl.CountryID equals c.ID
                join l in await this._unitOfWork.LanguageRepository.GetAllAsync() on cl.LanguageID equals l.ID
                orderby a.ApplicationName, c.CountryName, l.LanguageName ascending
                select new ApplicationLanguage()
                {
                    LanguageID = l.ID,
                    LanguageCode = l.LanguageCode,
                    LanguageName = l.LanguageName,
                    ApplicationID = a.ID,
                    LanguageNameEnglish = l.LanguageNameEnglish
                }).ToList();
        }

        public List<ApplicationLanguage> GetLanguagesWithCountry(string applicationID)
        {
            return (from a in this._unitOfWork.ApplicationRepository.GetMany(a => a.ApplicationKey == applicationID)
                join cl in this._unitOfWork.CountryLanguageRepository.GetAll() on a.ID equals cl.ApplicationID
                join c in this._unitOfWork.CountryRepository.GetAll() on cl.CountryID equals c.ID
                join l in this._unitOfWork.LanguageRepository.GetAll() on cl.LanguageID equals l.ID
                orderby a.ApplicationName, c.CountryName, l.LanguageName ascending
                select new ApplicationLanguage()
                {
                    CountryID = c.ID,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    CountryFlag = c.CountryFlag,
                    LanguageID = l.ID,
                    LanguageCode = l.LanguageCode,
                    LanguageName = l.LanguageName,
                    ApplicationID = a.ID,
                    LanguageNameEnglish = l.LanguageNameEnglish
                }).ToList();
        }

        public async Task<List<ApplicationLanguage>> GetLanguagesWithCountryAsync(string applicationID)
        {
            return (from a in await this._unitOfWork.ApplicationRepository.GetManyAsync(a =>
                    a.ApplicationKey == applicationID)
                join cl in await this._unitOfWork.CountryLanguageRepository.GetAllAsync() on a.ID equals cl
                    .ApplicationID
                join c in await this._unitOfWork.CountryRepository.GetAllAsync() on cl.CountryID equals c.ID
                join l in await this._unitOfWork.LanguageRepository.GetAllAsync() on cl.LanguageID equals l.ID
                orderby a.ApplicationName, c.CountryName, l.LanguageName ascending
                select new ApplicationLanguage()
                {
                    CountryID = c.ID,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    CountryFlag = c.CountryFlag,
                    LanguageID = l.ID,
                    LanguageCode = l.LanguageCode,
                    LanguageName = l.LanguageName,
                    ApplicationID = a.ID,
                    LanguageNameEnglish = l.LanguageNameEnglish
                }).ToList();
        }

        public bool Insert(ref ValidationErrors validationErrors, Language entity)
        {
            var success = false;
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.LanguageRepository.Insert(entity);
                bool result = _unitOfWork.Commit();
                if (result)
                {
                    success = true;
                }
            }

            return success;
        }

        public bool Update(ref ValidationErrors validationErrors, Language entity)
        {
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.LanguageRepository.Update(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            Language obj = (Language) objValue[0];
            if (obj.LanguageCode.Trim().Length == 0)
                validationErrors.Add("LanguageCodeRequired");
            if (obj.LanguageName.Trim().Length == 0)
                validationErrors.Add("LanguageNameRequired");
            return validationErrors.Count() == 0;
        }
    }
}