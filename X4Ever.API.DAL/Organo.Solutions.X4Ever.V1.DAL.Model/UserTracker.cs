using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_tracker", Schema = "x4ever")]
    public sealed class UserTracker : IBase64Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_id")]
        public Int64 UserID { get; set; }

        [Column(name: "attr_name")]
        public String AttributeName { get; set; }

        [Column(name: "attr_value")]
        public String AttributeValue { get; set; }

        [Column(name: "attr_label")]
        public String AttributeLabel { get; set; }

        [Column(name: "modify_date")]
        public DateTime ModifyDate { get; set; }

        [Column(name: "media_link")]
        public String MediaLink { get; set; }

        [Column(name: "rev_number")]
        public String RevisionNumber { get; set; }
    }
}