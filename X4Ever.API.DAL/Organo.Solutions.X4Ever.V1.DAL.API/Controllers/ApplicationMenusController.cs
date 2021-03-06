﻿using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
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
            List<Menu> menuList=new List<Menu>();
            var platform = Platform;
            if (platform == PlatformType.Wrong)
                menuList = new List<Menu>();
            else if (platform == PlatformType.None)
                menuList = (await _menuServices.GetByApplicationAsync(ApplicationKey)).ToList();
            else if (VersionNumber == 0)
                menuList = (await _menuServices.GetByApplicationAsync(ApplicationKey, platform)).ToList();
            else
                menuList = (await _menuServices.GetByApplicationAsync(ApplicationKey, platform, VersionNumber)).ToList();

            if (this.UserID == 55)
            {
                int[] menuIDs = {7, 8, 12};
                menuList = menuList.Where(m => menuIDs.Contains(m.ID) == false).ToList();
            }

            return Ok(menuList);
        }
    }
}