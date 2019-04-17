using Organo.Solutions.X4Ever.V1.DAL.Helper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.API.Security.Helpers
{
    public readonly struct FilterLog : IFilterLog
    {
        private readonly bool _isLogAllow;
        private IHelper Helper => new DAL.Helper.Helper();

        public bool IsLogAllow => _isLogAllow;
        public FilterLog(bool isLogAllow)
        {
            _isLogAllow = isLogAllow;
        }

        public async void Save(LogType logType, string[] values, string email)
        {
            if (!IsLogAllow) return;
            
            var filterEmails = Helper.GetAppSetting(CommonConstants.AuthenticationFilterEmails);
            var emails = filterEmails?.Split(';');
            if (string.IsNullOrEmpty(email) || filterEmails.Trim().Length == 0 || (emails.Count() > 0 ? (!string.IsNullOrEmpty(email) ? emails.Any(e => e == email) : false) : true))
            {
                // Today's file name
                var fileName = $"{DateTime.Now:yyyy-MM-dd}-filter.log";

                // File's full path
                var path = HttpContext.Current.Request.MapPath("~/" + Helper.GetAppSetting(CommonConstants.FilterLogPath) + "/" + fileName);
                try
                {
                    // This text is added only once to the file.
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            await WriteText(sw, logType, values);
                        }
                    }
                    else
                    {
                        // This text is always added, making the file longer over time
                        // if it is not deleted.
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            await WriteText(sw, logType, values);
                        }
                    }
                }
                catch (Exception)
                {
                    //return BadRequest(exception.Message);
                }
            }
        }

        private async Task WriteText(StreamWriter sw, LogType logType, string[] values)
        {
            try
            {
                var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                await sw.WriteLineAsync(dateString + " | CONT-" + logType.ToString().ToUpper() + "-FILTER | LOG");
                foreach (var value in values)
                {
                    await sw.WriteLineAsync(value);
                }
                await sw.WriteLineAsync("Class: " + logType.ToString());
                await sw.WriteLineAsync("Date: " + dateString);
                await sw.WriteLineAsync(Environment.NewLine);
            }
            catch
            {
                //
            }
        }
    }
}