using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface ITestimonialServices
    {
        IEnumerable<Testimonial> Get(bool active);
        Task<IEnumerable<Testimonial>> GetAsync(bool active);

        IEnumerable<Testimonial> Get();
        Task<IEnumerable<Testimonial>> GetAsync();
    }
}