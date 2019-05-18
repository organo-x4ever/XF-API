
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    //[AuthorizationRequired]
    [RoutePrefix("api/constants")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConstantsController : ApiController//Override
    {
        private readonly IHelper _helper;
        public ConstantsController()
        {
            _helper = new Helper.Helper();
        }

        [GET("blogs")]
        [Route("blogs")]
        public string GetBlogsLink() => _helper.GetAppSetting(CommonConstants.X4EverBlogs);
    }
}