using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_application_menu", Schema = "x4ever")]
    public class ApplicationMenu
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int32 ID { get; set; }

        [Column(name: "application_id")] public int ApplicationID { get; set; }
        [Column(name: "application_key")] public string ApplicationKey { get; set; }
        [Column(name: "menu_id")] public short MenuID { get; set; }
        [Column(name: "platform_excluded")] public string PlatformExcluded { get; set; }
    }
}