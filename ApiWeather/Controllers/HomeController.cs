using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiWeather.Models;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ApiWeather.Services;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;

namespace ApiWeather.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService; //get service via dependency injection
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); //display view for input data (city and town)
        }

        [HttpGet]
        public IActionResult CurrentWeatherAndForecast(CurrentWeatherAndForecast currentAndForecast)
        {
            return View("CurrentAndForecast", currentAndForecast); //send model and display data of forecast
        }

        [HttpPost]
        public IActionResult Index(Town town)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _weatherService.GetCurrentAndForecast(town); //get forecast by town
                    return CurrentWeatherAndForecast(result);
                }
                return View(town);
            }
            catch
            {
                ModelState.AddModelError("", "City not found!"); //add error if api give an error
                return View(town);
            }
        }
    }
}
