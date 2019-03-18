using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_usertoken", Schema = "x4ever")]
    public sealed class UserToken : IBase64Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }

        [Column(name: "auth_token")] public string AuthToken { get; set; }

        [Column(name: "issued_on")] public DateTime IssuedOn { get; set; }

        [Column(name: "expires_on")] public DateTime ExpiresOn { get; set; }

        [Column(name: "last_active_on")] public DateTime LastActiveOn { get; set; }
    }
}