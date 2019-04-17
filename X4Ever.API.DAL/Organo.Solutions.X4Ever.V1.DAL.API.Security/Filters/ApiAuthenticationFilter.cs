namespace Organo.Solutions.X4Ever.V1.API.Security.Filters
{
    using Organo.Solutions.X4Ever.V1.DAL.Helper;
    using Organo.Solutions.X4Ever.V1.DAL.Services;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Http.Controllers;

    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {
        IHelper _helper = new Helper();

        /// <summary>
        /// Default Authentication Constructor
        /// </summary>
        public ApiAuthenticationFilter()
        {
        }

        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive">
        /// </param>
        public ApiAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        /// <summary>
        /// Protected overriden method for authorizing user
        /// </summary>
        /// <param name="username">
        /// </param>
        /// <param name="password">
        /// </param>
        /// <param name="actionContext">
        /// </param>
        /// <returns>
        /// </returns>
        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            // The code is changed to below (it is working fine)
            var provider = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(UserPivotServices)) as IUserPivotServices;
            if (provider != null)
            {
                long userId = 0;
                if (password != null && password.Trim().Length > 0) {
                    userId = provider.Authenticate(username, password);

                    if((bool) _helper.GetAppSetting(CommonConstants.AuthenticationFilterCollect, typeof(bool))) {
                        
                        var filterEmails = _helper.GetAppSetting( CommonConstants.AuthenticationFilterEmails);
                        var emails = filterEmails?.Split(';');
                        if(filterEmails.Trim().Length == 0 || (emails.Count() > 0 ? (!string.IsNullOrEmpty(username) ? emails.Any(e => e == username) : false) : true)) {
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
                                        WriteText(sw,  username, password, userId);
                                    }
                                }
                                else
                                {
                                    // This text is always added, making the file longer over time
                                    // if it is not deleted.
                                    using (StreamWriter sw = File.AppendText(path))
                                    {
                                       WriteText(sw, username, password, userId);
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
                if (userId > 0)
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if (basicAuthenticationIdentity != null)
                        basicAuthenticationIdentity.UserId = userId;
                    return true;
                }
            }
            return false;
        }

        private void WriteText(StreamWriter sw, string username, string password, long userId)
        {
            try
            {
                var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                sw.WriteLine(dateString + " | API-AUTHENTICATION-FILTER | LOG");
                sw.WriteLine("Username: " + username);
                sw.WriteLine("Key: " + Convert.ToBase64String(Encoding.UTF8.GetBytes(password)));
                sw.WriteLine("ID: " + userId);
                sw.WriteLine("Class: " + typeof(ApiAuthenticationFilter).FullName);
                sw.WriteLine("Date: " + dateString);
                sw.WriteLine(Environment.NewLine);
            }
            catch
            {
                //
            }
        }
    }
}