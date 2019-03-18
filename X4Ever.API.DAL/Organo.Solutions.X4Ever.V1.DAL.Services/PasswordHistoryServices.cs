using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class PasswordHistoryServices : IPasswordHistoryServices
    {
        private IUnitOfWork _unitOfWork;
        private Helper.IHelper _helper;

        public PasswordHistoryServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
        }

        public DateTime Now => DateTime.Now;

        public UserPasswordHistory GetByKey(long userID, string validationKey)
        {
            var userPasswords = _unitOfWork.UserPasswordHistoryRepository.GetMany(h => h.UserID == userID);
            foreach (var userPassword in userPasswords)
            {
                if (_helper.CompareSHA512(userPassword.UserPassword, validationKey))
                {
                    return userPassword;
                }

                // Temporary
                if (_helper.VerifyPassword(validationKey, userPassword.UserPassword))
                {
                    return userPassword;
                }
            }

            return null;
        }

        public async Task<UserPasswordHistory> GetByKeyAsync(long userID, string validationKey)
        {
            var userPasswords = await _unitOfWork.UserPasswordHistoryRepository.GetManyAsync(h => h.UserID == userID);
            foreach (var userPassword in userPasswords)
            {
                if (_helper.CompareSHA512(userPassword.UserPassword, validationKey))
                {
                    return userPassword;
                }

                // Temporary
                if (_helper.VerifyPassword(validationKey, userPassword.UserPassword))
                {
                    return userPassword;
                }
            }

            return null;
        }

        public void Insert(UserPasswordHistory userPassword)
        {
            _unitOfWork.UserPasswordHistoryRepository.Insert(userPassword);
        }

        public bool IsMatch(long userID, string validationKey)
        {
            var userPasswords = _unitOfWork.UserPasswordHistoryRepository.GetMany(h => h.UserID == userID);
            foreach (var userPassword in userPasswords)
            {
                if (_helper.CompareSHA512(userPassword.UserPassword, validationKey))
                {
                    return true;
                }

                // Temporary
                if (_helper.VerifyPassword(validationKey, userPassword.UserPassword))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsMatchAsync(long userID, string validationKey)
        {
            var userPasswords = await _unitOfWork.UserPasswordHistoryRepository.GetManyAsync(h => h.UserID == userID);
            foreach (var userPassword in userPasswords)
            {
                if (_helper.CompareSHA512(userPassword.UserPassword, validationKey))
                {
                    return true;
                }

                // Temporary
                if (_helper.VerifyPassword(validationKey, userPassword.UserPassword))
                {
                    return true;
                }
            }

            return false;
        }
    }
}