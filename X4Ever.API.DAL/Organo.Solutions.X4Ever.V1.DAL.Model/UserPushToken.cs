using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_userpushtoken", Schema = "x4ever")]
    public class UserPushToken : IBase32Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }
        [Column(name: "device_token")] public string DeviceToken { get; set; }
        [Column(name: "device_platform")] public string DevicePlatform { get; set; }
        [Column(name: "device_identity")] public string DeviceIdentity { get; set; }
        [Column(name: "device_idiom")] public string DeviceIdiom { get; set; }
        [Column(name: "device_application")] public string DeviceApplication { get; set; }
        [Column(name: "issued_on")] public DateTime IssuedOn { get; set; }
    }
}