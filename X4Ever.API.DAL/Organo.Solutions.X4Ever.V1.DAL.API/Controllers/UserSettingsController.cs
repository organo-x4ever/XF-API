using System.Threading.Tasks;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/usersettings")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserSettingsController : ApiControllerOverride
    {
        private readonly IUserSettingServices _userSettingServices;

        public UserSettingsController(UserSettingServices userSettingServices)
        {
            _userSettingServices = userSettingServices;
        }

        // GET: api/UserSettings/5
        [GET("getsetting")]
        [Route("getsetting")]
        [ResponseType(typeof(UserSetting))]
        public async Task<IHttpActionResult> GetUserSetting()
        {
            UserSetting userSetting = new UserSetting();
            userSetting = await _userSettingServices.GetAsync(base.UserID);
            if (userSetting == null)
            {
                return NotFound();
            }

            return Ok(userSetting);
        }

        // POST: api/UserSettings
        [POST("postsetting")]
        [Route("postsetting")]
        [ResponseType(typeof(UserSetting))]
        public IHttpActionResult PostUserSetting(UserSetting userSetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationErrors validationErrors = new ValidationErrors();
            if (_userSettingServices.Insert(ref validationErrors, base.UserID, userSetting))
                return Ok(HttpConstants.SUCCESS);
            return Ok(validationErrors.Show());
        }

        // POST: api/UserSettings
        [POST("postuserlanguage")]
        [Route("postuserlanguage")]
        public IHttpActionResult PostUserLanguage(ApplicationLanguageRequest applicationLanguage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationErrors validationErrors = new ValidationErrors();
            if (_userSettingServices.UpdateUserLanguage(ref validationErrors, base.UserID,
                applicationLanguage.LanguageCode, applicationLanguage.LanguageName))
                return Ok(HttpConstants.SUCCESS);
            return Ok(validationErrors.Show());
        }

        // POST: api/UserSettings
        [POST("postuserweightvolume")]
        [Route("postuserweightvolume")]
        public IHttpActionResult PostUserWeightVolume(UserWeightVolume weightVolume)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidationErrors validationErrors = new ValidationErrors();
            if (_userSettingServices.UpdateUserWeightVolume(ref validationErrors, base.UserID,
                weightVolume.WeightVolume))
                return Ok(HttpConstants.SUCCESS);
            return Ok(validationErrors.Show());
        }

        // DELETE: api/UserSettings/5
        [POST("postremovesetting")]
        [Route("postremovesetting")]
        public IHttpActionResult DeleteUserSetting()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            if (_userSettingServices.Delete(ref validationErrors, base.UserID))
                return Ok(HttpConstants.SUCCESS);
            return Ok(validationErrors.Show());
        }
    }
}