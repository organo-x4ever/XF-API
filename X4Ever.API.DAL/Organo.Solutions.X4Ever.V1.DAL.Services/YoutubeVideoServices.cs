using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class YoutubeVideoServices : IYoutubeVideoServices
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        public YoutubeConfiguration Get(string application)
        {
            return (from c in _unitOfWork.YoutubeConfigurationRepository.GetMany(c => c.ApplicationKey == application)
                select new YoutubeConfiguration()
                {
                    ModifyDate = c.ModifyDate,
                    ID = c.ID,
                    ApplicationID = c.ApplicationID,
                    ApplicationKey = c.ApplicationKey,
                    UserApiKey = c.UserApiKey,
                    VideoChannelApiUrl = c.VideoChannelApiUrl,
                    ShowDislike = c.ShowDislike,
                    ShowDescription = c.ShowDescription,
                    ShowView = c.ShowView,
                    VideoPlaylistApiUrl = c.VideoPlaylistApiUrl,
                    ShowLike = c.ShowLike,
                    ShowFavourite = c.ShowFavourite,
                    ShowComment = c.ShowComment,
                    VideoWatchApiUrl = c.VideoWatchApiUrl,
                    VideoDetailApiUrl = c.VideoDetailApiUrl,
                    YoutubeVideoCollection = _unitOfWork.YoutubeVideoCollectionRepository
                        .GetMany(v => v.YoutubeConfigurationID == c.ID && v.Active).ToList()
                }).FirstOrDefault();
        }

        public async Task<YoutubeConfiguration> GetAsync(string application)
        {
            return (from c in await _unitOfWork.YoutubeConfigurationRepository.GetManyAsync(c =>
                    c.ApplicationKey == application)
                select new YoutubeConfiguration()
                {
                    ModifyDate = c.ModifyDate,
                    ID = c.ID,
                    ApplicationID = c.ApplicationID,
                    ApplicationKey = c.ApplicationKey,
                    UserApiKey = c.UserApiKey,
                    VideoChannelApiUrl = c.VideoChannelApiUrl,
                    ShowDislike = c.ShowDislike,
                    ShowDescription = c.ShowDescription,
                    ShowView = c.ShowView,
                    VideoPlaylistApiUrl = c.VideoPlaylistApiUrl,
                    ShowLike = c.ShowLike,
                    ShowFavourite = c.ShowFavourite,
                    ShowComment = c.ShowComment,
                    VideoWatchApiUrl = c.VideoWatchApiUrl,
                    VideoDetailApiUrl = c.VideoDetailApiUrl,
                    YoutubeVideoCollection = _unitOfWork.YoutubeVideoCollectionRepository
                        .GetMany(v => v.YoutubeConfigurationID == c.ID && v.Active).ToList()
                }).FirstOrDefault();
        }
    }
}