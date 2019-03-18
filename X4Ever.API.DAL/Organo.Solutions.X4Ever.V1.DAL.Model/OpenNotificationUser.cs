
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_open_notification_users_view", Schema = "x4ever")]
    public class OpenNotificationUser
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }
        [Column(name: "device_token")] public string DeviceToken { get; set; }
        [Column(name: "device_platform")] public string DevicePlatform { get; set; }
        [Column(name: "user_email")] public string UserEmail { get; set; }
        [Column(name: "language_code")] public string LanguageCode { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
    }

    public class OpenNotificationUserMap
    {
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string DeviceToken { get; set; }
        public string DevicePlatform { get; set; }
        public string UserEmail { get; set; }
        public string LanguageCode { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsStartToday { get; set; }
    }
}