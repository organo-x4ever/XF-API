using System;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using System.Linq;
using System.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.Helper.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model.EnumerationTypes;
using System.Web.Http.ModelBinding;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    public class ApiControllerOverride : ApiController
    {
        protected string Token => GetToken();
        protected long UserID => GetUserID();
        private string GetToken()
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

        private long GetUserID()
        {
            ModelState.TryGetValue(HttpConstants.USER_ID, out ModelState modelState);
            long.TryParse(modelState.Value.AttemptedValue, out long ID);
            return ID;
        }

        protected string ApplicationKey => GetApplication();

        private string GetApplication()
        {
            var request = Request;
            if (request != null && request.Headers != null)
            {
                var headers = request.Headers;
                if (headers.Contains(HttpConstants.APPLICATION))
                {
                    return headers.GetValues(HttpConstants.APPLICATION).First();
                }
            }

            return "";
        }

        protected string Language => GetLanguage();

        private string GetLanguage()
        {
            var request = Request;
            if (request != null && request.Headers != null)
            {
                var headers = request.Headers;
                if (headers.Contains(HttpConstants.LANGUAGE))
                {
                    return headers.GetValues(HttpConstants.LANGUAGE).First();
                }
            }

            return "";
        }

        protected PlatformType Platform => GetPlatform();

        private PlatformType GetPlatform()
        {
            var request = Request;
            if (request != null && request.Headers != null)
            {
                var headers = request.Headers;
                if (headers.Contains(HttpConstants.PLATFORM))
                {
                    try
                    {
                        var type = headers.GetValues(HttpConstants.PLATFORM).First();
                        return (PlatformType) Enum.Parse(typeof(PlatformType), type, true);
                    }
                    catch (Exception)
                    {
                        //
                    }

                    return PlatformType.Wrong;
                }
            }

            return PlatformType.None;
        }

        public object GetHeader(string httpConstants)
        {
            var request = Request;
            if (request != null && request.Headers != null)
            {
                var headers = request.Headers;
                if (headers.Contains(httpConstants))
                {
                    return headers.GetValues(httpConstants).First();
                }
            }

            return null;
        }
    }
}