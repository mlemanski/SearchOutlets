using System.Web.Http;

namespace SearchOutlets
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // display all profiles or a single profile, without searching
            config.Routes.MapHttpRoute(
                name: "SearchOutletsApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
