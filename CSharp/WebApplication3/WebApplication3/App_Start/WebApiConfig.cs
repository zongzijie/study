using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication3
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Default", 
                routeTemplate:"api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
        }
    }
}