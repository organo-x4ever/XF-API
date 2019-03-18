using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "media_type", Schema = "x4ever")]
    public sealed class MediaType
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int16 ID { get; set; }

        [Column(name: "title")] public string Title { get; set; }

        [Column(name: "short_title")] public string ShortTitle { get; set; }

        [Column(name: "create_date")] public DateTime CreateDate { get; set; }
    }
}