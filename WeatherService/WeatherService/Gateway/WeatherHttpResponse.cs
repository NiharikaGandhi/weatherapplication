using System.Collections.Generic;

namespace WeatherService.Gateway
{
    public class WeatherHttpResponse
    {
        public Coordinate Coord { get;set; }
        public List<Weather> Weather { get;set; }
        public string Base { get;set; }
        public Main Main { get;set; }
        public double Visibility { get; set; }
        public Wind  Wind { get;set; }
        public Clouds Clouds { get;set; }
        public long dt { get;set; }
        public Sys Sys { get;set; }
        public long Id { get; set; }
        public string Name { get;set; }
        public long Cod { get;set; }

    }

    public class Coordinate
    {
        public decimal Lon { get;set; }
        public decimal Lat { get;set; }
    }

    public class Weather
    {
        public long Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Main
    {
        public decimal Temp { get;set; }
        public decimal Pressure { get; set; }
        public decimal Humidity { get; set; }
        public decimal TempMin { get;set; }
    }

    public class Wind
    {
        public decimal Speed { get; set; }
        public decimal Deg { get;set; }
    }

    public class Clouds
    {
        public int All { get; set; }
    }
    public class Sys
    {
        public int Type {get; set; }
        public decimal Message { get; set; }
        public string Country { get; set; }
        public long Sunrise { get;set; }
        public long Sunset { get;set; }
    }
}

