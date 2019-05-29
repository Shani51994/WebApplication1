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
                name: "Display",
                url: "Display/{ip}/{port}",
                defaults: new { controller = "First", action = "Index"}
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "MapPath",
              url: "Display/{ip}/{port}/{time}",
              defaults: new { controller = "First", action = "viewMapPath"}
          );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "save",
              url: "save/{ip}/{port}/{time}/{seconds}/{fileName}",
              defaults: new { controller = "First", action = "Save" }
          );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "PathByFile",
              url: "display1/{fileName}/{time}",
              defaults: new { controller = "First", action = "ViewFilePath" }
          );

            routes.MapRoute(
             name: "Default",
             url: "{action}/{id}",
             defaults: new { controller = "First", action = "maayan", id= UrlParameter.Optional }
         );
        }
    }
}
