using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_milestone_percentage", Schema = "x4ever")]
    public sealed class MilestonePercentage
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")] public Int16 ID { get; set; }
        [Column(name: "application_key")] public string ApplicationKey { get; set; }
        [Column(name: "milestone_percentage_id")] public Int16 MilestonePercentageId { get; set; }
        [Column(name: "language_code")] public string LanguageCode { get; set; }

        [Column(name: "milestone_title")] public String MilestoneTitle { get; set; }

        [Column(name: "milestone_subtitle")] public String MilestoneSubTitle { get; set; }

        [Column(name: "target_percent_value")] public short TargetPercentValue { get; set; }

        [Column(name: "is_percent")] public bool IsPercent { get; set; }

        [Column(name: "achieved_message")] public String AchievedMessage { get; set; }

        [Column(name: "achievement_icon")] public String AchievementIcon { get; set; }

        [Column(name: "achievement_gift_image")]
        public String AchievementGiftImage { get; set; }

        [Column(name: "active")] public bool Active { get; set; }
    }
}