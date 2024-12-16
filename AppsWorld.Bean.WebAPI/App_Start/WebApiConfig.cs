using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using AppsWorld.Bean.WebAPI.Controllers.Attributes;

namespace AppsWorld.Bean.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            // config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = true };
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //config.Routes.MapHttpRoute(
            //    name: "DemoApi",
            //    routeTemplate: "api/v2/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsAuthorize"]))
            {
                config.Filters.Add(new AuthorizeAttribute());
            }
            //config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new LoggingFilterAttribute());
        }
    }
}
