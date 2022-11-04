using Core.Domain;

namespace UI.AvansMeals.Models
{
    public class ListItemMealboxViewModel
    {
        // Mealbox
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAgeBound { get; set; }
        public string PickupDate { get; set; }

        // Category
        public string CategoryImageBase64 { get; set; }
        public string CategoryName { get; set; }

        // Reservation
        public bool Reserved { get; set; }
        public Student? ReservedBy { get; set; }
    }
}
