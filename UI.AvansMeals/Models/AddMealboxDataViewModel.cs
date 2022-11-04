using Core.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.AvansMeals.Models
{
    public class AddMealboxDataViewModel
    {
        public Canteen Canteen { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
