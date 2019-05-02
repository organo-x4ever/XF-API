using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Models
{
    public class NotificationSetting
    {
        public NotificationSetting()
        {
            IsGeneralMessage = false;
            Intimation = false;
            IsPromotional = false;
            IsSpecialOffer = false;
            IsVersionUpdate = false;
            IsWeightSubmitReminder = false;
        }
        
        /// <summary>
        /// Get Notification Allowed by User or not
        /// </summary>
        public bool IsGeneralMessage { get; set; }
        
        /// <summary>
        /// Get Notification Allowed by User or not
        /// </summary>
        public bool Intimation { get; set; }
        
        /// <summary>
        /// Get Notification Allowed by User or not
        /// </summary>
        public bool IsPromotional { get; set; }
        
        /// <summary>
        /// Get Notification Allowed by User or not
        /// </summary>
        public bool IsSpecialOffer { get; set; }
        
        /// <summary>
        /// Get Notification Allowed by User or not
        /// </summary>
        public bool IsVersionUpdate { get; set; }
        
        /// <summary>
        /// Get Notification Allowed by User or not
        /// </summary>
        public bool IsWeightSubmitReminder { get; set; }
    }
}