using System.Web;
using System.Web.Mvc;

namespace webstide_bằng_mô_hình_MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
