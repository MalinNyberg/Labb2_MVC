namespace Labb2_MVC.Models
{
    public class Dish
    {
        public int MenuId { get; set; }
        public string NameOfDish { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
