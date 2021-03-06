﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossWeather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForecastView : ContentPage
    {
        public ForecastView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                Icon = new FileImageSource { File = "tab2.png" };
        }
    }
}
