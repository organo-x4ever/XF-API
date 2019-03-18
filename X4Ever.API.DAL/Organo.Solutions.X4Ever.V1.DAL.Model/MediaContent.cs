using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "media_content", Schema = "x4ever")]
    public sealed class MediaContent
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public int ID { get; set; }

        [Column(name: "media_category_id")] public Int16 MediaCategoryID { get; set; }
        [Column(name: "media_type_id")] public Int16 MediaTypeID { get; set; }
        [Column(name: "media_title")] public string MediaTitle { get; set; }
        [Column(name: "media_url")] public string MediaUrl { get; set; }
        [Column(name: "sets_and_repeats")] public string SetsAndRepeats { get; set; }
        [Column(name: "preview_image_url")] public string PreviewImageUrl { get; set; }
        [Column(name: "display_sequence")] public Int16 DisplaySequence { get; set; }
        [Column(name: "create_date")] public DateTime CreateDate { get; set; }
        [Column(name: "media_description")] public string MediaDescription { get; set; }
        [Column(name: "workout_level")] public string WorkoutLevel { get; set; }
        [Column(name: "workout_week")] public string WorkoutWeek { get; set; }
        [Column(name: "workout_day")] public string WorkoutDay { get; set; }
        [Column(name: "active")] public bool Active { get; set; }
    }
}