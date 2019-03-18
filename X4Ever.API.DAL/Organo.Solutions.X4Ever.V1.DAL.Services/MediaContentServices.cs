using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class MediaContentServices : IMediaContentServices
    {
        private IUnitOfWork _unitOfWork;

        public MediaContentServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<MediaContent> Get()
        {
            return this._unitOfWork.MediaContentRepository.GetAll();
        }

        public async Task<IEnumerable<MediaContent>> GetAsync()
        {
            return await this._unitOfWork.MediaContentRepository.GetAllAsync();
        }

        public IEnumerable<MediaContentDetail> GetDetail()
        {
            var list = (from content in this._unitOfWork.MediaContentRepository.GetMany(c => c.Active)
                join category in _unitOfWork.MediaCategoryRepository.GetAll() on content.MediaCategoryID equals
                    category.ID
                join type in _unitOfWork.MediaTypeRepository.GetAll() on content.MediaTypeID equals type.ID
                select new MediaContentDetail()
                {
                    MediaCategoryID = category.ID,
                    ID = content.ID,
                    MediaTypeID = type.ID,
                    CategoryDescription = category.Description,
                    CategoryTitle = category.Title,
                    CreateDate = content.CreateDate,
                    MediaDescription = content.MediaDescription,
                    MediaTypeShortTitle = type.ShortTitle,
                    MediaTypeTitle = type.Title,
                    MediaUrl = content.MediaUrl,
                    SetsAndRepeats = content.SetsAndRepeats,
                    MediaTitle = content.MediaTitle,
                    DisplaySequence = content.DisplaySequence,
                    PreviewImageUrl = content.PreviewImageUrl,
                    WorkoutLevel = content.WorkoutLevel,
                    WorkoutWeek = content.WorkoutWeek,
                    WorkoutDay = content.WorkoutDay,
                    Active = content.Active
                });

            return list;
        }

        public async Task<IEnumerable<MediaContentDetail>> GetDetailAsync()
        {
            var list = (from content in await this._unitOfWork.MediaContentRepository.GetManyAsync(c => c.Active)
                join category in await _unitOfWork.MediaCategoryRepository.GetAllAsync() on content.MediaCategoryID
                    equals
                    category.ID
                join type in await _unitOfWork.MediaTypeRepository.GetAllAsync() on content.MediaTypeID equals type.ID
                select new MediaContentDetail()
                {
                    MediaCategoryID = category.ID,
                    ID = content.ID,
                    MediaTypeID = type.ID,
                    CategoryDescription = category.Description,
                    CategoryTitle = category.Title,
                    CreateDate = content.CreateDate,
                    MediaDescription = content.MediaDescription,
                    MediaTypeShortTitle = type.ShortTitle,
                    MediaTypeTitle = type.Title,
                    MediaUrl = content.MediaUrl,
                    SetsAndRepeats = content.SetsAndRepeats,
                    MediaTitle = content.MediaTitle,
                    DisplaySequence = content.DisplaySequence,
                    PreviewImageUrl = content.PreviewImageUrl,
                    WorkoutLevel = content.WorkoutLevel,
                    WorkoutWeek = content.WorkoutWeek,
                    WorkoutDay = content.WorkoutDay,
                    Active = content.Active
                });

            return list;
        }
    }
}