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
    [RoutePrefix("api/pushnotifications")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserPushTokensController : ApiControllerOverride
    {
        private readonly IUserPushTokenServices _userPushTokenServices;

        public UserPushTokensController(UserPushTokenServices userPushTokenServices)
        {
            _userPushTokenServices = userPushTokenServices;
        }

        // GET: api/news
        [GET("get")]
        [Route("get")]
        public async Task<IHttpActionResult> GetAsync()
        {
            var pushToken = await _userPushTokenServices.GetAsync(base.UserID);
            if (pushToken == null)
                return NotFound();
            return Ok(pushToken);
        }

        // GET: api/news
        [POST("post")]
        [Route("post")]
        public async Task<IHttpActionResult> Post(UserPushToken userPushToken)
        {
            var validationErrors = new ValidationErrors();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid Request");
                }

                userPushToken.DeviceApplication = ApplicationKey;
                userPushToken.DevicePlatform = Platform.ToString();
                var response = _userPushTokenServices.Insert(ref validationErrors, base.UserID, userPushToken);
                if (response)
                    return Ok(HttpConstants.SUCCESS);
            }
            catch
            {
                //
            }

            return Ok(validationErrors.Show());
        }

        // GET: api/news
        [POST("posttokenasync")]
        [Route("posttokenasync")]
        public async Task<IHttpActionResult> PostTokenAsync(UserPushTokenRegister userPushToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            userPushToken.DeviceApplication = ApplicationKey;
            userPushToken.DevicePlatform = Platform.ToString();
            var validationErrors = new ValidationErrors();
            var response = _userPushTokenServices.Insert(ref validationErrors, base.UserID, userPushToken);
            if (response)
                return Ok(HttpConstants.SUCCESS);
            return Ok(validationErrors.Show());
        }
    }
}