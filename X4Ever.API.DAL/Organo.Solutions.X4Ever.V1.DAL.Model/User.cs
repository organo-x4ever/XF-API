using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_user", Schema = "x4ever")]
    public sealed class User : IBaseProperties
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_key")] public String UserKey { get; set; }

        [Column(name: "user_login")] public String UserLogin { get; set; }

        [Column(name: "user_pass")] public String UserPassword { get; set; }

        [Column(name: "user_first_name")] public String UserFirstName { get; set; }

        [Column(name: "user_last_name")] public String UserLastName { get; set; }

        [Column(name: "user_email")] public string UserEmail { get; set; }

        [Column(name: "user_type")] public String UserType { get; set; }

        [Column(name: "date_created")] public DateTime UserRegistered { get; set; }

        [Column(name: "user_activation_key")] public string UserActivationKey { get; set; }

        [Column(name: "user_status")] public string UserStatus { get; set; }

        [Column(name: "user_application")] public string UserApplication { get; set; }
    }
}