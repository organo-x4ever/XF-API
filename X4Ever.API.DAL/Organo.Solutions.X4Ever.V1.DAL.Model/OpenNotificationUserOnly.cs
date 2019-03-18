using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_open_notification_only_users_view", Schema = "x4ever")]
    public class OpenNotificationOnlyUser
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }
        [Column(name: "device_token")] public string DeviceToken { get; set; }
        [Column(name: "device_platform")] public string DevicePlatform { get; set; }
        [Column(name: "user_email")] public string UserEmail { get; set; }
        [Column(name: "language_code")] public string LanguageCode { get; set; }
    }
}