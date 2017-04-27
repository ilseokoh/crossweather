using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWeather.Model
{
    public class WeatherSummary
    {
        private string temp;
        /// <summary>
        /// 현재온도
        /// </summary>
        public string Temperature
        {
            get { return temp + "℃"; }
            set { temp = value; }
        }
        /// <summary>
        /// 위치 (City 구)
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 하늘 설명(ex, 구름조금)
        /// </summary>
        public string Description { get; set; }

        private string iconUrl;
        /// <summary>
        /// 아이콘 URL 
        /// </summary>
        public string IconUrl
        {
            get { return string.Format("http://www.weatherplanet.co.kr/images/common/ico/weather/ico_b_{0}.png", iconUrl.ToLower()); }
            set { iconUrl = value; }
        }

        private string humidity;
        /// <summary>
        /// 습도
        /// </summary>
        public string Humidity
        {
            get { return humidity + "%"; }
            set { humidity = value; }
        }


        /// <summary>
        /// 측정시각
        /// </summary>
        public string ReleaseTime { get; set; }
    }
}
