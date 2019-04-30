using System;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/testtrackers")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestUserTrackersController : ApiControllerOverride
    {
        private readonly IUserTrackerServices _userTrackerServices;
        private IEmailContent _emailContent;
        private IUserServices _userServices;

        public TestUserTrackersController(UserTrackerServices userTrackerServices, UserServices userServices,
            EmailContent emailContent)
        {
            //try
            //{
                _userTrackerServices = userTrackerServices;
                _userServices = userServices;
                _emailContent = emailContent;
            //}
            //catch (Exception ex)
            //{
            //    new LogsController().PostDebugLog(ex.ToString() + ", Token:" + base.Token).GetAwaiter();
            //}
        }

        // GET: api/UserTrackers
        [GET("getbyuser")]
        [Route("getbyuser")]
        public async Task<HttpResponseMessage> GetByUser()
        {
            //try
            //{
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var users = await _userTrackerServices.GetAsync(base.UserID);
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

        // GET: api/UserTrackers
        [GET("getlostbyuser")]
        [Route("getlostbyuser")]
        public async Task<HttpResponseMessage> GetLostByUser()
        {
            //try
            //{
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var users = await _userTrackerServices.GetAsync(base.UserID);
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

        // GET: api/UserTrackers
        [GET("getlatestbyuser")]
        [Route("getlatestbyuser")]
        public async Task<HttpResponseMessage> GetLatestByUser()
        {
            //try
            //{
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var users = await _userTrackerServices.GetLatestAsync(base.UserID);
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

        // GET: api/UserTrackers
        [GET("getfirstandlast")]
        [Route("getfirstandlast")]
        public async Task<HttpResponseMessage> GetFirstAndLastByUser()
        {
            //try
            //{
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var users = await _userTrackerServices.GetFirstAndLastAsync(base.UserID);
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

        // GET: api/UserTrackers/5
        [GET("getbykey")]
        [Route("getbykey")]
        [ResponseType(typeof(UserTracker))]
        public async Task<HttpResponseMessage> GetUserMeta(string attributeKey)
        {
            //try
            //{
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var users = await _userTrackerServices.GetByAttributeAsync(base.UserID, attributeKey);
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
        [POST("posttracker")]
        [Route("posttracker")]
        [ResponseType(typeof(UserTracker))]
        public async Task<HttpResponseMessage> PostUserTracker(UserTracker userTracker)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

            return await Task.Factory.StartNew(() => {
                ValidationErrors validationErrors = new ValidationErrors();
                if (_userTrackerServices.Insert(ref validationErrors, base.UserID, userTracker))
                    return Request.CreateResponse(HttpStatusCode.OK, HttpConstants.SUCCESS);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, validationErrors.Show());
            });
        }

        // POST: api/UserTrackers
        [POST("posttrackers")]
        [Route("posttrackers")]
        [ResponseType(typeof(UserTracker))]
        public async Task<HttpResponseMessage> PostUserTrackers(List<UserTracker> userTrackers)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            //try
            //{
                ValidationErrors validationErrors = new ValidationErrors();
                if (_userTrackerServices.Insert(ref validationErrors, base.UserID, userTrackers))
                {
                    var trackers = (await _userTrackerServices.GetByAttributeAsync(base.UserID, "currentweight"))
                        .OrderBy(t => t.ModifyDate).ToList();
                    short.TryParse(trackers.FirstOrDefault()?.AttributeValue, out short firstWeight);
                    short.TryParse(trackers.LastOrDefault()?.AttributeValue, out short lastWeight);
                    bool isEmail = false;
                    EmailType emailType = EmailType.ACHIEVED_GOAL;
                    DateTime.TryParse(String.Format("{0:MM-dd-yyyy}", trackers.LastOrDefault()?.ModifyDate),
                        out DateTime modifyDate);

                    if (lastWeight >= firstWeight &&
                        ModifyDate(GetTarget(trackers.ToList(), firstWeight, firstWeight), lastWeight))
                    {
                        isEmail = true;
                        emailType = EmailType.ACHIEVED_GOAL;
                    }
                    else if ((firstWeight - lastWeight) >= 100 &&
                             ModifyDate(GetTarget(trackers.ToList(), firstWeight, 100), lastWeight))
                    {
                        isEmail = true;
                        emailType = EmailType.LOSING_100_LBS;
                    }
                    else if ((firstWeight - lastWeight) >= 50 &&
                             ModifyDate(GetTarget(trackers.ToList(), firstWeight, 50), lastWeight))
                    {
                        isEmail = true;
                        emailType = EmailType.LOSING_50_LBS;
                    }
                    else if ((firstWeight - lastWeight) >= 25 &&
                             ModifyDate(GetTarget(trackers.ToList(), firstWeight, 25), lastWeight))
                    {
                        isEmail = true;
                        emailType = EmailType.LOSING_25_LBS;
                    }
                    else if ((firstWeight - lastWeight) >= 10 &&
                             ModifyDate(GetTarget(trackers.ToList(), firstWeight, 10), lastWeight))
                    {
                        isEmail = true;
                        emailType = EmailType.LOSING_10_LBS;
                    }

                    if (isEmail)
                    {
                        string message = "";
                        var user = await _userServices.GetAsync(base.UserID);
                        var content = _emailContent.GetEmailDetail(user.LanguageCode, emailType,
                            new string[] { });
                        if (content != null)
                            new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject,
                                content.Body, true);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, HttpConstants.SUCCESS);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, validationErrors.Show());
            //}
            //catch (Exception ex)
            //{
            //    new LogsController().PostDebugLog(ex.ToString() + ", Token:" + base.Token).GetAwaiter();
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            //}
        }

        bool ModifyDate(UserTracker userTracker, short lastWeight)
        {
            //try
            //{
                if (userTracker != null && short.TryParse(userTracker.AttributeValue, out short target) &&
                    target == lastWeight)
                {
                    return userTracker.ModifyDate.Date == DateTime.Today.Date;
                }
            //}
            //catch (Exception ex)
            //{
            //    new LogsController().PostDebugLog(ex.ToString() + ", Token:" + base.Token).GetAwaiter();
            //}

            return false;
        }

        UserTracker GetTarget(List<UserTracker> userTrackers, short firstWeight, short currentAchived)
        {
            //try
            //{
                foreach (var userTracker in userTrackers)
                {
                    if (short.TryParse(userTracker.AttributeValue, out short target))
                    {
                        if ((firstWeight - target) >= currentAchived)
                            return userTracker;
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    new LogsController().PostDebugLog(ex.ToString() + ", Token:" + base.Token).GetAwaiter();
            //}

            return new UserTracker();
        }

        // DELETE: api/UserTrackers/5
        [POST("posttracker1")]
        [Route("posttracker1")]
        [ResponseType(typeof(UserTracker))]
        public HttpResponseMessage DeleteUserTracker(List<UserTracker> userTrackers)
        {
            ValidationErrors validationErrors = new ValidationErrors();
            if (_userTrackerServices.Delete(ref validationErrors, base.UserID, userTrackers))
                return Request.CreateResponse(HttpStatusCode.OK, HttpConstants.SUCCESS);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, validationErrors.Show());
        }
    }
}