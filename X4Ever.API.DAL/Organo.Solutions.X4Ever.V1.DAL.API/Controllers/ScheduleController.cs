using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [RoutePrefix("api/schedule")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ScheduleController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelper _helper;
        internal X4EverContext context;

        public ScheduleController(UnitOfWork unitOfWork, X4EverContext context)
        {
            this.context = context;
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
        }

        [POST("checkasync")]
        [Route("checkasync")]
        public async Task<HttpResponseMessage> CheckAsync()
        {
            double.TryParse(_helper.GetAppSetting("WeightSubmitIntervalDays"), out double timeInterval);
            var date = DateTime.Today.AddDays(-timeInterval);

            await _unitOfWork.UserRepository.SqlQueryAsync("UserTrackerReminderSelect");

            var userTrackers = (from u in await _unitOfWork.UserRepository.GetAllAsync()
                where _unitOfWork.UserTrackerRepository.GetAll().Where(t => t.ModifyDate > date)
                          .Any(t => t.UserID == u.ID) == false
                orderby u.ID
                select new
                {
                    ID = u.ID
                }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, userTrackers);

            //return Request.CreateResponse(Ok("Success"));
        }
    }
}