using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
   public class UserNotificationSettingViewModel
    {
        public long ID { get; set; }
        public long UserID { get; set; }

        public bool IsVisibleGeneralMessage { get; set; }
        public bool IsGeneralMessage { get; set; }
        public bool IsVisibleIntimation { get; set; }
        public bool Intimation { get; set; }
        public bool IsPromotional { get; set; }
        public bool IsVisiblePromotional { get; set; }
        public bool IsSpecialOffer { get; set; }
        public bool IsVisibleSpecialOffer { get; set; }
        public bool IsVersionUpdate { get; set; }
        public bool IsVisibleVersionUpdate { get; set; }
        public bool IsWeightSubmitReminder { get; set; }       
        public bool IsVisibleWeightSubmitReminder { get; set; }       

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}