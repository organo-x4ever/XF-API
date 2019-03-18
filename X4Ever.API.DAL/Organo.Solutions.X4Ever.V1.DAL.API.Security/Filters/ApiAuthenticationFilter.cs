namespace Organo.Solutions.X4Ever.V1.API.Security.Filters
{
    using Organo.Solutions.X4Ever.V1.DAL.Services;
    using System.Threading;
    using System.Web.Http.Controllers;

    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {
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
                if (password != null && password.Trim().Length > 0)
                    userId = provider.Authenticate(username, password);

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
    }
}