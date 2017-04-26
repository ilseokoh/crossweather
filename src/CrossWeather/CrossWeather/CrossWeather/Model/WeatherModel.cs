using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWeather.Model
{
    public class WeatherModel
    {
        public Weather weather { get; set; }
        public Common common { get; set; }
        public WeatherResult result { get; set; }
    }

    public class Weather
    {
        public Hourly[] hourly { get; set; }
    }

    public class Hourly
    {
        public Grid grid { get; set; }
        public Wind wind { get; set; }
        public Precipitation precipitation { get; set; }
        public Sky sky { get; set; }
        public Temperature temperature { get; set; }
        public string humidity { get; set; }
        public string lightning { get; set; }
        public string timeRelease { get; set; }
    }

    public class Grid
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string village { get; set; }
    }

    public class Wind
    {
        public string wdir { get; set; }
        public string wspd { get; set; }
    }

    public class Precipitation
    {
        public string sinceOntime { get; set; }
        public string type { get; set; }
    }

    public class Sky
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Temperature
    {
        public string tc { get; set; }
        public string tmax { get; set; }
        public string tmin { get; set; }
    }

    public class Common
    {
        public string alertYn { get; set; }
        public string stormYn { get; set; }
    }

    public class WeatherResult
    {
        public int code { get; set; }
        public string requestUrl { get; set; }
        public string message { get; set; }
    }

}
