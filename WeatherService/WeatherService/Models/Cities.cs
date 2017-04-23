using System.Collections.Generic;

namespace WeatherService.Models
{
    public class City
    {
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }

    public class CityList
    {
        public List<City> Cities;
    }
}