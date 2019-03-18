using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IMediaTypeServices
    {
        IEnumerable<MediaType> Get();
        Task<IEnumerable<MediaType>> GetAsync();
    }
}