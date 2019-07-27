
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using AttributeRouting.Web.Http;
using Organo.Solutions.PushNotification;
using Organo.Solutions.PushNotification.Apple;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System.Collections.Generic;
using System.IO;
using Organo.Solutions.X4Ever.V1.DAL.API.Extensions;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Newtonsoft.Json;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/frontend")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FrontendController : ApiControllerOverride
    {
        private readonly IUserMetaPivotServices _userMetaPivotServices;
        private readonly IUserTrackerPivotServices _userTrackerPivotServices;
        private readonly IOpenNotificationUserServices _openNotificationUserServices;
        private readonly IUserNotificationServices _notificationServices;
        private readonly IUserTrackerRealtimeServices _userTrackerRealtimeServices;
        private readonly IWebUserMetaRealtimeServices _webUserMetaRealtimeServices;
        private readonly IUserNotificationServices _userNotificationServices;
        private readonly IUserTrackerReportServices _trackerReportServices;
        private readonly IUserPivotServices _userPivotServices;
        private readonly ITrackerPhotoSkipServices _trackerPhotoSkipServices;
        private readonly IFeedbackServices _feedbackServices;
        private IEmailContent _emailContent;
        private INotification _notification;
        private readonly IHelper _helper;
        private readonly IUserNotificationSettingsViewServices _userNotificationSettingsViewServices;
        private readonly string baseUrl;

        public FrontendController(UserPivotServices userPivotServices, OpenNotificationUserServices openNotificationUserServices,
            UserNotificationServices notificationServices, UserTrackerRealtimeServices userTrackerRealtimeServices,
            UserMetaPivotServices userMetaPivotServices, UserTrackerPivotServices userTrackerPivotServices,
            WebUserMetaRealtimeServices webUserMetaRealtimeServices, UserNotificationSettingsViewServices userNotificationSettingsViewServices,
            UserNotificationServices userNotificationServices, UserTrackerReportServices trackerReportServices,
            TrackerPhotoSkipServices trackerPhotoSkipServices, FeedbackServices feedbackServices)
        {
            _userPivotServices = userPivotServices;
            _userMetaPivotServices = userMetaPivotServices;
            _userTrackerPivotServices = userTrackerPivotServices;
            _openNotificationUserServices = openNotificationUserServices;
            _emailContent = new EmailContent();
            _notificationServices = notificationServices;
            _userTrackerRealtimeServices = userTrackerRealtimeServices;
            _webUserMetaRealtimeServices = webUserMetaRealtimeServices;
            _userNotificationServices = userNotificationServices;
            _trackerReportServices = trackerReportServices;
            _userNotificationSettingsViewServices = userNotificationSettingsViewServices;
            _trackerPhotoSkipServices = trackerPhotoSkipServices;
            _feedbackServices = feedbackServices;
            _helper = new Helper.Helper();
            _notification = new AppleNotification(
                HttpContext.Current.Server.MapPath(_helper.GetAppSetting(NotificationConstant.CertificatePathDev)),
                _helper.GetAppSetting(NotificationConstant.CertificatePassDev), false);

#if DEBUG
            baseUrl = "http://localhost:55155/Uploads/";
#else
            baseUrl = "https://mapp.oghq.ca/Uploads/";
#endif
        }

        [GET("getusers")]
        [Route("getusers")]
        public async Task<HttpResponseMessage> GetUsers(bool showEmptyRecords = false)
        {
            var watch = Stopwatch.StartNew();
            var users = await _userPivotServices.GetAsync(showEmptyRecords);
            watch.Stop();
            var response = Request.CreateResponse(HttpStatusCode.OK, users);
            var elapsedMs = watch.ElapsedMilliseconds;
            response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
            return response;
        }

        //[GET("gettrackerdetailasync")]
        //[Route("gettrackerdetailasync")]
        //public async Task<HttpResponseMessage> GetTrackerDetailAsync()
        //{
        //    var watch = Stopwatch.StartNew();
        //    var userTrackers = await _trackerReportServices.GetTrackerDetailAsync();
        //    var response = Request.CreateResponse(HttpStatusCode.OK, userTrackers);
        //    watch.Stop();
        //    response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
        //    return response;
        //}

        [GET("gettrackerdetailasync")]
        [Route("gettrackerdetailasync")]
        public async Task<HttpResponseMessage> GetTrackerDetailLatestAsync(DateTime fromDate, DateTime toDate)
        {
            var watch = Stopwatch.StartNew();
            var userTrackers = await _trackerReportServices.GetTrackerDetailPeriodAsync(fromDate, toDate);
            var response = Request.CreateResponse(HttpStatusCode.OK, userTrackers);
            watch.Stop();
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        [GET("getrealtimetrackerasync")]
        [Route("getrealtimetrackerasync")]
        public async Task<HttpResponseMessage> GetRealtimeTrackerAsync(DateTime fromDate)
        {
            fromDate = fromDate.AddDays(-1);
            return Request.CreateResponse(HttpStatusCode.OK,
                await _userTrackerRealtimeServices.GetRealtimeWeightTrackingAsync(fromDate));
        }

        [GET("getrealtimetrackerrefreshasync")]
        [Route("getrealtimetrackerrefreshasync")]
        public async Task<HttpResponseMessage> GetRealtimeTrackerRefreshAsync(DateTime fromDate, long skipUptoId = 0)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                await _userTrackerRealtimeServices.GetRealtimeWeightTrackingAsync(fromDate, skipUptoId));
        }

        [GET("getrealtimetrackerrefresh2018async")]
        [Route("getrealtimetrackerrefresh2018async")]
        public async Task<HttpResponseMessage> GetRealtimeTrackerRefresh2018Async(DateTime fromDate,
            long skipUptoId = 0)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                await _userTrackerRealtimeServices.GetRealtimeWeightTrackingIn2018Async(fromDate, skipUptoId));
        }

        [GET("getwebusermetarealtimeasync")]
        [Route("getwebusermetarealtimeasync")]
        public async Task<HttpResponseMessage> GetWebUserMetaRealtimeAsync(DateTime fromDate)
        {
            fromDate = fromDate.AddDays(-1);
            var list = await _webUserMetaRealtimeServices.GetWebUserWeightTrackingAsync(fromDate);
            return Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [GET("getwebusermetarealtime2018async")]
        [Route("getwebusermetarealtime2018async")]
        public async Task<HttpResponseMessage> GetWebUserMetaRealtime2018Async(DateTime fromDate)
        {
            fromDate = fromDate.AddDays(-1);
            var list = await _webUserMetaRealtimeServices.GetWebUserWeightTracking2018Async(fromDate);
            return Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [GET("getwebusermetaasync")]
        [Route("getwebusermetaasync")]
        public async Task<HttpResponseMessage> GetWebUserMetaAsync()
        {
            var list = await _webUserMetaRealtimeServices.GetWebUserTrackerViewPivotAsync();
            return Request.CreateResponse(HttpStatusCode.OK, list);
        }

        [GET("getmetarowbyuserid")]
        [Route("getmetarowbyuserid")]
        public async Task<HttpResponseMessage> GetMetaRowByUser(long id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _userMetaPivotServices.GetMetaAsync(id));
        }

        [GET("gettrackerrowbyuserid")]
        [Route("gettrackerrowbyuserid")]
        public async Task<HttpResponseMessage> GetTrackerRowByUser(long id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _userTrackerPivotServices.GetTrackersAsync(id));
        }

        [GET("getbydateasync")]
        [Route("getbydateasync")]
        public async Task<HttpResponseMessage> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var watch = Stopwatch.StartNew();
            var notification =
                await _userNotificationServices.GetByDateAsync(fromDate, toDate, q => q.OrderByDescending(n => n.ID));
            var response = Request.CreateResponse(HttpStatusCode.OK, notification);
            watch.Stop();
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        [GET("getnotifysettings")]
        [Route("getnotifysettings")]
        public async Task<HttpResponseMessage> GetNotifySettingsAsync()
        {
            var watch = Stopwatch.StartNew();
            var result = await _userNotificationSettingsViewServices.GetAsync();
            watch.Stop();
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        [GET("getnotificationbodybykeyasync")]
        [Route("getnotificationbodybykeyasync")]
        public async Task<HttpResponseMessage> GetNotificationBodyByKeyAsync(long key)
        {
            var watch = Stopwatch.StartNew();
            var notification = await _userNotificationServices.GetBodyByIdAsync(key);
            var response = Request.CreateResponse(HttpStatusCode.OK, notification);
            watch.Stop();
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        [POST("postpushnotificationasync")]
        [Route("postpushnotificationasync")]
        public async Task<HttpResponseMessage> PostCustomPushNotificationAsync(string authCode, string emailIDs = "",
            string sendToPlatforms = "", string notificationTitle = "", string notificationBody = "",
            string emailSubject = "", string emailBody = "", bool attachFooter = false)
        {
            try
            {
                if (string.IsNullOrEmpty(authCode))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "PARAM_MISSIING, SYSTEM_CODE:0003");
                //else if (string.IsNullOrEmpty(userToken.ToString()))
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, "PARAM_MISSIING, SYSTEM_CODE:0004");
                else if (string.IsNullOrEmpty(emailIDs))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "PARAM_MISSIING, SYSTEM_CODE:0005");
                else if (string.IsNullOrEmpty(sendToPlatforms))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "PARAM_MISSIING, SYSTEM_CODE:0006");
                else
                {
                    var auth_code = _helper.GetAppSetting(HttpConstants.FRONTEND_AUTH_CODE);
                    if (authCode != auth_code)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            "PARAM_INVALID, SYSTEM_CODE:0101");
                    }

                    var auth_valid = _helper.GetAppSetting(HttpConstants.FRONTEND_AUTH_VALIDITY);
                    DateTime.TryParse(auth_valid, out DateTime auth_valid_date);
                    if (DateTime.Now > auth_valid_date)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            "PARAM_INVALID, SYSTEM_CODE:0102");
                    }
                }

                var platforms = sendToPlatforms?.ToLower().Split(',');
                var sendEmail = platforms?.ToList().Any(p => p == "email");
                if (sendEmail ?? false)
                {
                    if (string.IsNullOrEmpty(emailSubject) || string.IsNullOrEmpty(emailBody))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            "PARAM_MISSIING, SYSTEM_CODE:0007 and/or 0008");
                    }
                }

                var sendPush = platforms?.ToList().Any(p => p.ToLower() == "ios" || p.ToLower() == "android");
                if (sendPush ?? false)
                {
                    if (string.IsNullOrEmpty(notificationTitle) || string.IsNullOrEmpty(notificationBody))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            "PARAM_MISSIING, SYSTEM_CODE:0009 and/or 0010");
                    }
                }

                var emails = emailIDs.ToLower().Split(',');

                var watch = Stopwatch.StartNew();
                ValidationErrors validationErrors = new ValidationErrors();

                var users = await _openNotificationUserServices.GetUserOnlyAsync(emails, platforms);
                var environment = _helper.GetAppSetting(NotificationConstant.ApiEnvironment);
                if (environment != null && environment.ToLower().Contains("production") && (platforms?.ToList().Any(p => p.ToLower() == "ios") ?? false))
                {
                    _notification = new AppleNotification(
                        HttpContext.Current.Server.MapPath(
                            _helper.GetAppSetting(NotificationConstant.CertificatePathProd)),
                        _helper.GetAppSetting(NotificationConstant.CertificatePassProd),
                        environment.ToLower().Contains("production"));
                }
                foreach (var user in users)
                {
                    if (sendPush ?? false && user.DeviceToken != null)
                    {
                        var response = "";
                        if (user.DevicePlatform.ToLower().Contains("ios") && platforms.Contains("ios"))
                        {
                            await _notification.SendAsync(user.DeviceToken.Replace(" ", ""), notificationBody);
                            foreach (var msg in _notification.Messages)
                            {
                                response += msg;
                            }
                        }
                        else if (user.DevicePlatform.ToLower().Contains("android") && platforms.Contains("android"))
                        {
                            response = await new PushNotificationsController()
                                .PostPushNotificationAsync(user.DeviceToken, notificationTitle, notificationBody);
                        }

                        validationErrors = await AddValidation(validationErrors,
                            await _notificationServices.Insert(user.UserID, user.DeviceToken, DateTime.Now,
                                notificationTitle, notificationBody,
                                !string.IsNullOrEmpty(response) ? response : "No response returned", $"Frontend Custom Message [AUTH_CODE: {authCode}],[USER_TOKEN: {Token}]", true, false, user.DevicePlatform.ToLower()));
                    }

                    if (sendEmail ?? false)
                    {
                        string message = "";
                        try
                        {
                            var content = "";
                            if (attachFooter)
                            {
                                content = _emailContent.FooterContent(user.LanguageCode);
                            }
                            new Message().SendMail(ref message, user.UserEmail, "", "", emailSubject, emailBody + (attachFooter ? content ?? "" : ""), true);
                            emailBody = emailBody + (attachFooter ? content ?? "" : "");
                            var messageBody = string.IsNullOrEmpty(message) ? "E-mail sent successfully" : message;
                            var messageIdentity = $"Frontend Custom Message [AUTH_CODE: {authCode}],[USER_TOKEN: {Token}]";
                            validationErrors.Add(message);
                            var result = await _notificationServices.Insert(user.UserID, user.UserEmail, DateTime.Now, emailSubject, messageBody, messageBody, messageIdentity, false, false, "email");
                            validationErrors = await AddValidation(validationErrors, result);
                        }
                        catch (Exception exception)
                        {
                            validationErrors.Add(exception.ToString());
                        }
                    }
                }

                watch.Stop();
                var responseMain = Request.CreateResponse(HttpStatusCode.OK,
                    "Synchronization " + (validationErrors.Count() > 0 ? "finished with errors" : "successful"));
                responseMain.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
                return responseMain;
            }
            catch (Exception exception)
            {
                await new LogsController().PostDebugLog(exception.Message);
                if (exception.InnerException != null)
                    await new LogsController().PostDebugLog(exception.InnerException.Message);
                await new LogsController().PostDebugLog(exception.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, exception.ToString());
            }
        }

        private async Task<ValidationErrors> AddValidation(ValidationErrors existingErrors, ValidationErrors newErrors)
        {
            return await Task.Factory.StartNew(() =>
            {
                var validationErrors = new ValidationErrors();
                foreach (var error in existingErrors.Get())
                {
                    validationErrors.Add(error);
                }

                foreach (var error in newErrors.Get())
                {
                    validationErrors.Add(error);
                }

                return validationErrors;
            });
        }

        [GET("listOfSkippedPhotos")]
        [Route("listOfSkippedPhotos")]
        public async Task<HttpResponseMessage> GetSkippedPhotos()
        {
           return Request.CreateResponse(HttpStatusCode.OK, await _trackerPhotoSkipServices.GetJsonAsync(FileLocator.TRACKER_PHOTO_SKIP_JSON_PATH));
        }

        [GET("listOfFeedbacks")]
        [Route("listOfFeedbacks")]
        public async Task<HttpResponseMessage> GetFeedbacks()
        {
           return Request.CreateResponse(HttpStatusCode.OK, await _feedbackServices.GetJsonAsync(FileLocator.FEEDBACK_FILE_PATH));
        }

        [POST("deleteFeedback")]
        [Route("deleteFeedback")]
        public async Task<HttpResponseMessage> PostFeedbackRemoval(string id)
        {
            bool isUserAllowed = false;
            var userId = UserID;
            if (userId != 0)
            {
                var allowedUserSetting = _helper.GetAppSetting(CommonConstants.AllowedUsersKey);
                var allowedUsers = allowedUserSetting.Split(';');
                isUserAllowed = allowedUsers.Any(a => userId.ToString().Equals(a));
            }
            if (!isUserAllowed) return Request.CreateResponse(HttpStatusCode.Unauthorized, HttpConstants.UNAUTHORIZED);
            var response = await _feedbackServices.DeleteAsync(FileLocator.FEEDBACK_FILE_PATH, id);
            return response ? Request.CreateResponse(HttpStatusCode.OK, HttpConstants.SUCCESS) : Request.CreateResponse(HttpStatusCode.BadRequest, HttpConstants.INVALID);
        }

        [GET("all_photos_model_single")]
        [Route("all_photos_model_single")]
        public async Task<HttpResponseMessage> GetAllPhotoModelSingle(int page = 0, int size = 100)
        {
            page = page > 0 ? page - 1 : 0;
            var _filePath = HttpContext.Current.Request.MapPath("~/Uploads");
            var directories = Directory.GetFiles(_filePath);
            if(directories.Count() <= (page * size))
                page = (directories.Count() / size) - (directories.Count() % size == 0 ? 1 : 0);
            var list = directories.Skip(page * size).Take(size);                
            var response = Request.CreateResponse(HttpStatusCode.OK, GetModelAllFiles(list.ToArray()));
            response.Headers.Add("count", directories.Count().ToString());
            response.Headers.Add("previous", page > 0 ? "1" : "0");
            response.Headers.Add("next", directories.Count() <= ((page+1) * size) ? "0" : "1");
            return response;
        }

        private List<PhotoModel> GetModelAllFiles(string[] directories)
        {
            var photoModels = new List<PhotoModel>();
            int index = 1;
            var photoModel = new PhotoModel();
            foreach (var file in directories)
            {
                if (index > 10)
                {
                    photoModels.Add(photoModel);
                    photoModel = new PhotoModel();
                    index = 1;
                }
                switch (index)
                {
                    case 1:
                        photoModel.COL_1 = baseUrl + file.GetFileName();
                        break;
                    case 2:
                        photoModel.COL_2 = baseUrl + file.GetFileName();
                        break;
                    case 3:
                        photoModel.COL_3 = baseUrl + file.GetFileName();
                        break;
                    case 4:
                        photoModel.COL_4 = baseUrl + file.GetFileName();
                        break;
                    case 5:
                        photoModel.COL_5 = baseUrl + file.GetFileName();
                        break;
                    case 6:
                        photoModel.COL_6 = baseUrl + file.GetFileName();
                        break;
                    case 7:
                        photoModel.COL_7 = baseUrl + file.GetFileName();
                        break;
                    case 8:
                        photoModel.COL_8 = baseUrl + file.GetFileName();
                        break;
                    case 9:
                        photoModel.COL_9 = baseUrl + file.GetFileName();
                        break;
                    case 10:
                        photoModel.COL_10 = baseUrl + file.GetFileName();
                        break;
                }
                index++;
            }
            photoModels.Add(photoModel);
            return photoModels;
        }
    }
}