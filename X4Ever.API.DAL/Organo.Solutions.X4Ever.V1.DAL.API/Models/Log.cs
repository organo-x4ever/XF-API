﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Models
{
    public class Log
    {
        public string Application { get; set; }
        public string Device { get; set; }
        public string Platform { get; set; }
        public string Idiom { get; set; }
        public string Identity { get; set; }
        public string IPAddress { get; set; }
        public string Token { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public Uri RequestUri { get; set; }
    }
}