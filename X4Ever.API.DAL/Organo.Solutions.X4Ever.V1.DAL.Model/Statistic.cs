using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_user_statistic", Schema = "x4ever")]
    public  class Statistic
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")] public long ID { get; set; }
        [Column(name: "user_id")] public long UserID { get; set; }

        [Column(name: "statistic_date")] public string StatisticDate { get; set; }
        [Column(name: "statistic_page")] public string StatisticPage { get; set; }
        [Column(name: "statistic_message")] public string StatisticMessage { get; set; }

        [Column(name: "entry_date")] public DateTime CreateDate { get; set; }
    }
}