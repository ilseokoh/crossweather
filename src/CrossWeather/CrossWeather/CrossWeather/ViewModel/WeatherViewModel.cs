using CrossWeather.Helpers;
using CrossWeather.Model;
using CrossWeather.Services;
using MvvmHelpers;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Plugin.TextToSpeech;

namespace CrossWeather.ViewModel
{
    public class WeatherViewModel : BaseViewModel
    {
        WeatherService WeatherService { get; } = new WeatherService();
        WeatherLocationManager weatherLocationManager { get; } = new WeatherLocationManager();
    

        /// <summary>
        /// 날씨 요약정보
        /// </summary>
        WeatherSummary summary; 
        public WeatherSummary WeatherSummary
        {
            get { return summary; }
            set { summary = value; OnPropertyChanged(); }
        }

        List<ForecastSummary> forecasts;
        /// <summary>
        /// 예보 정보 from 날씨 API 그대로
        /// </summary>
        public List<ForecastSummary> Forecasts
        {
            get { return forecasts; }
            set { forecasts = value; OnPropertyChanged(); }
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

        private IEnumerable<WeatherLocation> locations;

        public IEnumerable<WeatherLocation> Locations
        {
            get { return locations; }
            set { locations = value; OnPropertyChanged(); }
        }


        ICommand saveLocation;
        public ICommand SaveLocationCommand =>
            saveLocation ?? (saveLocation = new Command(async () => await ExcuteSaveLocationCommand()));

        private async Task ExcuteSaveLocationCommand()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                var savedlocation = await weatherLocationManager.AddLocation(City + " " + Country + " " + Village);
                if (savedlocation == null)
                {
                    await Application.Current.MainPage.DisplayAlert("위치저장 실패", "위치저장에 실패했습니다.", "확인");

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("위치저장", "위치를 저장했습니다.", "확인");
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }
            ICommand getLocation;
        public ICommand GetLocationCommand =>
            getLocation ?? (getLocation = new Command(async () => await ExcuteGetLocationCommand()));

        private async Task ExcuteGetLocationCommand()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                var locations = await weatherLocationManager.GetLocation();
                if (locations?.FirstOrDefault() == null) return;
                var picked = await Application.Current.MainPage.DisplayActionSheet("위치 선택", "취소", null, locations.Select(x => x.Name).ToArray());
                if (picked == "취소") return;

                var saperatedloc = picked.Split(' ');
                City = saperatedloc[0];
                Country = saperatedloc[1];
                Village = saperatedloc[2];

                WeatherModel weather;
                ForecastModel forecast;
                weather = await WeatherService.GetWeather(City, Country, Village);
                SetWeatherSummary(weather);
                forecast = await WeatherService.GetForecast(City, Country, Village);
                SetForecastSummary(forecast);

                //var speak = string.Format("현재 {0}의 날씨는 {1}입니다. 온도는 {2}이며, 습도는 {3}입니다.",
                //                                WeatherSummary.Location,
                //                                WeatherSummary.Description,
                //                                WeatherSummary.Temperature,
                //                                WeatherSummary.Humidity);
                //CrossTextToSpeech.Current.Speak(speak);
            }
            finally
            {
                IsBusy = false;
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
                    var forecast = await WeatherService.GetForecast(gps.Latitude, gps.Longitude);
                    SetForecastSummary(forecast);

                    //var speak = string.Format("현재 {0}의 날씨는 {1}입니다. 온도는 {2}이며, 습도는 {3}입니다.",
                    //                            WeatherSummary.Location,
                    //                            WeatherSummary.Description,
                    //                            WeatherSummary.Temperature,
                    //                            WeatherSummary.Humidity);
                    //CrossTextToSpeech.Current.Speak(speak);
                }
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
                Humidity = info.humidity,
                IconUrl = info.sky.code,
                Location = city + " " + country,
                ReleaseTime = info.timeRelease,
                Temperature = info.temperature.tc
            };

            this.WeatherSummary = summary;
        }

        private void SetForecastSummary(ForecastModel result)
        {
            var summary = new List<ForecastSummary>();
            var item = result.weather.forecast6days[0];
            if (item == null)
            {
                this.Forecasts = summary;
                return;
            }
            var now = DateTime.Now;
            // 와! 배열을 안쓰고 왜 이런거지 ... 
            summary.Add(new ForecastSummary { Date = now.AddDays(1), DisplayIcon = item.sky.pmCode2day, Description = item.sky.pmName2day, DisplayTemp = item.temperature.tmin2day + " - " + item.temperature.tmax2day });
            summary.Add(new ForecastSummary { Date = now.AddDays(2), DisplayIcon = item.sky.pmCode3day, Description = item.sky.pmName3day, DisplayTemp = item.temperature.tmin3day + " - " + item.temperature.tmax3day });
            summary.Add(new ForecastSummary { Date = now.AddDays(3), DisplayIcon = item.sky.pmCode4day, Description = item.sky.pmName4day, DisplayTemp = item.temperature.tmin4day + " - " + item.temperature.tmax4day });
            summary.Add(new ForecastSummary { Date = now.AddDays(4), DisplayIcon = item.sky.pmCode5day, Description = item.sky.pmName5day, DisplayTemp = item.temperature.tmin5day + " - " + item.temperature.tmax5day });
            summary.Add(new ForecastSummary { Date = now.AddDays(5), DisplayIcon = item.sky.pmCode6day, Description = item.sky.pmName6day, DisplayTemp = item.temperature.tmin6day + " - " + item.temperature.tmax6day });
            summary.Add(new ForecastSummary { Date = now.AddDays(6), DisplayIcon = item.sky.pmCode7day, Description = item.sky.pmName7day, DisplayTemp = item.temperature.tmin7day + " - " + item.temperature.tmax7day });
            summary.Add(new ForecastSummary { Date = now.AddDays(7), DisplayIcon = item.sky.pmCode8day, Description = item.sky.pmName8day, DisplayTemp = item.temperature.tmin8day + " - " + item.temperature.tmax8day });
            summary.Add(new ForecastSummary { Date = now.AddDays(8), DisplayIcon = item.sky.pmCode9day, Description = item.sky.pmName9day, DisplayTemp = item.temperature.tmin9day + " - " + item.temperature.tmax9day });
            summary.Add(new ForecastSummary { Date = now.AddDays(9), DisplayIcon = item.sky.pmCode10day, Description = item.sky.pmName10day, DisplayTemp = item.temperature.tmin10day + " - " + item.temperature.tmax10day });

            this.Forecasts = summary;
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
