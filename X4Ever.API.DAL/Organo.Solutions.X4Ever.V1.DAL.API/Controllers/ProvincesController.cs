using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/provinces")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProvincesController : ApiControllerOverride
    {
        private IProvinceServices _provinceServices;

        public ProvincesController(ProvinceServices provinceServices)
        {
            _provinceServices = provinceServices;
        }

        [GET("get")]
        [System.Web.Http.Route("get")]
        public async Task<HttpResponseMessage> GetProvince()
        {
            var applicationKey = ApplicationKey;

            if (string.IsNullOrEmpty(applicationKey))
            {
                return Request.CreateResponse(BadRequest("Application Id is required"));
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var provinces = await _provinceServices.GetProvincesByApplicationAsync(applicationKey);
            // the code that you want to measure comes here
            watch.Stop();
            if (provinces.Count == 0)
            {
                return Request.CreateResponse(NotFound());
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, provinces);
            var elapsedMs = watch.ElapsedMilliseconds;
            response.Headers.Add("ExecutionTime", elapsedMs.ToString());

            return response;
        }
    }
}