using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class MediaTypeServices : IMediaTypeServices
    {
        private IUnitOfWork _unitOfWork;
        private Helper.IHelper _helper;

        public MediaTypeServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _helper = new Helper.Helper();
        }

        public IEnumerable<MediaType> Get()
        {
            return _unitOfWork.MediaTypeRepository.GetAll();
        }

        public async Task<IEnumerable<MediaType>> GetAsync()
        {
            return await _unitOfWork.MediaTypeRepository.GetAllAsync();
        }
    }
}