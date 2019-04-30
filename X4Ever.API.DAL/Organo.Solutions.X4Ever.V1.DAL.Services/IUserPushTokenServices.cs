using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserPushTokenServices : IDefaultServices<UserPushToken>
    {
        new UserPushToken Get(string token);
        new Task<UserPushToken> GetAsync(string token);

        UserPushToken Get(long userId);
        Task<UserPushToken> GetAsync(long userId);
        bool InsertInitial(ref ValidationErrors validationErrors, UserPushToken entity);
        bool Insert(ref ValidationErrors validationErrors, string token, UserPushTokenRegister entity);
        bool Insert(ref ValidationErrors validationErrors, string token, UserPushToken entity);

        bool Insert(ref ValidationErrors validationErrors, long userId, UserPushTokenRegister entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, UserPushToken entity);

        bool Update(ref ValidationErrors validationErrors, string token, UserPushToken entity);
        bool Update(ref ValidationErrors validationErrors, long userId, UserPushToken entity);
        bool Update(ref ValidationErrors validationErrors, string userKey, long userId, string userApplication);
        Task<IEnumerable<Notification_UserTracker>> CheckTrackerDue(int userId = 0);
    }
}