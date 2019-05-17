using Organo.Solutions.X4Ever.V1.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public  interface IStatisticServices
    {
        bool Insert(ref ValidationErrors validationErrors, Statistic model);
    }
}