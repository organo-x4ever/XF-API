using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_news", Schema = "x4ever")]
    public class News
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int16 ID { get; set; }

        [Column(name: "news_header")] public string Header { get; set; }
        [Column(name: "news_body")] public string Body { get; set; }
        [Column(name: "news_post_date")] public DateTime PostDate { get; set; }
        [Column(name: "news_posted_by")] public string PostedBy { get; set; }
        [Column(name: "news_image")] public string NewsImage { get; set; }
        [Column(name: "news_image_position")] public string NewsImagePosition { get; set; }
        [Column(name: "active")] public bool Active { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
        [Column(name: "modified_by")] public string ModifiedBy { get; set; }
        [Column(name: "language_code")] public string LanguageCode { get; set; }
        [Column(name: "application_id")] public int ApplicationId { get; set; }
    }
}