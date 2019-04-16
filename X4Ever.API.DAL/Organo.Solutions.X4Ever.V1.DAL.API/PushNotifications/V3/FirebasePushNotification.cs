using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Organo.Solutions.X4Ever.V1.DAL.API.Controllers;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.PushNotifications.V3
{ 
    public class FirebasePushNotification : IDisposable
    {
        private readonly string SERVER_KEY;
        private readonly string SERVER_URL = "https://fcm.googleapis.com/fcm/send";
        public string ICON { get; set; }

        public FirebasePushNotification(string serverKey)
        {
            SERVER_KEY = serverKey;
        }

        public async Task<string> PushNotifyAsync(string deviceToken, string title, string message)
        {
            try
            {
                string result = "";
                HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(SERVER_URL);
                httpWebRequest.ContentType = HttpConstants.MEDIA_TYPE_APPLICATION_JSON;
                httpWebRequest.Headers.Add(HttpRequestHeader.Authorization,
                    string.Format(HttpConstants.KEY_WITH_PARAMETER, SERVER_KEY));
                httpWebRequest.Method = WebRequestMethods.Http.Post;
                Stream stream = await httpWebRequest.GetRequestStreamAsync();
                StreamWriter streamWriter = new StreamWriter(stream);
                stream = (Stream) null;
                try
                {
                    var obj = new
                    {
                        to = deviceToken,
                        data = new
                        {
                            title = title,
                            text = message,
                            icon = ICON
                        }
                    };
                    string json = JsonConvert.SerializeObject((object) obj);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    obj = null;
                    json = (string) null;
                }
                finally
                {
                    streamWriter?.Dispose();
                }

                streamWriter = (StreamWriter) null;
                HttpWebResponse httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
                try
                {
                    result = streamReader.ReadToEnd();
                }
                finally
                {
                    streamReader?.Dispose();
                }

                streamReader = (StreamReader) null;
                if (httpResponse.StatusCode != HttpStatusCode.Accepted &&
                    httpResponse.StatusCode != HttpStatusCode.OK &&
                    httpResponse.StatusCode != HttpStatusCode.Created)
                    result = string.Format(
                        "An error has occurred when try to get server response: {httpResponse.StatusCode} ({httpResponse.StatusDescription})");
                return result;
            }
            catch (Exception ex)
            {
                return ex + (ex.InnerException != null ? ", INNER:" + ex.InnerException.Message : "");
            }

            return "failed";
        }

        public async Task<string> PushNotifyAsync(string[] deviceTokens, string title, string message)
        {
            try
            {
                string result = "";
                HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(SERVER_URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(HttpRequestHeader.Authorization,
                    string.Format("key={0}", (object) SERVER_KEY));
                httpWebRequest.Method = "POST";
                Stream stream = await httpWebRequest.GetRequestStreamAsync();
                StreamWriter streamWriter = new StreamWriter(stream);
                stream = (Stream) null;
                try
                {
                    var obj = new
                    {
                        to = deviceTokens,
                        data = new
                        {
                            title = title,
                            text = message,
                            icon = ICON
                        }
                    };
                    string json = JsonConvert.SerializeObject((object) obj);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    obj = null;
                    json = (string) null;
                }
                finally
                {
                    streamWriter?.Dispose();
                }

                streamWriter = (StreamWriter) null;
                HttpWebResponse httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
                try
                {
                    result = streamReader.ReadToEnd();
                }
                finally
                {
                    streamReader?.Dispose();
                }

                streamReader = (StreamReader) null;
                if (httpResponse.StatusCode != HttpStatusCode.Accepted &&
                    httpResponse.StatusCode != HttpStatusCode.OK &&
                    httpResponse.StatusCode != HttpStatusCode.Created)
                    result = string.Format(
                        "An error has occurred when try to get server response: {httpResponse.StatusCode} ({httpResponse.StatusDescription})");
                return result;
            }
            catch (Exception ex)
            {
                return ex + (ex.InnerException != null ? ", INNER:" + ex.InnerException.Message : "");
            }

            return "failed";
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}