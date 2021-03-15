using System.Web;
using System.Web.Mvc;
using Autoveille.App_Start;

namespace Autoveille.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}