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
    [RoutePrefix("api/applicationsettings")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApplicationSettingsController : ApiControllerOverride
    {
        private readonly IApplicationSettingServices _applicationSettingServices;
        private HttpResponseMessage response;

        public ApplicationSettingsController(ApplicationSettingServices applicationSettingServices)
        {
            _applicationSettingServices = applicationSettingServices;
        }

        [GET("get")]
        [Route("get")]
        public HttpResponseMessage Get()
        {
            response = null;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var settings = _applicationSettingServices.Get(base.ApplicationKey, base.Language);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            if (settings != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, settings);
                response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "No record found");
                response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
            }

            return response;
        }

        [GET("getasync")]
        [Route("getasync")]
        public async Task<HttpResponseMessage> GetAsync()
        {
            response = null;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var settings = await _applicationSettingServices.GetAsync(base.ApplicationKey, base.Language);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            if (settings != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, settings);
                response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "No record found");
                response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
            }

            return response;
        }
    }
}