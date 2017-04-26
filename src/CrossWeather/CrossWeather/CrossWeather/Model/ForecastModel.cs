using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWeather.Model
{
    public class ForecastModel
    {
        public ForecastWeather weather { get; set; }
        public Common common { get; set; }
        public Result result { get; set; }
    }

    public class ForecastWeather
    {
        public Forecast6days[] forecast6days { get; set; }
    }

    public class Forecast6days
    {
        public Grid grid { get; set; }
        public ForecastSky sky { get; set; }
        public ForecastTemperature temperature { get; set; }
        public string timeRelease { get; set; }
        public Location location { get; set; }
    }

    public class ForecastSky
    {
        public string pmName2day { get; set; }
        public string pmCode2day { get; set; }
        public string amCode2day { get; set; }
        public string amName2day { get; set; }
        public string amCode3day { get; set; }
        public string amName3day { get; set; }
        public string pmCode3day { get; set; }
        public string pmName3day { get; set; }
        public string amCode4day { get; set; }
        public string amName4day { get; set; }
        public string pmCode4day { get; set; }
        public string pmName4day { get; set; }
        public string amCode5day { get; set; }
        public string amName5day { get; set; }
        public string pmCode5day { get; set; }
        public string pmName5day { get; set; }
        public string amCode6day { get; set; }
        public string amName6day { get; set; }
        public string pmCode6day { get; set; }
        public string pmName6day { get; set; }
        public string amCode7day { get; set; }
        public string amName7day { get; set; }
        public string pmCode7day { get; set; }
        public string pmName7day { get; set; }
        public string amCode8day { get; set; }
        public string amName8day { get; set; }
        public string pmCode8day { get; set; }
        public string pmName8day { get; set; }
        public string amCode9day { get; set; }
        public string amName9day { get; set; }
        public string pmCode9day { get; set; }
        public string pmName9day { get; set; }
        public string amCode10day { get; set; }
        public string amName10day { get; set; }
        public string pmCode10day { get; set; }
        public string pmName10day { get; set; }
    }

    public class ForecastTemperature
    {
        public string tmax2day { get; set; }
        public string tmax3day { get; set; }
        public string tmin2day { get; set; }
        public string tmin3day { get; set; }
        public string tmax4day { get; set; }
        public string tmax5day { get; set; }
        public string tmax6day { get; set; }
        public string tmax7day { get; set; }
        public string tmin4day { get; set; }
        public string tmin5day { get; set; }
        public string tmin6day { get; set; }
        public string tmin7day { get; set; }
        public string tmax8day { get; set; }
        public string tmax9day { get; set; }
        public string tmax10day { get; set; }
        public string tmin8day { get; set; }
        public string tmin9day { get; set; }
        public string tmin10day { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
    }

}
