using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class StatisticServices : IStatisticServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatisticServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Insert(ref ValidationErrors validationErrors, Statistic model)
        {
            model.CreateDate = DateTime.Now;
            _unitOfWork.StatisticRepository.Insert(model);
            return _unitOfWork.Commit();
        }
    }
}