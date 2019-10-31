using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApiWeather.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApiWeather.Services
{
    public class WeatherService : IWeatherService, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public WeatherService(IConfiguration configuration)
        {
            _configuration = configuration; //get configuration to get key for API
            _httpClient = new HttpClient { BaseAddress = new Uri("http://api.openweathermap.org/") }; //base uri
        }

        public Forecast GetForecast(Town town)
        {
            //get json by API request
            var response = _httpClient.GetAsync($"data/2.5/forecast?q={town.Name},{town.Country}&units=metric&appid={_configuration.GetSection("WeatherToken").Value}").Result;

            //parse json into the Forecast class
            var result = JsonConvert.DeserializeObject<Forecast>(response.Content.ReadAsStringAsync().Result);

            foreach (var c in result.list)
                c.date = UnixTimeStampToDateTime(c.dt); //convert Unix TimeStamp to DateTime
            result.Days = SplitDays(result.list); //add only unique days without current day
            return result;
        }

        public CurrentWeather GetCurrentWeather(Town town)
        {
            //get json by API request
            var response = _httpClient.GetAsync($"data/2.5/weather?q={town.Name},{town.Country}&units=metric&appid={_configuration.GetSection("WeatherToken").Value}").Result;

            //parse json into the Forecast class
            var result = JsonConvert.DeserializeObject<CurrentWeather>(response.Content.ReadAsStringAsync().Result);

            //convert Unix TimeStamp to DateTime
            result.date = UnixTimeStampToDateTime(result.dt);
            result.sys.sunriseDate = UnixTimeStampToDateTime(result.sys.sunrise);
            result.sys.sunsetDate = UnixTimeStampToDateTime(result.sys.sunset);
            return result;
        }

        public CurrentWeatherAndForecast GetCurrentAndForecast(Town town)
        {
            var current = GetCurrentWeather(town); //get current weather
            var forecastWeather = GetForecast(town); //get weather forecast

            CurrentWeatherAndForecast currentWeatherAndForecast = new CurrentWeatherAndForecast() { currentWeather = current, forecast = forecastWeather, town = town };
            return currentWeatherAndForecast;
        }

        public List<DateTime> SplitDays(List<List> lists)
        {
            var splitedList = new List<DateTime>();
            var days = new List<int>();
            foreach(var listDate in lists)
            {
                if (listDate.date.Day == DateTime.Now.Day)
                    continue;
                if (!days.Contains(listDate.date.Day))
                {
                    splitedList.Add(listDate.date);
                    days.Add(listDate.date.Day);
                }
            }
            return splitedList;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
