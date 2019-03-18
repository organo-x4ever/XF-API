using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IMediaCategoryServices
    {
        IEnumerable<MediaCategory> Get();
        Task<IEnumerable<MediaCategory>> GetAsync();
    }
}