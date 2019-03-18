using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Models
{
    public class PushMessageModel
    {
        public PushMessage message { get; set; }
    }

    public class PushMessage
    {
        public string token { get; set; }
        public PushNotification notification { get; set; }
    }

    public class PushNotification
    {
        public string body { get; set; }
        public string title { get; set; }
    }
}