using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using WeatherService.Gateway;
using WeatherService.WeatherWebService;

namespace WeatherService.Tests.Gateway
{
    public class SoapbasedServiceGatewayTests
    {
        [UnderTest]
        public SoapBasedServiceGateway _sut;
        [Fake]
        public GlobalWeatherSoap GlobalWeatherSoap;
     
        [SetUp]
        public void SetUp()
        {
            Fake.InitializeFixture(this);
        }

        [Test]
        public void GivenValidCountry_WhenGetCitites_WeGetResponse()
        {
            A.CallTo(() => GlobalWeatherSoap.GetCitiesByCountry(A<string>.Ignored))
                .Returns(@"<NewDataSet>
                              <Table>
                                <Country>Australia</Country>
                                <City>Archerfield Aerodrome</City>
                              </Table>
                              <Table>
                                <Country>Australia</Country>
                                <City>Amberley Aerodrome</City>
                              </Table>
                          </NewDataSet>");

            var expected = new TableName { Table = new List<Details>() };
            expected.Table.Add(new Details { City = "Archerfield Aerodrome", Country = "Australia" });
            expected.Table.Add(new Details { City = "Amberley Aerodrome", Country = "Australia" });

            var response = _sut.GetCities("Australia");

            response.NewDataSet.Table.Count.ShouldBeEquivalentTo(expected.Table.Count);
        }

        [Test]
        public void GivenInValidCountry_WhenGetCitites_WeGetResponse()
        {
            A.CallTo(() => GlobalWeatherSoap.GetCitiesByCountry(A<string>.Ignored))
                .Returns("<NewDataSet />");

           var response = _sut.GetCities("Australia");

            response.NewDataSet.Should().BeNull();
        }

        [Test]
        public void GivenValidCountryAndCity_WhenGetWeather_WeGetResponse()
        {
            A.CallTo(() => GlobalWeatherSoap.GetWeather(A<string>.Ignored, A<string>.Ignored))
                .Returns(@"<CurrentWeather><Location>abc</Location><Time>1212121</Time><Wind>abc</Wind><Visibility>abc</Visibility><SkyConditions>abc</SkyConditions>
                           <Temperature>abc</Temperature><DewPoint>abc</DewPoint><RelativeHumidity>abc</RelativeHumidity><Pressure>abc</Pressure></CurrentWeather>");

            var response = _sut.GetWeather("Australia","Sydney");

            response.CurrentWeather.Location.Should().Be("abc");
        }

        [Test]
        public void GivenInValidCountryOrCity_WhenGetWeather_WeDontGetResponse()
        {
            A.CallTo(() => GlobalWeatherSoap.GetWeather(A<string>.Ignored, A<string>.Ignored))
                .Returns("Data Not Found");

            var response = _sut.GetWeather("abc","def");

            response.Should().BeNull();
        }
    }
}
