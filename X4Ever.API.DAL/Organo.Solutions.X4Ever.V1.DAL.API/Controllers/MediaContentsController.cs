using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/mediacontents")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MediaContentsController : ApiControllerOverride
    {
        private readonly IMediaContentServices _mediaContentServices;

        public MediaContentsController(MediaContentServices mediaContentServices)
        {
            _mediaContentServices = mediaContentServices;
        }

        // GET: api/mediacontents
        [GET("get")]
        [Route("get")]
        public async Task<IEnumerable<MediaContent>> Get()
        {
            return await _mediaContentServices.GetAsync();
        }

        // GET: api/mediacontents
        [GET("getdetails")]
        [Route("getdetails")]
        public async Task<IEnumerable<MediaContentDetail>> GetDetails()
        {
            return await _mediaContentServices.GetDetailAsync();
        }
    }
}