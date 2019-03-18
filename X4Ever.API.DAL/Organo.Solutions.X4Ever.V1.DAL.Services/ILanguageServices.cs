using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface ILanguageServices : IDefaultServices<Language>
    {
        List<ApplicationLanguage> GetLanguages(string applicationID);
        Task<List<ApplicationLanguage>> GetLanguagesAsync(string applicationID);
        List<ApplicationLanguage> GetLanguagesWithCountry(string applicationID);
        Task<List<ApplicationLanguage>> GetLanguagesWithCountryAsync(string applicationID);
        List<ApplicationLanguage> GetLanguageByCountryID(string applicationID, int countryID);
        Task<List<ApplicationLanguage>> GetLanguageByCountryIDAsync(string applicationID, int countryID);
        List<ApplicationLanguage> GetLanguageByLanguageID(string applicationID, int languageID);
        Task<List<ApplicationLanguage>> GetLanguageByLanguageIDAsync(string applicationID, int languageID);
    }
}