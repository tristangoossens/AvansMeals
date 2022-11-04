namespace UI.AvansMeals.Models
{
    public class ListItemReservationViewModel
    {
        public string MealboxName { get; set; }
        public int MealboxId { get; set; }

        public string PlacedOn { get; set; }
        public string PickupDate { get; set; }
        public string PickupTimes { get; set; }
        public bool IsPickedUp { get; set; }
    }
}
