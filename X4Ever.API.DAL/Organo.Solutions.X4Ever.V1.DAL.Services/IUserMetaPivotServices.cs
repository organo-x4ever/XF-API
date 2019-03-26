using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserMetaPivotServices
    {
        bool Delete(ref ValidationErrors validationErrors, List<UserMeta> entity);
        MetaPivot GetMeta(string token);
        MetaPivot GetMeta(long userId);
        Task<MetaPivot> GetMetaAsync(string token);
        Task<MetaPivot> GetMetaAsync(long userId);
        Task<MetaPivot> GetMetaAsync(long userId, string weightVolumeType);

        //Task<IEnumerable<UserMeta>> GetByUserAndAttributeAsync(string token, string attributeName);
        bool Insert(ref ValidationErrors validationErrors, string token, UserMeta entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, UserMeta entity);
        bool Insert(ref ValidationErrors validationErrors, string token, ICollection<UserMeta> entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, ICollection<UserMeta> entity);
        bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue);
    }
}