using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public sealed class TestimonialServices : ITestimonialServices
    {
        private IUnitOfWork _unitOfWork;

        public TestimonialServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Testimonial> Get()
        {
            return this._unitOfWork.TestimonialRepository.GetAll();
        }

        public async Task<IEnumerable<Testimonial>> GetAsync()
        {
            return await this._unitOfWork.TestimonialRepository.GetAllAsync();
        }

        public IEnumerable<Testimonial> Get(bool active)
        {
            return this._unitOfWork.TestimonialRepository.GetMany(t => t.Active == active);
        }

        public async Task<IEnumerable<Testimonial>> GetAsync(bool active)
        {
            return await this._unitOfWork.TestimonialRepository.GetManyAsync(t => t.Active == active);
        }

    }
}