using Labb2_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Labb2_MVC.Controllers
{
    [Route("Menu")]
    public class MenuController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri = "http://localhost:5112";

        public MenuController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("Menu")]
        public async Task<IActionResult> Menu()
        {

            try
            {
                var response = await _httpClient.GetAsync($"{_baseUri}/GetAllDishes");

                if (!response.IsSuccessStatusCode)
                {
                    // Returnerar en tom lista om API-anropet misslyckas
                    return View(new List<Dish>());
                }

                // Hämta JSON-svar som sträng
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialisera JSON-svar till en lista av dish-objekt
                var dishes = JsonConvert.DeserializeObject<List<Dish>>(jsonString);

                
                return View(dishes);

            }

            catch (Exception ex)
            {

                return View(new List<Dish>());
            }

        }

        [HttpGet("ManageMenu")]    
        public async Task<IActionResult> ManageMenu()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _httpClient.GetAsync($"{_baseUri}/GetAllDishes");

                if (!response.IsSuccessStatusCode)
                {
                    // Returnerar en tom lista om API-anropet misslyckas
                    return View(new List<Dish>());
                }

                // Hämta JSON-svar som sträng
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialisera JSON-svar till en lista av dish-objekt
                var dishes = JsonConvert.DeserializeObject<List<Dish>>(jsonString);


                return View(dishes);

            }

            catch (Exception ex)
            {

                return View(new List<Dish>());
            }
        }

        [HttpGet("CreateMenuItem")]
        public IActionResult CreateMenuItem()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["Title"] = "New MenuItem";

            return View();
        }



        [HttpPost("CreateMenuItem")]
        public async Task<IActionResult> CreateMenuItem(Dish dish, int MenuId)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {

                return View(dish);
            }

            var json = JsonConvert.SerializeObject(dish);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUri}/UpdateDish/{MenuId}", content);

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("ManageMenu");
            }
            else
            {

                var errorMessage = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("", "Error creating booking: " + errorMessage);

                return View("CreateMenuItem", dish);
            }
        }

        [HttpGet("/UpdateDish")]
        public async Task<IActionResult> UpdateDish(int Id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _httpClient.GetAsync($"{_baseUri}/GetDish/{Id}");

            if (!response.IsSuccessStatusCode)
            {
                // Hantera fel när bokningen inte hittas eller annat problem
                return NotFound(); // Eller visa ett felmeddelande
            }

            var json = await response.Content.ReadAsStringAsync();
            var dish = JsonConvert.DeserializeObject<Dish>(json);

            return View(dish);
        }

        [HttpPost("/UpdateDish")]
        public async Task<IActionResult> UpdateDish(Dish dish)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(dish);
            }

            var json = JsonConvert.SerializeObject(dish);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"{_baseUri}/UpdateDish", content);

            return RedirectToAction("ManageMenu");
        }

        [HttpPost("/DeleteDish")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _httpClient.DeleteAsync($"{_baseUri}/DeleteDish/{id}");

            return RedirectToAction("ManageMenu");
        }




    }
}