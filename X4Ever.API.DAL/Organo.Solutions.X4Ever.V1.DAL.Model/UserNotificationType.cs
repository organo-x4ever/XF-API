using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_user_notification_type", Schema = "x4ever")]
    public class UserNotificationType
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public short ID { get; set; }

        [Column(name: "type_name")] public string TypeName { get; set; }
        [Column(name: "type_code")] public string TypeCode { get; set; }
        [Column(name: "description")] public string Description { get; set; }
        [Column(name: "is_push")] public bool IsPush { get; set; }
        [Column(name: "is_email")] public bool IsEmail { get; set; }
    }
}