using System;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public sealed class UserMetaGlobal
    {
        public Int64 ID { get; set; }

        public Int64 UserID { get; set; }

        public string MetaKey { get; set; }

        public string MetaValue { get; set; }

        public DateTime ModifyDate { get; set; }

        public string MetaLabel { get; set; }

        public string MetaDescription { get; set; }

        public string MetaType { get; set; }
    }
}