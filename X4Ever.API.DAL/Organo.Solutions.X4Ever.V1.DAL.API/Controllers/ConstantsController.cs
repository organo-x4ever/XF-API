
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/constants")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConstantsController : ApiController
    {
        private readonly IHelper _helper;
        public ConstantsController()
        {
            _helper = new Helper.Helper();
        }

        [GET("blogs/{region:string}/{lang=string}")]
        [Route("blogs")]
        public string GetBlogsLink(string region, string lang) => _helper.GetAppSetting(region.Contains("1a9633fb-83b5-46b7-8669-1e9ad9f02fd3") ? ((lang ?? "en").ToLower().Contains("es") ? CommonConstants.X4EverBlogsSPA : CommonConstants.X4EverBlogs) : CommonConstants.X4EverEMEABlogs);

        [GET("more_links_path/{Application:string}/{Language=string}/{Version=string}/{Platform=string}")]
        [Route("more_links_path")]
        public string GetMoreLinksPath(string Application, string Language, string Version, string Platform) =>
            string.Format(_helper.GetAppSetting(CommonConstants.X4EverWebMore), (Language ?? "en").ToLower());

        [GET("weightlosewarningpercentile")]
        [Route("weightlosewarningpercentile")]
        public string GetWeightLoseWarningPercentage() => _helper.GetAppSetting(CommonConstants.TrackerWeightLoseWarning);
    }
}