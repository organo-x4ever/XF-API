
using System;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class UserTrackerMap
    {
        public double CurrentWeight { get; set; }
        public double WeightLost { get; set; }
        public string ShirtSize { get; set; }
        public string FrontImage { get; set; }
        public string SideImage { get; set; }
        public string AboutYourJourney { get; set; }
        public string RevisionNumber { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}