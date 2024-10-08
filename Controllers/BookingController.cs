using Labb2_MVC.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Session;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Labb2_MVC.Controllers
{
    [Route("Booking")]
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri = "http://localhost:5112";

        public BookingController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("ManageBookings")]
        public async Task<IActionResult> ManageBookings()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var response = await _httpClient.GetAsync($"{_baseUri}/Booking/GetAllBookings");

                if (!response.IsSuccessStatusCode)
                {
                    // Returnerar en tom lista om API-anropet misslyckas
                    return View(new List<Booking>());
                }

                // Hämta JSON-svar som sträng
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialisera JSON-svar till en lista av Booking-objekt
                var bookings = JsonConvert.DeserializeObject<List<Booking>>(jsonString);

                // Returnera vyn med listan över bokningar som modell
                return View(bookings);
            }
            catch (Exception ex)
            {
                
                return View(new List<Booking>());
            }
        }

        [HttpGet("BookATable")]
        public IActionResult BookATable()
        {
            ViewData["Title"] = "Book a Table";
            return View("BookTable"); 
        }

        [HttpGet("CreateBooking")]
        public IActionResult CreateBooking()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["Title"] = "New Booking";

            return View();
        }



        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking(Booking booking, string time)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                
                return View(booking);
            }

            if (DateTime.TryParse(time, out var parsedTime))
            {
                booking.Date = booking.Date.Date.Add(parsedTime.TimeOfDay);
            }

            var json = JsonConvert.SerializeObject(booking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUri}/Booking/CreateBooking", content);

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                
                return RedirectToAction("ManageBookings");
            }
            else
            {
                
                var errorMessage = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("", "Error creating booking: " + errorMessage);

                return View("CreateBooking", booking);
            }
        }


        [HttpGet("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _httpClient.GetAsync($"{_baseUri}/Booking/GetBookingById/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Hantera fel när bokningen inte hittas eller annat problem
                return NotFound(); // Eller visa ett felmeddelande
            }

            var json = await response.Content.ReadAsStringAsync();
            var booking = JsonConvert.DeserializeObject<Booking>(json);

            return View(booking);
        }

        [HttpPost("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking(Booking booking)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(booking);
            }
            var json = JsonConvert.SerializeObject(booking);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"{_baseUri}/Booking/UpdateBooking/{booking.Id}", content);

            return RedirectToAction("ManageBookings");
        }

        [HttpPost("Delete/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            await _httpClient.DeleteAsync($"{_baseUri}/Booking/deletebooking/{id}");

            return RedirectToAction("ManageBookings");
        }
    }
}
