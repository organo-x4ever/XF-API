using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IMealPlanServices
    {
        IEnumerable<MealPlan> Get();
        Task<IEnumerable<MealPlan>> GetAsync();

        IEnumerable<MealPlan> GetDetail(string languageCode);
        Task<IEnumerable<MealPlan>> GetDetailAsync(string languageCode);

        IEnumerable<MealPlan> GetDetail(string languageCode, bool active);
        Task<IEnumerable<MealPlan>> GetDetailAsync(string languageCode, bool active);
    }
}