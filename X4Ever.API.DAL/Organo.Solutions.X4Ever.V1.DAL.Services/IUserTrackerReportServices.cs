using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IUserTrackerReportServices
    {
        Task<IEnumerable<UserTrackerReportV1Map>> GetAllAsync();
        Task<IEnumerable<UserTrackerReportV1Map>> GetLatestAsync();

        Task<IEnumerable<UserTrackerDetailReportV2>> GetTrackerDetailAsync(
            Func<IQueryable<UserTrackerDetailReportV2>, IOrderedQueryable<UserTrackerDetailReportV2>> orderBy = null);

        Task<IEnumerable<UserTrackerDetailReportV2>> GetTrackerDetailPeriodAsync(DateTime fromDate, DateTime toDate,
            Func<IQueryable<UserTrackerDetailReportV2>, IOrderedQueryable<UserTrackerDetailReportV2>> orderBy = null);
    }
}