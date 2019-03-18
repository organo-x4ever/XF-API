using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/news")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NewsController : ApiControllerOverride
    {
        private readonly INewsServices _newsServices;

        public NewsController(NewsServices newsServices)
        {
            _newsServices = newsServices;
        }

        // GET: api/news
        [GET("getbyactive")]
        [Route("getbyactive")]
        public async Task<IEnumerable<News>> GetByActive(bool active)
        {
            return await _newsServices.GetOrderByDateAsync(active);
        }

        // GET: api/news
        [GET("get")]
        [Route("get")]
        public async Task<HttpResponseMessage> GetByLanguage(string languageCode, bool active)
        {
            var applicationKey = ApplicationKey;
            if (string.IsNullOrEmpty(applicationKey))
            {
                return Request.CreateResponse(BadRequest("Application Id is required"));
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var news = await _newsServices.GetByLanguageAndByDateAsync(applicationKey, languageCode, active);
            // the code that you want to measure comes here
            watch.Stop();
            if (news.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, news);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, news);
            var elapsedMs = watch.ElapsedMilliseconds;
            response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());

            return response;
        }
    }
}