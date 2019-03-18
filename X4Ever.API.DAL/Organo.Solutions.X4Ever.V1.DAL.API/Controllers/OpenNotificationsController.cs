using AttributeRouting.Web.Http;
using Organo.Solutions.PushNotification;
using Organo.Solutions.PushNotification.Apple;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/opennotifications")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OpenNotificationsController : ApiController
    {
        private readonly IUserMetaServices _userMetaServices;
        private readonly IUserTrackerServices _userTrackerServices;
        private readonly IOpenNotificationUserServices _openNotificationUserServices;
        private readonly IUserNotificationServices _notificationServices;
        private readonly IUserTrackerRealtimeServices _userTrackerRealtimeServices;
        private IEmailContent _emailContent;
        private INotification _notification;
        private readonly IHelper _helper;
        private IList<string> Messages { get; set; }

        public OpenNotificationsController(OpenNotificationUserServices openNotificationUserServices,
            UserNotificationServices notificationServices, UserTrackerRealtimeServices userTrackerRealtimeServices,
            UserMetaServices userMetaServices, UserTrackerServices userTrackerServices)
        {
            _userMetaServices = userMetaServices;
            _userTrackerServices = userTrackerServices;
            _openNotificationUserServices = openNotificationUserServices;
            _emailContent = new EmailContent();
            _notificationServices = notificationServices;
            _userTrackerRealtimeServices = userTrackerRealtimeServices;
            _helper = new Helper.Helper();
            _notification = new AppleNotification(
                HttpContext.Current.Server.MapPath(_helper.GetAppSetting(NotificationConstant.CertificatePathDev)),
                _helper.GetAppSetting(NotificationConstant.CertificatePassDev), false);
        }

        [GET("getusertrackerdueasync")]
        [Route("getusertrackerdueasync")]
        public async Task<HttpResponseMessage> GetUserTrackerDueAsync()
        {
            var watch = Stopwatch.StartNew();
            var users = await _openNotificationUserServices.GetWeeklyAsync();
            watch.Stop();
            var responseMain = Request.CreateResponse(HttpStatusCode.OK, users);
            responseMain.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return responseMain;
        }


        [GET("getuserregistrationdueasync")]
        [Route("getuserregistrationdueasync")]
        public async Task<HttpResponseMessage> GetUserRegistrationDueAsync()
        {
            var watch = Stopwatch.StartNew();
            ValidationErrors validationErrors = new ValidationErrors();
            var userList = await _openNotificationUserServices.GetRegisteredUserAfterWeekAsync();
            foreach (var user in userList)
            {
                if (user.IsStartToday)
                {
                    string message = "";
                    try
                    {
                        var content = _emailContent.GetEmailDetail(user.LanguageCode,
                            EmailType.AFTER_7_DAYS_ACCOUNT_CREATION,
                            new string[] { });
                        if (content != null)
                        {
                            if (new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject,
                                content.Body, true))
                            {
                                validationErrors = await AddValidation(validationErrors,
                                    await _notificationServices.Insert(
                                        user.UserID, user.UserEmail,
                                        DateTime.Now, content.Subject, content.Body, "E-mail sent successfully",
                                        EmailType.AFTER_7_DAYS_ACCOUNT_CREATION.ToString(), false, true, "email"));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //
                    }
                }
            }

            watch.Stop();
            var responseMain = Request.CreateResponse(HttpStatusCode.OK, userList);
            responseMain.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return responseMain;
        }

        //[POST("postusertrackersasync")]
        //[Route("postusertrackersasync")]
        //public async Task<HttpResponseMessage> PostUserTrackersAsync(string deviceToken, string message,
        //    string platform, string title = "No Title")
        //{
        //    try
        //    {
        //        var watch = Stopwatch.StartNew();
        //        var environment = _helper.GetAppSetting(NotificationConstant.ApiEnvironment);
        //        if (environment != null && environment.ToLower().Contains("production"))
        //        {
        //            _notification = new AppleNotification(
        //                HttpContext.Current.Server.MapPath(
        //                    _helper.GetAppSetting(NotificationConstant.CertificatePathProd)),
        //                _helper.GetAppSetting(NotificationConstant.CertificatePassProd),
        //                environment.ToLower().Contains("production"));
        //        }

        //        Messages = new List<string>();
        //        var result = await _notification.SendAsync(deviceToken.Replace(" ", ""), message);
        //        watch.Stop();
        //        if (!result)
        //        {
        //            var response = Request.CreateResponse(HttpStatusCode.BadRequest, _notification.Messages);
        //            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
        //            return response;
        //        }
        //        else
        //        {
        //            var response = Request.CreateResponse(HttpStatusCode.OK);
        //            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
        //            return response;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        await new LogsController().PostDebugLog(exception.Message);
        //        if (exception.InnerException != null)
        //            await new LogsController().PostDebugLog(exception.InnerException.Message);
        //        await new LogsController().PostDebugLog(exception.ToString());
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, exception.ToString());
        //    }
        //}

        [POST("postusertrackerdueasync")]
        [Route("postusertrackerdueasync")]
        public async Task<HttpResponseMessage> PostUserTrackerDueAsync()
        {
            try
            {
                var watch = Stopwatch.StartNew();
                ValidationErrors validationErrors = new ValidationErrors();
                var platforms = _helper.GetAppSetting(NotificationConstant.PushNotificationPlatform)?.Split(',');
                var sendEmail = platforms?.ToList().Any(p => p == "email");
                var users = await _openNotificationUserServices.GetWeeklyAsync();
                string notificationTitle = "SUBMIT YOUR WEIGHT",
                    notificationBody = "It's time to submit your weight to keep tracking your progress.";
                var environment = _helper.GetAppSetting(NotificationConstant.ApiEnvironment);
                if (environment != null && environment.ToLower().Contains("production"))
                {
                    _notification = new AppleNotification(
                        HttpContext.Current.Server.MapPath(
                            _helper.GetAppSetting(NotificationConstant.CertificatePathProd)),
                        _helper.GetAppSetting(NotificationConstant.CertificatePassProd),
                        environment.ToLower().Contains("production"));
                }

                foreach (var user in users)
                {
                    if (user.DeviceToken != null)
                    {
                        var response = "";
                        if (user.DevicePlatform.ToLower().Contains("ios"))
                        {
                            await _notification.SendAsync(user.DeviceToken.Replace(" ", ""), notificationBody);
                            foreach (var msg in _notification.Messages)
                            {
                                response += msg;
                            }
                        }

                        else if (user.DevicePlatform.ToLower().Contains("android"))
                        {
                            response = await new PushNotificationsController()
                                .PostPushNotificationAsync(user.DeviceToken, notificationTitle, notificationBody);
                        }

                        if (!string.IsNullOrEmpty(response))
                        {
                            validationErrors = await AddValidation(validationErrors, await _notificationServices.Insert(
                                user.UserID, user.DeviceToken,
                                DateTime.Now, notificationTitle, notificationBody, response,
                                EmailType.AFTER_EVERY_7_DAYS.ToString(), true, true, user.DevicePlatform.ToLower()));
                        }
                    }

                    if (user.IsStartToday && (sendEmail ?? false))
                    {
                        string message = "";
                        try
                        {
                            var content = _emailContent.GetEmailDetail(user.LanguageCode, EmailType.AFTER_EVERY_7_DAYS,
                                new string[] { });
                            if (content != null)
                            {
                                if (new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject,
                                    content.Body, true))
                                {
                                    validationErrors = await AddValidation(validationErrors,
                                        await _notificationServices.Insert(
                                            user.UserID, user.UserEmail,
                                            DateTime.Now, content.Subject, content.Body, "E-mail sent successfully",
                                            EmailType.AFTER_EVERY_7_DAYS.ToString(), false, true, "email"));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //
                        }
                    }
                }

                var userList = await _openNotificationUserServices.GetRegisteredUserAfterWeekAsync();
                foreach (var user in userList)
                {
                    if (user.IsStartToday && (sendEmail ?? false))
                    {
                        string message = "";
                        try
                        {
                            var content = _emailContent.GetEmailDetail(user.LanguageCode,
                                EmailType.AFTER_7_DAYS_ACCOUNT_CREATION,
                                new string[] { });
                            if (content != null)
                            {
                                if (new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject,
                                    content.Body, true))
                                {
                                    validationErrors = await AddValidation(validationErrors,
                                        await _notificationServices.Insert(
                                            user.UserID, user.UserEmail,
                                            DateTime.Now, content.Subject, content.Body, "E-mail sent successfully",
                                            EmailType.AFTER_7_DAYS_ACCOUNT_CREATION.ToString(), false, true, "email"));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //
                        }
                    }
                }

                watch.Stop();
                var responseMain = Request.CreateResponse(HttpStatusCode.OK, "Synchronization successful");
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
    }
}