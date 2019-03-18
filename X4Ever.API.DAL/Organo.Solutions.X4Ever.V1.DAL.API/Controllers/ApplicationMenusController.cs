using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Model.EnumerationTypes;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/appmenus")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApplicationMenusController : ApiControllerOverride
    {
        private readonly IMenuServices _menuServices;

        public ApplicationMenusController(MenuServices menuServices)
        {
            _menuServices = menuServices;
        }

        [GET("get")]
        [Route("get")]
        public async Task<IHttpActionResult> GetByUser()
        {
            List<Menu> menuList;
            var platform = Platform;
            if (platform == PlatformType.Wrong)
                menuList = new List<Menu>();
            else if (platform == PlatformType.None)
                menuList = _menuServices.GetByApplication(ApplicationKey).ToList();
            else
                menuList = _menuServices.GetByApplication(ApplicationKey, platform).ToList();
            return Ok(menuList);
        }
    }
}