using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserNotificationServices : IUserNotificationServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserNotificationServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserNotificationView>> GetBodyByIdAsync(long id)
        {
            return (from n in await _unitOfWork.UserNotificationRepository.GetManyAsync(n => n.ID == id)
                select new UserNotificationView()
                {
                    NotificationBody = n.NotificationBody
                });
        }

        public async Task<IEnumerable<UserNotificationView>> GetByDateAsync(DateTime fromDate, DateTime toDate,
            Func<IQueryable<UserNotificationView>, IOrderedQueryable<UserNotificationView>> orderBy = null)
        {
            toDate = toDate.AddDays(1);
            return await _unitOfWork.UserNotificationViewRepository.GetManyAsync(t =>
                t.SentDatetime >= fromDate && t.SentDatetime <= toDate, orderBy);
        }

        public async Task<ValidationErrors> Insert(UserNotification entity)
        {
            return await Task.Factory.StartNew(() =>
            {
                var validationErrors = new ValidationErrors();
                _unitOfWork.UserNotificationRepository.Insert(entity);
                if (!_unitOfWork.Commit())
                    validationErrors.Add(_unitOfWork.OutputLines);
                return validationErrors;
            });
        }

        public async Task<ValidationErrors> Insert(
            long userID = 0,
            string userDetail = "",
            DateTime sentDatetime = new DateTime(),
            string notificationTitle = "",
            string notificationBody = "",
            string statusDescription = "",
            string notificationType = "",
            bool isPush = false,
            bool isScheduled = false,
            string userPlatform = "")
        {
            return await Insert(new UserNotification()
            {
                ID = 0,
                UserID = userID,
                StatusDescription = statusDescription,
                IsPush = isPush,
                IsScheduled = isScheduled,
                NotificationBody = notificationBody,
                NotificationTitle = notificationTitle,
                NotificationType = notificationType,
                SentDatetime = sentDatetime,
                UserDetail = userDetail,
                UserPlatform = userPlatform
            });
        }
    }
}