using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeather.Models
{
    public class CurrentWeatherAndForecast
    {
        public Forecast forecast { get; set; }
        public CurrentWeather currentWeather { get; set; }
        public Town town { get; set; }
    }
}
