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
    [RoutePrefix("api/weight_volumes")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WeightVolumesController : ApiControllerOverride
    {
        private IWeightVolumeServices _weightVolumeServices;

        public WeightVolumesController(WeightVolumeServices weightVolumeServices)
        {
            _weightVolumeServices = weightVolumeServices;
        }

        [GET("get")]
        [Route("get")]
        public async Task<IEnumerable<WeightVolume>> Get()
        {
            return await _weightVolumeServices.GetWeightVolumesAsync(base.ApplicationKey);
        }
    }
}