using Organo.Solutions.X4Ever.V1.DAL.Model;
using System;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IPasswordHistoryServices
    {
        DateTime Now { get; }

        bool IsMatch(Int64 userID, string validationKey);
        Task<bool> IsMatchAsync(Int64 userID, string validationKey);

        UserPasswordHistory GetByKey(Int64 userID, string validationKey);
        Task<UserPasswordHistory> GetByKeyAsync(Int64 userID, string validationKey);

        void Insert(UserPasswordHistory userPassword);
    }
}