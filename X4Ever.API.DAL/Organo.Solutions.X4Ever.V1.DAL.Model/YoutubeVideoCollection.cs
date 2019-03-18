using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_youtube_video_collection", Schema = "x4ever")]
    public class YoutubeVideoCollection
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public short ID { get; set; }
        [Column(name: "youtube_config_id")] public short YoutubeConfigurationID { get; set; }

        [Column(name: "video_collection_api_key")]
        public string VideoCollectionApiKey { get; set; }

        [Column(name: "video_collection_type")]
        public string VideoCollectionType { get; set; }

        [Column(name: "video_collection_active")]
        public bool Active { get; set; }

        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
    }
}