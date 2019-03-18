using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Helper
{
    public static class CommonConstants
    {
        public static string YES => "yes";
        public static string NO => "no";
        public static string SPACE => "\n";
        public static double TargetDateCalculation => 0.33;

        public static string EMAIL_VALIDATION_REGEX =>
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static string Message => "message";
        public static string DATE_FORMAT_MMM_d_yyyy => "{0:MMM d, yyyy}";
        public static double KG_LB_CONVERT_VALUE => 2.20462262185;
    }
}