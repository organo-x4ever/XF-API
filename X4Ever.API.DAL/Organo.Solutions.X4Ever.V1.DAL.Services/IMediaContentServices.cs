using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IMediaContentServices
    {
        IEnumerable<MediaContent> Get();
        Task<IEnumerable<MediaContent>> GetAsync();

        IEnumerable<MediaContentDetail> GetDetail();
        Task<IEnumerable<MediaContentDetail>> GetDetailAsync();
    }
}