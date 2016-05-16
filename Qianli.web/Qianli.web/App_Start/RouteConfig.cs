using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Qianli.web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.RouteExistingFiles = false;           
            routes.MapRoute(
                 "html", // Route name
                "{action}.html", // URL with parameters
               new { controller = "hg", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               new[] {"Qianli.web.Controllers" }//默认命名空间
            );
            routes.MapRoute(
                name: "hgoods",
                url: "hg/hgoods.aspx",
                defaults: new { controller = "hg", action = "index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "v", action = "i", id = UrlParameter.Optional }
            );
        }
    }
}
