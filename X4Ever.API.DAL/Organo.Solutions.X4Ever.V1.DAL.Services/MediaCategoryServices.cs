using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class MediaCategoryServices : IMediaCategoryServices
    {
        private IUnitOfWork _unitOfWork;

        public MediaCategoryServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<MediaCategory> Get()
        {
            return this._unitOfWork.MediaCategoryRepository.GetAll();
        }

        public async Task<IEnumerable<MediaCategory>> GetAsync()
        {
            return await this._unitOfWork.MediaCategoryRepository.GetAllAsync();
        }
    }
}