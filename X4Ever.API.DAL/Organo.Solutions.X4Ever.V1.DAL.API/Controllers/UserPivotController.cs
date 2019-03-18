using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Organo.Solutions.X4Ever.V1.API.Security.Filters;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/userpivot")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserPivotController : ApiControllerOverride
    {
        private readonly IUserPivotServices _userPivotServices;
        private readonly IUserMetaPivotServices _userMetaPivotServices;
        private readonly IUserTrackerPivotServices _userTrackerPivotServices;
        private readonly IPasswordHistoryServices _passwordHistoryServices;
        private readonly IUserTokensServices _userTokensServices;
        private readonly IUserPushTokenServices _userPushTokenServices;
        private readonly IUserNotificationServices _notificationServices;
        private readonly Helper.IHelper _helper;
        private ValidationErrors _validationErrors = new ValidationErrors();
        private readonly IEmailContent _emailContent;

        public UserPivotController(UserPivotServices userPivotServices, PasswordHistoryServices passwordHistoryServices,
            UserMetaPivotServices userMetaPivotServices, UserTokensServices userTokensServices,
            UserTrackerPivotServices userTrackerPivotServices, EmailContent emailContent,
            UserPushTokenServices userPushTokenServices, UserNotificationServices notificationServices)
        {
            _userPivotServices = userPivotServices;
            _passwordHistoryServices = passwordHistoryServices;
            _userMetaPivotServices = userMetaPivotServices;
            _userTokensServices = userTokensServices;
            _userTrackerPivotServices = userTrackerPivotServices;
            _userPushTokenServices = userPushTokenServices;
            _helper = new Helper.Helper();
            _emailContent = emailContent;
            _notificationServices = notificationServices;
        }

        [POST("authuser_v2")]
        [Route("authuser_v2")]
        public async Task<HttpResponseMessage> PostAuthUser_V2()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var token = await _userTokensServices.GetDetailByTokenAsync(base.Token);
            if (token == null)
                return UnAuthorized();
            var user = await _userPivotServices.GetUserAsync(base.UserID);
            if (user == null)
                return UnAuthorized();
            watch.Stop();
            var response = Request.CreateResponse(HttpStatusCode.OK, user);
            response.Headers.Add(HttpConstants.TOKEN, Token);
            response.Headers.Add(HttpConstants.TOKEN_EXPIRY, token.ExpiresOn.ToString());
            response.Headers.Add(HttpConstants.ACCESS_CONTROL_EXPOSE_HEADERS, HttpConstants.TOKEN_COMMA_TOKEN_EXPIRY);
            var elapsedMs = watch.ElapsedMilliseconds;
            response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());

            return response;
        }

        [GET("getuser")]
        [Route("getuser")]
        public async Task<IHttpActionResult> GetByTokenAsync()
        {
            return Ok(await _userPivotServices.GetUserAsync(base.UserID));
        }

        [GET("getfulluser")]
        [Route("getfulluser")]
        public async Task<IHttpActionResult> GetFullUserByToken()
        {
            var user = await _userPivotServices.GetFullUserAsync(base.UserID);
            return Ok(user);
        }

        [POST("userstep1")]
        [Route("userstep1")]
        public IHttpActionResult Post_Step1(UserStep1 entity)
        {
            if (entity == null)
                return Ok("MessageInvalidObject");

            _validationErrors = new ValidationErrors();
            var user = new User()
            {
                UserFirstName = entity.UserFirstName,
                UserLastName = entity.UserLastName,
                UserType = entity.UserType,
                UserEmail = entity.UserEmail
            };
            if (_userPivotServices.Update(ref _validationErrors, base.UserID, user))
            {
                return Ok(HttpConstants.SUCCESS);
            }

            return Ok(_validationErrors.Count() > 0 ? _validationErrors.Show() : "Failed");
        }

        [POST("changepassword")]
        [Route("changepassword")]
        public async Task<IHttpActionResult> PostChangePassword(PasswordChange detail)
        {
            if (detail == null
                || detail.UserID == 0
                || detail.UserID.ToString().Trim().Length == 0
                || detail.CurrentPassword == null
                || detail.CurrentPassword.Trim().Length == 0
                || detail.Password == null
                || detail.Password.Trim().Length == 0)
                return Ok("MessageParameterRequired");
            else if (await _passwordHistoryServices.IsMatchAsync(detail.UserID, detail.Password))
                return Ok("MessagePasswordExistsInHistory");
            _validationErrors = new ValidationErrors();
            if (_userPivotServices.ChangePassword(ref _validationErrors, base.UserID, detail.CurrentPassword.Trim(),
                detail.Password.Trim()))
                return Ok(HttpConstants.SUCCESS);
            return Ok("MessageInvalidProvidedInformation");
        }

        /// <summary>
        /// Returns auth token for the validated user.
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        private HttpResponseMessage UnAuthorized()
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest, HttpConstants.UNAUTHORIZED);
        }
    }
}