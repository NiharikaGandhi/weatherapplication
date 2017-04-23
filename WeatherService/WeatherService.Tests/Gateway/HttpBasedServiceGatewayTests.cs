using System.Configuration;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using WeatherService.Gateway;

namespace WeatherService.Tests.Gateway
{
    public class HttpBasedServiceGatewayTests
    {
        [UnderTest]
        public HttpBasedServiceGateway _sut;
      
        [SetUp]
        public void SetUp()
        {
            Fake.InitializeFixture(this);
            ConfigurationManager.AppSettings["baseurl"] = "http://api.openweathermap.org/data/2.5/";
            ConfigurationManager.AppSettings["appkey"] = "601fb6f0f54c5cac1692931361349e03";
        }

        [Test]
        public void GivenValidCityName_WhenGetWeather_WeGetWeatherDetails()
        {
            var response = _sut.GetWeather("Archerfield Aerodrom");

            response.Should().NotBeNull();

        }

        [Test]
        public void GivenInvalidCityName_WhenGetWeather_WeDoNotGetWeatherDetails()
        {
            var response = _sut.GetWeather("--");

            response.Should().BeNull();

        }
    }
}
