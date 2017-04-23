using System;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;

namespace WeatherService.Gateway
{
    public interface IHttpBasedServiceGateway
    {
        WeatherHttpResponse GetWeather(string city);
    }

    public class HttpBasedServiceGateway : IHttpBasedServiceGateway
    {
        public WeatherHttpResponse GetWeather(string city)
        {
            try
            {
                var httpClient = new HttpClient {BaseAddress = new Uri(ConfigurationManager.AppSettings["baseurl"])};
                var url = string.Format("weather?q={0}&appid={1}", city, ConfigurationManager.AppSettings["appkey"]);
                var response = httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var resp = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<WeatherHttpResponse>(resp);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
          
        }
    }
}