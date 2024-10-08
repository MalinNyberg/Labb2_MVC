using System.ComponentModel.DataAnnotations;

namespace Labb2_MVC.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; } 
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select the number of Guests.")]
        [Range(1, 10, ErrorMessage = "Please select the number of Guests (1-10). Are you a larger group? Please contact us")]
        public int NumberOfPeople { get; set; }


        public DateTime Date{ get; set; }


        
    }
}
