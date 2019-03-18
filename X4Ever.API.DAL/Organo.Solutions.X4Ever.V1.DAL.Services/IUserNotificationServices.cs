using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserNotificationServices
    {
        Task<ValidationErrors> Insert(UserNotification entity);

        Task<ValidationErrors> Insert(
            long userID = 0,
            string userDetail = "",
            DateTime sentDatetime = new DateTime(),
            string notificationTitle = "",
            string notificationBody = "",
            string statusDescription = "",
            string notificationType = "",
            bool isPush = false,
            bool isScheduled = false,
            string userPlatform = "");

        Task<IEnumerable<UserNotificationView>> GetByDateAsync(DateTime fromDate, DateTime toDate,
            Func<IQueryable<UserNotificationView>, IOrderedQueryable<UserNotificationView>> orderBy = null);

        Task<IEnumerable<UserNotificationView>> GetBodyByIdAsync(long id);
    }
}