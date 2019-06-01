using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "MapPath",
              url: "display/{ip}/{port}/{time}",
              defaults: new { controller = "First", action = "viewMapPath" }
          );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "save",
              url: "save/{ip}/{port}/{time}/{seconds}/{fileName}",
              defaults: new { controller = "First", action = "Save" }
          );
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Display",
              url: "display/{stringToCheck}/{number}",
              defaults: new { controller = "First", action = "Display" }
          );

            routes.MapRoute(
             name: "Default",
             url: "{action}/{id}",
             defaults: new { controller = "First", action = "Welcome", id= UrlParameter.Optional }
         );
        }
    }
}
