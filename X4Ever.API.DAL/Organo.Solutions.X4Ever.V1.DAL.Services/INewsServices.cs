using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface INewsServices : IDefaultServices<News>
    {
        IEnumerable<News> GetOrderByDate(bool active);
        Task<IEnumerable<News>> GetOrderByDateAsync(bool active);
        List<News> GetByLanguageAndByDate(string applicationKey, string languageCode, bool active);
        Task<List<News>> GetByLanguageAndByDateAsync(string applicationKey, string languageCode, bool active);
    }
}