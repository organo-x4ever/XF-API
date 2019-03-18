using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_province", Schema = "x4ever")]
    public class Province
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")] public Int32 ID { get; set; }
        [Column(name: "country_id")] public int CountryId { get; set; }
        [Column(name: "province_code")] public string ProvinceCode { get; set; }
        [Column(name: "province_name")] public string ProvinceName { get; set; }
    }
}