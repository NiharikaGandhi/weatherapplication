using System.Collections.Generic;

namespace WeatherService.Gateway
{
    public class CitiesResponse
    {
        public TableName NewDataSet { get; set; }
    }

    public class TableName
    {
        public List<Details> Table = new List<Details>();
    }


    public class Details
    {
        public string Country { get; set; }
        public string City { get; set; }
    }

}