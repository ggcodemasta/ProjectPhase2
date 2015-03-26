using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            config.MapHttpAttributeRoutes();

            //find jobtitle & city
            config.Routes.MapHttpRoute(
                name: "TwoValueApi",
                routeTemplate: "api/{controller}/{jobTitle}/{city}",
                defaults: new { jobTitle = RouteParameter.Optional, city = RouteParameter.Optional }
            );

            //find city THIS WORKS
            //config.Routes.MapHttpRoute(
            //    name: "OneValueApi",
            //    routeTemplate: "api/{controller}/{city}",
            //    defaults: new { city = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
