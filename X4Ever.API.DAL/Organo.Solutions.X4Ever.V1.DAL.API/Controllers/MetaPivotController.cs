using System;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/metapivot")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MetaPivotController : ApiControllerOverride
    {
        private readonly IUserMetaPivotServices _userMetaPivotServices;

        public MetaPivotController(UserMetaPivotServices userMetaPivotServices)
        {
            _userMetaPivotServices = userMetaPivotServices;
        }

        // GET: api/UserTrackersPivot
        [GET("getbyuser")]
        [Route("getbyuser")]
        public IHttpActionResult GetMeta()
        {
            return Ok(_userMetaPivotServices.GetMeta(base.UserID));
        }

        // GET: api/UserTrackersPivot
        [GET("getbyuserasync")]
        [Route("getbyuserasync")]
        public async Task<IHttpActionResult> GetMetaAsync()
        {
            return Ok(await _userMetaPivotServices.GetMetaAsync(base.UserID));
        }

        [POST("postmeta")]
        [Route("postmeta")]
        // POST: api/UserMetas
        public IHttpActionResult PostUserMeta(List<UserMeta> userMeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationErrors validationErrors = new ValidationErrors();
            if (_userMetaPivotServices.Insert(ref validationErrors, base.UserID, userMeta))
                return Ok(HttpConstants.SUCCESS);
            else
                return Ok(validationErrors.Show());
        }

        [POST("postmetadata")]
        [Route("postmetadata")]
        // POST: api/UserMetas
        public IHttpActionResult PostUserMeta(UserMeta userMeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationErrors validationErrors = new ValidationErrors();
            if (_userMetaPivotServices.Insert(ref validationErrors, base.UserID, userMeta))
                return Ok(HttpConstants.SUCCESS);
            else
                return Ok(validationErrors.Show());
        }

        [POST("postmeta1")]
        [Route("postmeta1")]
        // DELETE: api/UserMetas/5
        [ResponseType(typeof(UserMeta))]
        public IHttpActionResult DeleteUserMeta(List<UserMeta> userMeta)
        {
            ValidationErrors validationErrors = new ValidationErrors();
            if (_userMetaPivotServices.Delete(ref validationErrors, userMeta))
                return Ok(HttpConstants.SUCCESS);
            else
                return Ok(validationErrors.Show());
        }
    }
}