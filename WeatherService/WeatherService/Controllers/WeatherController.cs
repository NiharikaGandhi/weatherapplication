using System;
using System.Web.Http;
using WeatherService.Service;

namespace WeatherService.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeatherController : ApiController
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [Route("{country}/cities")]
        [HttpGet]
        public IHttpActionResult Cities(string country)
        {
            try
            {
                if (string.IsNullOrEmpty(country))
                {
                    return BadRequest();
                }

                var response = _weatherService.GetCities(country);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get the weather based on country and city from http://www.webservicex.net/globalweather.asmx?WSDL
        /// If weather not found, get the weather from http://api.openweathermap.org/data/2.5/
        /// </summary>
        /// <param name="country"></param>
        /// <param name="city"></param>
        /// <returns>Weather Details</returns>
        [HttpGet]
        [Route("{country}/{city}")]
        public IHttpActionResult GetWeather(string country, string city)
        {
            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(city))
            {
                return BadRequest();
            }

            var weather = _weatherService.GetWeather(country, city);
            if (weather == null)
            {
                return NotFound();
            }
            return Ok(weather);
        }
    }
}
