
namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public class TrackerPhotoSkip
    {
        public long user_id { get; set; }
        public string user_email { get; set; }
        public bool skip_photo { get; set; }
        public string user_token { get; set; }
        public string modify_date { get; set; }
    }
}