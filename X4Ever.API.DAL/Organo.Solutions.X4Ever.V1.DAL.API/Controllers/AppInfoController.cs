using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using AttributeRouting.Web.Http;
using Newtonsoft.Json;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/appinfo")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AppInfoController : ApiController
    {
        private IHelper _helper;

        public AppInfoController()
        {
            _helper = new Helper.Helper();
        }

        [GET("get")]
        [Route("get")]
        public HttpResponseMessage Get(string platform)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            platform = platform.ToLower() + ":";
            var package = _helper.GetAppSetting(platform.ToLower() + "PackageName");
            var version = _helper.GetAppSetting(platform.ToLower() + "Version");
            var versionCode = _helper.GetAppSetting(platform.ToLower() + "VersionCode");
            var appName = _helper.GetAppSetting(platform.ToLower() + "AppName");

            var model = new AppInfoModel()
            {
                AppName = appName,
                Package = package,
                Version = version,
                VersionCode = versionCode
            };
            watch.Stop();
            //JsonConvert.SerializeObject(model)
            var response = Request.CreateResponse(HttpStatusCode.OK, model);
            var elapsedMs = watch.ElapsedMilliseconds;
            response.Headers.Add(HttpConstants.EXECUTION_TIME, elapsedMs.ToString());
            return response;
        }
    }
}