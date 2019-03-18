using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "media_category", Schema = "x4ever")]
    public sealed class MediaCategory
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int16 ID { get; set; }

        [Column(name: "title")] public string Title { get; set; }

        [Column(name: "description")] public string Description { get; set; }
    }
}