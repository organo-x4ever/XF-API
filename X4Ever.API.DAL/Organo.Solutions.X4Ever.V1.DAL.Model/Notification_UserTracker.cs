using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class Notification_UserTracker
    {
        public long UserID { get; set; }
        public string UserEmail { get; set; }
        public UserPushToken UserPushToken { get; set; }
        public string LanguageCode { get; set; }
        public DateTime? LastTrackerDate { get; set; }
    }
}