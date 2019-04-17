
using System.Globalization;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    using System.Linq;
    using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
    using AttributeRouting.Web.Http;
    using Organo.Solutions.X4Ever.V1.API.Security.Filters;
    using Organo.Solutions.X4Ever.V1.DAL.Model;
    using Organo.Solutions.X4Ever.V1.DAL.Services;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Organo.Solutions.X4Ever.V1.DAL.Helper;
    using Organo.Solutions.X4Ever.V1.API.Security.Helpers;
    using Helper = Helper.Helper;

    [ApiAuthenticationFilter]
    //[EnableCors(origins: "http://localhost:5100,http://localhost:63535", headers: "*", methods: "*")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthenticatePivotController : ApiController
    {
         #region Private variable.

        private readonly IUserTokensServices _tokenServices;
        private readonly IUserPivotServices _userPivotServices;
        private readonly IHelper _helper;
        private readonly IFilterLog _filterLog;

        #endregion Private variable.

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public AuthenticatePivotController(UserTokensServices tokenServices, UserPivotServices userPivotServices)
        {
            _tokenServices = tokenServices;
            _userPivotServices = userPivotServices;
            _helper = new Helper();
            _filterLog = new FilterLog((bool)_helper.GetAppSetting(CommonConstants.AuthenticationFilterCollect, typeof(bool)));
        }

        #endregion Public Constructor

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns>
        /// </returns>
        [POST("login")]
        public HttpResponseMessage Login()
        {
            _filterLog.Save(LogType.AuthenticatePivot, new string[] { "UserID: 0", "Login Step: 1" },"0");

            if (System.Threading.Thread.CurrentPrincipal == null ||
                !System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
                return UnAuthorized(HttpConstants.UNAUTHORIZED);

            _filterLog.Save(LogType.AuthenticatePivot, new string[] {"UserID: 0","Login Step: 2" },"0");
            
            if (!(System.Threading.Thread.CurrentPrincipal.Identity is BasicAuthenticationIdentity
                basicAuthenticationIdentity))
                return UnAuthorized(HttpConstants.UNAUTHORIZED);
            var userId=basicAuthenticationIdentity?.UserId ?? 0;
            _filterLog.Save(LogType.AuthenticatePivot, new string[] {"UserID: " + userId.ToString(),"Login Step: 3" },userId.ToString());
            var token = _tokenServices.GenerateToken(basicAuthenticationIdentity?.UserId ?? 0);
            _filterLog.Save(LogType.AuthenticatePivot, new string[] {"UserID: " + userId.ToString(),"Login Step: 4" },userId.ToString());
            var response = Request.CreateResponse(HttpStatusCode.OK);
            _filterLog.Save(LogType.AuthenticatePivot, new string[] {"UserID: " + userId.ToString(),"Login Step: 5" },userId.ToString());
            response.Headers.Add(HttpConstants.TOKEN, token.AuthToken);
            _filterLog.Save(LogType.AuthenticatePivot, new string[] {"UserID: " + userId.ToString(),"Login Step: 6" },userId.ToString());
            response.Headers.Add(HttpConstants.TOKEN_EXPIRY,
                token.ExpiresOn.ToString(CultureInfo.CurrentCulture));
            _filterLog.Save(LogType.AuthenticatePivot, new string[] {"UserID: " + userId.ToString(),"Login Step: 7" },userId.ToString());
            response.Headers.Add(HttpConstants.ACCESS_CONTROL_EXPOSE_HEADERS,
                HttpConstants.TOKEN_COMMA_TOKEN_EXPIRY);
            _filterLog.Save(LogType.AuthenticatePivot, new string[] {"UserID: " + userId.ToString(),"Login Step: 8" },userId.ToString());
            _filterLog.Save(LogType.AuthenticatePivot, new string[] {"UserID: " + userId.ToString(),"Token: "+token.AuthToken,"TokenExpiry: "+token.ExpiresOn.ToString(CultureInfo.CurrentCulture) },userId.ToString());
            return response;
        }
        
        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns>
        /// </returns>
        [POST("authenticate")]
        [POST("get/token")]
        //[POST("login")]
        public HttpResponseMessage Authenticate()
        {
            if (System.Threading.Thread.CurrentPrincipal != null &&
                System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity =
                    System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    var userId = basicAuthenticationIdentity.UserId;
                    var token = _tokenServices.GenerateToken(userId);

                    #region To return User Account with Token

                    var user = _userPivotServices.GetUser(userId);
                    if (user == null)
                        return UnAuthorized("USER NOT FOUND");
                    string application = "";
                    var request = Request;
                    if (request != null && request.Headers != null)
                    {
                        var headers = request.Headers;
                        if (headers.Contains(HttpConstants.APPLICATION))
                        {
                            application = headers.GetValues(HttpConstants.APPLICATION).First();
                        }
                    }

                    if (string.IsNullOrEmpty(user.UserApplication))
                    {
                        if (!string.IsNullOrEmpty(application))
                        {
                            var validationErrors = new ValidationErrors();
                            _userPivotServices.Update(ref validationErrors, token.AuthToken, application);
                        }
                    }

                    watch.Stop();

                    var response = Request.CreateResponse(HttpStatusCode.OK, user);

                    #endregion To return User Account with Token

                    response.Headers.Add(HttpConstants.TOKEN, token.AuthToken);
                    response.Headers.Add(HttpConstants.TOKEN_EXPIRY, token.ExpiresOn.ToString());
                    response.Headers.Add(HttpConstants.ACCESS_CONTROL_EXPOSE_HEADERS,
                        HttpConstants.TOKEN_COMMA_TOKEN_EXPIRY);
                    var elapsedMs = watch.ElapsedMilliseconds;
                    response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
                    return response;
                }
            }

            return UnAuthorized(HttpConstants.UNAUTHORIZED);
        }

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns>
        /// </returns>
        [POST("userauth")]
        public HttpResponseMessage UserAuth()
        {
            if (System.Threading.Thread.CurrentPrincipal != null &&
                System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity =
                    System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    var userId = basicAuthenticationIdentity.UserId;
                    var token = _tokenServices.GenerateToken(userId);

                    #region To return User Account with Token

                    var user = _userPivotServices.GetUser(userId);
                    if (user == null)
                        return UnAuthorized("USER NOT FOUND");
                    string application = "";
                    var request = Request;
                    if (request != null && request.Headers != null)
                    {
                        var headers = request.Headers;
                        if (headers.Contains(HttpConstants.APPLICATION))
                        {
                            application = headers.GetValues(HttpConstants.APPLICATION).First();
                        }
                    }

                    if (string.IsNullOrEmpty(user.UserApplication))
                    {
                        if (!string.IsNullOrEmpty(application))
                        {
                            var validationErrors = new ValidationErrors();
                            _userPivotServices.Update(ref validationErrors, token.AuthToken, application);
                        }
                    }

                    watch.Stop();

                    var response = Request.CreateResponse(HttpStatusCode.OK, user);

                    #endregion To return User Account with Token

                    response.Headers.Add(HttpConstants.TOKEN, token.AuthToken);
                    response.Headers.Add(HttpConstants.TOKEN_EXPIRY, token.ExpiresOn.ToString());
                    response.Headers.Add(HttpConstants.ACCESS_CONTROL_EXPOSE_HEADERS,
                        HttpConstants.TOKEN_COMMA_TOKEN_EXPIRY);
                    var elapsedMs = watch.ElapsedMilliseconds;
                    response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
                    return response;
                }
            }

            return UnAuthorized(HttpConstants.UNAUTHORIZED);
        }
        
        /// <summary>
        /// Returns auth token for the validated user.
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        private HttpResponseMessage UnAuthorized(string message = "")
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized, HttpConstants.UNAUTHORIZED);
            if (!string.IsNullOrEmpty(message))
                response.Headers.Add("Content-Description", message);
            return response;
        }

        public UserToken GetDetailByToken(string tokenId)
        {
            return _tokenServices.GetDetailByToken(tokenId);
        }

        /// <summary>
        /// Delete users current token.
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        //[HttpDelete]
        [POST("PostAuthToken/{tokenId:string}")]
        public HttpResponseMessage DeleteAuthToken(string tokenId)
        {
            var token = _tokenServices.Kill(tokenId);
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized, HttpConstants.UNAUTHORIZED);
            response.Headers.Add(HttpConstants.TOKEN, "");
            response.Headers.Add(HttpConstants.TOKEN_EXPIRY, "0");
            response.Headers.Add(HttpConstants.ACCESS_CONTROL_EXPOSE_HEADERS, HttpConstants.TOKEN_COMMA_TOKEN_EXPIRY);
            return response;
        }

        /// <summary>
        /// Delete users current token.
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        //[HttpPost]
        [POST("PostAuthTokenKill/{tokenId:string}")]
        [Route("PostAuthTokenKill")]
        public HttpResponseMessage PostAuthToken(string tokenId)
        {
            var token = _tokenServices.Update(tokenId);
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized, HttpConstants.UNAUTHORIZED);
            response.Headers.Add(HttpConstants.TOKEN, "");
            response.Headers.Add(HttpConstants.TOKEN_EXPIRY, "0");
            response.Headers.Add(HttpConstants.ACCESS_CONTROL_EXPOSE_HEADERS, HttpConstants.TOKEN_COMMA_TOKEN_EXPIRY);
            return response;
        }
    }
}
