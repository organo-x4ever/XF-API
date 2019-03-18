using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Organo.Solutions.X4Ever.V1.DAL.Helper;

namespace Organo.Solutions.X4Ever.V1.Scheduler
{
    class Program
    {
        private static readonly IHelper _helper = new Helper();
        private static string RequestUrl = _helper.GetAppSetting(SettingKeys.RequestUri);
        private static readonly string LogFilePath = _helper.GetAppSetting(SettingKeys.LogDirectory);

        static void Main(string[] args)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Scheduler application started...");
            Schedule();
        }

        static void Schedule()
        {
            try
            {
                Console.WriteLine("Preparing to send request...");
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(RequestUrl);
                request.ContentType = "application/json";
                request.Method = WebRequestMethods.Http.Post;
                Console.WriteLine("Sending request...");
                HttpWebResponse httpResponse = (HttpWebResponse) request.GetResponse();
                Console.WriteLine("Getting response...");
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Response Status: " + httpResponse.StatusCode.ToString());
                    Console.WriteLine("Description: " + httpResponse.StatusDescription);
                    Console.WriteLine("Finishing...");
                    SaveLog(httpResponse.StatusCode.ToString(), httpResponse.StatusDescription,
                        httpResponse.GetResponseHeader("ExecutionTime"));
                }
                else
                {
                    string message = string.Format(
                        "An error has occurred when try to get server response: {httpResponse.StatusCode} ({httpResponse.StatusDescription})");
                    SaveLog(httpResponse.StatusCode.ToString(), httpResponse.StatusDescription);
                    Console.WriteLine(message);
                }

                Console.WriteLine("Saving logs...");
            }
            catch (Exception ex)
            {
                SaveLog("", ex.ToString());
                Console.WriteLine(ex.ToString(), "Scheduler Error");
            }

            Console.WriteLine("Application will exit in 10 seconds");
            Thread.Sleep(10000);
        }

        private static async void SaveLog(string statusCode = "", string statusDesc = "", string executionTime = "")
        {
            // Today's file name
            var fileName = $"{DateTime.Now:yyyy-MM-dd}-task.log";

            // File's full path
            var path = LogFilePath + "\\" + fileName;
            try
            {
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        await WriteText(sw, statusCode, statusDesc, executionTime);
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        await WriteText(sw, statusCode, statusDesc, executionTime);
                    }
                }
            }
            catch (Exception)
            {
                //
            }
        }

        private static async Task WriteText(StreamWriter sw, string statusCode = "", string statusDesc = "",
            string executionTime = "")
        {
            var dateString = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            await sw.WriteLineAsync(dateString + " | LOG");
            await sw.WriteLineAsync("Application: " + typeof(Program).FullName);
            await sw.WriteLineAsync("RequestUri: " + new Uri(RequestUrl)?.AbsoluteUri);
            await sw.WriteLineAsync("Status Code: " + statusCode);
            await sw.WriteLineAsync("Status Description: " + statusDesc);
            await sw.WriteLineAsync("Execution Time: " + executionTime);
            await sw.WriteLineAsync("Date: " + DateTime.UtcNow);
            await sw.WriteLineAsync(Environment.NewLine);
        }
    }

    static class SettingKeys
    {
        public static string RequestUri = "requestUri";
        public static string LogDirectory = "logDirectory";
    }
}