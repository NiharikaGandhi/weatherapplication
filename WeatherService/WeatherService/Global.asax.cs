using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WeatherService.Gateway;
using WeatherService.Service;
using WeatherService.WeatherWebService;

namespace WeatherService
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterType<Service.WeatherService>().As<IWeatherService>();
            builder.RegisterType<GlobalWeatherSoapClient>().As<GlobalWeatherSoap>().SingleInstance();
            builder.RegisterType<SoapBasedServiceGateway>().As<ISoapBasedServiceGateway>();
            builder.RegisterType<HttpBasedServiceGateway>().As<IHttpBasedServiceGateway>().SingleInstance();
            var container = builder.Build();
            var config = GlobalConfiguration.Configuration;
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                }

            };

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
