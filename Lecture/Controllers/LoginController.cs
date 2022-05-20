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
    public class LoginController : Controller
    {

        public LoginController()
        {
        }

        public IActionResult Index()
        {
            return Redirect("/login/login");
        }

        #region 회원가입
        public IActionResult Register(string msg)
        {
            ViewData["msg"] = msg;

            return View();
        }

        [HttpPost]
        [Route("login/register")]
        public IActionResult RegisterProc([FromForm] UserModel model)
        {
            try
            {
                var password2 = Request.Form["password2"];

                if (model.Password != password2)
                {
                    throw new Exception("입력된 비밀번호가 다릅니다.");
                }

                model.Register();
                return Redirect("/login/login");
            }
            catch (Exception ex)
            {
                return Redirect($"/login/register?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }
        #endregion

        #region 로그인
        [HttpGet]
        public IActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        [Route("login/login")]
        public async Task<IActionResult> LoginProc([FromForm] UserModel model)
        {
            try
            {
                var user = model.Login();

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Seq.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMddHHmmss")));
                if(user.Name == "aa")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "ADMIN"));
                }

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = false, // 영속성 (로그인 유지여부, false : 브라우저가 꺼지면 권한 삭제)
                    ExpiresUtc = DateTime.UtcNow.AddHours(4),   // 권한 만기 시간
                    AllowRefresh = true // 갱신여부?
                });

                return Redirect("/");
            }
            catch (Exception ex)
            {
                return Redirect($"/login/login?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }
        #endregion

        #region 로그아웃

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}