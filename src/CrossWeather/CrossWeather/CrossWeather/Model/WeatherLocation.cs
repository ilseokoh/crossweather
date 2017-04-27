using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWeather.Model
{
    public class WeatherLocation 
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Version]
        public string Version { get; set; }

        public string Name { get; set; }

    }
}
