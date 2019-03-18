using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class ApplicationServices : IApplicationServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Application> Get()
        {
            return _unitOfWork.ApplicationRepository.GetMany(a => a.Active);
        }

        public async Task<IEnumerable<Application>> GetAsync()
        {
            return await _unitOfWork.ApplicationRepository.GetManyAsync(a => a.Active);
        }
    }
}