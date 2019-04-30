namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    using Model;
    using Repository;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserTokensServices : IUserTokensServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private Helper.IHelper _helper;

        public UserTokensServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
        }

        /// <summary>
        /// Function to generate unique token with expiry against the provided userId. Also add a
        /// record in database for generated token.
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        public UserToken GenerateToken(long userId)
        {
            var token = Guid.NewGuid().ToString().ToLower();
            DateTime issuedOn = DateTime.Now;
            DateTime expiredOn = this.TokenSessionTime();
            var tokendomain = new UserToken
            {
                UserID = userId,
                AuthToken = token,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn,
                LastActiveOn = issuedOn
            };

            //if (_unitOfWork.UserTokenRepository.Get(t =>
            //        t.AuthToken == tokendomain.AuthToken && t.UserID == tokendomain.UserID) == null)
            //{
            _unitOfWork.UserTokenRepository.Insert(tokendomain);
            _unitOfWork.Commit();
            //}
            //else
            //{
            //    _unitOfWork.UserTokenRepository.Update(tokendomain);
            //    _unitOfWork.Commit();
            //}

            return tokendomain;
        }
        
        public long Validate(string tokenId)
        {
            var token = _unitOfWork.UserTokenRepository.Get(t => t.AuthToken == tokenId.ToLower());
            if (token != null)
            {
                _unitOfWork.UserTokenRepository.Reload(token);
                if (!(DateTime.Now > token?.ExpiresOn))
                {
                    token.LastActiveOn = DateTime.Now;
                    token.ExpiresOn = this.TokenSessionTime();
                    _unitOfWork.UserTokenRepository.Update(token);
                    _unitOfWork.Commit();
                    return token.UserID;
                }
            }

            return 0;
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        /// <returns>
        /// </returns>
        public bool ValidateToken(string tokenId)
        {
            var token = _unitOfWork.UserTokenRepository.Get(t => t.AuthToken == tokenId.ToLower());
            if (token != null)
            {
                _unitOfWork.UserTokenRepository.Reload(token);
                if (!(DateTime.Now > token?.ExpiresOn))
                {
                    token.LastActiveOn = DateTime.Now;
                    token.ExpiresOn = this.TokenSessionTime();
                    _unitOfWork.UserTokenRepository.Update(token);
                    _unitOfWork.Commit();
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        /// <returns>
        /// </returns>
        public UserToken GetDetailByToken(string tokenId)
        {
            var token = _unitOfWork.UserTokenRepository.Get(t =>
                t.AuthToken == tokenId.ToLower() && t.ExpiresOn > DateTime.Now);
            if (token != null)
            {
                _unitOfWork.UserTokenRepository.Reload(token);
                if (!(DateTime.Now > token.ExpiresOn))
                {
                    return token;
                }
            }

            return null;
        }

        public async Task<UserToken> GetDetailByTokenAsync(string tokenId)
        {
            var token = await _unitOfWork.UserTokenRepository.GetAsync(t =>
                t.AuthToken == tokenId.ToLower() && t.ExpiresOn > DateTime.Now);
            if (token != null)
            {
                await _unitOfWork.UserTokenRepository.ReloadAsync(token);
                if (!(DateTime.Now > token.ExpiresOn))
                {
                    return token;
                }
            }

            return null;
        }

        /// <summary>
            /// Method to kill the provided token id.
            /// </summary>
            /// <param name="tokenId">
            /// true for successful delete
            /// </param>
            public bool Update(string tokenId)
        {
            var token = _unitOfWork.UserTokenRepository.GetSingle(testc => testc.AuthToken == tokenId.ToLower());
            token.ExpiresOn = DateTime.Now.AddMinutes(-1);
            _unitOfWork.UserTokenRepository.Update(token);
            _unitOfWork.Commit();
            var isNotDeleted = _unitOfWork.UserTokenRepository
                .GetMany(x => x.AuthToken == tokenId.ToLower() && x.ExpiresOn <= DateTime.Now).Any();
            if (isNotDeleted)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenId">
        /// true for successful delete
        /// </param>
        public bool Kill(string tokenId)
        {
            _unitOfWork.UserTokenRepository.Delete(x => x.AuthToken == tokenId.ToLower());
            _unitOfWork.Commit();
            var isNotDeleted = _unitOfWork.UserTokenRepository.GetMany(x => x.AuthToken == tokenId.ToLower()).Any();
            if (isNotDeleted)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// true for successful delete
        /// </returns>
        public bool DeleteByUserId(long userId)
        {
            _unitOfWork.UserTokenRepository.Delete(x => x.UserID == userId);
            _unitOfWork.Commit();

            var isNotDeleted = _unitOfWork.UserTokenRepository.GetMany(x => x.UserID == userId).Any();
            return !isNotDeleted;
        }

        public DateTime TokenSessionTime()
        {
            var datetime = DateTime.Now;
            try
            {
                if (int.TryParse(_helper.GetAppSetting("AuthTokenExpiry"), out int period))
                    period = 30;

                var sessionType = SessionType.Min;
                var type = _helper.GetAppSetting("AuthTokenExpiry_PeriodType", typeof(string));
                if (type != null || type.ToString().Length != 0)
                    SessionType.TryParse(type, out sessionType);

                switch (sessionType)
                {
                    case SessionType.Y:
                        datetime = DateTime.Now.AddYears(period);
                        break;

                    case SessionType.M:
                        datetime = DateTime.Now.AddMonths(period);
                        break;

                    case SessionType.D:
                        datetime = DateTime.Now.AddDays(period);
                        break;

                    case SessionType.H:
                        datetime = DateTime.Now.AddHours(period);
                        break;

                    case SessionType.Min:
                        datetime = DateTime.Now.AddHours(period);
                        break;

                    default:
                        datetime = DateTime.Now.AddMinutes(30);
                        break;
                }
            }
            catch (Exception)
            {
                datetime = DateTime.Now.AddMinutes(30);
            }

            return datetime;
        }
    }

    public enum SessionType
    {
        Y,
        M,
        D,
        H,
        Min
    }
}