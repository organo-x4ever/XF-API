namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    using Organo.Solutions.X4Ever.V1.DAL.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserMetaServices : IDefaultServices<UserMeta>
    {
        string GetByMetaKey(string token, string metaKey);
        Task<string> GetByMetaKeyAsync(string token, string metaKey);

        string GetByMetaKey(long userId, string metaKey);
        Task<string> GetByMetaKeyAsync(long userId, string metaKey);

        UserMeta GetByUser(string token, string metaKey);
        Task<UserMeta> GetByUserAsync(string token, string metaKey);

        UserMeta GetByUser(long userId, string metaKey);
        Task<UserMeta> GetByUserAsync(long userId, string metaKey);

        List<UserMeta> GetByUser(string token);
        Task<List<UserMeta>> GetByUserAsync(string token);
        List<UserMeta> GetByUser(long userId);
        Task<List<UserMeta>> GetByUserAsync(long userId);
        Task<UserMetaPivot> GetUserRowByIdAsync(long userId);

        List<UserMeta> GetByUser(string token, string[] keys);
        Task<List<UserMeta>> GetByUserAsync(string token, string[] keys);

        List<UserMeta> GetByUser(long userId, string[] keys);
        Task<List<UserMeta>> GetByUserAsync(long userId, string[] keys);

        bool Insert(ref ValidationErrors validationErrors, string token, UserMeta entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, UserMeta entity);

        bool Insert(ref ValidationErrors validationErrors, string token, List<UserMeta> entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, List<UserMeta> entity);

        bool Delete(ref ValidationErrors validationErrors, List<UserMeta> entity);
    }
}