using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserSettingServices : IUserSettingServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IUserTokensServices _tokensServices;

        public UserSettingServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokensServices = new UserTokensServices(unitOfWork);
        }

        public bool Delete(ref ValidationErrors validationErrors, string token)
        {
            throw new NotImplementedException();
            //var userSetting = GetByToken(token);

            //if (!(userSetting is null))
            //{
            //    this._unitOfWork.UserSettingRepository.Delete(userSetting);
            //    return _unitOfWork.Commit();
            //}
            //else
            //    validationErrors.Add("MessageInvalidProvidedInformation");

            //return validationErrors.Count() == 0;
        }
        public bool Delete(ref ValidationErrors validationErrors, long userId)
        {
            throw new NotImplementedException();
        }

        public UserSetting Get(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return Get(tokenDetail?.UserID ?? 0);
        }
        
        public UserSetting Get(long userId)
        {
            return _unitOfWork.UserSettingRepository.Get(u => u.UserID == userId);
        }

        public async Task<UserSetting> GetAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetAsync(tokenDetail?.UserID ?? 0);
        }
        
        public async Task<UserSetting> GetAsync(long userId)
        {
            return await _unitOfWork.UserSettingRepository.GetAsync(u => u.UserID == userId);
        }

        public bool Insert(ref ValidationErrors validationErrors, string token, UserSetting entity)
        {
            var tokenData = Get(token);
            return Insert(ref validationErrors, tokenData?.UserID ?? 0, entity);
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, UserSetting entity)
        {
            var date = DateTime.Now;
            if (entity != null)
            {
                entity.UserID = userId;
                entity.ModifyDate = date;
            }

            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                var record = _unitOfWork.UserSettingRepository.GetMany(m => m.UserID == entity.UserID).LastOrDefault();
                if (record != null)
                {
                    record.UserID = entity.UserID;
                    record.LanguageCode = (entity.LanguageCode != null ? entity.LanguageCode : record.LanguageCode);
                    record.LanguageDetail =
                        (entity.LanguageDetail != null ? entity.LanguageDetail : record.LanguageDetail);
                    record.WeightVolumeType = (entity.WeightVolumeType != null
                        ? entity.WeightVolumeType
                        : record.WeightVolumeType);
                    record.ModifyDate = date;
                    _unitOfWork.UserSettingRepository.Update(record);
                }
                else
                    _unitOfWork.UserSettingRepository.Insert(entity);

                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, string token, UserSetting entity)
        {
            var tokenData = Get(token);
            return Update(ref validationErrors, tokenData?.UserID ?? 0, entity);
        }
        
        public bool Update(ref ValidationErrors validationErrors, long userId, UserSetting entity)
        {
            var date = DateTime.Now;
            if (entity != null)
            {
                entity.UserID = userId;
                entity.ModifyDate = date;
            }

            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                var record = _unitOfWork.UserSettingRepository.GetMany(m => m.UserID == entity.UserID).LastOrDefault();
                if (record != null)
                {
                    record.UserID = entity.UserID;
                    record.LanguageCode = (entity.LanguageCode != null ? entity.LanguageCode : record.LanguageCode);
                    record.LanguageDetail =
                        (entity.LanguageDetail != null ? entity.LanguageDetail : record.LanguageDetail);
                    record.WeightVolumeType = (entity.WeightVolumeType != null
                        ? entity.WeightVolumeType
                        : record.WeightVolumeType);
                    record.ModifyDate = date;
                    _unitOfWork.UserSettingRepository.Update(record);
                }
                else
                    _unitOfWork.UserSettingRepository.Insert(entity);

                return _unitOfWork.Commit();
            }

            return false;
        }
        public bool Exists(Int64 userId)
        {
            return _unitOfWork.UserSettingRepository.GetMany(m => m.UserID == userId).Any();
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var obj = (UserSetting) objValue[0];
            if (obj.UserID == 0)
            {
                validationErrors.Add("UserIDRequired");
            }

            return validationErrors.Count() == 0;
        }

        public bool UpdateUserLanguage(ref ValidationErrors validationErrors, string token, string languageCode,
            string languageDetail)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            if (tokenDetail == null)
                validationErrors.Add("MessageTokenExpired");
            if (validationErrors.Count() == 0)
            {
                return UpdateUserLanguage(ref validationErrors, tokenDetail?.UserID ?? 0, languageCode, languageDetail);
            }

            return false;
        }

        public bool UpdateUserLanguage(ref ValidationErrors validationErrors, long userId, string languageCode,
            string languageDetail)
        {
            var date = DateTime.Now;
            if (languageCode == null || languageCode.Trim().Length < 2 || languageCode.Trim().Length > 10)
                //|| !languageCode.Contains("-"))
                validationErrors.Add("MessageInvalidLanguageCode");
            if (languageDetail == null || languageDetail.Trim().Length < 5 || languageDetail.Trim().Length > 100)
                validationErrors.Add("MessageInvalidLanguageName");
            if (validationErrors.Count() == 0)
            {
                var record = _unitOfWork.UserSettingRepository.Get(u => u.UserID == userId);
                if (record != null)
                {
                    record.LanguageCode = languageCode.Trim();
                    record.LanguageDetail = languageDetail.Trim();
                    record.ModifyDate = date;
                    _unitOfWork.UserSettingRepository.Update(record);
                }
                else
                {
                    _unitOfWork.UserSettingRepository.Insert(new UserSetting()
                    {
                        UserID = userId,
                        LanguageCode = languageCode.Trim(),
                        LanguageDetail = languageDetail.Trim(),
                        ModifyDate = date
                    });
                }

                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool UpdateUserWeightVolume(ref ValidationErrors validationErrors, string token, string weightVolume)
        {

            var tokenDetail = _tokensServices.GetDetailByToken(token);
            if (tokenDetail == null)
                validationErrors.Add("MessageTokenExpired");

            if (validationErrors.Count() == 0)
            {
                return UpdateUserWeightVolume(ref validationErrors, tokenDetail?.UserID ?? 0, weightVolume);
            }

            return false;
        }

        public bool UpdateUserWeightVolume(ref ValidationErrors validationErrors, long userId, string weightVolume)
        {
            var date = DateTime.Now;
            if (weightVolume == null || weightVolume.Trim().Length == 0 || weightVolume.Trim().Length > 10)
                validationErrors.Add("MessageInvalidWeightVolume");
            if (validationErrors.Count() == 0)
            {
                var record = _unitOfWork.UserSettingRepository.Get(u => u.UserID == userId);
                if (record != null)
                {
                    record.WeightVolumeType = weightVolume.Trim();
                    record.ModifyDate = date;
                    _unitOfWork.UserSettingRepository.Update(record);
                }
                else
                {
                    _unitOfWork.UserSettingRepository.Insert(new UserSetting()
                    {
                        UserID = userId,
                        WeightVolumeType = weightVolume.Trim(),
                        ModifyDate = date
                    });
                }

                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Insert(ref ValidationErrors validationErrors, UserSetting entity)
        {
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.UserSettingRepository.Insert(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public async Task<UserSetting> GetAsync(Func<UserSetting, bool> where)
        {
            return await _unitOfWork.UserSettingRepository.GetAsync(where);
        }

        public UserSetting Get(Func<UserSetting, bool> where)
        {
            return _unitOfWork.UserSettingRepository.Get(where);
        }

    }
}