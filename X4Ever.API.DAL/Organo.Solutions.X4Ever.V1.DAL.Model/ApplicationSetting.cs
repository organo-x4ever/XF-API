using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_application_setting", Schema = "x4ever")]
    public class ApplicationSetting
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public short ID { get; set; }

        [Column(name: "application_id")] public int ApplicationID { get; set; }
        [Column(name: "application_key")] public string ApplicationKey { get; set; }
        [Column(name: "language_code")] public string LanguageCode { get; set; }

        [Column(name: "community_facebook_url")]
        public string CommunityFacebookUrl { get; set; }

        [Column(name: "community_instagram_url")]
        public string CommunityInstagramUrl { get; set; }

        [Column(name: "ogx_file_url")] public string OGXFileUrl { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
    }
}