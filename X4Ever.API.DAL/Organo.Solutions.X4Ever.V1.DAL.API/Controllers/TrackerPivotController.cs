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
    [RoutePrefix("api/trackerpivot")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TrackerPivotController : ApiControllerOverride
    {
        private readonly IUserTrackerPivotServices _userTrackerPivotServices;
        private IEmailContent _emailContent;
        private IUserServices _userServices;

        public TrackerPivotController(UserTrackerPivotServices userTrackerPivotServices, UserServices userServices,
            EmailContent emailContent)
        {
            _userTrackerPivotServices = userTrackerPivotServices;
            _userServices = userServices;
            _emailContent = emailContent;
        }

        // GET: api/UserTrackersPivot
        [GET("trackerspivot")]
        [Route("trackerspivot")]
        public IHttpActionResult GetTrackers()
        {
            return Ok(_userTrackerPivotServices.GetTrackers(base.UserID));
        }

        // GET: api/UserTrackersPivot
        [GET("trackerspivotasync")]
        [Route("trackerspivotasync")]
        public async Task<IHttpActionResult> GetTrackersAsync()
        {
            return Ok(await _userTrackerPivotServices.GetTrackersAsync(base.UserID));
        }

        // GET: api/UserTrackersPivot
        [GET("latesttrackerpivot")]
        [Route("latesttrackerpivot")]
        public IHttpActionResult GetTracker()
        {
            return Ok(_userTrackerPivotServices.GetLatestTracker(base.UserID));
        }

        // GET: api/UserTrackersPivot
        [GET("latesttrackerpivotasync")]
        [Route("latesttrackerpivotasync")]
        public async Task<IHttpActionResult> GetTrackerAsync()
        {
            return Ok(await _userTrackerPivotServices.GetLatestTrackerAsync(base.UserID));
        }

        // POST: api/UserTrackers
        [POST("posttrackeredit")]
        [Route("posttrackeredit")]
        [ResponseType(typeof(UserTracker))]
        public async Task<IHttpActionResult> PostUserTrackerEdit(List<UserTracker> userTrackers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await Task.Factory.StartNew(() => {
                ValidationErrors validationErrors = new ValidationErrors();
                if (validationErrors.Count() == 0)
                {
                    return Ok(HttpConstants.SUCCESS);
                }
                else
                    return Ok(validationErrors.Show());
            });
        }

        // POST: api/UserTrackers
        [POST("posttrackers")]
        [Route("posttrackers")]
        [ResponseType(typeof(UserTracker))]
        public async Task<IHttpActionResult> PostUserTrackers(List<UserTracker> userTrackers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationErrors validationErrors = new ValidationErrors();
            if (_userTrackerPivotServices.Insert(ref validationErrors, base.UserID, userTrackers))
            {
                var trackers = (await _userTrackerPivotServices.GetByAttributeAsync(base.UserID, "currentweight"))
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

                return Ok(HttpConstants.SUCCESS);
            }
            else
                return Ok(validationErrors.Show());
        }

        private bool ModifyDate(UserTracker userTracker, short lastWeight)
        {
            if (userTracker != null && short.TryParse(userTracker.AttributeValue, out short target) &&
                target == lastWeight)
            {
                return userTracker.ModifyDate.Date == DateTime.Today.Date;
            }

            return false;
        }

        private UserTracker GetTarget(List<UserTracker> userTrackers, short firstWeight, short currentAchived)
        {
            foreach (var userTracker in userTrackers)
            {
                if (short.TryParse(userTracker.AttributeValue, out short target))
                {
                    if ((firstWeight - target) >= currentAchived)
                        return userTracker;
                }
            }

            return new UserTracker();
        }

        // POST: api/UserTrackers
        [POST("posttrackerdelete")]
        [Route("posttrackerdelete")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> PostUserTrackerDelete(string revisionNumber)
        {
            if (int.TryParse(revisionNumber, out int rn) && await _userTrackerPivotServices.Delete(this.UserID, rn))
            {
                return Ok(HttpConstants.SUCCESS);
            }
            else
                return Ok(HttpConstants.INVALID);
        }
    }
}