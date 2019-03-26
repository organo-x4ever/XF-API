using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Helper
{
    public class Converter : IConverter
    {
        private double ConversionValue => CommonConstants.KG_LB_CONVERT_VALUE_1; //CommonConstants.KG_LB_CONVERT_VALUE;

        public double ConvertKilogramToPound(double kilograms)
        {
            return Math.Round(kilograms / ConversionValue, 0);
        }

        public string ConvertKilogramToPound(string kilograms)
        {
            double.TryParse(kilograms, out double kg);
            return Math.Round((kg / ConversionValue) + .10, 0).ToString(CultureInfo.CurrentCulture);
        }

        public double ConvertPoundToKilogram(double pounds)
        {
            return Math.Round(pounds * ConversionValue, 0);
        }

        public string ConvertPoundToKilogram(string pounds)
        {
            double.TryParse(pounds, out double lb);
            return Math.Round(lb * ConversionValue, 0).ToString(CultureInfo.CurrentCulture);
        }
    }

    public interface IConverter
    {
        double ConvertKilogramToPound(double kilograms);
        string ConvertKilogramToPound(string kilograms);
        double ConvertPoundToKilogram(double pounds);
        string ConvertPoundToKilogram(string pounds);
    }
}