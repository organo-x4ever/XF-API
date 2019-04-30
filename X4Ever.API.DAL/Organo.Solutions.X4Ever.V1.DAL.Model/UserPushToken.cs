using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_userpushtoken", Schema = "x4ever")]
    public class UserPushToken : IBase64Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public long ID { get; set; }

        [Column(name: "user_id")] public long UserID { get; set; }
        [Column(name: "device_token")] public string DeviceToken { get; set; }
        [Column(name: "device_platform")] public string DevicePlatform { get; set; }
        [Column(name: "device_identity")] public string DeviceIdentity { get; set; }
        [Column(name: "device_idiom")] public string DeviceIdiom { get; set; }
        [Column(name: "device_application")] public string DeviceApplication { get; set; }
        [Column(name: "issued_on")] public DateTime IssuedOn { get; set; }
        [Column(name: "user_key")] public string UserKey { get; set; }
    }
}