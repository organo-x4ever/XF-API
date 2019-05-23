using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Models;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/messageservice")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MessageServicesController : ApiControllerOverride
    {
        private IUserServices _userServices;

        public MessageServicesController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [POST("sendasync")]
        [Route("sendasync")]
        public async Task<IHttpActionResult> SendAsync([FromBody] MessageDetail content)
        {
            var user = _userServices.Get(base.UserID);
            string message = "";
            await Task.Run(() =>
            {
                if (content != null)
                    new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject, content.Body, true);
                return Ok(string.IsNullOrEmpty(message) ? "Success" : message);
            });
            return Ok();
        }

        [POST("send")]
        [Route("send")]
        public IHttpActionResult Send([FromBody] MessageDetail content)
        {
            string message = "";
            var user = _userServices.Get(base.UserID);
            if (content != null)
            {
                new Message().SendMail(ref message, user.UserEmail, "", "", content.Subject, content.Body, true);
                return Ok(string.IsNullOrEmpty(message) ? "Success" : message);
            }
            return Ok();
        }
    }
}