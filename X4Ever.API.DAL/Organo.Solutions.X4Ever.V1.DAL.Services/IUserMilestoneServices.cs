using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserMilestoneServices : IDefaultServices<UserMilestone>
    {
        IDictionary<string, object> GetDetailByUser(string token);
        Task<IDictionary<string, object>> GetDetailByUserAsync(string token);

        IDictionary<string, object> GetDetailByUser(long userId);
        Task<IDictionary<string, object>> GetDetailByUserAsync(long userId);

        IEnumerable<UserMilestone> GetByUser(string token);
        Task<IEnumerable<UserMilestone>> GetByUserAsync(string token);

        IEnumerable<UserMilestone> GetByUser(long userId);
        Task<IEnumerable<UserMilestone>> GetByUserAsync(long userId);

        UserMilestoneExtended GetExtendedByUser(string token);
        Task<UserMilestoneExtended> GetExtendedByUserAsync(string token);

        UserMilestoneExtended GetExtendedByUser(long userId);
        Task<UserMilestoneExtended> GetExtendedByUserAsync(long userId);

        UserMilestoneExtended GetExtendedByUser(string token, string languageCode);
        Task<UserMilestoneExtended> GetExtendedByUserAsync(string token, string languageCode);

        UserMilestoneExtended GetExtendedByUser(long userId, string languageCode);
        Task<UserMilestoneExtended> GetExtendedByUserAsync(long userId, string languageCode);

        bool Insert(ref ValidationErrors validationErrors, string token, UserMilestone entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, UserMilestone entity);
    }
}