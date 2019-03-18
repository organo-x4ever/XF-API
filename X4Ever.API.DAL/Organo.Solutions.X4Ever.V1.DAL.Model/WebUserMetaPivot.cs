using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{

    [Table(name: "x4_webusermeta_pivot", Schema = "x4ever")]
    public class WebUserMetaPivot
    {
        public WebUserMetaPivot()
        {
            ID = 0;
            user_id = 0;
            user_first_name = string.Empty;
            user_last_name = string.Empty;
            user_email = string.Empty;
            date_created = DateTime.Now;
            application_name = string.Empty;
            address = string.Empty;
            city = string.Empty;
            state = string.Empty;
            postal_code = string.Empty;
            country = string.Empty;
            gender = string.Empty;
            start_weight = string.Empty;
            weight_to_lose = string.Empty;
            weight_volume_type = string.Empty;
            weight_to_lose = string.Empty;
            total_weight_lost = string.Empty;
            weight_goal_reached = string.Empty;
            tshirt_size = string.Empty;
            front_photo = string.Empty;
            side_photo = string.Empty;
            testimonials = string.Empty;
            tracker_start_date = DateTime.Now;
            tracker_last_date = DateTime.Now;
        }

        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_id")] public Int64 user_id { get; set; }
        [Column(name: "user_first_name")] public string user_first_name { get; set; }
        [Column(name: "user_last_name")] public string user_last_name { get; set; }
        [Column(name: "user_email")] public string user_email { get; set; }
        [Column(name: "date_created")] public DateTime date_created { get; set; }
        [Column(name: "application_name")] public string application_name { get; set; }
        [Column(name: "address")] public string address { get; set; }
        [Column(name: "city")] public string city { get; set; }
        [Column(name: "state")] public string state { get; set; }
        [Column(name: "postal_code")] public string postal_code { get; set; }
        [Column(name: "country")] public string country { get; set; }
        [Column(name: "gender")] public string gender { get; set; }
        [Column(name: "start_weight")] public string start_weight { get; set; }
        [Column(name: "weight_to_lose")] public string weight_to_lose { get; set; }
        [Column(name: "weight_volume_type")] public string weight_volume_type { get; set; }
        [Column(name: "weekly_weight_lost")] public string weekly_weight_lost { get; set; }
        [Column(name: "total_weight_lost")] public string total_weight_lost { get; set; }
        [Column(name: "weight_goal_reached")] public string weight_goal_reached { get; set; }
        [Column(name: "tshirt_size")] public string tshirt_size { get; set; }
        [Column(name: "front_photo")] public string front_photo { get; set; }
        [Column(name: "side_photo")] public string side_photo { get; set; }
        [Column(name: "testimonials")] public string testimonials { get; set; }
        [Column(name: "tracker_start_date")] public DateTime tracker_start_date { get; set; }
        [Column(name: "tracker_last_date")] public DateTime tracker_last_date { get; set; }
    }
}