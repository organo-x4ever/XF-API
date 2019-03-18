using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "testimonial", Schema = "x4ever")]
    public sealed class Testimonial
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public int ID { get; set; }

        [Column(name: "person_name")] public string PersonName { get; set; }
        [Column(name: "person_photo")] public string PersonPhoto { get; set; }
        [Column(name: "video_url")] public string VideoUrl { get; set; }
        [Column(name: "short_description")] public string ShortDescription { get; set; }
        [Column(name: "long_description")] public string LongDescription { get; set; }
        [Column(name: "create_date")] public DateTime CreateDate { get; set; }
        [Column(name: "modified_date")] public DateTime ModifiedDate { get; set; }
        [Column(name: "created_by")] public Int64 CreatedBy { get; set; }
        [Column(name: "star_rating")] public decimal StarRating { get; set; }
        [Column(name: "display_sequence")] public Int16 DisplaySequence { get; set; }
        [Column(name: "active")] public bool Active { get; set; }
    }
}