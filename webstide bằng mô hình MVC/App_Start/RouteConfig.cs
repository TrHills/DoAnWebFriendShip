using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace webstide_bằng_mô_hình_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Products", action = "ProductList", id = UrlParameter.Optional }
            );

            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Images/{*pathInfo}");

        }
    }
}
