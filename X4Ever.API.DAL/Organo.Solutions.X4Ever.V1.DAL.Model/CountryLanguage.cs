using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_country_language", Schema = "x4ever")]
    public class CountryLanguage
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }

        [Column(name: "application_id")] public int ApplicationID { get; set; }
        [Column(name: "country_id")] public int CountryID { get; set; }
        [Column(name: "language_id")] public int LanguageID { get; set; }
    }
}