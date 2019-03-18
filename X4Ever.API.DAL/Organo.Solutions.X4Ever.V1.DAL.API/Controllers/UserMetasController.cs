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
using Microsoft.AspNet.Identity;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/usermeta")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserMetasController : ApiControllerOverride
    {
        private readonly IUserMetaServices _userMetaServices;

        public UserMetasController(UserMetaServices userMetaServices)
        {
            _userMetaServices = userMetaServices;
        }

        [GET("getbyuser")]
        [Route("getbyuser")]
        // GET: api/UserMetas
        public async Task<IEnumerable<UserMeta>> GetByUser()
        {
            return await _userMetaServices.GetByUserAsync(base.UserID);
        }

        [GET("getbyid")]
        [Route("getbyid")]
        // GET: api/UserMetas
        public async Task<IEnumerable<UserMeta>> GetByUser(long id)
        {
            return await _userMetaServices.GetByUserAsync(id);
        }

        [GET("getbykey")]
        [Route("getbykey")]
        // GET: api/UserMetas/5
        [ResponseType(typeof(UserMeta))]
        public async Task<IHttpActionResult> GetUserMeta(string key)
        {
            var userMeta = await _userMetaServices.GetByUserAsync(base.UserID, key);
            if (userMeta == null)
            {
                return NotFound();
            }

            return Ok(userMeta);
        }

        [GET("getbykeys")]
        [Route("getbykeys")]
        // GET: api/UserMetas/5
        [ResponseType(typeof(UserMeta))]
        public async Task<IHttpActionResult> GetKeysByUser(string key)
        {
            string[] keys = key.Split(',');
            var userMetas = await _userMetaServices.GetByUserAsync(base.UserID, keys);
            if (userMetas == null)
            {
                return NotFound();
            }

            return Ok(userMetas);
        }

        [GET("getbykeys")]
        [Route("getbykeys")]
        // GET: api/UserMetas/5
        [ResponseType(typeof(UserMeta))]
        public async Task<IHttpActionResult> GetKeysByUser(string[] keys)
        {
            var userMetas = await _userMetaServices.GetByUserAsync(base.UserID, keys);
            if (userMetas == null)
            {
                return NotFound();
            }

            return Ok(userMetas);
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
            if (_userMetaServices.Insert(ref validationErrors, base.UserID, userMeta))
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
            if (_userMetaServices.Insert(ref validationErrors, base.UserID, userMeta))
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
            if (_userMetaServices.Delete(ref validationErrors, userMeta))
                return Ok(HttpConstants.SUCCESS);
            else
                return Ok(validationErrors.Show());
        }
    }
}