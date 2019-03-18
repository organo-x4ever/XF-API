using AttributeRouting.Web.Http;
using Organo.Solutions.X4Ever.V1.DAL.API.Security.ActionFilters;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/testimonials")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestimonialsController : ApiControllerOverride
    {
        private readonly ITestimonialServices _testimonialServices;

        public TestimonialsController(TestimonialServices testimonialServices)
        {
            _testimonialServices = testimonialServices;
        }

        // GET: api/testimonials
        [GET("get")]
        [Route("get")]
        public async Task<IEnumerable<Testimonial>> Get()
        {
            return await _testimonialServices.GetAsync();
        }

        // GET: api/testimonials
        [GET("get")]
        [Route("get")]
        public async Task<IEnumerable<Testimonial>> Get(bool active)
        {
            return await _testimonialServices.GetAsync(active);
        }
    }
}