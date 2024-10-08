using Labb2_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Labb2_MVC.Controllers
{
    public class AdminController : Controller
    {
        
        public IActionResult Index()
        {
            // Logic for the admin home page
            return View();
        }


    }
}
