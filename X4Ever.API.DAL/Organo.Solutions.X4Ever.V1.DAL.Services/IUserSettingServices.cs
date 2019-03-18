using System;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserSettingServices
    {
        UserSetting Get(Func<UserSetting, bool> where);
        Task<UserSetting> GetAsync(Func<UserSetting, bool> where);
        bool Delete(ref ValidationErrors validationErrors, string token);
        bool Delete(ref ValidationErrors validationErrors, long userId);

        UserSetting Get(string token);
        UserSetting Get(long userId);
        Task<UserSetting> GetAsync(string token);
        Task<UserSetting> GetAsync(long userId);
        bool Insert(ref ValidationErrors validationErrors, UserSetting entity);
        bool Insert(ref ValidationErrors validationErrors, string token, UserSetting entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, UserSetting entity);

        bool Update(ref ValidationErrors validationErrors, string token, UserSetting entity);
        bool Update(ref ValidationErrors validationErrors, long userId, UserSetting entity);

        bool UpdateUserLanguage(ref ValidationErrors validationErrors, string token, string languageCode,
            string languageDetail);

        bool UpdateUserLanguage(ref ValidationErrors validationErrors, long userId, string languageCode,
            string languageDetail);

        bool UpdateUserWeightVolume(ref ValidationErrors validationErrors, string token, string weightVolume);
        bool UpdateUserWeightVolume(ref ValidationErrors validationErrors, long userId, string weightVolume);
    }
}