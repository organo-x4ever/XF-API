using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_webusermeta_view_pivot", Schema = "x4ever")]
    public class WebUserTrackerViewPivot
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }
        [Column(name: "user_id")] public string user_id { get; set; }
        [Column(name: "user_login")] public string user_login { get; set; }
        [Column(name: "user_pass")] public string user_pass { get; set; }
        [Column(name: "user_nicename")] public string user_nicename { get; set; }
        [Column(name: "user_email")] public string user_email { get; set; }
        [Column(name: "user_url")] public string user_url { get; set; }
        [Column(name: "user_registered")] public DateTime user_registered { get; set; }
        [Column(name: "user_activation_key")] public string user_activation_key { get; set; }
        [Column(name: "user_first_name")] public string user_first_name { get; set; }
        [Column(name: "userid")] public string userid { get; set; }
        [Column(name: "user_last_name")] public string user_last_name { get; set; }
        [Column(name: "user_address")] public string user_address { get; set; }
        [Column(name: "have_you_purchased")] public string have_you_purchased { get; set; }
        [Column(name: "age")] public string age { get; set; }
        [Column(name: "gender")] public string gender { get; set; }
        [Column(name: "weekly_weight_lose")] public string weekly_weight_lose { get; set; }
        [Column(name: "weight_lose_goal")] public string weight_lose_goal { get; set; }
        [Column(name: "why_you_join")] public string why_you_join { get; set; }
        [Column(name: "address")] public string address { get; set; }
        [Column(name: "country")] public string country { get; set; }
        [Column(name: "city")] public string city { get; set; }
        [Column(name: "province")] public string province { get; set; }
        [Column(name: "postal_code")] public string postal_code { get; set; }
        [Column(name: "weeks")] public string weeks { get; set; }
        [Column(name: "weight_goal")] public string weight_goal { get; set; }
        [Column(name: "status")] public string status { get; set; }
        [Column(name: "lbs_lost")] public string lbs_lost { get; set; }
        [Column(name: "user_status")] public int user_status { get; set; }
        [Column(name: "display_name")] public string display_name { get; set; }
        [Column(name: "item_user_name")] public string item_user_name { get; set; }
        [Column(name: "update_date")] public DateTime update_date { get; set; }
        [Column(name: "date_created")] public DateTime date_created { get; set; }
    }
}