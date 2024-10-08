using Labb2_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Labb2_MVC.Controllers
{
    public class AccountController : Controller
    {

        // Hardcoded username and password
        private const string Username = "admin";
        private const string Password = "password123";

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login login)
        {
            if (login.Username == Username && login.Password == Password)
            {
                HttpContext.Session.SetString("UserRole", "Admin");
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                // Invalid credentials, return error
                ViewBag.Error = "Invalid username or password.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            // Logic to handle logout (if needed)
            return RedirectToAction("Login");
        }

    }
}
