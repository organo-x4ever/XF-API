using System.Web.Mvc;

namespace Organo.Solutions.X4Ever.V1.DAL.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}