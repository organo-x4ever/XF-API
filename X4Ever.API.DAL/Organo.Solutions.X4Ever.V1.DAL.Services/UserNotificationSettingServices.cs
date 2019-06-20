
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserNotificationSettingServices : IUserNotificationSettingServices
    {
        private IUnitOfWork _unitOfWork;
        public UserNotificationSettingServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserNotificationSetting Get(long userId)
        {
            return _unitOfWork.UserNotificationSettingRepository.Get(n => n.UserID == userId);
        }

        public async Task<UserNotificationSetting> GetAsync(long userId)
        {
            return await _unitOfWork.UserNotificationSettingRepository.GetAsync(n => n.UserID == userId);
        }

        public async Task<UserNotificationSettingViewModel> GetNewAsync(long userId)
        {
            var settings = await _unitOfWork.UserNotificationSettingRepository.GetAsync(n => n.UserID == userId);
            if (settings != null)
            {
                return new UserNotificationSettingViewModel()
                {
                    UserID = settings.UserID,
                    IsWeightSubmitReminder = settings.IsWeightSubmitReminder,
                    CreateDate = settings.CreateDate,
                    ID = settings.ID,
                    Intimation = settings.Intimation,
                    IsGeneralMessage = settings.IsGeneralMessage,
                    IsPromotional = settings.IsPromotional,
                    IsSpecialOffer = settings.IsSpecialOffer,
                    IsVersionUpdate = settings.IsVersionUpdate,
                    ModifyDate = settings.ModifyDate,
                    IsVisibleGeneralMessage = true,
                    IsVisibleIntimation = true,
                    IsVisiblePromotional = true,
                    IsVisibleSpecialOffer = true,
                    IsVisibleVersionUpdate = true,
                    IsVisibleWeightSubmitReminder = true
                };
            }
            return null;
        }

        public bool Insert(ref ValidationErrors validationErrors, UserNotificationSetting userNotification)
        {
            if (Validate(ref validationErrors, new object[] { userNotification }))
            {   
                userNotification.CreateDate = DateTime.Now;
                userNotification.ModifyDate = DateTime.Now;
                _unitOfWork.UserNotificationSettingRepository.Insert(userNotification);
                return _unitOfWork.Commit();
            }
            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, UserNotificationSetting userNotification)
        {
            if (Validate(ref validationErrors, new object[] { userNotification }))
            {
                var entity = Get(userId: userNotification.UserID);
                if (entity == null)
                {
                    return Insert(ref validationErrors, userNotification);
                }
                else
                {
                    entity.Intimation = userNotification.Intimation;
                    entity.IsGeneralMessage = userNotification.IsGeneralMessage;
                    entity.IsPromotional= userNotification.IsPromotional;
                    entity.IsSpecialOffer = userNotification.IsSpecialOffer;
                    entity.IsVersionUpdate = userNotification.IsVersionUpdate;
                    entity.IsWeightSubmitReminder = userNotification.IsWeightSubmitReminder;

                    entity.ModifyDate = DateTime.Now;
                    _unitOfWork.UserNotificationSettingRepository.Update(entity);
                }
                return _unitOfWork.Commit();
            }
            return false;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var entity = (UserNotificationSetting)objValue[0];
            if (entity == null)
                validationErrors.Add("The Notification Setting object can not be null");
            else if (string.IsNullOrEmpty(entity.UserID.ToString()))
                validationErrors.Add("User Token can not be null or empty");
            else if (!long.TryParse(entity.UserID.ToString(), out long userId) && userId == 0)
                validationErrors.Add("User ID can not be null or empty or zero");
            return validationErrors.Count() == 0;
        }
    }
}
