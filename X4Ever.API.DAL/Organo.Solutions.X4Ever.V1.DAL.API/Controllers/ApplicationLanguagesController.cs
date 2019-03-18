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
    [RoutePrefix("api/application_languages")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApplicationLanguagesController : ApiControllerOverride
    {
        private ILanguageServices _languageServices;

        public ApplicationLanguagesController(LanguageServices languageServices)
        {
            _languageServices = languageServices;
        }

        [GET("getwithcountry")]
        [Route("getwithcountry")]
        public async Task<IEnumerable<ApplicationLanguage>> GetWithCountry()
        {
            return await _languageServices.GetLanguagesWithCountryAsync(base.ApplicationKey);
        }

        [GET("get")]
        [Route("get")]
        public async Task<IEnumerable<ApplicationLanguage>> Get()
        {
            return await _languageServices.GetLanguagesAsync(base.ApplicationKey);
        }

        [GET("getbycountry")]
        [Route("getbycountry")]
        public async Task<IEnumerable<ApplicationLanguage>> GetByCountry(int countryID)
        {
            return await _languageServices.GetLanguageByCountryIDAsync(base.ApplicationKey, countryID);
        }

        [GET("getbylanguage")]
        [Route("getbylanguage")]
        public async Task<IEnumerable<ApplicationLanguage>> GetByLanguage(int languageID)
        {
            return await _languageServices.GetLanguageByLanguageIDAsync(base.ApplicationKey, languageID);
        }
    }
}