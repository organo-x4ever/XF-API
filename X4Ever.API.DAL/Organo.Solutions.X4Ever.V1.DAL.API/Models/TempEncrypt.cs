using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Models
{
    public class TempEncrypt
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string EncryPassword { get; set; }
        public string SavedPassword { get; set; }
        public bool MisMatch { get; set; }
    }
}