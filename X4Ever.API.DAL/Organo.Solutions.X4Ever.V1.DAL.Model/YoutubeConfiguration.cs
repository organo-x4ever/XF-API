using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    [Table(name: "x4_youtube_config", Schema = "x4ever")]
    public class YoutubeConfiguration
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column(name: "id")]
        public short ID { get; set; }

        [Column(name: "application_id")] public int ApplicationID { get; set; }
        [Column(name: "application_key")] public string ApplicationKey { get; set; }
        [Column(name: "user_api_key")] public string UserApiKey { get; set; }

        [Column(name: "video_channel_api_url")]
        public string VideoChannelApiUrl { get; set; }

        [Column(name: "video_playlist_api_url")]
        public string VideoPlaylistApiUrl { get; set; }

        [Column(name: "video_detail_api_url")] public string VideoDetailApiUrl { get; set; }
        [Column(name: "video_watch_api_url")] public string VideoWatchApiUrl { get; set; }
        [Column(name: "show_description")] public bool ShowDescription { get; set; }
        [Column(name: "show_view")] public bool ShowView { get; set; }
        [Column(name: "show_like")] public bool ShowLike { get; set; }
        [Column(name: "show_comment")] public bool ShowComment { get; set; }
        [Column(name: "show_favourite")] public bool ShowFavourite { get; set; }
        [Column(name: "show_dislike")] public bool ShowDislike { get; set; }
        [Column(name: "modify_date")] public DateTime ModifyDate { get; set; }
        public ICollection<YoutubeVideoCollection> YoutubeVideoCollection { get; set; }
    }
}