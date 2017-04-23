using System;
using System.Collections.Generic;
using System.Globalization;
using WeatherService.Gateway;
using WeatherService.Models;

namespace WeatherService.Service
{
    public interface IWeatherService
    {
        CityList GetCities(string country);
        WeatherResponse GetWeather(string country, string city);
    }

    public class WeatherService : IWeatherService
    {
        private readonly ISoapBasedServiceGateway _soapBased;
        private readonly IHttpBasedServiceGateway _httpBased;

        public WeatherService(ISoapBasedServiceGateway soapBased, IHttpBasedServiceGateway httpBased)
        {
            _soapBased = soapBased;
            _httpBased = httpBased;
        }


        public CityList GetCities(string country)
        {
            var response = _soapBased.GetCities(country);
            if (response != null && response.NewDataSet != null)
            {
                var cities = new CityList { Cities = new List<City>() };

                foreach (var table in response.NewDataSet.Table)
                {
                    var city = new City
                    {
                        CityName = table.City,
                        CountryName = table.Country
                    };

                    cities.Cities.Add(city);
                }
                return cities;
            }
            return null;
        }

        
        public WeatherResponse GetWeather(string country, string city)
        {
            var response = _soapBased.GetWeather(country, city);
            if (response != null && response.CurrentWeather != null)
            {
                return GetWeather(response.CurrentWeather);
            }

            var result = _httpBased.GetWeather(city);
            return GetWeather(result);
        }

        private static WeatherResponse GetWeather(CurrentWeather response)
        {
            
            return new WeatherResponse
            {
                DewPoint = response.DewPoint,
                Location = response.Location,
                Pressure = response.Pressure,
                RelativeHumidity = response.RelativeHumidity,
                SkyConditions = response.SkyConditions,
                Temperature = response.Temperature,
                Time = response.Time,
                Visibility = response.Visibility,
                Wind = response.Wind
            };
        }

        private static WeatherResponse GetWeather(WeatherHttpResponse resp)
        {
            if (resp != null)
            {
                return new WeatherResponse
                {
                    Location = resp.Name ?? string.Empty,
                    DewPoint = resp.Main !=null ? resp.Main.Humidity.ToString(CultureInfo.InvariantCulture) : string.Empty,
                    Pressure = resp.Main != null ? resp.Main.Pressure.ToString(CultureInfo.InvariantCulture) : string.Empty,
                    RelativeHumidity = resp.Main != null ? resp.Main.Humidity.ToString(CultureInfo.InvariantCulture) : string.Empty,
                    SkyConditions =resp.Clouds !=null ? resp.Clouds.All.ToString(CultureInfo.InvariantCulture) : string.Empty,
                    Temperature = resp.Main != null ? resp.Main.Temp.ToString(CultureInfo.InvariantCulture) : string.Empty,
                    Time = resp.dt.ToString(),
                    Visibility = resp.Visibility.ToString(CultureInfo.InvariantCulture),
                    Wind = resp.Wind !=null?  resp.Wind.Speed.ToString(CultureInfo.InvariantCulture) : string.Empty
                };
            }

            return null;
        }
    }
}

