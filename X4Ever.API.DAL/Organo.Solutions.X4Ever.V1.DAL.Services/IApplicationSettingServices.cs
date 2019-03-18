using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IApplicationSettingServices
    {
        ApplicationSetting Get(string applicationKey, string languageCode);

        Task<ApplicationSetting> GetAsync(string applicationKey, string languageCode);
    }
}