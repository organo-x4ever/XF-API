using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class OpenNotificationUserServices : IOpenNotificationUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelper _helper;

        public OpenNotificationUserServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
        }

        public async Task<IEnumerable<OpenNotificationUser>> GetAsync()
        {
            double.TryParse(_helper.GetAppSetting("WeightSubmitIntervalDays"), out double timeInterval);
            var date = DateTime.Today.AddDays(-(timeInterval));
            return await _unitOfWork.OpenNotificationUserRepository.GetManyAsync(u => u.ModifyDate <= date);
        }

        public async Task<IEnumerable<OpenNotificationUserRegistrationMap>> GetRegisteredUserAfterWeekAsync()
        {
            double.TryParse(_helper.GetAppSetting("WeightSubmitIntervalDays"), out double timeInterval);
            var date = DateTime.Today.AddDays(-(timeInterval));
            return (from o in await _unitOfWork.OpenNotificationUserRegistrationRepository.GetManyAsync(u =>
                    u.CreateDate == date)
                select new OpenNotificationUserRegistrationMap()
                {
                    ID = o.ID,
                    UserID = o.UserID,
                    DeviceToken = o.DeviceToken,
                    CreateDate = o.CreateDate,
                    UserEmail = o.UserEmail,
                    LanguageCode = o.LanguageCode,
                    DevicePlatform = o.DevicePlatform,
                    IsStartToday = o.CreateDate == date
                });
        }

        public async Task<IEnumerable<OpenNotificationOnlyUser>> GetUserOnlyAsync()
        {
            return await _unitOfWork.OpenNotificationOnlyUserRepository.GetAllAsync();
        }

        public async Task<IEnumerable<OpenNotificationOnlyUser>> GetUserOnlyAsync(string[] emailIDs, string[] platforms)
        {
            return await _unitOfWork.OpenNotificationOnlyUserRepository.GetManyAsync(u =>
                (emailIDs.Contains("all") || emailIDs.Contains(u.UserEmail.ToLower())) &&
                (platforms.Contains("email") || platforms.Contains("all") ||
                 platforms.Contains(u.DevicePlatform.ToLower())));
        }

        public async Task<IEnumerable<OpenNotificationUserMap>> GetWeeklyAsync()
        {
            double.TryParse(_helper.GetAppSetting("WeightSubmitIntervalDays"), out double timeInterval);
            var date = DateTime.Today.AddDays(-(timeInterval));

            var userList = await _unitOfWork.OpenNotificationUserRepository.GetManyAsync(u => u.ModifyDate <= date);
            var notifications = await _unitOfWork.UserNotificationSettingRepository.GetManyAsync(n => n.IsWeightSubmitReminder && userList.Any(u => u.UserID == n.UserID));
            return (from o in userList 
                    where notifications.Any(n => n.UserID == o.UserID)
                select new OpenNotificationUserMap()
                {
                    ID = o.ID,
                    UserID = o.UserID,
                    DeviceToken = o.DeviceToken,
                    ModifyDate = o.ModifyDate,
                    UserEmail = o.UserEmail,
                    LanguageCode = o.LanguageCode,
                    DevicePlatform = o.DevicePlatform,
                    IsStartToday = o.ModifyDate == date
                });
        }
    }
}