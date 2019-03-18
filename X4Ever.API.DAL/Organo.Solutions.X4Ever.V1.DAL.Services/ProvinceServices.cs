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
    public class ProvinceServices : IProvinceServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProvinceServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<CountryProvince> GetProvincesByApplication(string appId)
        {
            return (from c in _unitOfWork.CountryRepository.GetMany(c =>
                    _unitOfWork.ApplicationCountryRepository
                        .GetMany(ac => ac.ApplicationKey == appId && ac.CountryID == c.ID).Any())
                select new CountryProvince()
                {
                    ID = c.ID,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    Provinces = _unitOfWork.ProvinceRepository.GetMany(p => p.CountryId == c.ID).ToList()
                }).ToList();
        }

        public async Task<List<CountryProvince>> GetProvincesByApplicationAsync(string appId)
        {
            return (from c in await _unitOfWork.CountryRepository.GetManyAsync(c =>
                    _unitOfWork.ApplicationCountryRepository
                        .GetMany(ac => ac.ApplicationKey == appId && ac.CountryID == c.ID).Any())
                select new CountryProvince()
                {
                    ID = c.ID,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    Provinces = _unitOfWork.ProvinceRepository.GetMany(p => p.CountryId == c.ID).ToList()
                }).ToList();
        }
    }
}