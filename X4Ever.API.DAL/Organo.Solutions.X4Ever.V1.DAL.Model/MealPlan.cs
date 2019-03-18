using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "meal_plan", Schema = "x4ever")]
    public sealed class MealPlan
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int16 ID { get; set; }

        [Column(name: "language_code")] public string LanguageCode { get; set; }
        [Column(name: "meal_title")] public string MealTitle { get; set; }
        [Column(name: "meal_plan_photo")] public string MealPlanPhoto { get; set; }
        [Column(name: "display_sequence")] public Int16 DisplaySequence { get; set; }
        [Column(name: "active")] public bool Active { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
        [Column(name: "modified_by")] public string ModifiedBy { get; set; }
        [Column(name: "view_type")] public string ViewType { get; set; }
        public List<MealPlanOption> MealPlanOptionDetails { get; set; }
    }
}