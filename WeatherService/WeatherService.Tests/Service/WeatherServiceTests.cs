using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using WeatherService.Gateway;
using WeatherService.Models;

namespace WeatherService.Tests.Service
{
    public class WeatherServiceTests
    {
        [UnderTest] public WeatherService.Service.WeatherService _sut;
        [Fake] public ISoapBasedServiceGateway SoapBasedServiceGateway;
        [Fake] public IHttpBasedServiceGateway HttpBasedServiceGateway;

        [SetUp]
        public void SetUp()
        {
            Fake.InitializeFixture(this);
        }

        [Test]
        public void GivenValidCountryName_WhenCities_WeGetCities()
        {
            var newDataSet = new TableName {Table = new List<Details>()};
            newDataSet.Table.Add(new Details{ City = "abc",Country = "xyz"});
            var mockData = new CitiesResponse {NewDataSet = newDataSet};
            A.CallTo(() => SoapBasedServiceGateway.GetCities(A<string>.Ignored))
                .Returns(mockData);
            
            var response = _sut.GetCities("Australia");

            response.Cities.Count.Should().Be(1);
        }

        [Test]
        public void GivenInValidCountryName_WhenCities_WeGetCities()
        {
            var newDataSet = new TableName { Table = new List<Details>() };
            var mockData = new CitiesResponse { NewDataSet = newDataSet };

            A.CallTo(() => SoapBasedServiceGateway.GetCities(A<string>.Ignored))
                .Returns(mockData);

            var response = _sut.GetCities("Australia");

            response.Cities.Count.Should().Be(0);
        }

        [Test]
        public void GivenValidCountryAndCityName_WhenSoapBasedCallDoesNotReturnWeather_HttpCallMustHappened()
        {
            A.CallTo(() => SoapBasedServiceGateway.GetWeather(A<string>.Ignored, A<string>.Ignored)).Returns(null);
            
            _sut.GetWeather("Australia", "Sydney");

            A.CallTo(() => HttpBasedServiceGateway.GetWeather(A<string>.Ignored)).MustHaveHappened();
        }
    }
}
