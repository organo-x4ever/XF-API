using System.Collections.Generic;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class UserMilestoneExtended
    {
        public IEnumerable<UserMilestone> UserMilestones { get; set; }
        public IEnumerable<Milestone> Milestones { get; set; }
        public IEnumerable<MilestonePercentage> MilestonePercentages { get; set; }
    }
}