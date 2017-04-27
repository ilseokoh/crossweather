using CrossWeather.Helpers;
using CrossWeather.Model;
using CrossWeather.Services;
using MvvmHelpers;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CrossWeather.ViewModel
{
    public class WeatherViewModel : BaseViewModel
    {
        WeatherService WeatherService { get; } = new WeatherService();

        /// <summary>
        /// 날씨 요약정보
        /// </summary>
        WeatherSummary summary; 
        public WeatherSummary WeatherSummary
        {
            get { return summary; }
            set { summary = value; OnPropertyChanged(); }
        }

        ForecastModel forecast;
        /// <summary>
        /// 예보 정보 from 날씨 API 그대로
        /// </summary>
        public ForecastModel Forecast
        {
            get { return forecast; }
            set { forecast = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 현재 날씨의 도시
        /// </summary>
        string city = Settings.City;
        public string City
        {
            get { return city; }
            set
            {
                SetProperty(ref city, value);
                Settings.City = value;
            }
        }
        
        string country = Settings.Country;
        /// <summary>
        /// 현재 날씨의 시,군,구
        /// </summary>
        public string Country
        {
            get { return country; }
            set
            {
                SetProperty(ref country, value);
                Settings.Country = value;
            }
        }

        string village = Settings.Village;
        /// <summary>
        /// 현재 날씨의 읍,면,동
        /// </summary>
        public string Village
        {
            get { return village; }
            set
            {
                SetProperty(ref village, value);
                Settings.Village = value;
            }
        }

        ICommand getWeather;
        public ICommand GetWeatherCommand =>
            getWeather ??
            (getWeather = new Command(async () => await ExcuteGetWeatherCommand()));

        private async Task ExcuteGetWeatherCommand()
        {
            if (IsBusy) return;

            IsBusy = true; 
            try
            {
                var hasPermission = await CheckPermissions();
                WeatherModel weather;
                if (hasPermission)
                {
                    var gps = await CrossGeolocator.Current.GetPositionAsync(10000);
                    weather = await WeatherService.GetWeather(gps.Latitude, gps.Longitude);
                    SetWeatherSummary(weather);
                    forecast = await WeatherService.GetForecast(gps.Latitude, gps.Longitude);
                }
                else
                {
                    // weather = await WeatherService.GetWeather();
                    // forecast = await WeatherService.GetForecast();
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// API에서 가져온 정보를 필요한 것만 뽑아서 WeatherSummary에 넣는다. 
        /// </summary>
        /// <param name="result">API에서 전달받은 정보</param>
        private void SetWeatherSummary(WeatherModel result)
        {
            var info = result.weather.hourly[0];
            if (info == null) return;

            city = info.grid.city;
            country = info.grid.county;
            village = info.grid.village;

            WeatherSummary summary = new WeatherSummary
            {
                Description = info.sky.name,
                humidity = info.humidity + "%",
                IconUrl = info.sky.code,
                Location = city + " " + country,
                ReleaseTime = info.timeRelease,
                Temperature = info.temperature.tc + "℃"
            };

            this.WeatherSummary = summary;
        }

        private async Task<bool> CheckPermissions()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    var title = "Location Permission";
                    var question = "To get your current city the location permission is required. Please go into Settings and turn on Location for the app.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return false;
                }
                request = true;
            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (newStatus.ContainsKey(Permission.Location) && newStatus[Permission.Location] != PermissionStatus.Granted)
                {
                    var title = "Location Permission";
                    var question = "To get your current city the location permission is required.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
