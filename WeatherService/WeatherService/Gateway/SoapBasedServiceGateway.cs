using System;
using System.Xml;
using Newtonsoft.Json;
using WeatherService.WeatherWebService;

namespace WeatherService.Gateway
{
    public interface ISoapBasedServiceGateway
    {
        CitiesResponse GetCities(string country);
        WeatherResult GetWeather(string country, string city);
    }

    public class SoapBasedServiceGateway : ISoapBasedServiceGateway
    {
        private readonly GlobalWeatherSoap _client;

        public SoapBasedServiceGateway(GlobalWeatherSoap client)
        {
            _client = client;
        }

        public CitiesResponse GetCities(string country)
        {
            try
            {
                var xmlResponse = _client.GetCitiesByCountry(country);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlResponse);
                var json = JsonConvert.SerializeXmlNode(xmlDoc);
                var response = JsonConvert.DeserializeObject<CitiesResponse>(json);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public WeatherResult GetWeather(string country, string city)
        {
            try
            {
                var response = _client.GetWeather(city, country);
                if (string.IsNullOrEmpty(response) || response.ToLower() == "data not found")
                {
                    return null;
                }

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response);
                var json = JsonConvert.SerializeXmlNode(xmlDoc);
                return JsonConvert.DeserializeObject<WeatherResult>(json);
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
    }
}