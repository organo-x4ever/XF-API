
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Linq;
using System.Threading.Tasks;
//using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/constants")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConstantsController : ApiController
    {
        private readonly IHelper _helper;
        private readonly ITrackerPhotoSkipServices _trackerPhotoSkipServices;
        private readonly IUserTokensServices _userTokensServices;
        //private readonly string _restLogFilePath;
        //private const string _skipPhotoFileName = "skip_photo_log.json";
        public ConstantsController(UserTokensServices userTokensServices,TrackerPhotoSkipServices trackerPhotoSkipServices)
        {
            _helper = new Helper.Helper();
            _trackerPhotoSkipServices = trackerPhotoSkipServices;
            _userTokensServices = userTokensServices;
            //_restLogFilePath = HttpContext.Current.Request.MapPath("~/" + _helper.GetAppSetting(CommonConstants.RestOfTheLogs) + "/" + _skipPhotoFileName);
        }

        [GET("blogs/{region:string}/{lang=string}")]
        [Route("blogs")]
        public string GetBlogsLink(string region, string lang) => _helper.GetAppSetting(region.Contains("1a9633fb-83b5-46b7-8669-1e9ad9f02fd3") ? ((lang ?? "en").ToLower().Contains("es") ? CommonConstants.X4EverBlogsSPA : CommonConstants.X4EverBlogs) : CommonConstants.X4EverEMEABlogs);

        [GET("more_links_path/{Application:string}/{Language=string}/{Version=string}/{Platform=string}")]
        [Route("more_links_path")]
        public string GetMoreLinksPath(string Application, string Language, string Version, string Platform) =>
            string.Format(_helper.GetAppSetting(CommonConstants.X4EverWebMore), (Language ?? "en").ToLower());

        [GET("weightlosewarningpercentile")]
        [Route("weightlosewarningpercentile")]
        public string GetWeightLoseWarningPercentage() => _helper.GetAppSetting(CommonConstants.TrackerWeightLoseWarning);

        //[GET("trackerskipphotoonsteps")]
        //[Route("trackerskipphotoonsteps")]
        //public string GetTrackerSkipPhotoOnSteps() => _helper.GetAppSetting(CommonConstants.TrackerSkipPhotoOnSteps);

        [GET("trackerskipphotoonsteps")]
        [Route("trackerskipphotoonsteps")]
        public async Task<string> GetTrackerSkipPhotoOnSteps()
        {
            var result = _helper.GetAppSetting(CommonConstants.TrackerSkipPhotoOnSteps)??CommonConstants.NO;
            if(!result.ToLower().Equals(CommonConstants.YES))
                return result;

            var userID = _userTokensServices.Validate(GetToken());
            var list = await _trackerPhotoSkipServices.GetByUserIDAsync(FileLocator.TRACKER_PHOTO_SKIP_JSON_PATH, userID);
            if (list.Count >= 1)
                return CommonConstants.NO;

            return result;
            string GetToken()
            {
                var request = Request;
                if (request != null && request.Headers != null)
                {
                    var headers = request.Headers;
                    if (headers.Contains(HttpConstants.TOKEN))
                    {
                        return headers.GetValues(HttpConstants.TOKEN).First();
                    }
                }
                return "";
            }
        }
    }
}