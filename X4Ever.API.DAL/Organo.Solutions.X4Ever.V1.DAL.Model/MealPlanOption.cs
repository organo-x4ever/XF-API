using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "meal_plan_option", Schema = "x4ever")]
    public sealed class MealPlanOption
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int16 ID { get; set; }

        [Column(name: "meal_plan_id")] public Int16 MealPlanID { get; set; }

        [Column(name: "language_code")] public string LanguageCode { get; set; }
        [Column(name: "meal_option_title")] public string MealOptionTitle { get; set; }
        [Column(name: "meal_option_photo")] public string MealOptionPhoto { get; set; }

        [Column(name: "meal_option_subtitle")] public string MealOptionSubtitle { get; set; }
        [Column(name: "meal_option_desc")] public string MealOptionDesc { get; set; }

        [Column(name: "display_sequence")] public Int16 DisplaySequence { get; set; }

        [Column(name: "active")] public bool Active { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
        [Column(name: "modified_by")] public string ModifiedBy { get; set; }

        public List<MealPlanOptionGrid> MealPlanOptionGridDetails { get; set; }
        public List<MealPlanOptionList> MealPlanOptionListDetails { get; set; }
    }
}