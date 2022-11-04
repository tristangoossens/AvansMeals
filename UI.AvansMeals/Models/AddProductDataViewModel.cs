using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.AvansMeals.Models
{
    public class AddProductDataViewModel
    {
        public IEnumerable<SelectListItem> Products { get; set; }
    }
}
