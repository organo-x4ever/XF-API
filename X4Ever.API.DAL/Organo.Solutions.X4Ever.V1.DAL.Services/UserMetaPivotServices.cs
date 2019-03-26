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
    public class UserMetaPivotServices : IUserMetaPivotServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokensServices _tokensServices;
        private readonly IConverter _converter;

        public UserMetaPivotServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokensServices = new UserTokensServices(unitOfWork);
            _converter = new Converter();
        }

        public bool Delete(ref ValidationErrors validationErrors, List<UserMeta> entity)
        {
            foreach (var record in entity)
            {
                _unitOfWork.UserMetaRepository.Delete(record);
                _unitOfWork.Commit();
            }

            return true;
        }

        public MetaPivot GetMeta(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            if (tokenDetail == null)
                return null;
            return GetMeta(tokenDetail.UserID);
        }
        
        public MetaPivot GetMeta(long ID)
        {
            var metaList = _unitOfWork.UserMetaRepository.GetMany(ut => ut.UserID == ID);
            return metaList.GroupBy(ut => new
                {ut.UserID}).Select(
                ut => new MetaPivot
                {
                    UserId = ut.Key.UserID,
                    WeightVolumeType = ut.Where(u => u.MetaKey.ToLower() == "weightvolumetype")?.FirstOrDefault()
                                           ?.MetaValue ?? "",
                    State = ut.Where(u => u.MetaKey.ToLower() == "state")?.FirstOrDefault()?.MetaValue ?? "",
                    Address = ut.Where(u => u.MetaKey.ToLower() == "address")?.FirstOrDefault()?.MetaValue ?? "",
                    Age = ut.Where(u => u.MetaKey.ToLower() == "age")?.FirstOrDefault()?.MetaValue ?? "",
                    City = ut.Where(u => u.MetaKey.ToLower() == "city")?.FirstOrDefault()?.MetaValue ?? "",
                    Country = ut.Where(u => u.MetaKey.ToLower() == "country")?.FirstOrDefault()?.MetaValue ?? "",
                    Gender = ut.Where(u => u.MetaKey.ToLower() == "gender")?.FirstOrDefault()?.MetaValue ?? "",
                    PostalCode = ut.Where(u => u.MetaKey.ToLower() == "postalcode")?.FirstOrDefault()?.MetaValue ??
                                 "",
                    ProfilePhoto =
                        ut.Where(u => u.MetaKey.ToLower() == "profilephoto")?.FirstOrDefault()?.MetaValue ?? "",
                    WeightLossGoal = ut.Where(u => u.MetaKey.ToLower() == "weightlossgoal")?.FirstOrDefault()
                                         ?.MetaValue ?? "",
                    WeightLossGoalUI = ut.Where(u => u.MetaKey.ToLower() == "weightlossgoal_ui")?.FirstOrDefault()
                                           ?.MetaValue ?? "",
                    ModifyDate = metaList.OrderByDescending(m => m.ModifyDate).FirstOrDefault().ModifyDate
                }).FirstOrDefault();
        }

        public async Task<MetaPivot> GetMetaAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            if (tokenDetail == null)
                return null;
            return await GetMetaAsync(tokenDetail.UserID);
        }
        
        public async Task<MetaPivot> GetMetaAsync(long userId)
        {
            var metaList = await _unitOfWork.UserMetaRepository.GetManyAsync(ut => ut.UserID == userId);
            var meta = metaList.GroupBy(ut => new
                {ut.UserID}).Select(
                ut => new MetaPivot
                {
                    UserId = ut.Key.UserID,
                    WeightVolumeType = ut.Where(u => u.MetaKey.ToLower() == "weightvolumetype")?.FirstOrDefault()
                                           ?.MetaValue ?? "",
                    State = ut.Where(u => u.MetaKey.ToLower() == "state")?.FirstOrDefault()?.MetaValue ?? "",
                    Address = ut.Where(u => u.MetaKey.ToLower() == "address")?.FirstOrDefault()?.MetaValue ?? "",
                    Age = ut.Where(u => u.MetaKey.ToLower() == "age")?.FirstOrDefault()?.MetaValue ?? "",
                    City = ut.Where(u => u.MetaKey.ToLower() == "city")?.FirstOrDefault()?.MetaValue ?? "",
                    Country = ut.Where(u => u.MetaKey.ToLower() == "country")?.FirstOrDefault()?.MetaValue ?? "",
                    Gender = ut.Where(u => u.MetaKey.ToLower() == "gender")?.FirstOrDefault()?.MetaValue ?? "",
                    PostalCode = ut.Where(u => u.MetaKey.ToLower() == "postalcode")?.FirstOrDefault()?.MetaValue ??
                                 "",
                    ProfilePhoto =
                        ut.Where(u => u.MetaKey.ToLower() == "profilephoto")?.FirstOrDefault()?.MetaValue ?? "",
                    WeightLossGoal = ut.Where(u => u.MetaKey.ToLower() == "weightlossgoal")?.FirstOrDefault()
                                         ?.MetaValue ?? "",
                    WeightLossGoalUI = ut.Where(u => u.MetaKey.ToLower() == "weightlossgoal_ui")?.FirstOrDefault()
                                           ?.MetaValue ?? "",
                    ModifyDate = metaList.OrderByDescending(m => m.ModifyDate).FirstOrDefault().ModifyDate
                }).FirstOrDefault();

            return meta;
        }

        public async Task<MetaPivot> GetMetaAsync(long userId, string weightVolumeType)
        {
            var metaList = await _unitOfWork.UserMetaRepository.GetManyAsync(ut => ut.UserID == userId);
            var meta = metaList.GroupBy(ut => new
                {ut.UserID}).Select(
                ut => new MetaPivot
                {
                    UserId = ut.Key.UserID,
                    WeightVolumeType = ut.Where(u => u.MetaKey.ToLower() == "weightvolumetype")?.FirstOrDefault()
                                           ?.MetaValue ?? "",
                    State = ut.Where(u => u.MetaKey.ToLower() == "state")?.FirstOrDefault()?.MetaValue ?? "",
                    Address = ut.Where(u => u.MetaKey.ToLower() == "address")?.FirstOrDefault()?.MetaValue ?? "",
                    Age = ut.Where(u => u.MetaKey.ToLower() == "age")?.FirstOrDefault()?.MetaValue ?? "",
                    City = ut.Where(u => u.MetaKey.ToLower() == "city")?.FirstOrDefault()?.MetaValue ?? "",
                    Country = ut.Where(u => u.MetaKey.ToLower() == "country")?.FirstOrDefault()?.MetaValue ?? "",
                    Gender = ut.Where(u => u.MetaKey.ToLower() == "gender")?.FirstOrDefault()?.MetaValue ?? "",
                    PostalCode = ut.Where(u => u.MetaKey.ToLower() == "postalcode")?.FirstOrDefault()?.MetaValue ??
                                 "",
                    ProfilePhoto =
                        ut.Where(u => u.MetaKey.ToLower() == "profilephoto")?.FirstOrDefault()?.MetaValue ?? "",
                    WeightLossGoal = ut.Where(u => u.MetaKey.ToLower() == "weightlossgoal")?.FirstOrDefault()
                                         ?.MetaValue ?? "",
                    WeightLossGoalUI = ut.Where(u => u.MetaKey.ToLower() == "weightlossgoal_ui")?.FirstOrDefault()
                                           ?.MetaValue ?? "",
                    ModifyDate = metaList.OrderByDescending(m => m.ModifyDate).FirstOrDefault().ModifyDate
                }).FirstOrDefault();

            if (meta != null && weightVolumeType.ToLower().Contains("lb"))
            {
                meta.WeightLossGoal = meta.WeightLossGoalUI.Trim().Length == 0
                    ? _converter.ConvertKilogramToPound(meta.WeightLossGoal)
                    : meta.WeightLossGoalUI;
            }

            return meta;
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, UserMeta entity)
        {
            if (entity != null)
                entity.UserID = userId;
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                var record = _unitOfWork.UserMetaRepository
                    .GetMany(m =>
                        m.UserID == entity.UserID && m.MetaKey.Trim().ToLower() == entity.MetaKey.Trim().ToLower())
                    .LastOrDefault();
                if (record != null)
                {
                    record.MetaDescription = entity.MetaDescription;
                    record.MetaLabel = entity.MetaLabel;
                    record.MetaType = entity.MetaType;
                    record.MetaValue = entity.MetaValue;
                    record.ModifyDate = entity.ModifyDate;
                    _unitOfWork.UserMetaRepository.Update(record);
                }
                else
                    _unitOfWork.UserMetaRepository.Insert(entity);

                var result = _unitOfWork.Commit();
                if (result)
                    InsertHistory(entity);
                return result;
            }
            return false;
        }

        public bool Insert(ref ValidationErrors validationErrors, string token, UserMeta entity)
        {
            if (entity != null)
            {
                entity.UserID = _tokensServices.GetDetailByToken(token)?.UserID ?? 0;
                return Insert(ref validationErrors, entity.UserID, entity);
            }

            return false;
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, ICollection<UserMeta> entity)
        {
            foreach (var record in entity)
            {
                record.UserID = userId;
                dynamic[] obj = {record};
                if (this.Validate(ref validationErrors, obj))
                {
                    var result = _unitOfWork.UserMetaRepository.GetMany(m =>
                            m.UserID == record.UserID && m.MetaKey.Trim().ToLower() == record.MetaKey.Trim().ToLower())
                        .LastOrDefault();
                    if (result != null)
                    {
                        result.MetaDescription = record.MetaDescription;
                        result.MetaLabel = record.MetaLabel;
                        result.MetaType = record.MetaType;
                        result.MetaValue = record.MetaValue;
                        result.ModifyDate = record.ModifyDate;
                        _unitOfWork.UserMetaRepository.Update(result);
                    }
                    else
                    {
                        _unitOfWork.UserMetaRepository.Insert(record);
                    }

                    if (!_unitOfWork.Commit())
                    {
                        validationErrors.Add("MessageSomeRecordsCouldNotSave");
                    }
                    else
                        InsertHistory(record);
                }
            }

            return validationErrors.Count() == 0;
        }

        public bool Insert(ref ValidationErrors validationErrors, string token, ICollection<UserMeta> entity)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            if (tokenDetail != null)
                return Insert(ref validationErrors, tokenDetail.UserID, entity);

            return validationErrors.Count() == 0;
        }

        private bool InsertHistory(UserMeta userMeta)
        {
            try
            {
                var entity = new UserMetaHistory()
                {
                    MetaDescription = userMeta.MetaDescription,
                    MetaKey = userMeta.MetaKey,
                    MetaLabel = userMeta.MetaLabel,
                    MetaType = userMeta.MetaType,
                    MetaValue = userMeta.MetaValue,
                    ModifyDate = userMeta.ModifyDate,
                    UserID = userMeta.UserID
                };
                _unitOfWork.UserMetaHistoryRepository.Insert(entity);
                return _unitOfWork.Commit();
            }
            catch (Exception)
            {
                //
            }

            return false;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var obj = (UserMeta) objValue[0];
            if (obj.UserID == 0)
            {
                validationErrors.Add("UserIDRequired");
            }

            if (obj.MetaKey.Trim().Length == 0)
            {
                validationErrors.Add("MetaKeyRequired");
            }

            if (obj.MetaValue.Trim().Length == 0)
            {
                validationErrors.Add("MetaValueRequired");
            }

            return validationErrors.Count() == 0;
        }
    }
}