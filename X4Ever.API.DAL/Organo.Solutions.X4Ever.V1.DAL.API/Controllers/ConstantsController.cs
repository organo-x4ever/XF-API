
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

        //[GET("blogs")]
        //[Route("blogs")]
        //public string GetBlogsLink() => _helper.GetAppSetting(CommonConstants.X4EverBlogs);

        [GET("blogs/{region:string}/{lang=string}")]
        [Route("blogs")]
        public string GetBlogsLink(string region, string lang)=> _helper.GetAppSetting(region.Contains("1a9633fb-83b5-46b7-8669-1e9ad9f02fd3") ? ((lang??"en").ToLower().Contains("es")?CommonConstants.X4EverBlogsSPA: CommonConstants.X4EverBlogs) : CommonConstants.X4EverEMEABlogs);
    }
}