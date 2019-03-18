using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_open_notification_user_registration_view", Schema = "x4ever")]
    public class OpenNotificationUserRegistration
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }
        [Column(name: "device_token")] public string DeviceToken { get; set; }
        [Column(name: "device_platform")] public string DevicePlatform { get; set; }
        [Column(name: "user_email")] public string UserEmail { get; set; }
        [Column(name: "language_code")] public string LanguageCode { get; set; }
        [Column(name: "date_created")] public DateTime CreateDate { get; set; }
    }

    public class OpenNotificationUserRegistrationMap
    {
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string DeviceToken { get; set; }
        public string DevicePlatform { get; set; }
        public string UserEmail { get; set; }
        public string LanguageCode { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsStartToday { get; set; }
    }
}