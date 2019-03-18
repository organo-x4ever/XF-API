using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_user_setting", Schema = "x4ever")]
    public class UserSetting : IBase32Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public int ID { get; set; }

        [Column(name: "user_id")] public long UserID { get; set; }

        [Column(name: "language_code")] public string LanguageCode { get; set; }

        [Column(name: "language_detail")] public string LanguageDetail { get; set; }

        [Column(name: "weight_volume_type")] public string WeightVolumeType { get; set; }

        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
    }
}