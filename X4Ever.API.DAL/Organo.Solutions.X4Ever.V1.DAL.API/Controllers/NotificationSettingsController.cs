
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/notificationsettings")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationSettingsController : ApiControllerOverride
    {
        private readonly IUserNotificationSettingServices _userNotificationSettingServices;

        public NotificationSettingsController(UserNotificationSettingServices userNotificationSettingServices)
        {
            _userNotificationSettingServices = userNotificationSettingServices;
        }

        [GET("getbytokenasync")]
        [Route("getbytokenasync")]
        public async Task<HttpResponseMessage> GetAsync()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = await _userNotificationSettingServices.GetAsync(UserID);
            watch.Stop();
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        [GET("getbytoken")]
        [Route("getbytoken")]
        public HttpResponseMessage Get()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = _userNotificationSettingServices.Get(UserID);
            watch.Stop();
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        // POST: api/UserTrackers
        [POST("postsettings")]
        [Route("postsettings")]
        [ResponseType(typeof(UserNotificationSetting))]
        public IHttpActionResult PostSettings(UserNotificationSetting userNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationErrors validationErrors = new ValidationErrors();
            userNotification.UserID = UserID;
            if (_userNotificationSettingServices.Update(ref validationErrors, userNotification))
            {
                return Ok(HttpConstants.SUCCESS);
            }
            else
                return Ok(validationErrors.Show());
        }
    }
}