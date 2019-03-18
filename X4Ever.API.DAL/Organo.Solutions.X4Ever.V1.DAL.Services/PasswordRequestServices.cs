using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class PasswordRequestServices : IPasswordRequestServices
    {
        private IUnitOfWork _unitOfWork;

        public PasswordRequestServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public UserPasswordRequest GetByRequestCode(string requestCode)
        {
            return _unitOfWork.PasswordRequestRepository.Get(r =>
                r.RequestCode.ToLower() == requestCode.ToLower() && r.RequestCodeExpiresOn > this.Now);
        }

        public async Task<UserPasswordRequest> GetByRequestCodeAsync(string requestCode)
        {
            return await _unitOfWork.PasswordRequestRepository.GetAsync(r =>
                r.RequestCode.ToLower() == requestCode.ToLower() && r.RequestCodeExpiresOn > this.Now);
        }

        public bool IsValid(string requestCode)
        {
            var request = _unitOfWork.PasswordRequestRepository.Get(r =>
                r.RequestCode.ToLower() == requestCode.ToLower() && r.RequestCodeExpiresOn > this.Now);
            return request != null;
        }

        public async Task<bool> IsValidAsync(string requestCode)
        {
            var request = await _unitOfWork.PasswordRequestRepository.GetAsync(r =>
                r.RequestCode.ToLower() == requestCode.ToLower() && r.RequestCodeExpiresOn > this.Now);
            return request != null;
        }
    }
}