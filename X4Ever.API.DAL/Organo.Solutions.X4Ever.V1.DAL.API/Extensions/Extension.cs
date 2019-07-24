using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Extensions
{
    public static class Extension
    {
        public static string GetFileName(this string fileName)
        {
            var str = fileName?.Split('\\');
            return str[str.Length-1];
        }
    }
}