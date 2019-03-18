using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/usermilestones")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserMilestonesController : ApiControllerOverride
    {
        private readonly IUserMilestoneServices _userMilestoneServices;

        public UserMilestonesController(UserMilestoneServices milestoneServices)
        {
            _userMilestoneServices = milestoneServices;
        }

        // GET: api/UserMilestones
        [GET("getbyuser")]
        [Route("getbyuser")]
        public async Task<IEnumerable<UserMilestone>> GetByUser()
        {
            return await _userMilestoneServices.GetByUserAsync(base.UserID);
        }

        // GET: api/UserMilestones
        [GET("getdetail")]
        [Route("getdetail")]
        public async Task<IDictionary<string, object>> GetDetailByUser()
        {
            return await _userMilestoneServices.GetDetailByUserAsync(base.UserID);
        }

        // GET: api/UserMilestones
        [GET("getextended")]
        [Route("getextended")]
        public async Task<UserMilestoneExtended> GetExtendedByUser()
        {
            return await _userMilestoneServices.GetExtendedByUserAsync(base.UserID);
        }

        // GET: api/UserMilestones
        [GET("getextended")]
        [Route("getextended")]
        public async Task<UserMilestoneExtended> GetExtendedByUser(string languageCode)
        {
            return await _userMilestoneServices.GetExtendedByUserAsync(base.UserID, languageCode);
        }

        // POST: api/UserTrackers
        [POST("postusermilestone")]
        [Route("postusermilestone")]
        [ResponseType(typeof(UserMilestone))]
        public async Task<IHttpActionResult> PostUserMilestone(UserMilestone userMilestone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationErrors validationErrors = new ValidationErrors();
            if (_userMilestoneServices.Insert(ref validationErrors, base.UserID, userMilestone))
                return Ok(HttpConstants.SUCCESS);
            else
                return Ok(validationErrors.Show());
        }
    }
}