using ApiWeather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeather.Services
{
    public interface IWeatherService
    {
        Forecast GetForecast(Town town);
        CurrentWeather GetCurrentWeather(Town town);
        CurrentWeatherAndForecast GetCurrentAndForecast(Town town);
    }
}
