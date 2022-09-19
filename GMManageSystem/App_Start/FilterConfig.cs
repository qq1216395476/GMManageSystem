using GMManageSystem.Common;
using System.Web;
using System.Web.Mvc;

namespace GMManageSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
