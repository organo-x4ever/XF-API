using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserTrackerRealtimeServices
    {
        Task<IEnumerable<UserTrackerRealtime>> GetRealtimeWeightTrackingAsync(DateTime fromDate);
        Task<IEnumerable<UserTrackerRealtime>> GetRealtimeWeightTrackingAsync(DateTime fromDate, long skipUptoId);
        Task<IEnumerable<UserTrackerRealtime>> GetRealtimeWeightTrackingIn2018Async(DateTime fromDate, long skipUptoId);
    }
}