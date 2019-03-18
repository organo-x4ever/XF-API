using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/milestonepercentages")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MilestonePercentagesController : ApiControllerOverride
    {
        private IMilestonePercentageServices _milestonePercentageServices;

        public MilestonePercentagesController(MilestonePercentageServices milestonePercentageServices)
        {
            _milestonePercentageServices = milestonePercentageServices;
        }

        [GET("getbylangauge/{languageCode:string}")]
        [Route("getbylangauge")]
        public async Task<IEnumerable<MilestonePercentage>> GetByLanguage(string languageCode)
        {
            return await _milestonePercentageServices.GetByLanguageAsync(languageCode);
        }
    }
}