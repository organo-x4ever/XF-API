using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserPivotServices
    {
        long Authenticate(string userName, string password);
        UserAuthentication GetUser(string token);
        Task<UserAuthentication> GetUserAsync(string token);
        UserAuthentication GetUser(long id);
        Task<UserAuthentication> GetUserAsync(long id);
        Task<UserPivot> GetFullUserAsync(string token);
        Task<UserPivot> GetFullUserAsync(long id);
        bool Update(ref ValidationErrors validationErrors, string token, User entity);
        bool Update(ref ValidationErrors validationErrors, string token, string application);
        bool Update(ref ValidationErrors validationErrors, long id, User entity);
        bool Update(ref ValidationErrors validationErrors, long id, string application);

        bool ChangePassword(ref ValidationErrors validationErrors, string token, string currentPassword,
            string password);

        bool ChangePassword(ref ValidationErrors validationErrors, long id, string currentPassword,
            string password);
    }
}