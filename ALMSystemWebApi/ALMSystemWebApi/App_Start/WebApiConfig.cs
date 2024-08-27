using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
=======
using System.Web.Http.Cors;
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
using System.Web.Http;

namespace ALMSystemWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            GlobalConfiguration.Configuration.Formatters.Remove
                (GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

<<<<<<< HEAD
=======
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
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
