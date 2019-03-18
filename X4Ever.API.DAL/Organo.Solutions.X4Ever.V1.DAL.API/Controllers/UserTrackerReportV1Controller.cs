using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/userreports")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserTrackerReportV1Controller : ApiController //Override
    {
        private readonly IUserTrackerReportServices _trackerReportServices;

        public UserTrackerReportV1Controller(UserTrackerReportServices trackerReportServices)
        {
            _trackerReportServices = trackerReportServices;
        }

        [GET("getallasync")]
        [Route("getallasync")]
        public async Task<HttpResponseMessage> GetAllAsync()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var userTrackers = await _trackerReportServices.GetAllAsync();
            watch.Stop();
            var response = Request.CreateResponse(HttpStatusCode.OK, userTrackers);
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        [GET("getlatestasync")]
        [Route("getlatestasync")]
        public async Task<HttpResponseMessage> GetLatestAsync()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var userTrackers = await _trackerReportServices.GetLatestAsync();
            watch.Stop();
            var response = Request.CreateResponse(HttpStatusCode.OK, userTrackers);
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        //private Func<IQueryable<UserTrackerDetailReportV2>, IOrderedQueryable<UserTrackerDetailReportV2>> OrderBy(
        //    string orderByColumnName, bool asc = false)
        //{
        //    switch (orderByColumnName)
        //    {
        //        case "user_id":
        //            if (asc)
        //                return q => q.OrderBy(s => s.UserID);
        //            else
        //                return q => q.OrderByDescending(s => s.UserID);
        //        case "user_first_name":
        //            if (asc)
        //                return q => q.OrderBy(s => s.FirstName);
        //            else
        //                return q => q.OrderByDescending(s => s.FirstName);
        //        case "user_last_name":
        //            if (asc)
        //                return q => q.OrderBy(s => s.LastName);
        //            else
        //                return q => q.OrderByDescending(s => s.LastName);
        //        case "user_email":
        //            if (asc)
        //                return q => q.OrderBy(s => s.EmailAddress);
        //            else
        //                return q => q.OrderByDescending(s => s.EmailAddress);
        //        case "date_created":
        //            if (asc)
        //                return q => q.OrderBy(s => s.CreateDate);
        //            else
        //                return q => q.OrderByDescending(s => s.CreateDate);
        //        case "application_name":
        //            if (asc)
        //                return q => q.OrderBy(s => s.ApplicationName);
        //            else
        //                return q => q.OrderByDescending(s => s.ApplicationName);
        //        case "country":
        //            if (asc)
        //                return q => q.OrderBy(s => s.Country);
        //            else
        //                return q => q.OrderByDescending(s => s.Country);
        //        case "gender":
        //            if (asc)
        //                return q => q.OrderBy(s => s.Gender);
        //            else
        //                return q => q.OrderByDescending(s => s.Gender);
        //        case "start_weight":
        //            if (asc)
        //                return q => q.OrderBy(s => s.StartWeight);
        //            else
        //                return q => q.OrderByDescending(s => s.StartWeight);
        //        case "weight_to_lose":
        //            if (asc)
        //                return q => q.OrderBy(s => s.WeightToLose);
        //            else
        //                return q => q.OrderByDescending(s => s.WeightToLose);
        //        case "weight_volume_type":
        //            if (asc)
        //                return q => q.OrderBy(s => s.WeightVolumeType);
        //            else
        //                return q => q.OrderByDescending(s => s.WeightVolumeType);
        //        case "weekly_weight_lost":
        //            if (asc)
        //                return q => q.OrderBy(s => s.WeeklyWeightLost);
        //            else
        //                return q => q.OrderByDescending(s => s.WeeklyWeightLost);
        //        case "weight_goal_reached":
        //            if (asc)
        //                return q => q.OrderBy(s => s.WeightGoalReached);
        //            else
        //                return q => q.OrderByDescending(s => s.WeightGoalReached);
        //        case "tshirt_size":
        //            if (asc)
        //                return q => q.OrderBy(s => s.TShirtSize);
        //            else
        //                return q => q.OrderByDescending(s => s.TShirtSize);
        //        case "tracker_create_date":
        //            if (asc)
        //                return q => q.OrderBy(s => s.TrackerCreateDate);
        //            else
        //                return q => q.OrderByDescending(s => s.TrackerCreateDate);
        //        default:
        //            if (asc)
        //                return q => q.OrderBy(s => s.UserID);
        //            else
        //                return q => q.OrderByDescending(s => s.UserID);
        //    }
        //}
    }
}