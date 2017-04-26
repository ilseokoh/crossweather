using CrossWeather.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using static Newtonsoft.Json.JsonConvert;
using System.Net.Http;

namespace CrossWeather.Services
{
    public class WeatherService
    {
        const string WeatherCoordinatesUri = "http://apis.skplanetx.com/weather/current/hourly?version=1&lat={0}&lon={1}";
        const string ForecastUri = "http://apis.skplanetx.com/weather/forecast/6days?version=1&lat={0}&lon={1}";


        //public async Task<WeatherModel> GetWeather(double latitude, double longitude)
        //{
        //    using (var client = new HttpClient())
        //    {

        //    }
        //}
    }
}
