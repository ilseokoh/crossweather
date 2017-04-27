using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWeather.Model
{
    public class WeatherSummary
    {
        /// <summary>
        /// 현재온도
        /// </summary>
        public string Temperature { get; set; }
        /// <summary>
        /// 위치 (City 구)
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 하늘 설명(ex, 구름조금)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 아이콘 URL 
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 습도
        /// </summary>
        public string humidity { get; set; }

        /// <summary>
        /// 측정시각
        /// </summary>
        public string ReleaseTime { get; set; }
    }
}
