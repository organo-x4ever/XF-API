using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_weight_volume", Schema = "x4ever")]
    public sealed class WeightVolume
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }

        [Column(name: "application_id")] public int ApplicationID { get; set; }
        [Column(name: "volume_code")] public string VolumeCode { get; set; }

        [Column(name: "volume_name")] public string VolumeName { get; set; }
    }
}