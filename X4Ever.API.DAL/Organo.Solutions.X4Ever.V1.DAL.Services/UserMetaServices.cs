using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserMetaServices : IUserMetaServices, IValidation
    {
        private IUnitOfWork _unitOfWork;
        private readonly IUserTokensServices _tokensServices;

        public UserMetaServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokensServices = new UserTokensServices(unitOfWork);
        }

        public bool Delete(ref ValidationErrors validationErrors, UserMeta entity)
        {
            _unitOfWork.UserMetaRepository.Delete(entity);
            return _unitOfWork.Commit();
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

        public IEnumerable<UserMeta> Get()
        {
            return _unitOfWork.UserMetaRepository.GetAll();
        }

        public async Task<IEnumerable<UserMeta>> GetAsync()
        {
            return await _unitOfWork.UserMetaRepository.GetAllAsync();
        }

        public IEnumerable<UserMeta> Get(Expression<Func<UserMeta, bool>> filter = null,
            Func<IQueryable<UserMeta>, IOrderedQueryable<UserMeta>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.UserMetaRepository.GetMany(filter, orderBy, includeProperties);
        }

        public async Task<IEnumerable<UserMeta>> GetAsync(Expression<Func<UserMeta, bool>> filter = null,
            Func<IQueryable<UserMeta>, IOrderedQueryable<UserMeta>> orderBy = null, string includeProperties = "")
        {
            return await _unitOfWork.UserMetaRepository.GetManyAsync(filter, orderBy, includeProperties);
        }

        public UserMeta Get(string ID)
        {
            return _unitOfWork.UserMetaRepository.GetByID(ID);
        }

        public async Task<UserMeta> GetAsync(string ID)
        {
            return await _unitOfWork.UserMetaRepository.GetByIDAsync(ID);
        }

        public UserMeta GetByUser(string token, string metaKey)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return GetByUser(tokenDetail?.UserID ?? 0, metaKey);
        }

        public UserMeta GetByUser(long userId, string metaKey)
        {
            return _unitOfWork.UserMetaRepository.Get(m =>
                m.UserID == userId && m.MetaKey == metaKey);
        }

        public async Task<UserMeta> GetByUserAsync(string token, string metaKey)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetByUserAsync(tokenDetail?.UserID ?? 0, metaKey);
        }

        public async Task<UserMeta> GetByUserAsync(long userId, string metaKey)
        {
            return await _unitOfWork.UserMetaRepository.GetAsync(m =>
                m.UserID == userId && m.MetaKey == metaKey);
        }

        public List<UserMeta> GetByUser(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == (tokenDetail?.UserID ?? 0)).ToList();
        }

        public async Task<List<UserMeta>> GetByUserAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return (await _unitOfWork.UserMetaRepository.GetManyAsync(m => m.UserID == (tokenDetail?.UserID ?? 0)))
                .ToList();
        }

        public List<UserMeta> GetByUser(string token, string[] keys)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return GetByUser(tokenDetail?.UserID ?? 0, keys);
        }

        public List<UserMeta> GetByUser(long userId, string[] keys)
        {
            return _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == userId && keys.Any(k => k == m.MetaKey))
                .ToList();
        }

        public async Task<List<UserMeta>> GetByUserAsync(string token, string[] keys)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return await GetByUserAsync(tokenDetail?.UserID ?? 0, keys);
        }

        public async Task<List<UserMeta>> GetByUserAsync(long userId, string[] keys)
        {
            return (await _unitOfWork.UserMetaRepository.GetManyAsync(m =>
                m.UserID == userId && keys.Any(k => k == m.MetaKey))).ToList();
        }

        public List<UserMeta> GetByUser(long userId)
        {
            return _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == userId).ToList();
        }
        
        public async Task<List<UserMeta>> GetByUserAsync(long userId)
        {
            return (await _unitOfWork.UserMetaRepository.GetManyAsync(m => m.UserID == userId)).ToList();
        }

        public string GetByMetaKey(long userId, string metaKey)
        {
            var meta = _unitOfWork.UserMetaRepository.Get(m => m.UserID == userId && m.MetaKey == metaKey);
            if (meta != null)
                return meta.MetaValue;
            return "";
        }

        public async Task<string> GetByMetaKeyAsync(long userId, string metaKey)
        {
            var meta = (await _unitOfWork.UserMetaRepository.GetAsync(m =>
                m.UserID == userId && m.MetaKey == metaKey));
            if (meta != null)
                return meta.MetaValue;
            return "";
        }

        public string GetByMetaKey(string token, string metaKey)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var meta = _unitOfWork.UserMetaRepository.Get(m => m.UserID == (tokenDetail?.UserID ?? 0));
            if (meta != null)
                return meta.MetaValue;
            return "";
        }

        public async Task<string> GetByMetaKeyAsync(string token, string metaKey)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var meta = await _unitOfWork.UserMetaRepository.GetAsync(m => m.UserID == (tokenDetail?.UserID ?? 0));
            if (meta != null)
                return meta.MetaValue;
            return "";
        }

        public bool Insert(ref ValidationErrors validationErrors, UserMeta entity)
        {
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                if (Exists(entity.UserID, entity.MetaKey))
                    _unitOfWork.UserMetaRepository.Update(entity);
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
                Insert(ref validationErrors, entity.UserID, entity);
            }

            return false;
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

        public bool Insert(ref ValidationErrors validationErrors, string token, List<UserMeta> entity)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            if (tokenDetail != null)
                Insert(ref validationErrors, tokenDetail.UserID, entity);
            return validationErrors.Count() == 0;
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, List<UserMeta> entity)
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

        public bool Update(ref ValidationErrors validationErrors, UserMeta entity)
        {
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.UserMetaRepository.Update(entity);
                var result = _unitOfWork.Commit();
                if (result)
                    InsertHistory(entity);
                return result;
            }

            return false;
        }

        public bool Exists(Int64 userId, string key)
        {
            return _unitOfWork.UserMetaRepository.GetMany(m =>
                m.UserID == userId && m.MetaKey.Trim().ToLower() == key.Trim().ToLower()).Any();
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

        public async Task<UserMetaPivot> GetUserRowByIdAsync(long userId)
        {
            return (await _unitOfWork.UserMetaPivotRepository.SqlQueryAsync(
                "x4ever.x4_user_meta_select_by_userid @user_id",
                new SqlParameter("user_id", SqlDbType.BigInt) {Value = userId}
            ))?.FirstOrDefault();
        }
    }
}