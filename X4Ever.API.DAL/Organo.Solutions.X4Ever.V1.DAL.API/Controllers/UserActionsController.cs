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

        public UserActionsController(UserServices userServices, CountryServices countryServices,
            UserSettingServices userSettingServices)
        {
            _userServices = userServices;
            _countryServices = countryServices;
            _userSettingServices = userSettingServices;
            _helper = new Helper.Helper();
        }

        [GET("getbase")]
        [Route("getbase")]
        public async Task<IHttpActionResult> GetBaseString(string username, string password)
        {
            return Ok(Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));
        }

        //[POST("postencryption")]
        //[Route("postencryption")]
        //public IHttpActionResult PostEncryption(List<TempEncrypt> tempEncrypts)
        //{
        //    if (tempEncrypts != null)
        //    {
        //        var users = _userServices.GetLoginCredential("");
        //        foreach (var tempEncrypt in tempEncrypts)
        //        {
        //            //var response = _userServices.UpdateDirectPassword(tempEncrypt.Email, tempEncrypt.Password);
        //            tempEncrypt.EncryPassword = _helper.ConvertToSHA512(tempEncrypt.Password);
        //            tempEncrypt.SavedPassword =
        //                users.FirstOrDefault(u => u.Username.Trim().ToLower() == tempEncrypt.Email.Trim().ToLower())
        //                    ?.Password;
        //            tempEncrypt.MisMatch = tempEncrypt.EncryPassword == tempEncrypt.SavedPassword;
        //        }
        //    }

        //    return Ok(tempEncrypts.Where(e => e.MisMatch == false));
        //}

        //[POST("posttestemail")]
        //[Route("posttestemail")]
        //public IHttpActionResult PostTestEmail()
        //{
        //    string message = "";
        //    var response = false;
        //    EmailContent emailContent = new EmailContent();
        //    EmailDetail content = new EmailDetail();
        //    //content = emailContent.GetEmailDetail("en", EmailType.NEW_ACCOUNT, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject,
        //    //    content.Body, true);
        //    //message += SetupLog(response, EmailType.NEW_ACCOUNT) + " - " + message;
        //    //content = emailContent.GetEmailDetail("it", EmailType.WEIGHT_GOAL_SETUP, new string[] { });
        //    //if (content != null)
        //    //{
        //    //    response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //}

        //    //message += SetupLog(response, EmailType.WEIGHT_GOAL_SETUP);
        //    content = emailContent.GetEmailDetail("it", EmailType.AFTER_7_DAYS_ACCOUNT_CREATION, new string[] { });
        //    if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    message += SetupLog(response, EmailType.AFTER_7_DAYS_ACCOUNT_CREATION);
        //    //content = emailContent.GetEmailDetail("it", EmailType.LOSING_10_LBS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.LOSING_10_LBS);
        //    //content = emailContent.GetEmailDetail("en", EmailType.LOSING_25_LBS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.LOSING_25_LBS);
        //    //content = emailContent.GetEmailDetail("en", EmailType.LOSING_50_LBS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.LOSING_50_LBS);
        //    //content = emailContent.GetEmailDetail("en", EmailType.LOSING_100_LBS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.LOSING_100_LBS);
        //    ////content = emailContent.GetEmailDetail("en", EmailType.ACHIEVED_GOAL, new string[] { });
        //    ////if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    ////message += SetupLog(response, EmailType.ACHIEVED_GOAL);
        //    //content = emailContent.GetEmailDetail("en", EmailType.AFTER_EVERY_7_DAYS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.AFTER_EVERY_7_DAYS);


        //    //content = emailContent.GetEmailDetail("es", EmailType.WEIGHT_GOAL_SETUP, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject,
        //    //    content.Body, true);
        //    //message += SetupLog(response, EmailType.WEIGHT_GOAL_SETUP);
        //    ////content = emailContent.GetEmailDetail("es", EmailType.AFTER_7_DAYS_ACCOUNT_CREATION, new string[] { });
        //    ////if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    ////message += SetupLog(response, EmailType.AFTER_7_DAYS_ACCOUNT_CREATION);
        //    //content = emailContent.GetEmailDetail("es", EmailType.LOSING_10_LBS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.LOSING_10_LBS);
        //    //content = emailContent.GetEmailDetail("es", EmailType.LOSING_25_LBS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.LOSING_25_LBS);
        //    //content = emailContent.GetEmailDetail("es", EmailType.LOSING_50_LBS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.LOSING_50_LBS);
        //    //content = emailContent.GetEmailDetail("es", EmailType.LOSING_100_LBS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.LOSING_100_LBS);
        //    ////content = emailContent.GetEmailDetail("es", EmailType.ACHIEVED_GOAL, new string[] { });
        //    ////if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    ////message += SetupLog(response, EmailType.ACHIEVED_GOAL);
        //    //content = emailContent.GetEmailDetail("es", EmailType.AFTER_EVERY_7_DAYS, new string[] { });
        //    //if (content != null) response = new Message().SendMail(ref message, "gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    //message += SetupLog(response, EmailType.AFTER_EVERY_7_DAYS);

        //    ////var requestCode = _helper.GetUniqueCode();
        //    ////content = emailContent.GetEmailDetail("en", EmailType.FORGOT_PASSWORD, new string[] { requestCode });
        //    ////if (content != null) response = new Message().SendMail("gdeol.cf9e@organogold.com", "", "", content.Subject, content.Body, true);
        //    ////message += SetupLog(response, EmailType.FORGOT_PASSWORD);
        //    string SetupLog(bool success, EmailType emailType)
        //    {
        //        return emailType.ToString() + ": " + (success ? "Success" : message) + ", ";
        //    }

        //    return Ok(message);
        //}

        [POST("fileupload")]
        [Route("fileupload")]
        public async Task<HttpResponseMessage> PostFileUpload()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                int size = 1024;
                try
                {
                    size = (int) this._helper.GetAppSetting("photoUploadSizeKB", typeof(System.Int32));
                }
                catch
                {
                }

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
        }

        //[POST("register")]
        //[Route("register")]
        //public IHttpActionResult PostUser(UserRegisterNew entity)
        //{
        //    if (entity == null)
        //    {
        //        return BadRequest("MessageInvalidObject");
        //    }

        // Guid guid = Guid.NewGuid(); var user = new User() { UserActivationKey = guid.ToString(),
        // UserEmail = entity.Email, UserFirstName = entity.FirstName, UserLogin = entity.Username,
        // UserRegistered = DateTime.Now, UserStatus = entity.Status, UserPassword = entity.UserCode };

        // var list = new List<UserMeta>(); list.Add(new UserMeta() { MetaDescription = "User Weight
        // Loss Goal", MetaKey = "WeightLossGoal", MetaLabel = "UserMeta", MetaType =
        // "weightlossgoal", MetaValue = entity.WeightLossGoal, ModifyDate = DateTime.Now, });
        // list.Add(new UserMeta() { MetaDescription = "Gender", MetaKey = "Gender", MetaLabel =
        // "UserMeta", MetaType = "gender", MetaValue = entity.Gender, ModifyDate = DateTime.Now, });

        //    validationErrors = new ValidationErrors();
        //    if (_userServices.Insert(ref validationErrors, user, list))
        //        return Ok(HttpConstants.SUCCESS);
        //    else
        //        return BadRequest(validationErrors.Show());
        //}

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
                    if (entity.LanguageCode != null && entity.LanguageCode.Trim().Length > 0 &&
                        entity.WeightVolumeType != null && entity.WeightVolumeType.Trim().Length > 0)
                    {
                        var userDetail = await _userServices.GetAsync(user.UserEmail);
                        if (userDetail != null)
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
                    }

                    string message = "";
                    EmailContent emailContent = new EmailContent();
                    var content = emailContent.GetEmailDetail("en", EmailType.NEW_ACCOUNT, new string[] { });
                    if (content != null)
                        new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject, content.Body,
                            true);
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
        public async Task<IHttpActionResult> PostForgotPasswordRequest(Models.ForgotPassword request)
        {
            if (request == null || request.UserLogin == null || request.UserLogin.Trim().Length == 0 ||
                request.UserEmail == null || request.UserEmail.Trim().Length == 0)
                return Ok("MessageAllInputRequired");
            validationErrors = new ValidationErrors();
            string requestCode = "";
            var user = _userServices.ForgotPasswordRequest(ref validationErrors, request.UserLogin.Trim(),
                request.UserEmail.Trim(), out requestCode);
            if (user != null)
            {
                string message = "";
                EmailContent emailContent = new EmailContent();
                var content = emailContent.GetEmailDetail("en", EmailType.FORGOT_PASSWORD, new string[] {requestCode});
                if (content != null)
                    new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject, content.Body, true);
                return Ok(HttpConstants.SUCCESS);
            }
            else
                return Ok(validationErrors.Show());
        }

        [POST("updatepassword")]
        [Route("updatepassword")]
        public async Task<IHttpActionResult> PostForgotPasswordUpdate([FromBody] Models.PasswordDetail detail)
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
            string message = "Wrong param or no param is sent";
            if (activation_code.Trim().Length == 0)
                return Ok(message);
            else if (string.IsNullOrEmpty(activation_code) || string.IsNullOrWhiteSpace(activation_code))
                return Ok("Wrong param or no param is sent");
            validationErrors = new ValidationErrors();
            if (_userServices.UpdateUserStatus(ref validationErrors, activation_code))
                return Ok("E-mail verified successfully");
            return Ok(message);
        }

        [GET("checkconnection")]
        [Route("checkconnection")]
        public HttpResponseMessage GetConnection()
        {
            var userPivot = new UserPivot();
            return Request.CreateResponse(HttpStatusCode.OK, "Connected");
        }

        [GET("AA332BE3FFE848E7A387B15DB6956462")]
        [Route("AA332BE3FFE848E7A387B15DB6956462")]
        public async Task<IHttpActionResult> GetDataToCheck()
        {
            return Ok(await _countryServices.GetAsync());
        }
    }
}