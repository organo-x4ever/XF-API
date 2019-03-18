using System;
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
    [RoutePrefix("api/milestones")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MilestonesController : ApiControllerOverride
    {
        private readonly IMilestoneServices _milestoneServices;

        public MilestonesController(MilestoneServices milestoneServices)
        {
            _milestoneServices = milestoneServices;
        }

        // GET: api/Milestones
        [GET("get")]
        [Route("get")]
        public async Task<IEnumerable<Milestone>> GetByUser()
        {
            return await _milestoneServices.GetAsync();
        }
    }
}