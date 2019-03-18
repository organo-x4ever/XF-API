using Organo.Solutions.X4Ever.V1.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IWebUserMetaRealtimeServices : IWebUserServices<WebUserTrackerViewPivot>
    {
        Task<IEnumerable<WebUserMetaPivot>> GetWebUserWeightTrackingAsync(DateTime fromDate);
        Task<IEnumerable<WebUserMetaPivot>> GetWebUserWeightTracking2018Async(DateTime fromDate);
    }

    public interface IWebUserServices<T>
    {
        Task<IEnumerable<WebUserTrackerViewPivot>> GetWebUserTrackerViewPivotAsync(
            Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    }
}