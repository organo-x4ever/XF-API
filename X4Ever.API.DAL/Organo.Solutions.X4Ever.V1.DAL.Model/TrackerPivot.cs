using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class TrackerPivot
    {
        public TrackerPivot()
        {
            UserId = 0;
            ShirtSize = string.Empty;
            CurrentWeight = string.Empty;
            CurrentWeightUI = string.Empty;
            WeightVolumeType = string.Empty;
            FrontImage = string.Empty;
            SideImage = string.Empty;
            AboutJourney = string.Empty;
            ModifyDate = DateTime.Now;
            RevisionNumber = string.Empty;

            IsDeleteAllowed = false;
            IsEditAllowed = false;
        }

        public Int64 UserId { get; set; }
        public string ShirtSize { get; set; }
        public string CurrentWeight { get; set; }
        public string CurrentWeightUI { get; set; }
        public string WeightVolumeType { get; set; }
        public string FrontImage { get; set; }
        public string SideImage { get; set; }
        public string AboutJourney { get; set; }
        public DateTime ModifyDate { get; set; }
        public string RevisionNumber { get; set; }
        public bool IsDeleteAllowed { get; set; }
        public bool IsEditAllowed { get; set; }
    }
}