using Core.Domain;

namespace UI.AvansMeals.Models
{
    public class SingleMealboxViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string City { get; set; }
        public string PickupFrom { get; set; }
        public string PickupUntil { get; set; }
        public Canteen Canteen { get; set; }
        public SingleCategoryViewModel Category { get; set; }
    }
}
