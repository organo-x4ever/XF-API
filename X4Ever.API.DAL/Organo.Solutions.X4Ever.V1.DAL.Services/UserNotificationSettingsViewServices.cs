
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserNotificationSettingsViewServices : IUserNotificationSettingsViewServices
    {
        private IUnitOfWork _unitOfWork;
        public UserNotificationSettingsViewServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<List<UserNotificationSettingsView>> GetAsync()
        {
            return (await _unitOfWork.UserNotificationSettingsViewRepository.GetManyAsync()).ToList();
        }
    }
}