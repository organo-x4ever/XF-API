using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Services;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/youtubevideos")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class YoutubeVideosController : ApiControllerOverride
    {
        private readonly IYoutubeVideoServices _youtubeVideoServices = new YoutubeVideoServices();

        [GET("getasync")]
        [Route("getasync")]
        public async Task<HttpResponseMessage> GetAsync()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var response =
                Request.CreateResponse(HttpStatusCode.OK, await _youtubeVideoServices.GetAsync(ApplicationKey));
            watch.Stop();
            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }
    }
}