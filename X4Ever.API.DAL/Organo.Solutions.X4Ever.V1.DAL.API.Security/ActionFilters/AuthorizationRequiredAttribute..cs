
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters
{
    using System.Threading;
    using Organo.Solutions.X4Ever.V1.API.Security.Filters;
    using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
    using Organo.Solutions.X4Ever.V1.DAL.Services;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            // Get API key provider
            var provider = filterContext.ControllerContext.Configuration.DependencyResolver
                .GetService(typeof(UserTokensServices)) as IUserTokensServices;

            if (provider != null && filterContext.Request.Headers.Contains(HttpConstants.TOKEN))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(HttpConstants.TOKEN).First();
                // Validate Token
                var userId = provider.Validate(tokenValue);
                if (userId > 0)
                {
                    ValueProviderResult result = new ValueProviderResult(userId, userId.ToString(), null);
                    filterContext.ModelState.Add(HttpConstants.USER_ID, new ModelState() {Value = result});
                }
                else
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {ReasonPhrase = HttpConstants.UNAUTHORIZED};
                    filterContext.Response = responseMessage;
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);
        }

        //public override void OnActionExecuting(HttpActionContext filterContext)
        //{
        //    // Get API key provider
        //    var provider = filterContext.ControllerContext.Configuration.DependencyResolver
        //        .GetService(typeof(UserTokensServices)) as IUserTokensServices;

        //    if (filterContext.Request.Headers.Contains(Token))
        //    {
        //        var tokenValue = filterContext.Request.Headers.GetValues(Token).First();
        //        // Validate Token
        //        if (provider != null && !provider.ValidateToken(tokenValue))
        //        {
        //            var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        //                {ReasonPhrase = HttpConstants.UNAUTHORIZED};
        //            filterContext.Response = responseMessage;
        //        }
        //    }
        //    else
        //    {
        //        filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        //    }

        //    base.OnActionExecuting(filterContext);
        //}
    }
}