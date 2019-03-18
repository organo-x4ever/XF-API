using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public interface ICountryProvince
    {
        int ID { get; set; }
        string CountryCode { get; set; }
        string CountryName { get; set; }
        string CountryFlag { get; set; }
        IList<Province> Provinces { get; set; }
    }
}