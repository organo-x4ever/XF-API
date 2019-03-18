using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "meal_plan_option_grid", Schema = "x4ever")]
    public sealed class MealPlanOptionGrid
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int16 ID { get; set; }

        [Column(name: "meal_plan_option_id")] public Int16 MealPlanOptionID { get; set; }

        [Column(name: "language_code")] public string LanguageCode { get; set; }
        [Column(name: "meal_option_volume")] public string MealOptionVolume { get; set; }

        [Column(name: "meal_option_volume_type")]
        public string MealOptionVolumeType { get; set; }

        [Column(name: "meal_option_shake_title")]
        public string MealOptionShakeTitle { get; set; }

        [Column(name: "meal_option_detail_photo")]
        public string MealOptionDetailPhoto { get; set; }

        [Column(name: "display_sequence")] public Int16 DisplaySequence { get; set; }

        [Column(name: "active")] public bool Active { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
        [Column(name: "modified_by")] public string ModifiedBy { get; set; }
    }
}