
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/pushnotifications")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserPushTokensController : ApiControllerOverride
    {
        private readonly IUserPushTokenServices _userPushTokenServices;

        public UserPushTokensController(UserPushTokenServices userPushTokenServices)
        {
            _userPushTokenServices = userPushTokenServices;
        }

        // GET: api/pushnotifications
        [GET("get")]
        [Route("get")]
        public async Task<IHttpActionResult> GetAsync()
        {
            var pushToken = await _userPushTokenServices.GetAsync(base.UserID);
            if (pushToken == null)
                return NotFound();
            return Ok(pushToken);
        }

        // GET: api/pushnotifications
        [POST("post")]
        [Route("post")]
        public async Task<IHttpActionResult> Post(UserPushToken userPushToken)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request");
            return await Task.Factory.StartNew(() =>
            {
                var validationErrors = new ValidationErrors();
                if (string.IsNullOrEmpty(userPushToken.DeviceApplication) && !string.IsNullOrEmpty(ApplicationKey))
                    userPushToken.DeviceApplication = ApplicationKey;
                if (string.IsNullOrEmpty(userPushToken.DevicePlatform) && !string.IsNullOrEmpty(Platform.ToString()))
                    userPushToken.DevicePlatform = Platform.ToString();
                var response = _userPushTokenServices.Insert(ref validationErrors, base.UserID, userPushToken);
                if (response)
                    return Ok(HttpConstants.SUCCESS);
                return Ok(validationErrors.Show());
            });
        }

        // GET: api/pushnotifications
        [POST("posttokenasync")]
        [Route("posttokenasync")]
        public async Task<IHttpActionResult> PostTokenAsync(UserPushTokenRegister userPushToken)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request");
            return await Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrEmpty(userPushToken.DeviceApplication) && !string.IsNullOrEmpty(ApplicationKey))
                    userPushToken.DeviceApplication = ApplicationKey;
                if (string.IsNullOrEmpty(userPushToken.DevicePlatform) && !string.IsNullOrEmpty(Platform.ToString()))
                    userPushToken.DevicePlatform = Platform.ToString();
                var validationErrors = new ValidationErrors();
                var response = _userPushTokenServices.Insert(ref validationErrors, base.UserID, userPushToken);
                if (response)
                    return Ok(HttpConstants.SUCCESS);
                return Ok(validationErrors.Show());
            });
        }
    }
}