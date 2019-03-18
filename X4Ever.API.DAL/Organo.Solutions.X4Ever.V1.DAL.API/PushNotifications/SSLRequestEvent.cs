using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.PushNotification
{
    public class SSLRequestEvent
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public byte[] DataBytes { get; set; }
        public object Current { get; set; }

        public SSLRequestEvent(bool success, string message, byte[] dataBytes, object current)
        {
            IsSuccess = success;
            Message = message;
            DataBytes = dataBytes;
            Current = current;
        }
    }
}