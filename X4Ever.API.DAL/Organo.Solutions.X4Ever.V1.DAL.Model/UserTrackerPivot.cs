using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_tracker_pivot", Schema = "x4ever")]
    public class UserTrackerPivot
    {
        public UserTrackerPivot()
        {
            ID = 0;
            user_id = 0;
            ShirtSize = string.Empty;
            CurrentWeight = string.Empty;
            FrontImage = string.Empty;
            SideImage = string.Empty;
            AboutJourney = string.Empty;
            ModifyDate = DateTime.Now;
            rev_number = string.Empty;
        }

        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public Int64 ID { get; set; }

        [Column(name: "user_id")] public Int64 user_id { get; set; }
        [Column(name: "shirtsize")] public string ShirtSize { get; set; }
        [Column(name: "currentweight")] public string CurrentWeight { get; set; }
        [Column(name: "frontimage")] public string FrontImage { get; set; }
        [Column(name: "sideimage")] public string SideImage { get; set; }
        [Column(name: "aboutjourney")] public string AboutJourney { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
        [Column(name: "rev_number")] public string rev_number { get; set; }
    }
}