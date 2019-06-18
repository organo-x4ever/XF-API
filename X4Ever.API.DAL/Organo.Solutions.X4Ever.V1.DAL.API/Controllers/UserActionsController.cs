using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model.EnumerationTypes;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/actions")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserActionsController : ApiController
    {
        private readonly IUserServices _userServices;
        private readonly IUserSettingServices _userSettingServices;
        private ValidationErrors validationErrors = new ValidationErrors();
        private IHelper _helper;
        private ICountryServices _countryServices;
        private readonly IYoutubeVideoServices _youtubeVideoServices = new YoutubeVideoServices();
        private readonly IUserPushTokenServices _userPushTokenServices;
        private readonly IUserNotificationSettingServices _userNotificationSettingServices;
        private readonly IOpenNotificationUserServices _openNotificationUserServices;

        public UserActionsController(UserServices userServices, CountryServices countryServices,
            UserSettingServices userSettingServices,UserPushTokenServices userPushTokenServices,
            UserNotificationSettingServices userNotificationSettingServices,
            OpenNotificationUserServices openNotificationUserServices)
        {
            _userPushTokenServices = userPushTokenServices;
            _userServices = userServices;
            _countryServices = countryServices;
            _userSettingServices = userSettingServices;
            _userNotificationSettingServices=userNotificationSettingServices;
            _openNotificationUserServices = openNotificationUserServices;
            _helper = new Helper.Helper();
        }

        [GET("getbase")]
        [Route("getbase")]
        public async Task<IHttpActionResult> GetBaseString(string username, string password)
        {
            return await Task.Factory.StartNew(() => {
                return Ok(Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));
            });
        }

        [POST("fileupload")]
        [Route("fileupload")]
        public async Task<HttpResponseMessage> PostFileUpload()
        {
            return await Task.Factory.StartNew(() => {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                try
                {
                    if(!int.TryParse(_helper.GetAppSetting("photoUploadSizeKB"),out int size))
                        size=1024;
                    var httpRequest = HttpContext.Current.Request;
                    foreach (string file in httpRequest.Files)
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            IList<string> AllowedFileExtensions = new List<string> {".jpg", ".gif", ".png"};
                            var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                var message = string.Format("Please Upload File of type .jpg,.gif,.png.");
                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }
                            //else if (postedFile.ContentLength > size)
                            //{
                            //    var message = string.Format("Please Upload a file upto " + size + " KB.");
                            //    dict.Add("error", message);
                            //    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            //}
                            else
                            {
                                var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName);
                                postedFile.SaveAs(filePath);
                            }
                        }

                        var message1 = string.Format("MessageFileUploadedSuccessfully");
                        return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                        ;
                    }

                    var res = string.Format("Please Upload a image.");
                    dict.Add("error", res);
                    return Request.CreateResponse(HttpStatusCode.NotFound, dict);
                }
                catch (Exception ex)
                {
                    var msg = ex;
                    var res = string.Format("some Message");
                    dict.Add("error", res);
                    return Request.CreateResponse(HttpStatusCode.NotFound, dict);
                }
            });
        }

        // GET: api/news
        [POST("postnotification")]
        [Route("postnotification")]
        public async Task<IHttpActionResult> PostNotification(UserPushToken userPushToken)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request");
            return await Task.Factory.StartNew(() => {
                var validationErrors = new ValidationErrors();
                try
                {
                    userPushToken.DevicePlatform = GetPlatform().ToString();
                    var response = _userPushTokenServices.InsertInitial(ref validationErrors, userPushToken);
                    if (response)
                        return Ok(HttpConstants.SUCCESS);
                }
                catch(Exception)
                {
                    //
                }

                return Ok(validationErrors.Show());
            });
        }

        [POST("register")]
        [Route("register")]
        public async Task<IHttpActionResult> PostUser(UserRegister entity)
        {
            try
            {
                if (entity == null)
                    return Ok("MessageInvalidObject");
                else if(string.IsNullOrEmpty(entity.UserApplication))
                    return Ok("MessageUserApplicationRequired");

                var user = new User()
                {
                    UserActivationKey = entity.UserActivationKey,
                    UserEmail = entity.UserEmail,
                    UserFirstName = entity.UserFirstName,
                    UserKey = entity.UserKey,
                    UserLastName = entity.UserLastName,
                    UserLogin = entity.UserEmail,
                    UserPassword = entity.UserPassword,
                    UserRegistered = entity.UserRegistered,
                    UserStatus = entity.UserStatus,
                    UserType = entity.UserType,
                    UserApplication = entity.UserApplication
                };

                validationErrors = new ValidationErrors();
                if (_userServices.Insert(ref validationErrors, user))
                {
                    var userDetail = await _userServices.GetByEmailAsync(user.UserEmail);
                    if (userDetail != null)
                    {
                        if(!string.IsNullOrEmpty(user.UserKey))
                            _userPushTokenServices.Update(ref validationErrors, user.UserKey, userDetail.ID, userDetail.UserApplication);

                        if (!string.IsNullOrEmpty(entity?.LanguageCode) && !string.IsNullOrEmpty(entity?.WeightVolumeType))
                        {
                            var date = DateTime.Now;
                            _userSettingServices.Insert(ref validationErrors, new UserSetting()
                            {
                                LanguageCode = entity.LanguageCode,
                                WeightVolumeType = entity.WeightVolumeType,
                                UserID = userDetail.ID,
                                ModifyDate = date,
                                LanguageDetail = entity.LanguageDetail
                            });
                        }

                        _userNotificationSettingServices.Insert(ref validationErrors, new UserNotificationSetting
                        {
                            UserID = userDetail.ID,
                            CreateDate = DateTime.Now,
                            Intimation = true,
                            IsGeneralMessage = true,
                            IsPromotional = true,
                            IsSpecialOffer = true,
                            IsVersionUpdate = true,
                            IsWeightSubmitReminder = true,
                            ModifyDate = DateTime.Now
                        });
                    }

                    string message = "";
                    EmailContent emailContent = new EmailContent();
                    var content = emailContent.GetEmailDetail("en", EmailType.NEW_ACCOUNT, new string[] { });
                    if (content != null)
                        new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject, content.Body, true);
                    return Ok(HttpConstants.SUCCESS);
                }
                else
                    return Ok(validationErrors.Show());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [POST("requestpassword")]
        [Route("requestpassword")]
        public IHttpActionResult PostForgotPasswordRequest(ForgotPassword request)
        {
            if (string.IsNullOrEmpty(request?.UserLogin ?? "") || string.IsNullOrEmpty(request?.UserEmail ?? ""))
                return Ok("MessageAllInputRequired");
            validationErrors = new ValidationErrors();
            string requestCode = "";
            var user = _userServices.ForgotPasswordRequest(ref validationErrors, request.UserLogin.Trim(), request.UserEmail.Trim(), out requestCode);
            if (user != null)
            {
                string message = "";
                EmailContent emailContent = new EmailContent();
                var content = emailContent.GetEmailDetail("en", EmailType.FORGOT_PASSWORD, new string[] { requestCode });
                if (content != null)
                    _ = new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject, content.Body, true);
                return Ok(HttpConstants.SUCCESS);
            }
            else
                return Ok(validationErrors.Show());
        }
        
        public void EmailSend_Complete(List<string> logs)
        {
            new LogsController().WriteEmailLog(logs);
        }

        [POST("updatepassword")]
        [Route("updatepassword")]
        public IHttpActionResult PostForgotPasswordUpdate([FromBody] Models.PasswordDetail detail)
        {
            if (detail == null || detail.RequestCode == null || detail.RequestCode.Trim().Length == 0 ||
                detail.Password == null || detail.Password.Trim().Length == 0)
                return Ok("MessageAllInputRequired");
            validationErrors = new ValidationErrors();
            if (_userServices.ChangeForgotPassword(ref validationErrors, detail.RequestCode.Trim(),
                detail.Password.Trim()))
                return Ok(HttpConstants.SUCCESS);
            return Ok(validationErrors.Show());
        }

        [GET("updateuserstatus")]
        [Route("updateuserstatus")]
        public async Task<IHttpActionResult> GetUserStatus(string activation_code)
        {
            return await Task.Factory.StartNew(() => { 
                string message = "Wrong param or no param is sent";
                if (activation_code.Trim().Length == 0)
                    return Ok(message);
                else if (string.IsNullOrEmpty(activation_code) || string.IsNullOrWhiteSpace(activation_code))
                    return Ok("Wrong param or no param is sent");
                validationErrors = new ValidationErrors();
                if (_userServices.UpdateUserStatus(ref validationErrors, activation_code))
                    return Ok("E-mail verified successfully");
                return Ok(message);
            });
        }

        [GET("checkconnection")]
        [Route("checkconnection")]
        public async Task<HttpResponseMessage> GetConnection()
        {
            //var users = await _openNotificationUserServices.GetWeeklyAsync();

            var userPivot = new UserPivot();
            return Request.CreateResponse(HttpStatusCode.OK, "Connected");
        }

        [GET("AA332BE3FFE848E7A387B15DB6956462")]
        [Route("AA332BE3FFE848E7A387B15DB6956462")]
        public async Task<IHttpActionResult> GetDataToCheck()
        {
            return Ok(await _countryServices.GetAsync());
        }
    
        
        private PlatformType GetPlatform()
        {
            var request = Request;
            if (request != null && request.Headers != null)
            {
                var headers = request.Headers;
                if (headers.Contains(HttpConstants.PLATFORM))
                {
                    try
                    {
                        var type = headers.GetValues(HttpConstants.PLATFORM).First();
                        return (PlatformType) Enum.Parse(typeof(PlatformType), type, true);
                    }
                    catch (Exception)
                    {
                        //
                    }

                    return PlatformType.Wrong;
                }
            }

            return PlatformType.None;
        }
        }
}