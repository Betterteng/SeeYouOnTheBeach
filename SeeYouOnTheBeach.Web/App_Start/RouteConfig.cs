using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SeeYouOnTheBeach.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("BeachDetail", "BeachDetail/{action}",
                new { controller = "BeachDetail", action = "Index", name = "" }
            );

            routes.MapRoute("Filter", "Filter/{action}",
                new { controller = "Filter", action = "Index", name = "" }
            );

            routes.MapRoute("Home", "home/{action}",
                new { controller = "Home", action = "Index", name = "" }
            );

            routes.MapRoute("Photos", "Photos/{action}",
    new { controller = "Photos", action = "Index", name = "" }
);

            /*
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            */

            routes.MapRoute("Error", "{*url}",
        new { controller = "Home", action = "Index" }
    );
        }
    }
}
