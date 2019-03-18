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
    [RoutePrefix("api/mealplans")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MealPlansController : ApiControllerOverride
    {
        private readonly IMealPlanServices _mealPlanServices;

        public MealPlansController(MealPlanServices mealPlanServices)
        {
            _mealPlanServices = mealPlanServices;
        }

        // GET: api/mealplans
        [GET("get")]
        [Route("get")]
        public async Task<IEnumerable<MealPlan>> Get()
        {
            return await _mealPlanServices.GetAsync();
        }

        // GET: api/mealplans
        [GET("getdetails")]
        [Route("getdetails")]
        public async Task<IEnumerable<MealPlan>> GetDetails(string languageCode = "en-US")
        {
            return await _mealPlanServices.GetDetailAsync(languageCode);
        }

        // GET: api/mealplans
        [GET("getdetails")]
        [Route("getdetails")]
        public async Task<IEnumerable<MealPlan>> Get(bool active, string languageCode = "en-US")
        {
            return await _mealPlanServices.GetDetailAsync(languageCode, active);
        }
    }
}