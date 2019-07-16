using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public abstract class BaseServices : IBaseServices
    {
        public IUnitOfWork _unitOfWork => new UnitOfWork();
    }
}