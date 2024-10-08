namespace Labb2_MVC.Models
{
    public class BookingById
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime BookingTime { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalCost { get; set; }


        public IEnumerable<BookingDish>? OrderedDishes { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }

    }
}
