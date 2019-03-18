using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class UserPushTokenRegister
    {
        public Int32 ID { get; set; }

        public Int64 UserID { get; set; }
        public string OldDeviceToken { get; set; }
        public string DeviceToken { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceIdentity { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceApplication { get; set; }
        public DateTime IssuedOn { get; set; }
    }
}