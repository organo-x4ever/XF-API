
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Model.EnumerationTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IMenuServices : IDefaultServices<Menu>
    {
        IEnumerable<Menu> GetByApplication(string applicationKey);
        Task<IEnumerable<Menu>> GetByApplicationAsync(string applicationKey);
        IEnumerable<Menu> GetByApplication(string applicationKey, PlatformType platformType);
        Task<IEnumerable<Menu>> GetByApplicationAsync(string applicationKey, PlatformType platformType);
        Task<IEnumerable<Menu>> GetByApplicationAsync(string applicationKey, PlatformType platformType,int version);
    }
}