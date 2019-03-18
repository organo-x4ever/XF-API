using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class WebUserMetaRealtimeServices : IWebUserMetaRealtimeServices
    {
        private IUnitOfWork _unitOfWork;

        public WebUserMetaRealtimeServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WebUserTrackerViewPivot>> GetWebUserTrackerViewPivotAsync(
            Expression<Func<WebUserTrackerViewPivot, bool>> filter = null,
            Func<IQueryable<WebUserTrackerViewPivot>, IOrderedQueryable<WebUserTrackerViewPivot>> orderBy = null)
        {
            return await _unitOfWork.WebUserTrackerViewPivotRepository.GetManyAsync(filter, orderBy);
        }

        public async Task<IEnumerable<WebUserMetaPivot>> GetWebUserWeightTrackingAsync(DateTime fromDate)
        {
            var list = await _unitOfWork.WebUserMetaPivotRepository.SqlQueryAsync("x4ever.x4_webusermeta_pivot");
            return list.Where(w => w.tracker_last_date >= fromDate);
        }


        public async Task<IEnumerable<WebUserMetaPivot>> GetWebUserWeightTracking2018Async(DateTime fromDate)
        {
            var list = await _unitOfWork.WebUserMetaPivotRepository.SqlQueryAsync("x4ever.x4_webusermeta_pivot");
            return list.Where(w => w.tracker_last_date >= fromDate && w.tracker_last_date.Year == 2018);
        }
    }
}