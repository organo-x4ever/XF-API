using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_user_password_request", Schema = "x4ever")]
    public sealed class UserPasswordRequest : IBase32Properties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public int ID { get; set; }

        [Column(name: "user_id")]
        public Int64 UserID { get; set; }

        [Column(name: "user_login")]
        public string UserLogin { get; set; }

        [Column(name: "user_email")]
        public string UserEmail { get; set; }

        [Column(name: "request_date")]
        public DateTime RequestDate { get; set; }

        [Column(name: "request_code")]
        public string RequestCode { get; set; }

        [Column(name: "request_code_expires_on")]
        public DateTime RequestCodeExpiresOn { get; set; }
    }
}