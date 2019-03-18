using System;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/testusermilestones")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestUserMilestonesController : ApiControllerOverride
    {
        private readonly IUserMilestoneServices _userMilestoneServices;

        public TestUserMilestonesController(UserMilestoneServices milestoneServices)
        {
            _userMilestoneServices = milestoneServices;
        }

        // GET: api/UserMilestones
        [GET("getbyuser")]
        [Route("getbyuser")]
        public async Task<HttpResponseMessage> GetByUser()
        {
            //try
            //{
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var users = await _userMilestoneServices.GetByUserAsync(base.UserID);
                watch.Stop();
                var response = Request.CreateResponse(HttpStatusCode.OK, users);
                var elapsedMs = watch.ElapsedMilliseconds;
                response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
                return response;
            //}
            //catch (Exception ex)
            //{
            //    new LogsController().PostDebugLog(ex.ToString() + ", Token:" + base.Token).GetAwaiter();
            //    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.ToString());
            //}
        }

        // GET: api/UserMilestones
        [GET("getdetail")]
        [Route("getdetail")]
        public async Task<IDictionary<string, object>> GetDetailByUser()
        {
            return await _userMilestoneServices.GetDetailByUserAsync(base.UserID);
        }

        // GET: api/UserMilestones
        [GET("getextended")]
        [Route("getextended")]
        public async Task<HttpResponseMessage> GetExtendedByUser()
        {
            //try
            //{
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var users = await _userMilestoneServices.GetExtendedByUserAsync(base.UserID);
                watch.Stop();
                var response = Request.CreateResponse(HttpStatusCode.OK, users);
                var elapsedMs = watch.ElapsedMilliseconds;
                response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
                return response;
            //}
            //catch (Exception ex)
            //{
            //    new LogsController().PostDebugLog(ex.ToString() + ", Token:" + base.Token).GetAwaiter();
            //    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.ToString());
            //}
        }

        // GET: api/UserMilestones
        [GET("getextended")]
        [Route("getextended")]
        public async Task<HttpResponseMessage> GetExtendedByUser(string languageCode)
        {
            //try
            //{
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var users = await _userMilestoneServices.GetExtendedByUserAsync(base.UserID, languageCode);
                watch.Stop();
                var response = Request.CreateResponse(HttpStatusCode.OK, users);
                var elapsedMs = watch.ElapsedMilliseconds;
                response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
                return response;
            //}
            //catch (Exception ex)
            //{
            //    new LogsController().PostDebugLog(ex.ToString() + ", Token:" + base.Token).GetAwaiter();
            //    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.ToString());
            //}
        }

        // POST: api/UserTrackers
        [POST("postusermilestone")]
        [Route("postusermilestone")]
        [ResponseType(typeof(UserMilestone))]
        public async Task<HttpResponseMessage> PostUserMilestone(UserMilestone userMilestone)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            //try
            //{
                bool success = false;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                ValidationErrors validationErrors = new ValidationErrors();
                if (_userMilestoneServices.Insert(ref validationErrors, base.UserID, userMilestone))
                {
                    watch.Stop();
                    var response = Request.CreateResponse(HttpStatusCode.OK, HttpConstants.SUCCESS);
                    var elapsedMs = watch.ElapsedMilliseconds;
                    response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
                    return response;
                }
                else
                {
                    watch.Stop();
                    var response = Request.CreateResponse(HttpStatusCode.NotFound, validationErrors.Show());
                    var elapsedMs = watch.ElapsedMilliseconds;
                    response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
                    return response;
                }
            //}
            //catch (Exception ex)
            //{
            //    new LogsController().PostDebugLog(ex.ToString() + ", Token:" + base.Token).GetAwaiter();
            //    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.ToString());
            //}
        }
    }
}