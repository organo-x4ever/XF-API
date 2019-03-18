using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_language", Schema = "x4ever")]
    public class Language
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }

        [Column(name: "language_code")] public string LanguageCode { get; set; }
        [Column(name: "language_Name")] public string LanguageName { get; set; }

        [Column(name: "language_name_english")]
        public string LanguageNameEnglish { get; set; }
    }
}