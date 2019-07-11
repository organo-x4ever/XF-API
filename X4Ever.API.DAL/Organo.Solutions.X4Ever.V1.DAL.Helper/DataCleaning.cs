using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Helper
{
   public static class DataCleaning
    {
        public static string Clean(this string data)
        {
            try {
                    data=data.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">","");
                    data=data.Replace("<string xmlns=http://schemas.microsoft.com/2003/10/Serialization/>","");
                    data=data.Replace("</string>","");
                }
                catch {
                    //
                }
            return data;
        }
    }
}