using System.Web.Http;

namespace Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Lessons",
                routeTemplate: "api/courses/{courseId}/lessons",
                defaults: new { controller = "Lesson" }
            );
        }
    }
}
