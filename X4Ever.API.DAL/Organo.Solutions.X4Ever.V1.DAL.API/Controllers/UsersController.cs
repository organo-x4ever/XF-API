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
using Newtonsoft.Json;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/user")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiControllerOverride
    {
        private readonly IUserServices _userServices;
        private readonly IUserMetaServices _userMetaServices;
        private readonly IUserTrackerServices _userTrackerServices;
        private readonly IPasswordHistoryServices _passwordHistoryServices;
        private readonly IUserTokensServices _userTokensServices;
        private readonly IUserPushTokenServices _userPushTokenServices;
        private readonly IUserNotificationServices _notificationServices;
        private readonly Helper.IHelper _helper;
        private ValidationErrors _validationErrors = new ValidationErrors();
        private readonly IEmailContent _emailContent;

        public UsersController(UserServices userServices, PasswordHistoryServices passwordHistoryServices,
            UserMetaServices userMetaServices, UserTokensServices userTokensServices,
            UserTrackerServices userTrackerServices, EmailContent emailContent,
            UserPushTokenServices userPushTokenServices, UserNotificationServices notificationServices)
        {
            _userServices = userServices;
            _passwordHistoryServices = passwordHistoryServices;
            _userMetaServices = userMetaServices;
            _userTokensServices = userTokensServices;
            _userTrackerServices = userTrackerServices;
            _userPushTokenServices = userPushTokenServices;
            _helper = new Helper.Helper();
            _emailContent = emailContent;
            _notificationServices = notificationServices;
        }

        //[GET("get")]
        //[Route("get")]
        //public async Task<HttpResponseMessage> Get()
        //{
        //    var watch = Stopwatch.StartNew();
        //    //var users = _userServices.Get().OrderBy(u => u.UserFirstName).ThenBy(u => u.UserLastName)
        //    //    .ThenBy(u => u.UserRegistered);
        //    var users = new UserGlobal();
        //    watch.Stop();
        //    var response = Request.CreateResponse(HttpStatusCode.OK, users);
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
        //    return response;
        //}

        [POST("authuser")]
        [Route("authuser")]
        public async Task<HttpResponseMessage> PostAuthUser()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var token = await _userTokensServices.GetDetailByTokenAsync(base.Token);
            if (token == null)
                return UnAuthorized();
            var user = await _userServices.GetAsync(token.UserID, MetaConstants.PROFILE_PHOTO);
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

        [POST("authuser_v2")]
        [Route("authuser_v2")]
        public async Task<HttpResponseMessage> PostAuthUser_V2()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var token = await _userTokensServices.GetDetailByTokenAsync(base.Token);
            if (token == null)
                return UnAuthorized();
            var user = await _userServices.GetUserAuthenticationAsync(token.UserID);
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

        [GET("getusernew")]
        [Route("getusernew")]
        public async Task<HttpResponseMessage> GetByTokenAsync()
        {
            var user = await _userServices.GetAsync(base.UserID, MetaConstants.PROFILE_PHOTO);
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [GET("getuser")]
        [Route("getuser")]
        public async Task<IHttpActionResult> GetByToken()
        {
            var user = await _userServices.GetAsync(base.UserID);
            var meta = await _userMetaServices.GetByMetaKeyAsync(user.ID, MetaConstants.PROFILE_PHOTO);
            var userGlobal = new UserGlobal()
            {
                ID = user.ID,
                UserEmail = user.UserEmail,
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                UserRegistered = user.UserRegistered,
                UserStatus = user.UserStatus,
                UserType = user.UserType,
                UserLogin = user.UserLogin,
                ProfileImage = meta.Trim().Length > 0 ? meta : "user_no_photo.png"
            };
            return Ok(userGlobal);
        }

        [GET("getfulluser")]
        [Route("getfulluser")]
        public async Task<IHttpActionResult> GetFullUserByToken()
        {
            var user = await _userServices.GetFullUserAsync(base.UserID);
            return Ok(user);
        }

        [POST("update")]
        [Route("update")]
        public IHttpActionResult PostUser(UserRegister entity)
        {
            if (entity == null)
                return Ok("MessageInvalidObject");

            var user = new User()
            {
                UserActivationKey = entity.UserActivationKey,
                UserEmail = entity.UserEmail,
                UserFirstName = entity.UserFirstName,
                UserKey = entity.UserKey,
                UserLastName = entity.UserLastName,
                UserLogin = entity.UserLogin,
                UserPassword = entity.UserPassword,
                UserRegistered = entity.UserRegistered,
                UserStatus = entity.UserStatus,
                UserType = entity.UserType
            };
            _validationErrors = new ValidationErrors();
            if (_userServices.Update(ref _validationErrors, base.UserID, user))
                return Ok(HttpConstants.SUCCESS);
            else
                return Ok(_validationErrors.Show());
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
            if (_userServices.Update(ref _validationErrors, base.UserID, user))
            {
                return Ok(HttpConstants.SUCCESS);
            }

            return Ok(_validationErrors.Count() > 0 ? _validationErrors.Show() : "Failed");
        }

        [POST("updatefirst")]
        [Route("updatefirst")]
        public async Task<IHttpActionResult> PostUser(UserFirstUpdate entity)
        {
            if (entity == null)
                return Ok("MessageInvalidObject");

            var date = DateTime.Now;
            _validationErrors = new ValidationErrors();
            var metaList = new List<UserMeta>();
            foreach (var meta in entity.UserMetas)
            {
                if (meta.MetaKey == "weightlossgoal")
                {
                    int.TryParse(meta.MetaValue, out int val);
                    if (val < 4)
                    {
                        return Ok("Weight Lose Goal value is invalid");
                    }
                }

                var userMeta = new UserMeta()
                {
                    ID = meta.ID,
                    MetaDescription = meta.MetaDescription,
                    ModifyDate = meta.ModifyDate,
                    UserID = meta.UserID,
                    MetaKey = meta.MetaKey,
                    MetaLabel = meta.MetaLabel,
                    MetaType = meta.MetaType,
                    MetaValue = meta.MetaValue
                };
                metaList.Add(userMeta);
            }

            var trackerList = new List<UserTracker>();
            foreach (var tracker in entity.UserTrackers)
            {
                if (tracker.AttributeName == "weightlossgoal")
                {
                    int.TryParse(tracker.AttributeValue, out int val);
                    if (val < 4)
                    {
                        return Ok("Current Weight value is invalid");
                    }
                }

                var userTracker = new UserTracker()
                {
                    AttributeLabel = tracker.AttributeLabel,
                    AttributeName = tracker.AttributeName,
                    AttributeValue = tracker.AttributeValue,
                    ID = tracker.ID,
                    MediaLink = tracker.MediaLink,
                    ModifyDate = tracker.ModifyDate,
                    UserID = tracker.UserID
                };
                trackerList.Add(userTracker);
            }

            if (_userMetaServices.Insert(ref _validationErrors, base.UserID, metaList))
            {
                if (_userTrackerServices.Insert(ref _validationErrors, base.UserID, trackerList))
                {
                    var user = new User()
                    {
                        UserFirstName = entity.UserFirstName,
                        UserLastName = entity.UserLastName,
                        UserType = entity.UserType,
                        UserEmail = entity.UserEmail
                    };
                    if (_userServices.Update(ref _validationErrors, base.UserID, user))
                    {
                        string message = "";
                        var userDetail = await _userServices.GetAsync(base.UserID);
                        var content = _emailContent.GetEmailDetail(
                            userDetail.LanguageCode != null ? userDetail.LanguageCode : "en",
                            EmailType.WEIGHT_GOAL_SETUP,
                            new string[] { });
                        if (content != null)
                            new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject,
                                content.Body, true);
                        return Ok(HttpConstants.SUCCESS);
                    }
                }
            }

            return Ok(_validationErrors.Show());
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
            if (_userServices.ChangePassword(ref _validationErrors, UserID, detail.CurrentPassword.Trim(),
                detail.Password.Trim()))
                return Ok(HttpConstants.SUCCESS);
            return Ok("MessageInvalidProvidedInformation");
        }

        [POST("fileupload")]
        [Route("fileupload")]
        public IHttpActionResult PostFileUpload([FromBody] string attachment)
        {
            if (attachment.Trim().Length == 0)
                return Ok("MessageParameterRequired");
            else
                return Ok(attachment);
        }

        [GET("signature")]
        [Route("signature")]
        public HttpResponseMessage GetSignature()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Gurpreet Deol");
        }

        [POST("deleteaccount")]
        [Route("deleteaccount")]
        public IHttpActionResult PostAccountDelete(Models.AccountDelete account)
        {
            if (account.UserID == 0
                || account.UserID.ToString().Trim().Length == 0
                || account.Password == null
                || account.Password.Trim().Length == 0)
                return Ok("MessageInvalidUserIDORPassword");
            _validationErrors = new ValidationErrors();
            if (_userServices.Delete(ref _validationErrors, UserID, account.Password.Trim()))
                return Ok();
            return Ok("MessageAccountNotDeleted");
        }

        /// <summary>
        /// Delete users current token.
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        [POST("PostAuthTokenKill")]
        [Route("PostAuthTokenKill")]
        public HttpResponseMessage PostAuthToken()
        {
            var token = _userTokensServices.Update(base.Token);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add(HttpConstants.TOKEN, "");
            response.Headers.Add(HttpConstants.TOKEN_EXPIRY, "0");
            response.Headers.Add(HttpConstants.ACCESS_CONTROL_EXPOSE_HEADERS, HttpConstants.TOKEN_COMMA_TOKEN_EXPIRY);
            return response;
        }
    }
}