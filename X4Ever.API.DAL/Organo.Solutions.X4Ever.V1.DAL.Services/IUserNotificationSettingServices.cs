
using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserNotificationSettingServices : IValidation
    {
        UserNotificationSetting Get(long userId);
        Task<UserNotificationSetting> GetAsync(long userId);
        Task<UserNotificationSettingViewModel> GetNewAsync(long userId);
        bool Insert(ref ValidationErrors validationErrors, UserNotificationSetting userNotification);
        bool Update(ref ValidationErrors validationErrors, UserNotificationSetting userNotification);
    } 
}