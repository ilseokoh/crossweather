using CrossWeather.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using static Newtonsoft.Json.JsonConvert;

namespace CrossWeather.Services
{
    public class WeatherService
    {
        const string WeatherCoordinatesUri = "http://apis.skplanetx.com/weather/current/hourly?version=1&lat={0}&lon={1}";
        const string WeatherCityUri = "http://apis.skplanetx.com/weather/current/hourly?version=1&city={0}&county={1}&village={2}";
        const string ForecastCoordinatesUri = "http://apis.skplanetx.com/weather/forecast/6days?version=1&lat={0}&lon={1}";
        const string ForecastCityUri = "http://apis.skplanetx.com/weather/forecast/6days?version=1&city={0}&county={1}&village={2}";


        public async Task<WeatherModel> GetWeather(double latitude, double longitude)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCoordinatesUri, latitude, longitude);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("appKey", WeatherPlanetServiceKey.ServiceAppKey);
                client.DefaultRequestHeaders.Add("x-skpop-userId", WeatherPlanetServiceKey.ServiceUserId);

                var json = await client.GetStringAsync(url);
                if (string.IsNullOrWhiteSpace(json)) return null;

                return DeserializeObject<WeatherModel>(json);
            }
        }

        public async Task<WeatherModel> GetWeather(string city, string county, string village)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCityUri, city, county, village);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("appKey", WeatherPlanetServiceKey.ServiceAppKey);
                client.DefaultRequestHeaders.Add("x-skpop-userId", WeatherPlanetServiceKey.ServiceUserId);

                var json = await client.GetStringAsync(url);
                if (string.IsNullOrWhiteSpace(json)) return null;

                return DeserializeObject<WeatherModel>(json);
            }
        }

        public async Task<ForecastModel> GetForecast(double latitude, double longitude)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(ForecastCoordinatesUri, latitude, longitude);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("appKey", WeatherPlanetServiceKey.ServiceAppKey);
                client.DefaultRequestHeaders.Add("x-skpop-userId", WeatherPlanetServiceKey.ServiceUserId);

                var json = await client.GetStringAsync(url);
                if (string.IsNullOrWhiteSpace(json)) return null;

                return DeserializeObject<ForecastModel>(json);
            }
        }

        public async Task<ForecastModel> GetForecast(string city, string county, string village)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(ForecastCityUri, city, county, village);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("appKey", WeatherPlanetServiceKey.ServiceAppKey);
                client.DefaultRequestHeaders.Add("x-skpop-userId", WeatherPlanetServiceKey.ServiceUserId);

                var json = await client.GetStringAsync(url);
                if (string.IsNullOrWhiteSpace(json)) return null;

                return DeserializeObject<ForecastModel>(json);
            }
        }
    }
}
