using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class UserTrackerReportV1Map
    {
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public string StartWeight { get; set; }
        public string WeightToLose { get; set; }
        public string WeeklyWeightLost { get; set; }
        public string WeightGoalReached { get; set; }
        public string TShirtSize { get; set; }
        public string FrontPhoto { get; set; }
        public string SidePhoto { get; set; }
        public string Testimonials { get; set; }
        public List<UserTrackerMap> UserTrackers { get; set; }
        public DateTime CreateDate { get; set; }
        public string ApplicationName { get; set; }
        public string WeightVolumeType { get; set; }
    }
}