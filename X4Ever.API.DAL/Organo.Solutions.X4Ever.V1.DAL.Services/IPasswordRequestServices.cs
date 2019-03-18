using Organo.Solutions.X4Ever.V1.DAL.Model;
using System;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IPasswordRequestServices
    {
        DateTime Now { get; }

        UserPasswordRequest GetByRequestCode(string requestCode);
        Task<UserPasswordRequest> GetByRequestCodeAsync(string requestCode);

        bool IsValid(string requestCode);
        Task<bool> IsValidAsync(string requestCode);
    }
}