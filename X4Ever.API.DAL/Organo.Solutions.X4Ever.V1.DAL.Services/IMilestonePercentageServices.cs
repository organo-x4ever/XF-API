using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IMilestonePercentageServices : IDefaultServices<MilestonePercentage>
    {
        IEnumerable<MilestonePercentage> GetByLanguage(string languageCode);
        Task<IEnumerable<MilestonePercentage>> GetByLanguageAsync(string languageCode);
        MilestonePercentage GetLast(Func<MilestonePercentage, bool> predicate = null);
        Task<MilestonePercentage> GetLastAsync(Func<MilestonePercentage, bool> predicate = null);
    }
}