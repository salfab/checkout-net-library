using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TestLibraryWebsite
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
           name: "DefaultApi",
           routeTemplate: "{controller}/{id}",
           defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
