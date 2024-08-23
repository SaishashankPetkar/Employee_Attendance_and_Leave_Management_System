using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebAppProject
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //1. We will specify media type formatting, as to what our application wants to support
            GlobalConfiguration.Configuration.Formatters.Remove
                (GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            //2. in order to avoid self refrencing loop of pk and fk
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
