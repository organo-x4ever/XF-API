using System;
using System.Collections.Generic;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public sealed class UserGlobal : IBaseProperties
    {
        public UserGlobal()
        {
            ID = 0;
            UserKey = string.Empty;
            UserLogin = string.Empty;
            UserPassword = string.Empty;
            UserFirstName = string.Empty;
            UserLastName = string.Empty;
            UserEmail = string.Empty;
            UserType = string.Empty;
            IsWeightSubmissionRequired = false;
            UserRegistered = DateTime.Parse("1900-01-01");
            UserActivationKey = string.Empty;
            LanguageCode = string.Empty;
            WeightVolumeType = string.Empty;
            UserStatus = string.Empty;
            ProfileImage = string.Empty;
            UserApplication = string.Empty;
            Achievement = new MilestonePercentage();
            UserMetas = new List<UserMetaGlobal>();
            UserTrackers = new List<UserTrackerGlobal>();
        }

        public Int64 ID { get; set; }
        public string UserKey { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserType { get; set; }
        public bool IsWeightSubmissionRequired { get; set; }
        public DateTime UserRegistered { get; set; }
        public string UserActivationKey { get; set; }
        public string LanguageCode { get; set; }
        public string WeightVolumeType { get; set; }
        public string UserStatus { get; set; }
        public string ProfileImage { get; set; }
        public string UserApplication { get; set; }
        public string DisplayName => this.UserFirstName;
        public MilestonePercentage Achievement { get; set; }
        public ICollection<UserMetaGlobal> UserMetas { get; set; }
        public ICollection<UserTrackerGlobal> UserTrackers { get; set; }
    }
}