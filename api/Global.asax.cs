using Newtonsoft.Json;
using System.Globalization;
using System.Web;
using System.Web.Http;

namespace Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                Culture = CultureInfo.GetCultureInfo("pt-br"),
                DateFormatString = "dd/MM/yyyy"
            };

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
