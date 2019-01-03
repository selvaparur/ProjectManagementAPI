
using ProjectManager.Services.MessageHandlers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ProjectManager.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            
            config.MessageHandlers.Add(new RequestResponseMessageHandler());
        }
    }
}
