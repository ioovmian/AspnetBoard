using Lecture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lecture.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        public AdminController()
        {
        }

        public IActionResult GetCheck()
        {
            if (User.IsInRole("ADMIN"))
            {
                return Json(new { GetCheck = "admin" });
            }

            return Json(new { GetCheck = "Not admin" });
        }

        [AllowAnonymous] // 로그인이 되어있지 않는 익명 사용자도 사용할 수 있음.
        public IActionResult GetUserCheck()
        {
            if (User.Identity.IsAuthenticated) // 로그인 여부 -> true
            {
                return Json(new { a = 1 });
            }

            return Json(new { a = 0 });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}