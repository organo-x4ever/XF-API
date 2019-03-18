using Organo.Solutions.X4Ever.V1.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserServices
    {
        long Authenticate(string userName, string password);
        Task<long> AuthenticateAsync(string userName, string password);
        IEnumerable<UserGlobal> Get();
        Task<IEnumerable<UserGlobal>> GetAsync();
        IEnumerable<UserGlobal> Get(bool showEmptyRecords = false);
        Task<IEnumerable<UserGlobal>> GetAsync(bool showEmptyRecords = false);

        IEnumerable<UserGlobal> Get(Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null, string includeProperties = "");

        Task<IEnumerable<UserGlobal>> GetAsync(Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null, string includeProperties = "");
        UserGlobal Get(long ID);
        Task<UserGlobal> GetAsync(long ID);
        UserGlobal Get(string token);
        Task<UserGlobal> GetAsync(string token);

        User GetByEmail(string email);
        Task<User> GetByEmailAsync(string email);
        UserGlobal Get(string token, string metaKey);
        Task<UserGlobal> GetAsync(string token, string metaKey);
        UserGlobal Get(long id, string metaKey);
        Task<UserGlobal> GetAsync(long id, string metaKey);
        UserAuthentication GetUserAuthentication(string token);
        Task<UserAuthentication> GetUserAuthenticationAsync(string token);
        UserAuthentication GetUserAuthentication(long id);
        Task<UserAuthentication> GetUserAuthenticationAsync(long id);
        UserGlobal Get(long id, string metaKey, string application);
        Task<UserGlobal> GetAsync(long id, string metaKey, string application);
        UserGlobal GetFullUser(string token);
        Task<UserGlobal> GetFullUserAsync(string token);
        UserGlobal GetFullUser(long userId);
        Task<UserGlobal> GetFullUserAsync(long userId);
        bool Insert(ref ValidationErrors validationErrors, User entity);
        bool Insert(ref ValidationErrors validationErrors, User entity, ICollection<UserMeta> userMetas);
        bool Delete(ref ValidationErrors validationErrors, long userID, string password);
        bool Update(ref ValidationErrors validationErrors, string token, User entity);
        bool Update(ref ValidationErrors validationErrors, long userId, User entity);
        bool Update(ref ValidationErrors validationErrors, string token, string application);
        bool Update(ref ValidationErrors validationErrors, long userId, string application);
        bool Update(ref ValidationErrors validationErrors, string token, User entity, ICollection<UserMeta> userMetas);
        bool Update(ref ValidationErrors validationErrors, long userId, User entity, ICollection<UserMeta> userMetas);
        bool UpdateUserType(ref ValidationErrors validationErrors, string token, User entity);
        bool UpdateUserType(ref ValidationErrors validationErrors, long userId, User entity);
        bool UpdateUserStatus(ref ValidationErrors validationErrors, string token, User entity);
        bool UpdateUserStatus(ref ValidationErrors validationErrors, long userId, User entity);
        bool UpdateUserTypeAndStatus(ref ValidationErrors validationErrors, string token, User entity);
        bool UpdateUserTypeAndStatus(ref ValidationErrors validationErrors, long userId, User entity);
        bool ForgotPasswordRequest(ref ValidationErrors validationErrors, string username, string email);

        User ForgotPasswordRequest(ref ValidationErrors validationErrors, string username, string email,
            out string requestCode);

        bool ChangeForgotPassword(ref ValidationErrors validationErrors, string requestCode, string password);

        bool ChangePassword(ref ValidationErrors validationErrors, string token, string currentPassword,
            string password);

        bool ChangePassword(ref ValidationErrors validationErrors, long userId, string currentPassword,
            string password);

        bool UpdateUserStatus(ref ValidationErrors validationErrors, string activation_code);
        List<LoginCredential> GetLoginCredential(string username);
        Task<List<LoginCredential>> GetLoginCredentialAsync(string username);

        //string UpdateDirectPassword(string email, string password);
    }
}