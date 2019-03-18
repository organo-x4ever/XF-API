using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Services;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/applications")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApplicationsController : ApiController
    {
        private readonly IApplicationServices _applicationServices;

        public ApplicationsController(ApplicationServices applicationServices)
        {
            _applicationServices = applicationServices;
        }

        [GET("getasync")]
        [Route("getasync")]
        public async Task<HttpResponseMessage> GetAsync()
        {
            var watch = Stopwatch.StartNew();
            HttpResponseMessage response;
            try
            {
                var applications = await _applicationServices.GetAsync();
                watch.Stop();
                if (applications?.Count() > 0)
                    response = Request.CreateResponse(HttpStatusCode.OK, applications);
                else
                    response = Request.CreateResponse(HttpStatusCode.NotFound, "No record found");
            }
            catch (Exception exception)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, exception.ToString());
            }

            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }

        [GET("get")]
        [Route("get")]
        public HttpResponseMessage Get()
        {
            var watch = Stopwatch.StartNew();
            HttpResponseMessage response;
            try
            {
                var applications = _applicationServices.Get();
                watch.Stop();
                if (applications?.Count() > 0)
                    response = Request.CreateResponse(HttpStatusCode.OK, applications);
                else
                    response = Request.CreateResponse(HttpStatusCode.NotFound, "No record found");
            }
            catch (Exception exception)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, exception.ToString());
            }

            response.Headers.Add(HttpConstants.EXECUTION_TIME, watch.ElapsedMilliseconds.ToString());
            return response;
        }
    }
}