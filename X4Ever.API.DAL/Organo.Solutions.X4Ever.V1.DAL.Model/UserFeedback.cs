
namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public sealed class UserFeedback
    {
        public UserFeedback()
        {
            ID = string.Empty;
            FullName = string.Empty;
            Email = string.Empty;
            Experience = string.Empty;
            Comments = string.Empty;
            AttachedFileName = string.Empty;
            AllowAccess = string.Empty;
            Token = string.Empty;
            Date = string.Empty;
        }

        public string ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Experience { get; set; }
        public string Comments { get; set; }
        public string AttachedFileName { get; set; }
        public string AllowAccess { get; set; }
        public string Token { get; set; }
        public string Date { get; set; }
    }
}