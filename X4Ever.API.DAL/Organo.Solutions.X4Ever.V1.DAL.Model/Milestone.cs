using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_milestone", Schema = "x4ever")]
    public sealed class Milestone
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int16 ID { get; set; }

        [Column(name: "application_key")] public string ApplicationKey { get; set; }

        [Column(name: "milestone_id")] public Int16 MilestoneId { get; set; }
        [Column(name: "language_code")] public string LanguageCode { get; set; }

        [Column(name: "milestone_title")] public String MilestoneTitle { get; set; }

        [Column(name: "milestone_subtitle")] public String MilestoneSubTitle { get; set; }

        [Column(name: "target_value")] public int TargetValue { get; set; }

        [Column(name: "achieved_message")] public String AchievedMessage { get; set; }
        [Column(name: "active")] public bool Active { get; set; }
    }
}