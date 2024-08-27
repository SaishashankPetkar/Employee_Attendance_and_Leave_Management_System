using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ALMSystem2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
<<<<<<< HEAD
=======
            );

            routes.MapRoute(
                name: "Employee",
                url: "Employee/{action}/{id}",
                defaults: new { controller = "Employee", action = "Login", id = UrlParameter.Optional }
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
            );

            routes.MapRoute(
                name: "Employee",
                url: "Employee/{action}/{id}",
                defaults: new { controller = "Employee", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Admin",
                url: "Admin/{action}/{id}",
                defaults: new { controller = "Admin", action = "Login", id = UrlParameter.Optional }
             );
        }
    }
}