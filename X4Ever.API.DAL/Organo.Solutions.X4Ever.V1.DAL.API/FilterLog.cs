using Organo.Solutions.X4Ever.V1.DAL.Helper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API
{
    public interface IFilterLog
    {
        void Save(LogType logType,string[] values,string email);
    }

    public enum LogType
    {
        UserPivot,
        UserPivot_AuthUser,
        AuthenticatePivot,
        Filter
    }

    public readonly struct FilterLog : IFilterLog
    {
        private IHelper _helper => new Helper.Helper();
        
        public async void Save(LogType logType,string[] values,string email) 
        {
            if((bool) _helper.GetAppSetting(CommonConstants.AuthenticationFilterCollect, typeof(bool))) {
                var filterEmails = _helper.GetAppSetting( CommonConstants.AuthenticationFilterEmails);
                var emails = filterEmails?.Split(';');
                if(filterEmails.Trim().Length == 0 || (emails.Count() > 0 ? (!string.IsNullOrEmpty(email) ? emails.Any(e => e == email) : false) : true)) {
                    // Today's file name
                    var fileName = $"{DateTime.Now:yyyy-MM-dd}-filter.log";
            
                    // File's full path
                    var path = HttpContext.Current.Request.MapPath("~/" + _helper.GetAppSetting(CommonConstants.FilterLogPath) + "/" + fileName);
                    try
                    {
                        // This text is added only once to the file.
                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                               await WriteText(sw,logType,values);
                            }
                        }
                        else
                        {
                            // This text is always added, making the file longer over time
                            // if it is not deleted.
                            using (StreamWriter sw = File.AppendText(path))
                            {
                              await WriteText(sw,logType,values);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //return BadRequest(exception.Message);
                    }
                }
            }
        }

        private async Task WriteText(StreamWriter sw,LogType logType,string[] values)
        {
            try
            {
                var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                await sw.WriteLineAsync(dateString + " | CONT-"+logType.ToString().ToUpper()+"-FILTER | LOG");
                foreach(var value in values){
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