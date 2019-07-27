
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/logs")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LogsController : ApiController
    {
        private readonly IFeedbackServices _feedbackServices;
        public LogsController()
        {
            _feedbackServices = new FeedbackServices();
        }

        [POST("post")]
        [Route("post")]
        public async Task<IHttpActionResult> Post(List<Log> logs)
        {
            // File's full path
            var path = FileLocator.ERROR_LOG_PATH;
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

            async Task WriteText(StreamWriter sw, List<Log> logList)
            {
                try
                {
                    foreach (var log in logList)
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
        }

        [POST("postdebuglog")]
        [Route("postdebuglog")]
        public async Task<IHttpActionResult> PostDebugLog(string debugLogstring)
        {
            // File's full path
            var path = FileLocator.DEBUG_LOG_PATH;
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

            async Task WriteText(StreamWriter sw, string logs)
            {
                var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                await sw.WriteLineAsync(dateString + ":: " + logs);
            }
        }

        public IHttpActionResult WriteEmailLog(List<string> emailLogStrings)
        {
            // File's full path
            var path = FileLocator.EMAIL_LOG_PATH;
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
        }

        [POST("feedback")]
        [Route("feedback")]
        public async Task<IHttpActionResult> PostFeedback(UserFeedback userFeedback)
        {
            // File's full path
            var _feedbackFilePath = FileLocator.FEEDBACK_FILE_PATH;
            try
            {
                var result = await _feedbackServices.SaveAsync(_feedbackFilePath, userFeedback);
                if (!result)
                {
                    return BadRequest(_feedbackServices.ValidationErrors.Show());
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok("Success");
        }
    }
}