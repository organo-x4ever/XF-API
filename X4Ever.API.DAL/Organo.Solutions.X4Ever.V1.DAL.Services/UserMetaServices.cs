using Organo.Solutions.X4Ever.V1.DAL.Helper;
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
            var meta = _unitOfWork.UserMetaRepository.GetAll();
            return from m in meta         
            select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public async Task<IEnumerable<UserMeta>> GetAsync()
        {
            var meta = await _unitOfWork.UserMetaRepository.GetAllAsync();
            return from m in meta         
            select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public IEnumerable<UserMeta> Get(Expression<Func<UserMeta, bool>> filter = null,
            Func<IQueryable<UserMeta>, IOrderedQueryable<UserMeta>> orderBy = null, string includeProperties = "")
        {
            var meta = _unitOfWork.UserMetaRepository.GetMany(filter, orderBy, includeProperties);
            return from m in meta         
            select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public async Task<IEnumerable<UserMeta>> GetAsync(Expression<Func<UserMeta, bool>> filter = null,
            Func<IQueryable<UserMeta>, IOrderedQueryable<UserMeta>> orderBy = null, string includeProperties = "")
        {
            var meta = await _unitOfWork.UserMetaRepository.GetManyAsync(filter, orderBy, includeProperties);
            return from m in meta         
            select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public UserMeta Get(string ID)
        {
            var m = _unitOfWork.UserMetaRepository.GetByID(ID);
            return new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public async Task<UserMeta> GetAsync(string ID)
        {
            var m = await _unitOfWork.UserMetaRepository.GetByIDAsync(ID);
            return new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public UserMeta GetByUser(string token, string metaKey)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var m = GetByUser(tokenDetail?.UserID ?? 0, metaKey);
            return new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public UserMeta GetByUser(long userId, string metaKey)
        {
            var m = _unitOfWork.UserMetaRepository.Get(mt =>
                mt.UserID == userId && mt.MetaKey == metaKey);
            return new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public async Task<UserMeta> GetByUserAsync(string token, string metaKey)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var m = await GetByUserAsync(tokenDetail?.UserID ?? 0, metaKey);
            return new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public async Task<UserMeta> GetByUserAsync(long userId, string metaKey)
        {
            var m = await _unitOfWork.UserMetaRepository.GetAsync(mt =>
                mt.UserID == userId && mt.MetaKey == metaKey);
            return new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            };
        }

        public List<UserMeta> GetByUser(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var meta = _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == (tokenDetail?.UserID ?? 0)).ToList();
            return (from m in meta
                select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            }).ToList();
        }

        public async Task<List<UserMeta>> GetByUserAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return (from m in (await _unitOfWork.UserMetaRepository.GetManyAsync(mt => mt.UserID == (tokenDetail?.UserID ?? 0)))
                select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            }).ToList();
        }

        public List<UserMeta> GetByUser(string token, string[] keys)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return (from m in GetByUser(tokenDetail?.UserID ?? 0, keys)
                    select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            }).ToList();
        }

        public List<UserMeta> GetByUser(long userId, string[] keys)
        {
            return (from m in _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == userId && keys.Any(k => k == m.MetaKey))
                select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            }).ToList();
        }

        public async Task<List<UserMeta>> GetByUserAsync(string token, string[] keys)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            return (from m in (await GetByUserAsync(tokenDetail?.UserID ?? 0, keys))
                    select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            }).ToList();
        }

        public async Task<List<UserMeta>> GetByUserAsync(long userId, string[] keys)
        {
            return (from m in (await _unitOfWork.UserMetaRepository.GetManyAsync(mt => mt.UserID == userId && keys.Any(k => k == mt.MetaKey)))
                select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            }).ToList();
        }

        public List<UserMeta> GetByUser(long userId)
        {
            return (from m in _unitOfWork.UserMetaRepository.GetMany(mt => mt.UserID == userId)
                    select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            }).ToList();
        }
        
        public async Task<List<UserMeta>> GetByUserAsync(long userId)
        {
            return (from m in (await _unitOfWork.UserMetaRepository.GetManyAsync(mt => mt.UserID == userId))
                    select new UserMeta()
            {
                UserID = m.UserID,
                ID=m.ID,
                MetaDescription=m.MetaDescription,
                MetaKey=m.MetaKey,
                MetaLabel=m.MetaLabel,
                MetaType=m.MetaType,
                MetaValue = m.MetaValue.Clean(),
                ModifyDate = m.ModifyDate
            }).ToList();
        }

        public string GetByMetaKey(long userId, string metaKey)
        {
            var meta = _unitOfWork.UserMetaRepository.Get(m => m.UserID == userId && m.MetaKey == metaKey);
            if (meta != null)
                return meta.MetaValue.Clean();
            return "";
        }

        public async Task<string> GetByMetaKeyAsync(long userId, string metaKey)
        {
            var meta = (await _unitOfWork.UserMetaRepository.GetAsync(m =>
                m.UserID == userId && m.MetaKey == metaKey));
            if (meta != null)
                return meta.MetaValue.Clean();
            return "";
        }

        public string GetByMetaKey(string token, string metaKey)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return GetByMetaKey(tokenDetail?.UserID??0,metaKey);
        }

        public async Task<string> GetByMetaKeyAsync(string token, string metaKey)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return await GetByMetaKeyAsync(tokenDetail?.UserID??0, metaKey);
        }

        public bool Insert(ref ValidationErrors validationErrors, UserMeta entity)
        {
            dynamic[] obj = {entity};
            if (this.Validate(ref validationErrors, obj))
            {
                try {
                    entity.MetaValue=entity.MetaValue.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">","");
                    entity.MetaValue=entity.MetaValue.Replace("<string xmlns=http://schemas.microsoft.com/2003/10/Serialization/>","");
                    entity.MetaValue=entity.MetaValue.Replace("</string>","");
                }
                catch {
                    //
                }
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
                try {
                    entity.MetaValue=entity.MetaValue.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">","");
                    entity.MetaValue=entity.MetaValue.Replace("<string xmlns=http://schemas.microsoft.com/2003/10/Serialization/>","");
                    entity.MetaValue=entity.MetaValue.Replace("</string>","");
                }
                catch {
                    //
                }
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
                    try {
                    record.MetaValue=record.MetaValue.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">","");
                    record.MetaValue=record.MetaValue.Replace("<string xmlns=http://schemas.microsoft.com/2003/10/Serialization/>","");
                    record.MetaValue=record.MetaValue.Replace("</string>","");
                }
                catch {
                    //
                }
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
                try {
                    entity.MetaValue=entity.MetaValue.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">","");
                    entity.MetaValue=entity.MetaValue.Replace("<string xmlns=http://schemas.microsoft.com/2003/10/Serialization/>","");
                    entity.MetaValue=entity.MetaValue.Replace("</string>","");
                }
                catch {
                    //
                }
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
            var m = (await _unitOfWork.UserMetaPivotRepository.SqlQueryAsync(
                "x4ever.x4_user_meta_select_by_userid @user_id",
                new SqlParameter("user_id", SqlDbType.BigInt) {Value = userId}
            ))?.FirstOrDefault();
            return new UserMetaPivot()
            {
                user_id = m.user_id,
                ID=m.ID,
                Address=m.Address,
                Age=m.Age,
                City=m.City,
                Country=m.Country,
                Gender=m.Gender,
                PostalCode=m.PostalCode,
                ProfilePhoto=m.ProfilePhoto.Clean(),
                State=m.State,
                WeightLossGoal=m.WeightLossGoal,
                ModifyDate = m.ModifyDate
            };
        }
    }
}