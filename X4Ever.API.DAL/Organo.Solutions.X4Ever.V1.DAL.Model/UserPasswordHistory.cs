using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_user_password_history", Schema = "x4ever")]
    public sealed class UserPasswordHistory : IBase32Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public int ID { get; set; }

        [Column(name: "user_id")]
        public Int64 UserID { get; set; }

        [Column(name: "user_pass")]
        public String UserPassword { get; set; }

        [Column(name: "change_date")]
        public DateTime ChangeDate { get; set; }

        [Column(name: "expires_date")]
        public DateTime ExpireDate { get; set; }
    }
}