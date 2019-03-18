using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Models
{
    public class AppInfoModel
    {
        public string AppName { get; set; }
        public string Package { get; set; }
        public string Version { get; set; }
        public string VersionCode { get; set; }
    }
}