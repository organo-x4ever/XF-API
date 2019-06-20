
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_user_notification_setting_view", Schema = "x4ever")]
    public class UserNotificationSettingsView
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")] public long ID { get; set; }
        [Column(name: "user_id")] public long UserID { get; set; }

        [Column(name: "general_message_allowed")] public bool IsGeneralMessage { get; set; }
        [Column(name: "intimation_allowed")] public bool Intimation { get; set; }
        [Column(name: "promotional_allowed")] public bool IsPromotional { get; set; }
        [Column(name: "special_offer_allowed")] public bool IsSpecialOffer { get; set; }
        [Column(name: "version_update_allowed")] public bool IsVersionUpdate { get; set; }
        [Column(name: "weight_submit_reminder_allowed")] public bool IsWeightSubmitReminder { get; set; }

        [Column(name: "create_date")] public DateTime CreateDate { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }


        [Column(name: "user_first_name")] public string FirstName { get; set; }
        [Column(name: "user_last_name")] public string LastName { get; set; }
        [Column(name: "user_email")] public string UserEmail { get; set; }
    }
}
