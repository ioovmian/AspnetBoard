using Microsoft.AspNetCore.Mvc;

namespace Lecture.Controllers.ViewComponents
{
    public class LeftMenu : ViewComponent
    {
        public LeftMenu()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
