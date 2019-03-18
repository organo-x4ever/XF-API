using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_application_country", Schema = "x4ever")]
    public class ApplicationCountry
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }

        [Column(name: "application_id")] public int ApplicationID { get; set; }
        [Column(name: "application_key")] public string ApplicationKey { get; set; }
        [Column(name: "country_id")] public int CountryID { get; set; }
    }
}