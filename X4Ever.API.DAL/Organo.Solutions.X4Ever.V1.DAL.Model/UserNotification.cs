using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_user_notification", Schema = "x4ever")]
    public class UserNotification
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public long ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }
        [Column(name: "sent_datetime")] public DateTime SentDatetime { get; set; }
        [Column(name: "notification_title")] public string NotificationTitle { get; set; }
        [Column(name: "notification_body")] public string NotificationBody { get; set; }
        [Column(name: "user_detail")] public string UserDetail { get; set; }
        [Column(name: "status_description")] public string StatusDescription { get; set; }
        [Column(name: "notification_type")] public string NotificationType { get; set; }
        [Column(name: "is_push")] public bool IsPush { get; set; }
        [Column(name: "is_scheduled")] public bool IsScheduled { get; set; }
        [Column(name: "user_platform")] public string UserPlatform { get; set; }
    }
}