using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class ApplicationSettingServices : IApplicationSettingServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationSettingServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ApplicationSetting Get(string applicationKey, string languageCode)
        {
            return _unitOfWork.ApplicationSettingRepository.GetLast(s =>
                s.ApplicationKey == applicationKey && s.LanguageCode == languageCode);
        }

        public async Task<ApplicationSetting> GetAsync(string applicationKey, string languageCode)
        {
            return await _unitOfWork.ApplicationSettingRepository.GetLastAsync(s =>
                s.ApplicationKey == applicationKey && s.LanguageCode == languageCode);
        }
    }
}