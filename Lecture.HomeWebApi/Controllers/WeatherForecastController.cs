using Lecture.HomeWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lecture.HomeWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecastModel> Get(int num)
        {
            return Enumerable.Range(1, num).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        //public WeatherForecastModel Post([FromForm] int addDays)
        public WeatherForecastModel Post(int addDays)
        {
            Console.WriteLine($"Weather Post : {addDays}");

            return new WeatherForecastModel() { Date = DateTime.Now.AddDays(addDays) };
        }
        [HttpPut]
        public IActionResult Put()
        {
            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete(int key)
        {
            return BadRequest(new { message = $"{key}잘못된 요청입니다." });
        }
    }
}