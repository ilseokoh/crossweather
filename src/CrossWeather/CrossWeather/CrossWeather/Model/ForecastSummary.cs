using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWeather.Model
{
    public class ForecastSummary
    {
        private string displayIcon;
        public string DisplayIcon
        {
            get { return string.Format("http://www.weatherplanet.co.kr/images/common/ico/weather/ico_s_{0}.png", displayIcon.ToLower()); }
            set { displayIcon = value; }
        }

        private string displayTemp;
        public string DisplayTemp
        {
            get { return displayTemp + "℃"; }
            set { displayTemp = value; }
        }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string DisplayDate
        {
            get { return Date.ToString("MM/dd"); }
        }
    }
}
