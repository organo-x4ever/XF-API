using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "user_milestone", Schema = "x4ever")]
    public sealed class UserMilestone : IBase32Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public int ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }

        [Column(name: "milestone_id")] public Int16 MilestoneID { get; set; }

        [Column(name: "achieve_date")] public DateTime AchieveDate { get; set; }

        [Column(name: "milestone_percentage_id")]
        public Int16 MilestonePercentageId { get; set; }

        [Column(name: "is_percent")] public bool IsPercentage { get; set; }
    }
}