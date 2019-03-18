using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public sealed class UserAuthentication : IBaseProperties
    {
        public UserAuthentication()
        {
            ID = 0;
            UserLogin = string.Empty;
            UserFirstName = string.Empty;
            UserLastName = string.Empty;
            UserEmail = string.Empty;
            UserRegistered = DateTime.Parse("1900-01-01");
            LanguageCode = string.Empty;
            WeightVolumeType = string.Empty;
            UserApplication = string.Empty;
            IsMetaExists = false;
            IsAddressExists = false;
            IsTrackerExists = false;
        }

        public Int64 ID { get; set; }
        public string UserLogin { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public DateTime UserRegistered { get; set; }
        public string LanguageCode { get; set; }
        public string WeightVolumeType { get; set; }
        public string UserApplication { get; set; }
        public string DisplayName => this.UserFirstName;
        public bool IsMetaExists { get; set; }
        public bool IsAddressExists { get; set; }
        public bool IsTrackerExists { get; set; }
    }
}
