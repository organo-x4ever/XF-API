using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_menu", Schema = "x4ever")]
    public class Menu
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int16 ID { get; set; }

        [Column(name: "menu_title")] public string MenuTitle { get; set; }
        [Column(name: "menu_type")] public string MenuType { get; set; }
        [Column(name: "menu_type_code")] public short MenuTypeCode { get; set; }
        [Column(name: "menu_icon")] public string MenuIcon { get; set; }
        [Column(name: "menu_icon_visible")] public bool MenuIconVisible { get; set; }
        [Column(name: "menu_active")] public bool MenuActive { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
    }
}