using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "application", Schema = "x4ever")]
    public class Application
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }

        [Column(name: "application_key")] public string ApplicationKey { get; set; }

        [Column(name: "application_name")] public string ApplicationName { get; set; }

        [Column(name: "active")] public bool Active { get; set; }
    }
}