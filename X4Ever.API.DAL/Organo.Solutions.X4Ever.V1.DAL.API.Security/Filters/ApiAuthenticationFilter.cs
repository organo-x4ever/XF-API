namespace Organo.Solutions.X4Ever.V1.API.Security.Filters
{
    using Organo.Solutions.X4Ever.V1.API.Security.Helpers;
    using Organo.Solutions.X4Ever.V1.DAL.Helper;
    using Organo.Solutions.X4Ever.V1.DAL.Services;
    using System;
    using System.Text;
    using System.Threading;
    using System.Web.Http.Controllers;

    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {
        private IHelper Helper => new DAL.Helper.Helper();
        private IFilterLog FilterLog  => new FilterLog((bool)Helper.GetAppSetting(CommonConstants.AuthenticationFilterCollect, typeof(bool)));

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
            FilterLog.Save(LogType.Filter, new string[] { "FUNCTION: OnAuthorizeUser()", $"Username: {username}", $"Password: {Convert.ToBase64String(Encoding.UTF8.GetBytes(password))}" }, username);
            // The code is changed to below (it is working fine)
            var provider = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(UserPivotServices)) as IUserPivotServices;
            FilterLog.Save(LogType.Filter, new string[] { "AFTER: var provider", $"Username: {username}", $"Password: {Convert.ToBase64String(Encoding.UTF8.GetBytes(password))}" }, username);
            if (provider != null)
            {
                FilterLog.Save(LogType.Filter, new string[] { "INITIAL: if (provider != null)", $"Username: {username}", $"Password: {Convert.ToBase64String(Encoding.UTF8.GetBytes(password))}" }, username);
                long userId = 0;
                if (password != null && password.Trim().Length > 0)
                {
                    FilterLog.Save(LogType.Filter, new string[] { "BEFORE: provider.Authenticate()", $"Username: {username}", $"Password: {Convert.ToBase64String(Encoding.UTF8.GetBytes(password))}" }, username);
                    userId = provider.Authenticate(username, password);
                    FilterLog.Save(LogType.Filter, new string[] { $"Username: {username}", $"Password: {Convert.ToBase64String(Encoding.UTF8.GetBytes(password))}", $"UserID: {userId.ToString()}" }, username);
                }
                FilterLog.Save(LogType.Filter, new string[] { $"Username: {username}", "BEFORE: if (userId > 0)", $"UserID: {userId.ToString()}" }, username);
                if (userId > 0)
                {
                    FilterLog.Save(LogType.Filter, new string[] { $"Username: {username}", "INSIDE: if (userId > 0)", $"UserID: {userId.ToString()}" }, username);
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    FilterLog.Save(LogType.Filter, new string[] { $"Username: {username}", "AFTER: var basicAuthenticationIdentity", $"UserID: {userId.ToString()}" }, username);
                    if (basicAuthenticationIdentity != null)
                    {
                        FilterLog.Save(LogType.Filter, new string[] { $"Username: {username}", "INSIDE: if (basicAuthenticationIdentity != null)", $"UserID: {userId.ToString()}" }, username);
                        basicAuthenticationIdentity.UserId = userId;
                    }
                    FilterLog.Save(LogType.Filter, new string[] { $"Username: {username}", "BEFORE: return true;", $"UserID: {userId.ToString()}" }, username);
                    return true;
                }
                FilterLog.Save(LogType.Filter, new string[] { $"Username: {username}", "INSIDE: if(provider != null)", $"UserID: {userId.ToString()}" }, username);
            }
            FilterLog.Save(LogType.Filter, new string[] { $"Username: {username}", "BEFORE: return false;" }, username);
            return false;
        }
    }
}