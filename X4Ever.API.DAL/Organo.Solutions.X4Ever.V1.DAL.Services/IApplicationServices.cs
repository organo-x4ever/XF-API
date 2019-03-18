using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IApplicationServices
    {
        IEnumerable<Application> Get();
        Task<IEnumerable<Application>> GetAsync();
    }
}