using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface ICountryServices : IDefaultServices<Country>
    {
        Country Get(int ID);
        Task<Country> GetAsync(int ID);
    }
}