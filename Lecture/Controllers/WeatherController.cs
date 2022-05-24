using Lecture.Models;
using Lecture.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;

namespace Lecture.Controllers
{
    public class WeatherController : Controller
    {

        public WeatherController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }
    }
}