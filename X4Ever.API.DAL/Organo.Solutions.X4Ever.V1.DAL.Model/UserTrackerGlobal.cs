using System;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public sealed class UserTrackerGlobal : IBase64Properties
    {
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public String AttributeName { get; set; }
        public String AttributeValue { get; set; }
        public String AttributeLabel { get; set; }
        public DateTime ModifyDate { get; set; }
        public String MediaLink { get; set; }
        public String RevisionNumber { get; set; }
    }
}