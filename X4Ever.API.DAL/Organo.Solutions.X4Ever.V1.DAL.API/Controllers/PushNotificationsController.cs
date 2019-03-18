using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.PushNotifications.V3;
using System.Text;
using System.Web;
using System.Web.Http.Cors;
//using System.Web.Mvc;
using AttributeRouting.Web.Http;
using Newtonsoft.Json;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/notifications")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    class PushNotificationsController : ApiController
    {
        private const string SERVER_KEY =
            "AAAAq-1wxFU:APA91bG0t0_f1_rrvhCtncq2sAnHbI7A24AXdm547QZ4G8WlamQD4O-mm3Qi4vzIPyypT83WbGYpZpuGuhtQ1j9dg2fiIV5FERT96Q2zoMaLPV6bImkT9oLlx3wd9fhBVr6N0TTZpsSW";

        [POST("sendpushmessageasync")]
        [Route("sendpushmessageasync")]
        public async Task<HttpResponseMessage> PostPushNotificationAsync()
        {
            FirebasePushNotification firebasePushNotification = new FirebasePushNotification(SERVER_KEY);
            var response = await firebasePushNotification.PushNotifyAsync(
                "ed0iY_G0gk8:APA91bF617VbnAR7wJk1UXh6k8TW3dgVGojl92UIFznZOSTJ9A61yPE1QaZeg3IHQZrfV1cRE3AJD9rEPzZ2B4smCoY0OJKa0n8DZqwUZjmqhS8BOMFbhgz5O4Kzk07KjV32aPhO8ybb",
                "SUBMIT YOUR WEIGHT", "It is time to submit your weight to keep tracking your progress.");
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [POST("sendpushmessageasync")]
        [Route("sendpushmessageasync")]
        public async Task<string> PostPushNotificationAsync(string deviceToken, string title, string text)
        {
            FirebasePushNotification firebasePushNotification = new FirebasePushNotification(SERVER_KEY);
            return await firebasePushNotification.PushNotifyAsync(deviceToken, title, text);
        }

        [POST("sendpushmessageasync")]
        [Route("sendpushmessageasync")]
        public async Task<string> PostPushNotificationAsync(string[] deviceToken, string title, string text)
        {
            FirebasePushNotification firebasePushNotification = new FirebasePushNotification(SERVER_KEY);
            return await firebasePushNotification.PushNotifyAsync(deviceToken, title, text);
        }
    }
}