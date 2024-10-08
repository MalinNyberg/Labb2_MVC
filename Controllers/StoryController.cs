using Microsoft.AspNetCore.Mvc;

namespace Labb2_MVC.Controllers
{
    public class StoryController : Controller
    {
        public IActionResult Story()
        {
            return View();
        }
    }
}
