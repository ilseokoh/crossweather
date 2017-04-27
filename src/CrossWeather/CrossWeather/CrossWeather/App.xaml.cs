using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CrossWeather
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CrossWeather.MainPage();

            var vm = new ViewModel.WeatherViewModel();
            // 시작하면서 데이터 가져와서
            vm.GetWeatherCommand.Execute(null);
            // View Model 바인딩
            MainPage.BindingContext = vm;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
