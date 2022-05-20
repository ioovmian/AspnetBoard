using Lecture.Models;
using Lecture.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Lecture.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        #region Ticket(Product)

        [Authorize]
        public IActionResult TicketList()
        {
            return View(TicketModel.GetList());
        }

        //public IActionResult TicketChange(int producttId, int categoryId)
        //{
        //    new TicketModel
        //    {
        //        Product_Id = producttId,
        //        Category_Id = categoryId
        //    }.Update();

        //    return Redirect("/home/TicketList");
        //}
        public IActionResult TicketChange([FromForm] TicketModel model)
        {
            model.Update();

            return Redirect("/home/TicketList");
        }

        #endregion

        #region 게시판

        public IActionResult BoardList(string search)
        {
            return View(BoardModel.GetList(search));
        }

        public IActionResult BoardView(int idx)
        {
            return View(BoardModel.Get(idx));
        }

        [Authorize]
        public IActionResult BoardWrite()
        {
            return View();
        }

        [Authorize]
        public IActionResult BoardWrite_Input(string title, string content)
        {
            var model = new BoardModel
            {
                Title = title,
                Content = content,
                Reg_User = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Reg_Username = User.Identity.Name
            };
            model.Insert();

            return Redirect("/home/BoardList");
        }

        [Authorize]
        public IActionResult BoardEdit(int idx, string type)
        {
            var model = BoardModel.Get(idx);

            var userSeq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (model.Reg_User != userSeq)
            {
                throw new Exception("수정 할 수 없는 사용자입니다.");
            }

            if (type == "U")
            {
                return View(model);
            }
            else if (type == "D")
            {
                model.Status_Flag = false;
                model.Delete();
                return Redirect("/home/BoardList");
            }

            throw new Exception("잘못된 요청입니다");
        }

        [Authorize]
        public IActionResult BoardEdit_Input(int idx, string title, string content)
        {
            var model = BoardModel.Get(idx);

            var userSeq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (model.Reg_User != userSeq)
            {
                throw new Exception("수정 할 수 없는 사용자입니다.");
            }

            model.Title = title;
            model.Content = content;
            model.Mod_Date = DateTime.Now;
            model.Update();

            return Redirect("/home/BoardList");
        }

        #endregion

        #region Test

        public IActionResult Chat()
        {
            return View();
        }


        public IActionResult TestA(int x, int y)
        {
            ViewBag.x = x;
            ViewBag.y = y;

            ViewBag.Title = "TestA 화면~~";

            if (x == 1)
            {
                // 전체 레이아웃 전달
                return View();
            }
            else
            {
                // 일부 레이아웃만 전달
                return PartialView();
            }
        }

        public IActionResult TestB(int x, int y)
        {
            var model = new TestBModel();
            model.X = x;
            model.Y = y;

            switch (x)
            {
                case 0:
                    return Json(new { x = 1, y = "ABC" });

                case 1:
                    return Json(model);

                case 2:
                    return Content("LG Twins && Real Madrid", "text/html");
            }

            return Json(null);
        }

        public IActionResult TestC(string x, string y)
        {
            ViewData["x"] = x;
            ViewBag.y = y;

            List<TestBModel> list01 = new List<TestBModel>()
            {
                new TestBModel(){X=1,Y = 101},
                new TestBModel(){X=2,Y = 102},
                new TestBModel(){X=3,Y = 103},
                new TestBModel(){X=4,Y = 104},
                new TestBModel(){X=5,Y = 105},
            };
            ViewData["list01"] = list01;

            List<TestBModel> list02 = new List<TestBModel>()
            {
                new TestBModel(){X=501,Y = 1011},
                new TestBModel(){X=502,Y = 1012},
                new TestBModel(){X=503,Y = 1013},
                new TestBModel(){X=504,Y = 1014},
                new TestBModel(){X=505,Y = 1015},
            };
            return View(list02);
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}