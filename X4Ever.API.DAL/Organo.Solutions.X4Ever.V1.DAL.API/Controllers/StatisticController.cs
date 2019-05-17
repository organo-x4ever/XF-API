using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/statistics")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StatisticController : ApiControllerOverride
    {
        private readonly IStatisticServices _statisticServices;
        public StatisticController(StatisticServices statisticServices)
        {
            _statisticServices = statisticServices;
        }

        [POST("post")]
        [Route("post")]
        public async Task<IHttpActionResult> Post(Statistic model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var validationErrors = new ValidationErrors();
            model.UserID = UserID;
            if (_statisticServices.Insert(ref validationErrors, model))
            {
                return Ok(HttpConstants.SUCCESS);
            }
            else
            {
                return Ok("Invalid Request");
            }
        }
    }
}