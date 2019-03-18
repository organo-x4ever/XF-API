using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class MetaPivot
    {
        public Int64 UserId { get; set; }
        public string Address { get; set; }
        public string Age { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string WeightLossGoal { get; set; }
        public string WeightLossGoalUI { get; set; }
        public string WeightVolumeType { get; set; }
        public string Gender { get; set; }
        public string ProfilePhoto { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}