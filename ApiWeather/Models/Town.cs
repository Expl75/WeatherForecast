using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeather.Models
{
    public class Town
    {
        [Required(ErrorMessage = "Please enter a city")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a country")]
        public string Country { get; set; }
    }
}
