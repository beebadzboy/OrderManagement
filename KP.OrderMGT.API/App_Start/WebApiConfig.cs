using KP.OrderMGT.API.Authen;
using System.Web.Http;

namespace KP.OrderMGT.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //To enable Basic Authentication for entire web application
            config.Filters.Add(new BasicAuthenticationAttribute());

            // Web API configuration and services

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
