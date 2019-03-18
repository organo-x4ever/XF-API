using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserTrackerRealtimeServices : IUserTrackerRealtimeServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserTrackerRealtimeServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserTrackerRealtime>> GetRealtimeWeightTrackingAsync(DateTime fromDate)
        {
            return await _unitOfWork.UserTrackerRealtimeRepository.GetManyAsync(t => t.TrackerLastDate >= fromDate);
        }

        public async Task<IEnumerable<UserTrackerRealtime>> GetRealtimeWeightTrackingAsync(DateTime fromDate,
            long skipUptoId)
        {
            return await _unitOfWork.UserTrackerRealtimeRepository.GetManyAsync(t =>
                t.TrackerLastDate >= fromDate && t.ID > skipUptoId);
        }

        public async Task<IEnumerable<UserTrackerRealtime>> GetRealtimeWeightTrackingIn2018Async(DateTime fromDate,
            long skipUptoId)
        {
            return await _unitOfWork.UserTrackerRealtimeRepository.GetManyAsync(t =>
                t.TrackerLastDate >= fromDate && t.TrackerLastDate.Year == 2018 && t.ID > skipUptoId);
        }
    }
}