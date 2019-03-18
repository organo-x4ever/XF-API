using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "user_tracker_report_v1", Schema = "x4ever")]
    public class UserTrackerReportV1
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_id")] public Int64 UserID { get; set; }
        [Column(name: "user_first_name")] public string FirstName { get; set; }
        [Column(name: "user_last_name")] public string LastName { get; set; }
        [Column(name: "user_email")] public string EmailAddress { get; set; }
        [Column(name: "address")] public string Address { get; set; }
        [Column(name: "city")] public string City { get; set; }
        [Column(name: "state")] public string State { get; set; }
        [Column(name: "postal_code")] public string PostalCode { get; set; }
        [Column(name: "country")] public string Country { get; set; }
        [Column(name: "gender")] public string Gender { get; set; }
        [Column(name: "start_weight")] public string StartWeight { get; set; }
        [Column(name: "weight_to_lose")] public string WeightToLose { get; set; }
        [Column(name: "weekly_weight_lost")] public string WeeklyWeightLost { get; set; }
        [Column(name: "weight_goal_reached")] public string WeightGoalReached { get; set; }
        [Column(name: "tshirt_size")] public string TShirtSize { get; set; }
        [Column(name: "front_photo")] public string FrontPhoto { get; set; }
        [Column(name: "side_photo")] public string SidePhoto { get; set; }
        [Column(name: "testimonials")] public string Testimonials { get; set; }
        [Column(name: "date_created")] public DateTime CreateDate { get; set; }
        [Column(name: "application_name")] public string ApplicationName { get; set; }
        [Column(name: "weight_volume_type")] public string WeightVolumeType { get; set; }
    }
}