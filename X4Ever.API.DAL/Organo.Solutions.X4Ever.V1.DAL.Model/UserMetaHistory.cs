using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_usermeta_history", Schema = "x4ever")]
    public sealed class UserMetaHistory : IBase32Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }

        [Column(name: "meta_key")] public string MetaKey { get; set; }

        [Column(name: "meta_value")] public string MetaValue { get; set; }

        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }

        [Column(name: "meta_label")] public string MetaLabel { get; set; }

        [Column(name: "meta_description")] public string MetaDescription { get; set; }

        [Column(name: "meta_type")] public string MetaType { get; set; }
    }
}