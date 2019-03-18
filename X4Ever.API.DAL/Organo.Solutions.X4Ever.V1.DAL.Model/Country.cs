using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_country", Schema = "x4ever")]
    public class Country
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }
        [Column(name: "country_code")] public string CountryCode { get; set; }
        [Column(name: "country_name")] public string CountryName { get; set; }
        [Column(name: "country_flag")] public string CountryFlag { get; set; }
    }
}