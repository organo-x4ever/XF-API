using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using AttributeRouting.Web.Http;
using Newtonsoft.Json;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Organo.Solutions.X4Ever.V1.DAL;
using System.Threading.Tasks;
using System.Web;
using Organo.Solutions.X4Ever.V1.DAL.Helper;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/logs")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LogsController : ApiController
    {
        private Helper.IHelper _helper;

        public LogsController()
        {
            _helper = new Helper.Helper();
        }

        [POST("post")]
        [Route("post")]
        public async Task<IHttpActionResult> Post(List<Log> logs)
        {
            // Today's file name
            var fileName = $"{DateTime.Now:yyyy-MM-dd}-exception.log";

            // File's full path
            var path = HttpContext.Current.Request.MapPath("~/" + LogFilePath + "/" + fileName);
            try
            {
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        await WriteText(sw, logs);
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        await WriteText(sw, logs);
                    }
                }
            }
            catch (Exception)
            {
                //return BadRequest(exception.Message);
            }

            return Ok("Success");
        }

        private async Task WriteText(StreamWriter sw, List<Log> logs)
        {
            try
            {
                foreach (var log in logs)
                {
                    var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    await sw.WriteLineAsync(dateString + " | EXCEPTION | LOG");
                    await sw.WriteLineAsync("Application: " + log.Application);
                    await sw.WriteLineAsync("Device: " + log.Device);
                    await sw.WriteLineAsync("Platform: " + log.Platform);
                    await sw.WriteLineAsync("Idiom: " + log.Idiom);
                    await sw.WriteLineAsync("Identity: " + log.Identity);
                    await sw.WriteLineAsync("IPAddress: " + log.IPAddress);
                    await sw.WriteLineAsync("RequestUri: " + log.RequestUri?.AbsoluteUri);
                    await sw.WriteLineAsync("Token: " + log.Token);
                    await sw.WriteLineAsync("Message: " + log.Title);
                    await sw.WriteLineAsync("Detail: " + log.Message);
                    await sw.WriteLineAsync("Date: " + log.Date);
                    await sw.WriteLineAsync(Environment.NewLine);
                }
            }
            catch
            {
                //
            }
        }

        [POST("postdebuglog")]
        [Route("postdebuglog")]
        public async Task<IHttpActionResult> PostDebugLog(string debugLogstring)
        {
            // Today's file name
            var fileName = $"{DateTime.Now:yyyy-MM-dd}-debug.log";

            // File's full path
            var path = HttpContext.Current.Request.MapPath("~/" + DebugLogFilePath + "/" + fileName);
            try
            {
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        await WriteText(sw, debugLogstring);
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        await WriteText(sw, debugLogstring);
                    }
                }
            }
            catch (Exception)
            {
                //return BadRequest(exception.Message);
            }

            return Ok("Success");
        }

        private async Task WriteText(StreamWriter sw, string logs)
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            await sw.WriteLineAsync(dateString + ":: " + logs);
        }

        public IHttpActionResult WriteEmailLog(List<string> emailLogStrings)
        {
            // Today's file name
            var fileName = $"{DateTime.Now:yyyy-MM-dd}-debug.log";

            // File's full path
            var path = HttpContext.Current.Request.MapPath("~/" + EmailLogFilePath + "/" + fileName);
            try
            {
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        WriteEmailLogText(sw, emailLogStrings);
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        WriteEmailLogText(sw, emailLogStrings);
                    }
                }
            }
            catch (Exception)
            {
                //return BadRequest(exception.Message);
            }

            return Ok("Success");
        }

        void WriteEmailLogText(StreamWriter sw, List<string> logs)
        {
            try
            {
                var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                sw.WriteLine(dateString + " | EMAIL | LOG");
                foreach (var log in logs)
                {
                    sw.WriteLine(log);
                }
                sw.WriteLine(Environment.NewLine);
            }
            catch
            {
                //
            }
        }

        [POST("feedback")]
        [Route("feedback")]
        public async Task<IHttpActionResult> PostFeedback(UserFeedback userFeedback)
        {
            // Today's file name
            var fileName = $"{DateTime.Now:yyyy-MM-dd}-feedback.log";

            // File's full path
            var path = HttpContext.Current.Request.MapPath("~/" + FeedbackFilePath + "/" + fileName);
            try
            {
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        await WriteText(sw, userFeedback);
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        await WriteText(sw, userFeedback);
                    }
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok("Success");
        }

        private async Task WriteText(StreamWriter sw, UserFeedback userFeedback)
        {
            var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            userFeedback.Date = dateString;
            await sw.WriteLineAsync(JsonConvert.SerializeObject(userFeedback));
        }

        string LogFilePath => _helper.GetAppSetting("errorLogs");
        string DebugLogFilePath => _helper.GetAppSetting("debugLogs");
        string EmailLogFilePath => _helper.GetAppSetting("emailError");
        string FeedbackFilePath => _helper.GetAppSetting(CommonConstants.FEEDBACK_DIR_PATH);
    }
}