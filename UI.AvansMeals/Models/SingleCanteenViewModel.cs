using Core.Domain;
using UI.AvansMeals.Models;

namespace UI.AvansMeals.Models
{
    public class SingleCanteenViewModel
    {
        public string Name { get; set; }
        public bool WarmMeals { get; set; }
        public City City { get; set; }

        public List<ListItemReservationViewModel>? Reservations { get; set; }

        public List<SingleProductViewModel> Products { get; set; }
    }
}
