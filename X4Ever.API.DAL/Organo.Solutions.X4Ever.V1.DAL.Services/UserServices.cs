using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserServices : IUserServices, IValidation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Helper.IHelper _helper;
        private readonly IUserMetaServices _userMetaServices;
        private readonly IUserTrackerServices _userTrackerServices;
        private readonly IUserMilestoneServices _userMilestoneServices;
        private readonly IMilestonePercentageServices _milestonePercentageServices;
        private readonly IUserTokensServices _tokensServices;
        private readonly IUserSettingServices _userSettingServices;

        public UserServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
            _userMetaServices = new UserMetaServices(unitOfWork);
            _userTrackerServices = new UserTrackerServices(unitOfWork);
            _tokensServices = new UserTokensServices(unitOfWork);
            _userMilestoneServices = new UserMilestoneServices(unitOfWork);
            _milestonePercentageServices = new MilestonePercentageServices(unitOfWork);
            _userSettingServices = new UserSettingServices(unitOfWork);
        }

        public long Authenticate(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.UserLogin.ToLower().Trim() == userName.ToLower().Trim());
            if (!(user is null))
            {
                _unitOfWork.UserRepository.Reload(user);
                if (VerifySha512(user.UserPassword, password))
                    return user.ID;

                if (VerifyHash(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Commit();
                    return user.ID;
                }

                if (VerifySha512_WrongEncryption(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Commit();
                    return user.ID;
                }
            }

            return 0;
        }

        public async Task<long> AuthenticateAsync(string userName, string password)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u =>
                u.UserLogin.ToLower().Trim() == userName.ToLower().Trim());
            if (!(user is null))
            {
                await _unitOfWork.UserRepository.ReloadAsync(user);
                if (VerifySha512(user.UserPassword, password))
                    return user.ID;

                if (VerifyHash(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    await _unitOfWork.CommitAsync();
                    return user.ID;
                }

                if (VerifySha512_WrongEncryption(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    await _unitOfWork.CommitAsync();
                    return user.ID;
                }
            }

            return 0;
        }

        private User GetUser(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return _unitOfWork.UserRepository.Get(u => u.ID == (tokenDetail?.UserID ?? 0));
        }

        public bool Delete(ref ValidationErrors validationErrors, long userId, string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.ID == userId);
            if (!(user is null))
            {
                if (VerifyPassword(user.UserPassword, password))
                {
                    _unitOfWork.UserRepository.Delete(user);
                    return _unitOfWork.Commit();
                }
                else if (VerifySha512_WrongEncryption(user.UserPassword, password))
                {
                    _unitOfWork.UserRepository.Delete(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public bool ForgotPasswordRequest(ref ValidationErrors validationErrors, string username, string email)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(u =>
                    u.UserLogin.ToLower().Trim() == username.ToLower().Trim() &&
                    u.UserEmail.ToLower().Trim() == email.ToLower().Trim());
                if (!(user is null))
                {
                    var date = DateTime.Now;
                    var val = (Int16) _helper.GetAppSetting("passwordRequestCodeExpireMinutes", typeof(System.Int16));
                    var expiresDate = date.AddMinutes(val);
                    var code = _helper.GetUniqueCode().Substring(0, 8);
                    var password = new UserPasswordRequest()
                    {
                        RequestDate = date,
                        UserEmail = user.UserEmail,
                        UserID = user.ID,
                        UserLogin = user.UserLogin,
                        RequestCode = code,
                        RequestCodeExpiresOn = expiresDate
                    };

                    _unitOfWork.PasswordRequestRepository.Insert(password);
                    if (!_unitOfWork.Commit())
                    {
                        validationErrors.Add("MessagePasswordRequestFailed");
                    }
                    else return true;
                }
                else
                    validationErrors.Add("MessageInvalidProvidedInformation");
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message +
                                     (ex.InnerException != null ? " Inner: " + ex.InnerException.Message : ""));
            }

            return false;
        }

        public User ForgotPasswordRequest(ref ValidationErrors validationErrors, string username, string email,
            out string requestCode)
        {
            requestCode = "";
            var user = _unitOfWork.UserRepository.Get(u =>
                u.UserLogin.ToLower().Trim() == username.ToLower().Trim() &&
                u.UserEmail.ToLower().Trim() == email.ToLower().Trim());
            if (!(user is null))
            {
                var date = DateTime.Now;
                var val = (Int16) _helper.GetAppSetting("passwordRequestCodeExpireMinutes", typeof(System.Int16));
                var expiresDate = date.AddMinutes(val);
                string code = _helper.GetUniqueCode().Substring(0, 8);
                var password = new UserPasswordRequest()
                {
                    RequestDate = date,
                    UserEmail = user.UserEmail,
                    UserID = user.ID,
                    UserLogin = user.UserLogin,
                    RequestCode = code,
                    RequestCodeExpiresOn = expiresDate
                };
                _unitOfWork.PasswordRequestRepository.Insert(password);
                if (_unitOfWork.Commit())
                {
                    requestCode = code;
                    return user;
                }

                validationErrors.Add("MessagePasswordRequestFailed");
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return null;
        }

        public bool ChangeForgotPassword(ref ValidationErrors validationErrors, string requestCode, string password)
        {
            var date = DateTime.Now;
            var user = _unitOfWork.UserRepository.GetMany(u => _unitOfWork.PasswordRequestRepository.GetMany(p =>
                u.ID == p.UserID && p.RequestCode.ToLower().Trim() == requestCode.ToLower().Trim() &&
                p.RequestCodeExpiresOn >= date).Any()).FirstOrDefault();
            if (!(user is null))
            {
                user.UserPassword = _helper.ConvertToSHA512(password);
                _unitOfWork.UserRepository.Update(user);
                if (_unitOfWork.Commit())
                {
                    return SavePasswordHistory(user.ID, user.UserPassword);
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        private bool SavePasswordHistory(long userId, string encryptedPassword)
        {
            var date = DateTime.Now;
            var expires = date;
            var days = (int) _helper.GetAppSetting("passwordExpireMinutes", typeof(System.Int32));
            if (days > 0)
                expires = date.AddDays(days);
            var history = new UserPasswordHistory()
            {
                ChangeDate = date,
                ExpireDate = expires,
                UserID = userId,
                UserPassword = encryptedPassword
            };

            _unitOfWork.UserPasswordHistoryRepository.Insert(history);
            return _unitOfWork.Commit();
        }

        public bool ChangePassword(ref ValidationErrors validationErrors, string token, string currentPassword,
            string password)
        {
            var user = GetUser(token);
            if (!(user is null))
            {
                if (VerifyPassword(user.UserPassword, currentPassword))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    if (_unitOfWork.Commit())
                        return SavePasswordHistory(user.ID, user.UserPassword);
                }
                else if (VerifySha512_WrongEncryption(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    if (_unitOfWork.Commit())
                        return SavePasswordHistory(user.ID, user.UserPassword);
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }
        public bool ChangePassword(ref ValidationErrors validationErrors, long userId, string currentPassword,
            string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.ID == userId);
            if (!(user is null))
            {
                if (VerifyPassword(user.UserPassword, currentPassword))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    if (_unitOfWork.Commit())
                        return SavePasswordHistory(user.ID, user.UserPassword);
                }
                else if (VerifySha512_WrongEncryption(user.UserPassword, password))
                {
                    user.UserPassword = _helper.ConvertToSHA512(password);
                    _unitOfWork.UserRepository.Update(user);
                    if (_unitOfWork.Commit())
                        return SavePasswordHistory(user.ID, user.UserPassword);
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public IEnumerable<UserGlobal> Get()
        {
            var records = (from user in _unitOfWork.UserRepository.GetMany()
                select new UserGlobal
                {
                    ID = user.ID,
                    UserFirstName = user.UserFirstName,
                    UserEmail = user.UserEmail,
                    UserLogin = user.UserLogin,
                    UserLastName = user.UserLastName,
                    UserRegistered = user.UserRegistered,
                    UserStatus = user.UserStatus,
                    UserKey = user.UserKey,
                    UserType = user.UserType
                });

            return records;
        }

        public async Task<IEnumerable<UserGlobal>> GetAsync()
        {
            var records = (from user in await _unitOfWork.UserRepository.GetManyAsync()
                select new UserGlobal
                {
                    ID = user.ID,
                    UserFirstName = user.UserFirstName,
                    UserEmail = user.UserEmail,
                    UserLogin = user.UserLogin,
                    UserLastName = user.UserLastName,
                    UserRegistered = user.UserRegistered,
                    UserStatus = user.UserStatus,
                    UserKey = user.UserKey,
                    UserType = user.UserType
                });

            return records;
        }

        public IEnumerable<UserGlobal> Get(bool showEmptyRecords = false)
        {
            var applications = _unitOfWork.ApplicationRepository.GetMany(a =>
                (!showEmptyRecords ? a.ApplicationName.Trim().Length > 0 : false));
            var records =
                (from user in _unitOfWork.UserRepository.GetMany(u =>
                        (!showEmptyRecords ? u.UserFirstName.Trim().Length > 0 : false))
                    select new UserGlobal
                    {
                        ID = user.ID,
                        UserFirstName = user.UserFirstName,
                        UserEmail = user.UserEmail,
                        UserLogin = user.UserLogin,
                        UserLastName = user.UserLastName,
                        UserRegistered = user.UserRegistered,
                        UserStatus = user.UserStatus,
                        UserKey = user.UserKey,
                        UserType = user.UserType,
                        UserApplication = applications.FirstOrDefault(a => user.UserApplication == a.ApplicationKey)
                                              ?.ApplicationName ?? ""
                    });

            return records;
        }

        public async Task<IEnumerable<UserGlobal>> GetAsync(bool showEmptyRecords = false)
        {
            var applications = _unitOfWork.ApplicationRepository.GetMany(a =>
                (!showEmptyRecords ? a.ApplicationName.Trim().Length > 0 : false));

            var records = (from user in await _unitOfWork.UserRepository.GetManyAsync(u =>
                    (!showEmptyRecords ? u.UserFirstName.Trim().Length > 0:false))
                select new UserGlobal
                {
                    ID = user.ID,
                    UserFirstName = user.UserFirstName,
                    UserEmail = user.UserEmail,
                    UserLogin = user.UserLogin,
                    UserLastName = user.UserLastName,
                    UserRegistered = user.UserRegistered,
                    UserStatus = user.UserStatus,
                    UserKey = user.UserKey,
                    UserType = user.UserType,
                    UserApplication = applications.FirstOrDefault(a => user.UserApplication == a.ApplicationKey)
                                          ?.ApplicationName ?? ""
                });

            return records;
        }

        public IEnumerable<UserGlobal> Get(Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null, string includeProperties = "")
        {
            var users = _unitOfWork.UserRepository.GetMany(filter, orderBy, includeProperties);
            var records = new List<UserGlobal>();
            foreach (var user in users)
                records.Add(new UserGlobal()
                {
                    ID = user.ID,
                    UserFirstName = user.UserFirstName,
                    UserEmail = user.UserEmail,
                    UserLogin = user.UserLogin,
                    UserLastName = user.UserLastName,
                    UserRegistered = user.UserRegistered,
                    UserStatus = user.UserStatus,
                    UserKey = user.UserKey,
                    UserType = user.UserType
                });
            return records;
        }

        public async Task<IEnumerable<UserGlobal>> GetAsync(Expression<Func<User, bool>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null, string includeProperties = "")
        {
            var users = await _unitOfWork.UserRepository.GetManyAsync(filter, orderBy, includeProperties);
            var records = new List<UserGlobal>();
            foreach (var user in users)
                records.Add(new UserGlobal()
                {
                    ID = user.ID,
                    UserFirstName = user.UserFirstName,
                    UserEmail = user.UserEmail,
                    UserLogin = user.UserLogin,
                    UserLastName = user.UserLastName,
                    UserRegistered = user.UserRegistered,
                    UserStatus = user.UserStatus,
                    UserKey = user.UserKey,
                    UserType = user.UserType
                });
            return records;
        }

        public UserGlobal Get(long id)
        {
            var user = _unitOfWork.UserRepository.GetByID(id);
            var record = new UserGlobal()
            {
                ID = user.ID,
                UserFirstName = user.UserFirstName,
                UserEmail = user.UserEmail,
                UserLogin = user.UserLogin,
                UserLastName = user.UserLastName,
                UserRegistered = user.UserRegistered,
                UserStatus = user.UserStatus,
                UserKey = user.UserKey,
                UserType = user.UserType
            };
            return record;
        }

        public async Task<UserGlobal> GetAsync(long ID)
        {
            var user = await _unitOfWork.UserRepository.GetByIDAsync(ID);
            var record = new UserGlobal()
            {
                ID = user.ID,
                UserFirstName = user.UserFirstName,
                UserEmail = user.UserEmail,
                UserLogin = user.UserLogin,
                UserLastName = user.UserLastName,
                UserRegistered = user.UserRegistered,
                UserStatus = user.UserStatus,
                UserKey = user.UserKey,
                UserType = user.UserType
            };
            return record;
        }

        public UserGlobal Get(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == (tokenDetail?.UserID ?? 0))
                select new UserGlobal()
                {
                    ID = u.ID,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLogin = u.UserLogin,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserStatus = u.UserStatus,
                    UserKey = u.UserKey,
                    UserType = u.UserType,
                    LanguageCode =
                        _unitOfWork.UserSettingRepository.Get(s => s.UserID == u.ID)?.LanguageCode ?? "en-US",
                }).FirstOrDefault();
            return user;
        }

        public async Task<UserGlobal> GetAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var user = (from u in await _unitOfWork.UserRepository.GetManyAsync(u => u.ID == (tokenDetail?.UserID ?? 0))
                select new UserGlobal()
                {
                    ID = u.ID,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLogin = u.UserLogin,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserStatus = u.UserStatus,
                    UserKey = u.UserKey,
                    UserType = u.UserType,
                    LanguageCode =
                        _unitOfWork.UserSettingRepository.Get(s => s.UserID == u.ID)?.LanguageCode ?? "en-US",
                }).FirstOrDefault();
            return user;
        }

        public bool Insert(ref ValidationErrors validationErrors, User entity)
        {
            dynamic[] obj = {entity, true, false, false, false, false};
            if (this.Validate(ref validationErrors, obj))
            {
                var code = _helper.GetUniqueCode();

                var activationCode = "";
                if (entity.UserActivationKey == null || entity.UserActivationKey.Trim().Length == 0)
                    activationCode = _helper.GetUniqueCode();
                else
                    activationCode = entity.UserActivationKey.Trim();

                var user = new User()
                {
                    UserFirstName = entity.UserFirstName,
                    UserActivationKey = activationCode,
                    UserEmail = entity.UserEmail,
                    UserLogin = entity.UserLogin,
                    UserLastName = entity.UserLastName,
                    UserPassword = _helper.ConvertToSHA512(entity.UserPassword),
                    UserRegistered = entity.UserRegistered,
                    UserStatus = "Active",
                    UserType = entity.UserType,
                    UserKey = code,
                    UserApplication = entity.UserApplication
                };

                _unitOfWork.UserRepository.Insert(user);
                var result = _unitOfWork.Commit();
                if (result)
                {
                    var userEntity = _unitOfWork.UserRepository.Get(u =>
                        u.UserLogin == user.UserLogin && u.UserKey == user.UserKey &&
                        u.UserActivationKey == user.UserActivationKey);
                    SavePasswordHistory(userEntity.ID, userEntity.UserPassword);
                }

                return result;
            }

            return false;
        }

        public bool Insert(ref ValidationErrors validationErrors, User entity, ICollection<UserMeta> userMetas)
        {
            dynamic[] obj = {entity, true, true, false, false, true};
            if (this.Validate(ref validationErrors, obj))
            {
                var code = _helper.GetUniqueCode();

                var activationCode = "";
                if (entity.UserActivationKey == null || entity.UserActivationKey.Trim().Length == 0)
                    activationCode = _helper.GetUniqueCode();
                else
                    activationCode = entity.UserActivationKey.Trim();

                var user = new User()
                {
                    UserFirstName = entity.UserFirstName,
                    UserActivationKey = activationCode,
                    UserEmail = entity.UserEmail,
                    UserLogin = entity.UserLogin,
                    UserLastName = entity.UserLastName,
                    UserPassword = _helper.ConvertToSHA512(entity.UserPassword),
                    UserRegistered = entity.UserRegistered,
                    UserStatus = "Active",
                    UserType = entity.UserType,
                    UserKey = code
                };

                _unitOfWork.UserRepository.Insert(user);
                var result = _unitOfWork.Commit();
                if (result)
                {
                    var userEntity = _unitOfWork.UserRepository.Get(u =>
                        u.UserLogin == user.UserLogin && u.UserKey == user.UserKey &&
                        u.UserActivationKey == user.UserActivationKey);
                    SavePasswordHistory(userEntity.ID, userEntity.UserPassword);

                    if (userMetas != null)
                    {
                        foreach (var userMeta in userMetas)
                        {
                            userMeta.UserID = userEntity.ID;
                            _userMetaServices.Insert(ref validationErrors, userMeta);
                        }
                    }
                }

                return result;
            }

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, string token, User entity)
        {
            var user = GetUser(token);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                dynamic[] obj = {user, false, true, false, false, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }
        public bool Update(ref ValidationErrors validationErrors, long userId, User entity)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.ID == userId);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                dynamic[] obj = {user, false, true, false, false, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public bool UpdateUserType(ref ValidationErrors validationErrors, string token, User entity)
        {
            var user = GetUser(token);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                user.UserType = entity.UserType;
                dynamic[] obj = {user, false, true, false, true, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }
        public bool UpdateUserType(ref ValidationErrors validationErrors, long userId, User entity)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.ID == userId);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                user.UserType = entity.UserType;
                dynamic[] obj = {user, false, true, false, true, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public bool UpdateUserStatus(ref ValidationErrors validationErrors, string token, User entity)
        {
            var user = GetUser(token);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                user.UserStatus = entity.UserStatus;
                dynamic[] obj = {user, false, true, true, false, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }
        
        public bool UpdateUserStatus(ref ValidationErrors validationErrors, long userId, User entity)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.ID == userId);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                user.UserStatus = entity.UserStatus;
                dynamic[] obj = {user, false, true, true, false, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public bool UpdateUserTypeAndStatus(ref ValidationErrors validationErrors, string token, User entity)
        {
            var user = GetUser(token);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                user.UserStatus = entity.UserStatus;
                user.UserType = entity.UserType;
                dynamic[] obj = {user, false, true, true, true, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }
        
        public bool UpdateUserTypeAndStatus(ref ValidationErrors validationErrors, long userId, User entity)
        {
            var user =  _unitOfWork.UserRepository.Get(u => u.ID == userId);
            if (!(user is null))
            {
                user.UserFirstName = entity.UserFirstName;
                user.UserEmail = entity.UserEmail;
                user.UserLastName = entity.UserLastName;
                user.UserStatus = entity.UserStatus;
                user.UserType = entity.UserType;
                dynamic[] obj = {user, false, true, true, true, false};
                if (this.Validate(ref validationErrors, obj))
                {
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var user = (User) objValue[0];
            var basic = (bool) objValue[1];
            var basicUpdate = (bool) objValue[2];
            var status = (bool) objValue[3];
            var type = (bool) objValue[4];
            var basicLastName = (bool) objValue[5];
            if (basic)
            {
                if (user.UserLogin == null || user.UserLogin.Trim().Length == 0)
                    validationErrors.Add("MessageUsernameRequired");
                else
                {
                    int userLengthMin = (Int16) _helper.GetAppSetting("usernameLengthMin", typeof(Int16));
                    int userLengthMax = (Int16) _helper.GetAppSetting("usernameLengthMax", typeof(Int16));
                    if (user.UserLogin.Trim().Length < userLengthMin || user.UserLogin.Trim().Length > userLengthMax)
                        validationErrors.Add("MessageUsernameLength");
                    else if (_unitOfWork.UserRepository.GetAll().Any(u =>
                        u.UserLogin.ToLower() == user.UserLogin.ToLower() && u.ID != user.ID))
                        validationErrors.Add("MessageUsernameExists");
                }

                if (user.UserPassword == null || user.UserPassword.Trim().Length == 0)
                    validationErrors.Add("MessagePasswordRequired");
                else
                {
                    int passLengthMin = (Int16) _helper.GetAppSetting("passwordLengthMin", typeof(Int16));
                    int passLengthMax = (Int16) _helper.GetAppSetting("passwordLengthMax", typeof(Int16));
                    if (user.UserPassword.Trim().Length < passLengthMin ||
                        user.UserPassword.Trim().Length > passLengthMax)
                        validationErrors.Add("MessagePasswordLength");
                }
            }

            if (basicUpdate)
            {
                if (user.UserFirstName == null || user.UserFirstName.Trim().Length == 0)
                    validationErrors.Add("MessageFirstNameRequired");
                if (basicLastName)
                    if (user.UserLastName == null || user.UserLastName.Trim().Length == 0)
                        validationErrors.Add("MessageLastNameRequired");
                if (user.UserEmail == null || user.UserEmail.Trim().Length == 0)
                    validationErrors.Add("MessageUserEmailRequired");
                else if (!_helper.IsEmailAddressValid(user.UserEmail.Trim().ToLower()))
                    validationErrors.Add("MessageEmailInvalid");
                else if (EmailExists(user.ID, user.UserEmail))
                    validationErrors.Add("MessageEmailExists");
            }

            if (status)
                if (user.UserStatus == null || user.UserStatus.Trim().Length == 0)
                    validationErrors.Add("MessageUserStatusRequired");

            if (type)
                if (user.UserType == null || user.UserType.Trim().Length == 0)
                    validationErrors.Add("MessageUserTypeRequired");

            return validationErrors.Count() == 0;
        }

        public bool UpdateUserStatus(ref ValidationErrors validationErrors, string activationCode)
        {
            var user = _unitOfWork.UserRepository.Get(u =>
                u.UserActivationKey.Trim().ToLower() == activationCode.Trim().ToLower());
            if (!(user is null))
            {
                user.UserActivationKey = user.UserActivationKey + "-done";
                user.UserStatus = "Active";
                _unitOfWork.UserRepository.Update(user);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public UserGlobal GetFullUser(string token)
        {
            var user = new UserGlobal();
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var userData = _unitOfWork.UserRepository.GetByID(tokenDetail.UserID);
            if (userData != null)
            {
                user = new UserGlobal()
                {
                    ID = userData.ID,
                    UserFirstName = userData.UserFirstName,
                    UserEmail = userData.UserEmail,
                    UserLogin = userData.UserLogin,
                    UserLastName = userData.UserLastName,
                    UserRegistered = userData.UserRegistered,
                    UserStatus = userData.UserStatus,
                    UserKey = userData.UserKey,
                    UserType = userData.UserType,
                };

                var userMilestones = _userMilestoneServices.Get(um => um.UserID == userData.ID && um.IsPercentage);

                var userAchievement = _milestonePercentageServices.GetLast(mp =>
                    userMilestones.Any(um => um.MilestonePercentageId == mp.MilestonePercentageId));

                if (userAchievement != null)
                    user.Achievement = new MilestonePercentage()
                    {
                        ID = userAchievement.MilestonePercentageId,
                        MilestonePercentageId = userAchievement.MilestonePercentageId,
                        LanguageCode = userAchievement.LanguageCode,
                        Active = userAchievement.Active,
                        MilestoneSubTitle = userAchievement.MilestoneSubTitle,
                        TargetPercentValue = userAchievement.TargetPercentValue,
                        AchievedMessage = userAchievement.AchievedMessage,
                        MilestoneTitle = userAchievement.MilestoneTitle,
                        IsPercent = userAchievement.IsPercent,
                        AchievementIcon = userAchievement.AchievementIcon,
                        AchievementGiftImage = userAchievement.AchievementGiftImage
                    };

                var userMetas = _userMetaServices.Get(m => m.UserID == userData.ID);

                if (userMetas != null)
                    user.UserMetas = (from m in userMetas
                        select new UserMetaGlobal()
                        {
                            ID = m.ID,
                            UserID = m.UserID,
                            MetaDescription = m.MetaDescription,
                            MetaKey = m.MetaKey,
                            MetaLabel = m.MetaLabel,
                            MetaType = m.MetaType,
                            MetaValue = m.MetaValue,
                            ModifyDate = m.ModifyDate
                        }).ToList();

                var userTrackers = _userTrackerServices.Get(userData.ID);
                if (userTrackers != null)
                    user.UserTrackers = (from ut in userTrackers
                        orderby ut.ID
                        select new UserTrackerGlobal()
                        {
                            ID = ut.ID,
                            UserID = ut.UserID,
                            ModifyDate = ut.ModifyDate,
                            AttributeLabel = ut.AttributeLabel,
                            AttributeName = ut.AttributeName,
                            AttributeValue = ut.AttributeValue,
                            MediaLink = ut.MediaLink,
                            RevisionNumber = ut.RevisionNumber
                        }).ToList();

                var userSetting = _userSettingServices.Get(s => s.UserID == userData.ID);
                user.LanguageCode = userSetting?.LanguageCode ?? "en-US";
            }

            #region Comments

            //var tokenDetail = _tokensServices.GetDetailByToken(token);
            //var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == (tokenDetail?.UserID ?? 0))
            //    select new UserGlobal()
            //    {
            //        ID = u.ID,
            //        UserFirstName = u.UserFirstName,
            //        UserEmail = u.UserEmail,
            //        UserLogin = u.UserLogin,
            //        UserLastName = u.UserLastName,
            //        UserRegistered = u.UserRegistered,
            //        UserStatus = u.UserStatus,
            //        UserKey = u.UserKey,
            //        UserType = u.UserType,
            //        Achievement = (from mp in _unitOfWork.MilestonePercentageRepository.GetMany(mp =>
            //                _unitOfWork.UserMilestoneRepository.GetMany(um => um.UserID == u.ID && um.MilestonePercentageId == mp.MilestonePercentageId && um.IsPercentage).Any())
            //            select new MilestonePercentage()
            //            {
            //                ID = mp.MilestonePercentageId,
            //                MilestonePercentageId = mp.MilestonePercentageId,
            //                LanguageCode = mp.LanguageCode,
            //                Active = mp.Active,
            //                MilestoneSubTitle = mp.MilestoneSubTitle,
            //                TargetPercentValue = mp.TargetPercentValue,
            //                AchievedMessage = mp.AchievedMessage,
            //                MilestoneTitle = mp.MilestoneTitle,
            //                IsPercent = mp.IsPercent,
            //                AchievementIcon = mp.AchievementIcon,
            //                AchievementGiftImage = mp.AchievementGiftImage
            //            }).OrderByDescending(mp => mp.ID)?.FirstOrDefault(),
            //        UserMetas = (from m in _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == u.ID)
            //            select new UserMetaGlobal()
            //            {
            //                ID = m.ID,
            //                UserID = m.UserID,
            //                MetaDescription = m.MetaDescription,
            //                MetaKey = m.MetaKey,
            //                MetaLabel = m.MetaLabel,
            //                MetaType = m.MetaType,
            //                MetaValue = m.MetaValue,
            //                ModifyDate = m.ModifyDate
            //            }).ToList(),
            //        UserTrackers = (from ut in _unitOfWork.UserTrackerRepository.GetMany(ut => ut.UserID == u.ID)
            //            orderby ut.ID
            //            select new UserTrackerGlobal()
            //            {
            //                ID = ut.ID,
            //                UserID = ut.UserID,
            //                ModifyDate = ut.ModifyDate,
            //                AttributeLabel = ut.AttributeLabel,
            //                AttributeName = ut.AttributeName,
            //                AttributeValue = ut.AttributeValue,
            //                MediaLink = ut.MediaLink,
            //                RevisionNumber = ut.RevisionNumber
            //            }).ToList(),
            //        LanguageCode =
            //            _unitOfWork.UserSettingRepository.Get(s => s.UserID == u.ID)?.LanguageCode ?? "en-US",
            //    }).FirstOrDefault();

            #endregion Comments

            if (user != null)
            {
                user.ProfileImage = user.UserMetas.FirstOrDefault(m =>
                                            m.MetaKey.Trim().ToLower() == ("profilePhoto").Trim().ToLower())
                                        ?.MetaValue ??
                                    "user_no_photo.png";
                bool matched = false;
                var strEmail = _helper.GetAppSetting("excludingSubmitCurrentWeight");
                if (!string.IsNullOrEmpty(strEmail))
                {
                    var strEmails = strEmail.Split(';');
                    foreach (var str in strEmails)
                    {
                        if (user.UserEmail.Trim().ToLower() == str.Trim().ToLower())
                            matched = true;
                    }
                }

                if (matched)
                    user.IsWeightSubmissionRequired = false;
                else
                {
                    var lastTracker = user.UserTrackers.OrderBy(t => t.ID).LastOrDefault();
                    double.TryParse(new Helper.Helper().GetAppSetting("WeightSubmitIntervalDays"),
                        out double interval);
                    DateTime.TryParse(lastTracker?.ModifyDate.AddDays(interval).ToShortDateString(),
                        out DateTime submitDate);
                    user.IsWeightSubmissionRequired = DateTime.Compare(submitDate, DateTime.Today) != 1;
                }
            }

            return user;
        }

        public async Task<UserGlobal> GetFullUserAsync(string token)
        {
            var user = new UserGlobal();
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var userData = await _unitOfWork.UserRepository.GetByIDAsync(tokenDetail.UserID);
            if (userData != null)
            {
                user = new UserGlobal()
                {
                    ID = userData.ID,
                    UserFirstName = userData.UserFirstName,
                    UserEmail = userData.UserEmail,
                    UserLogin = userData.UserLogin,
                    UserLastName = userData.UserLastName,
                    UserRegistered = userData.UserRegistered,
                    UserStatus = userData.UserStatus,
                    UserKey = userData.UserKey,
                    UserType = userData.UserType,
                };

                var userMilestones =
                    await _userMilestoneServices.GetAsync(um => um.UserID == userData.ID && um.IsPercentage);

                var userAchievement = await _milestonePercentageServices.GetLastAsync(mp =>
                    userMilestones.Any(um => um.MilestonePercentageId == mp.MilestonePercentageId));

                if (userAchievement != null)
                    user.Achievement = new MilestonePercentage()
                    {
                        ID = userAchievement.MilestonePercentageId,
                        MilestonePercentageId = userAchievement.MilestonePercentageId,
                        LanguageCode = userAchievement.LanguageCode,
                        Active = userAchievement.Active,
                        MilestoneSubTitle = userAchievement.MilestoneSubTitle,
                        TargetPercentValue = userAchievement.TargetPercentValue,
                        AchievedMessage = userAchievement.AchievedMessage,
                        MilestoneTitle = userAchievement.MilestoneTitle,
                        IsPercent = userAchievement.IsPercent,
                        AchievementIcon = userAchievement.AchievementIcon,
                        AchievementGiftImage = userAchievement.AchievementGiftImage
                    };

                var userMetas = await _userMetaServices.GetAsync(m => m.UserID == userData.ID);

                if (userMetas != null)
                    user.UserMetas = (from m in userMetas
                        select new UserMetaGlobal()
                        {
                            ID = m.ID,
                            UserID = m.UserID,
                            MetaDescription = m.MetaDescription,
                            MetaKey = m.MetaKey,
                            MetaLabel = m.MetaLabel,
                            MetaType = m.MetaType,
                            MetaValue = m.MetaValue,
                            ModifyDate = m.ModifyDate
                        }).ToList();

                var userTrackers = await _userTrackerServices.GetAsync(userData.ID);
                if (userTrackers != null)
                    user.UserTrackers = (from ut in userTrackers
                        orderby ut.ID
                        select new UserTrackerGlobal()
                        {
                            ID = ut.ID,
                            UserID = ut.UserID,
                            ModifyDate = ut.ModifyDate,
                            AttributeLabel = ut.AttributeLabel,
                            AttributeName = ut.AttributeName,
                            AttributeValue = ut.AttributeValue,
                            MediaLink = ut.MediaLink,
                            RevisionNumber = ut.RevisionNumber
                        }).ToList();

                var userSetting = await _userSettingServices.GetAsync(s => s.UserID == userData.ID);
                user.LanguageCode = userSetting?.LanguageCode ?? "en-US";
            }

            #region Comments

            //var user = (from u in await _unitOfWork.UserRepository.GetManyAsync(u => u.ID == (tokenDetail?.UserID ?? 0))
            //    select new UserGlobal()
            //    {
            //        ID = u.ID,
            //        UserFirstName = u.UserFirstName,
            //        UserEmail = u.UserEmail,
            //        UserLogin = u.UserLogin,
            //        UserLastName = u.UserLastName,
            //        UserRegistered = u.UserRegistered,
            //        UserStatus = u.UserStatus,
            //        UserKey = u.UserKey,
            //        UserType = u.UserType,
            //        Achievement = (from mp in _unitOfWork.MilestonePercentageRepository.GetMany(mp =>
            //                _unitOfWork.UserMilestoneRepository.GetMany(um =>
            //                    um.UserID == u.ID && um.MilestonePercentageId == mp.MilestonePercentageId &&
            //                    um.IsPercentage).Any())
            //            select new MilestonePercentage()
            //            {
            //                ID = mp.MilestonePercentageId,
            //                MilestonePercentageId = mp.MilestonePercentageId,
            //                LanguageCode = mp.LanguageCode,
            //                Active = mp.Active,
            //                MilestoneSubTitle = mp.MilestoneSubTitle,
            //                TargetPercentValue = mp.TargetPercentValue,
            //                AchievedMessage = mp.AchievedMessage,
            //                MilestoneTitle = mp.MilestoneTitle,
            //                IsPercent = mp.IsPercent,
            //                AchievementIcon = mp.AchievementIcon,
            //                AchievementGiftImage = mp.AchievementGiftImage
            //            }).OrderByDescending(mp => mp.ID)?.FirstOrDefault(),
            //        UserMetas = (from m in _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == u.ID)
            //            select new UserMetaGlobal()
            //            {
            //                ID = m.ID,
            //                UserID = m.UserID,
            //                MetaDescription = m.MetaDescription,
            //                MetaKey = m.MetaKey,
            //                MetaLabel = m.MetaLabel,
            //                MetaType = m.MetaType,
            //                MetaValue = m.MetaValue,
            //                ModifyDate = m.ModifyDate
            //            }).ToList(),
            //        UserTrackers = (from ut in _unitOfWork.UserTrackerRepository.GetMany(ut => ut.UserID == u.ID)
            //            orderby ut.ID
            //            select new UserTrackerGlobal()
            //            {
            //                ID = ut.ID,
            //                UserID = ut.UserID,
            //                ModifyDate = ut.ModifyDate,
            //                AttributeLabel = ut.AttributeLabel,
            //                AttributeName = ut.AttributeName,
            //                AttributeValue = ut.AttributeValue,
            //                MediaLink = ut.MediaLink,
            //                RevisionNumber = ut.RevisionNumber
            //            }).ToList(),
            //        LanguageCode =
            //            _unitOfWork.UserSettingRepository.Get(s => s.UserID == u.ID)?.LanguageCode ?? "en-US",
            //    }).FirstOrDefault();

            #endregion Comments

            if (user != null)
            {
                user.ProfileImage = user.UserMetas.FirstOrDefault(m =>
                                            m.MetaKey.Trim().ToLower() == ("profilePhoto").Trim().ToLower())
                                        ?.MetaValue ??
                                    "user_no_photo.png";
                bool matched = false;
                var strEmail = _helper.GetAppSetting("excludingSubmitCurrentWeight");
                if (!string.IsNullOrEmpty(strEmail))
                {
                    var strEmails = strEmail.Split(';');
                    foreach (var str in strEmails)
                    {
                        if (user.UserEmail.Trim().ToLower() == str.Trim().ToLower())
                            matched = true;
                    }
                }

                if (matched)
                    user.IsWeightSubmissionRequired = false;
                else
                {
                    var lastTracker = user.UserTrackers.OrderBy(t => t.ID).LastOrDefault();
                    double.TryParse(new Helper.Helper().GetAppSetting("WeightSubmitIntervalDays"),
                        out double interval);
                    DateTime.TryParse(lastTracker?.ModifyDate.AddDays(interval).ToShortDateString(),
                        out DateTime submitDate);
                    user.IsWeightSubmissionRequired = DateTime.Compare(submitDate, DateTime.Today) != 1;
                }
            }

            return user;
        }

        
        public UserGlobal GetFullUser(long userId)
        {
            var user = new UserGlobal();
            var userData = _unitOfWork.UserRepository.GetByID(userId);
            if (userData != null)
            {
                user = new UserGlobal()
                {
                    ID = userData.ID,
                    UserFirstName = userData.UserFirstName,
                    UserEmail = userData.UserEmail,
                    UserLogin = userData.UserLogin,
                    UserLastName = userData.UserLastName,
                    UserRegistered = userData.UserRegistered,
                    UserStatus = userData.UserStatus,
                    UserKey = userData.UserKey,
                    UserType = userData.UserType,
                };

                var userMilestones = _userMilestoneServices.Get(um => um.UserID == userData.ID && um.IsPercentage);

                var userAchievement = _milestonePercentageServices.GetLast(mp =>
                    userMilestones.Any(um => um.MilestonePercentageId == mp.MilestonePercentageId));

                if (userAchievement != null)
                    user.Achievement = new MilestonePercentage()
                    {
                        ID = userAchievement.MilestonePercentageId,
                        MilestonePercentageId = userAchievement.MilestonePercentageId,
                        LanguageCode = userAchievement.LanguageCode,
                        Active = userAchievement.Active,
                        MilestoneSubTitle = userAchievement.MilestoneSubTitle,
                        TargetPercentValue = userAchievement.TargetPercentValue,
                        AchievedMessage = userAchievement.AchievedMessage,
                        MilestoneTitle = userAchievement.MilestoneTitle,
                        IsPercent = userAchievement.IsPercent,
                        AchievementIcon = userAchievement.AchievementIcon,
                        AchievementGiftImage = userAchievement.AchievementGiftImage
                    };

                var userMetas = _userMetaServices.Get(m => m.UserID == userData.ID);

                if (userMetas != null)
                    user.UserMetas = (from m in userMetas
                        select new UserMetaGlobal()
                        {
                            ID = m.ID,
                            UserID = m.UserID,
                            MetaDescription = m.MetaDescription,
                            MetaKey = m.MetaKey,
                            MetaLabel = m.MetaLabel,
                            MetaType = m.MetaType,
                            MetaValue = m.MetaValue,
                            ModifyDate = m.ModifyDate
                        }).ToList();

                var userTrackers = _userTrackerServices.Get(userData.ID);
                if (userTrackers != null)
                    user.UserTrackers = (from ut in userTrackers
                        orderby ut.ID
                        select new UserTrackerGlobal()
                        {
                            ID = ut.ID,
                            UserID = ut.UserID,
                            ModifyDate = ut.ModifyDate,
                            AttributeLabel = ut.AttributeLabel,
                            AttributeName = ut.AttributeName,
                            AttributeValue = ut.AttributeValue,
                            MediaLink = ut.MediaLink,
                            RevisionNumber = ut.RevisionNumber
                        }).ToList();

                var userSetting = _userSettingServices.Get(s => s.UserID == userData.ID);
                user.LanguageCode = userSetting?.LanguageCode ?? "en-US";
            }

            if (user != null)
            {
                user.ProfileImage = user.UserMetas.FirstOrDefault(m =>
                                            m.MetaKey.Trim().ToLower() == ("profilePhoto").Trim().ToLower())
                                        ?.MetaValue ??
                                    "user_no_photo.png";
                bool matched = false;
                var strEmail = _helper.GetAppSetting("excludingSubmitCurrentWeight");
                if (!string.IsNullOrEmpty(strEmail))
                {
                    var strEmails = strEmail.Split(';');
                    foreach (var str in strEmails)
                    {
                        if (user.UserEmail.Trim().ToLower() == str.Trim().ToLower())
                            matched = true;
                    }
                }

                if (matched)
                    user.IsWeightSubmissionRequired = false;
                else
                {
                    var lastTracker = user.UserTrackers.OrderBy(t => t.ID).LastOrDefault();
                    double.TryParse(new Helper.Helper().GetAppSetting("WeightSubmitIntervalDays"),
                        out double interval);
                    DateTime.TryParse(lastTracker?.ModifyDate.AddDays(interval).ToShortDateString(),
                        out DateTime submitDate);
                    user.IsWeightSubmissionRequired = DateTime.Compare(submitDate, DateTime.Today) != 1;
                }
            }

            return user;
        }

        public async Task<UserGlobal> GetFullUserAsync(long userId)
        {
            var user = new UserGlobal();
            var userData = await _unitOfWork.UserRepository.GetByIDAsync(userId);
            if (userData != null)
            {
                user = new UserGlobal()
                {
                    ID = userData.ID,
                    UserFirstName = userData.UserFirstName,
                    UserEmail = userData.UserEmail,
                    UserLogin = userData.UserLogin,
                    UserLastName = userData.UserLastName,
                    UserRegistered = userData.UserRegistered,
                    UserStatus = userData.UserStatus,
                    UserKey = userData.UserKey,
                    UserType = userData.UserType,
                };

                var userMilestones =
                    await _userMilestoneServices.GetAsync(um => um.UserID == userData.ID && um.IsPercentage);

                var userAchievement = await _milestonePercentageServices.GetLastAsync(mp =>
                    userMilestones.Any(um => um.MilestonePercentageId == mp.MilestonePercentageId));

                if (userAchievement != null)
                    user.Achievement = new MilestonePercentage()
                    {
                        ID = userAchievement.MilestonePercentageId,
                        MilestonePercentageId = userAchievement.MilestonePercentageId,
                        LanguageCode = userAchievement.LanguageCode,
                        Active = userAchievement.Active,
                        MilestoneSubTitle = userAchievement.MilestoneSubTitle,
                        TargetPercentValue = userAchievement.TargetPercentValue,
                        AchievedMessage = userAchievement.AchievedMessage,
                        MilestoneTitle = userAchievement.MilestoneTitle,
                        IsPercent = userAchievement.IsPercent,
                        AchievementIcon = userAchievement.AchievementIcon,
                        AchievementGiftImage = userAchievement.AchievementGiftImage
                    };

                var userMetas = await _userMetaServices.GetAsync(m => m.UserID == userData.ID);

                if (userMetas != null)
                    user.UserMetas = (from m in userMetas
                        select new UserMetaGlobal()
                        {
                            ID = m.ID,
                            UserID = m.UserID,
                            MetaDescription = m.MetaDescription,
                            MetaKey = m.MetaKey,
                            MetaLabel = m.MetaLabel,
                            MetaType = m.MetaType,
                            MetaValue = m.MetaValue,
                            ModifyDate = m.ModifyDate
                        }).ToList();

                var userTrackers = await _userTrackerServices.GetAsync(userData.ID);
                if (userTrackers != null)
                    user.UserTrackers = (from ut in userTrackers
                        orderby ut.ID
                        select new UserTrackerGlobal()
                        {
                            ID = ut.ID,
                            UserID = ut.UserID,
                            ModifyDate = ut.ModifyDate,
                            AttributeLabel = ut.AttributeLabel,
                            AttributeName = ut.AttributeName,
                            AttributeValue = ut.AttributeValue,
                            MediaLink = ut.MediaLink,
                            RevisionNumber = ut.RevisionNumber
                        }).ToList();

                var userSetting = await _userSettingServices.GetAsync(s => s.UserID == userData.ID);
                user.LanguageCode = userSetting?.LanguageCode ?? "en-US";
            }

            if (user != null)
            {
                user.ProfileImage = user.UserMetas.FirstOrDefault(m =>
                                            m.MetaKey.Trim().ToLower() == ("profilePhoto").Trim().ToLower())
                                        ?.MetaValue ??
                                    "user_no_photo.png";
                bool matched = false;
                var strEmail = _helper.GetAppSetting("excludingSubmitCurrentWeight");
                if (!string.IsNullOrEmpty(strEmail))
                {
                    var strEmails = strEmail.Split(';');
                    foreach (var str in strEmails)
                    {
                        if (user.UserEmail.Trim().ToLower() == str.Trim().ToLower())
                            matched = true;
                    }
                }

                if (matched)
                    user.IsWeightSubmissionRequired = false;
                else
                {
                    var lastTracker = user.UserTrackers.OrderBy(t => t.ID).LastOrDefault();
                    double.TryParse(new Helper.Helper().GetAppSetting("WeightSubmitIntervalDays"),
                        out double interval);
                    DateTime.TryParse(lastTracker?.ModifyDate.AddDays(interval).ToShortDateString(),
                        out DateTime submitDate);
                    user.IsWeightSubmissionRequired = DateTime.Compare(submitDate, DateTime.Today) != 1;
                }
            }

            return user;
        }
        public bool Update(ref ValidationErrors validationErrors, string token, User entity,
            ICollection<UserMeta> userMetas)
        {
            dynamic[] obj = {entity, false, true, false, false, false};
            if (this.Validate(ref validationErrors, obj))
            {
                var date = DateTime.Now;
                var user = GetUser(token);
                if (!(user is null))
                {
                    user.UserFirstName = entity.UserFirstName;
                    user.UserEmail = entity.UserEmail;
                    user.UserLastName = entity.UserLastName;
                    _unitOfWork.UserRepository.Update(user);
                    var result = _unitOfWork.Commit();
                    if (result)
                        foreach (var userMeta in userMetas)
                        {
                            var meta = _userMetaServices.GetByUser(token, userMeta.MetaKey);
                            if (meta != null)
                            {
                                userMeta.UserID = user.ID;
                                _userMetaServices.Insert(ref validationErrors, userMeta);
                            }
                            else
                            {
                                meta.MetaValue = userMeta.MetaValue;
                                meta.ModifyDate = date;
                                meta.MetaDescription = meta.MetaDescription;
                                _userMetaServices.Update(ref validationErrors, meta);
                            }
                        }
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, long userId, User entity,
            ICollection<UserMeta> userMetas)
        {
            dynamic[] obj = {entity, false, true, false, false, false};
            if (this.Validate(ref validationErrors, obj))
            {
                var date = DateTime.Now;
                var user = _unitOfWork.UserRepository.Get(u => u.ID == userId);
                if (!(user is null))
                {
                    user.UserFirstName = entity.UserFirstName;
                    user.UserEmail = entity.UserEmail;
                    user.UserLastName = entity.UserLastName;
                    _unitOfWork.UserRepository.Update(user);
                    var result = _unitOfWork.Commit();
                    if (result)
                        foreach (var userMeta in userMetas)
                        {
                            var meta = _userMetaServices.GetByUser(userId, userMeta.MetaKey);
                            if (meta != null)
                            {
                                userMeta.UserID = user.ID;
                                _userMetaServices.Insert(ref validationErrors, userMeta);
                            }
                            else
                            {
                                meta.MetaValue = userMeta.MetaValue;
                                meta.ModifyDate = date;
                                meta.MetaDescription = meta.MetaDescription;
                                _userMetaServices.Update(ref validationErrors, meta);
                            }
                        }
                }
            }
            else
                validationErrors.Add("MessageInvalidProvidedInformation");

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, string token, string application)
        {
            if (!string.IsNullOrEmpty(application))
            {
                var date = DateTime.Now;
                var user = GetUser(token);
                if (!(user is null))
                {
                    user.UserApplication = application;
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }

            return false;
        }

        public bool Update(ref ValidationErrors validationErrors, long userId, string application)
        {
            if (!string.IsNullOrEmpty(application))
            {
                var date = DateTime.Now;
                var user = _unitOfWork.UserRepository.Get(u => u.ID == userId);
                if (!(user is null))
                {
                    user.UserApplication = application;
                    _unitOfWork.UserRepository.Update(user);
                    return _unitOfWork.Commit();
                }
            }

            return false;
        }
        public UserGlobal Get(string token, string metaKey)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == (tokenDetail?.UserID ?? 0))
                select new UserGlobal()
                {
                    ID = u.ID,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLogin = u.UserLogin,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserStatus = u.UserStatus,
                    UserKey = u.UserKey,
                    UserType = u.UserType,
                    UserApplication = u.UserApplication
                }).FirstOrDefault();
            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";

                var meta = _userMetaServices.GetByMetaKey(tokenDetail.UserID, metaKey);
                if (meta != null)
                {
                    user.ProfileImage = meta ?? "";
                }
            }

            return user;
        }
        //public UserGlobal Get(long userId, string metaKey)
        //{
        //    var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == userId)
        //        select new UserGlobal()
        //        {
        //            ID = u.ID,
        //            UserFirstName = u.UserFirstName,
        //            UserEmail = u.UserEmail,
        //            UserLogin = u.UserLogin,
        //            UserLastName = u.UserLastName,
        //            UserRegistered = u.UserRegistered,
        //            UserStatus = u.UserStatus,
        //            UserKey = u.UserKey,
        //            UserType = u.UserType,
        //            UserApplication = u.UserApplication
        //        }).FirstOrDefault();
        //    if (user != null)
        //    {
        //        var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
        //        user.LanguageCode = settings?.LanguageCode ?? "en-US";
        //        user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";

        //        var meta = _userMetaServices.GetByMetaKey(userId, metaKey);
        //        if (meta != null)
        //        {
        //            user.ProfileImage = meta ?? "";
        //        }
        //    }

        //    return user;
        //}

        public async Task<UserGlobal> GetAsync(string token, string metaKey)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            var user = (from u in await _unitOfWork.UserRepository.GetManyAsync(u => u.ID == tokenDetail?.UserID)
                select new UserGlobal()
                {
                    ID = u.ID,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLogin = u.UserLogin,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserStatus = u.UserStatus,
                    UserKey = u.UserKey,
                    UserType = u.UserType,
                    UserApplication = u.UserApplication
                }).FirstOrDefault();
            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";

                var meta = await _userMetaServices.GetByMetaKeyAsync(tokenDetail.UserID, "profilephoto");
                if (meta != null)
                {
                    user.ProfileImage = meta ?? "";
                }
            }

            return user;
        }

        public UserAuthentication GetUserAuthentication(long id)
        {
            var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == id)
                           select new UserAuthentication()
                           {
                               ID = u.ID,
                               UserLogin = u.UserLogin,
                               UserFirstName = u.UserFirstName,
                               UserEmail = u.UserEmail,
                               UserLastName = u.UserLastName,
                               UserRegistered = u.UserRegistered,
                               UserApplication = u.UserApplication
                           })?.FirstOrDefault() ?? null;

            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";
                var meta = _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == user.ID);
                user.IsMetaExists = meta.Any();
                user.IsAddressExists = meta.Any(ma => ma.MetaKey.Contains("address"));
                user.IsTrackerExists = _unitOfWork.UserTrackerRepository.Get(t => t.UserID == user.ID && t.AttributeName.Contains("frontimage")) != null;
            }

            return user;
        }

        public async Task<UserAuthentication> GetUserAuthenticationAsync(long id)
        {
            var user = (from u in await _unitOfWork.UserRepository.GetManyAsync(u => u.ID == id)
                           select new UserAuthentication()
                           {
                               ID = u.ID,
                               UserLogin = u.UserLogin,
                               UserFirstName = u.UserFirstName,
                               UserEmail = u.UserEmail,
                               UserLastName = u.UserLastName,
                               UserRegistered = u.UserRegistered,
                               UserApplication = u.UserApplication
                           })?.FirstOrDefault() ?? null;
            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";
                var meta = await _unitOfWork.UserMetaRepository.GetManyAsync(m => m.UserID == user.ID);
                user.IsMetaExists = meta.Any();
                user.IsAddressExists = meta.Any(ma => ma.MetaKey.Contains("address"));
                user.IsTrackerExists =
                    await _unitOfWork.UserTrackerRepository.GetAsync(t =>
                        t.UserID == user.ID && t.AttributeName.Contains("frontimage")) != null;
            }

            return user;
        }

        public UserAuthentication GetUserAuthentication(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == tokenDetail.UserID)
                select new UserAuthentication()
                {
                    ID = u.ID,
                    UserLogin = u.UserLogin,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserApplication = u.UserApplication
                })?.FirstOrDefault();
            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";
                var meta = _unitOfWork.UserMetaRepository.GetMany(m => m.UserID == user.ID);
                user.IsMetaExists = meta.Any();
                user.IsAddressExists = meta.Any(ma => ma.MetaKey.Contains("address"));
                user.IsTrackerExists = _unitOfWork.UserTrackerRepository.Get(t => t.UserID == user.ID && t.AttributeName.Contains("frontimage")) != null;
            }

            return user;
        }

        public async Task<UserAuthentication> GetUserAuthenticationAsync(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == tokenDetail.UserID)
                           select new UserAuthentication()
                           {
                               ID = u.ID,
                               UserLogin = u.UserLogin,
                               UserFirstName = u.UserFirstName,
                               UserEmail = u.UserEmail,
                               UserLastName = u.UserLastName,
                               UserRegistered = u.UserRegistered,
                               UserApplication = u.UserApplication
                           })?.FirstOrDefault() ?? null;

            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";
                var meta = await _unitOfWork.UserMetaRepository.GetManyAsync(m => m.UserID == user.ID);
                user.IsMetaExists = meta.Any();
                user.IsAddressExists = meta.Any(ma => ma.MetaKey.Contains("address"));
                user.IsTrackerExists =
                    await _unitOfWork.UserTrackerRepository.GetAsync(t =>
                        t.UserID == user.ID && t.AttributeName.Contains("frontimage")) != null;
            }

            return user;
        }

        public UserGlobal Get(long id, string metaKey)
        {
            var user = (from u in _unitOfWork.UserRepository.GetMany(u => u.ID == id)
                select new UserGlobal()
                {
                    ID = u.ID,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLogin = u.UserLogin,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserStatus = u.UserStatus,
                    UserKey = u.UserKey,
                    UserType = u.UserType,
                    UserApplication = u.UserApplication
                }).FirstOrDefault();
            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";
                var meta = _userMetaServices.GetByMetaKey(id, metaKey);
                if (meta != null)
                {
                    user.ProfileImage = meta ?? "";
                }
            }

            return user;
        }

        public async Task<UserGlobal> GetAsync(long id, string metaKey)
        {
            var user = (from u in await _unitOfWork.UserRepository.GetManyAsync(u => u.ID == id)
                select new UserGlobal()
                {
                    ID = u.ID,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLogin = u.UserLogin,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserStatus = u.UserStatus,
                    UserKey = u.UserKey,
                    UserType = u.UserType,
                    UserApplication = u.UserApplication,
                }).FirstOrDefault();

            if (user != null)
            {
                var settings = await _unitOfWork.UserSettingRepository.GetFirstAsync(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";

                var meta = await _userMetaServices.GetByMetaKeyAsync(id, metaKey);
                if (meta != null)
                {
                    user.ProfileImage = meta ?? "";
                }
            }

            return user;
        }

        public UserGlobal Get(long id, string metaKey, string application)
        {
            var user = (from u in _unitOfWork.UserRepository.GetMany(
                    u => u.ID == id && u.UserApplication == application)
                select new UserGlobal()
                {
                    ID = u.ID,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLogin = u.UserLogin,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserStatus = u.UserStatus,
                    UserKey = u.UserKey,
                    UserType = u.UserType,
                    UserApplication = u.UserApplication
                }).FirstOrDefault();

            if (user != null)
            {
                var settings = _unitOfWork.UserSettingRepository.GetFirst(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";
                var meta = _userMetaServices.GetByMetaKey(id, metaKey);
                if (meta != null)
                {
                    user.ProfileImage = meta ?? "";
                }
            }

            return user;
        }

        public async Task<UserGlobal> GetAsync(long id, string metaKey, string application)
        {
            var user = (from u in await _unitOfWork.UserRepository.GetManyAsync(
                    u => u.ID == id && u.UserApplication == application)
                select new UserGlobal()
                {
                    ID = u.ID,
                    UserFirstName = u.UserFirstName,
                    UserEmail = u.UserEmail,
                    UserLogin = u.UserLogin,
                    UserLastName = u.UserLastName,
                    UserRegistered = u.UserRegistered,
                    UserStatus = u.UserStatus,
                    UserKey = u.UserKey,
                    UserType = u.UserType,
                    UserApplication = u.UserApplication
                }).FirstOrDefault();

            if (user != null)
            {
                var settings = await _unitOfWork.UserSettingRepository.GetFirstAsync(s => s.UserID == user.ID);
                user.LanguageCode = settings?.LanguageCode ?? "en-US";
                user.WeightVolumeType = settings?.WeightVolumeType ?? "kg";
                var meta = await _userMetaServices.GetByMetaKeyAsync(id, metaKey);
                if (meta != null)
                {
                    user.ProfileImage = meta ?? "";
                }
            }

            return user;
        }

        private bool EmailExists(Int64 userId, string email)
        {
            return _unitOfWork.UserRepository.GetAll()
                .Any(u => u.ID != userId && u.UserEmail.Trim().ToLower() == email.Trim().ToLower());
        }

        private bool VerifyPassword(string encrypted, string plain)
        {
            return VerifySha512(encrypted, plain) || VerifyHash(encrypted, plain);
        }

        private bool VerifySha512(string encrypted, string plain)
        {
            return _helper.CompareSHA512(encrypted, plain);
        }

        private bool VerifySha512_WrongEncryption(string encrypted, string plain)
        {
            return _helper.CompareSHA512_WrongEncryption(encrypted, plain);
        }

        private bool VerifyHash(string encrypted, string plain)
        {
            return _helper.VerifyPassword(plain, encrypted);
        }

        public List<LoginCredential> GetLoginCredential(string username)
        {
            return (from u in _unitOfWork.UserRepository.GetMany(u =>
                    u.UserLogin == (username != null && username.Trim().Length > 0 ? username : u.UserLogin))
                select new LoginCredential()
                {
                    Username = u.UserLogin,
                    Password = u.UserPassword
                }).ToList();
        }

        public async Task<List<LoginCredential>> GetLoginCredentialAsync(string username)
        {
            return (from u in await _unitOfWork.UserRepository.GetManyAsync(u =>
                    u.UserLogin == (username != null && username.Trim().Length > 0 ? username : u.UserLogin))
                select new LoginCredential()
                {
                    Username = u.UserLogin,
                    Password = u.UserPassword
                }).ToList();
        }

        public User GetByEmail(string email)
        {
            return _unitOfWork.UserRepository.Get(u => u.UserEmail == email);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetAsync(u => u.UserEmail == email);
        }
    }
}