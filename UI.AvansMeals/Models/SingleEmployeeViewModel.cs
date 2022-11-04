using Core.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.AvansMeals.Models
{
    public class SingleEmployeeViewModel
    {
        public int EmployeeNr { get; set; }
        public SingleCanteenViewModel EmployeeCanteen { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        // Filters
        public FilterMenuSortType? SortType { get; set; }
        public List<ListItemMealboxViewModel> FilteredMealboxesList { get; set; }
        public int SelectedCanteenId { get; set; }
        public IEnumerable<SelectListItem> CanteensInCity { get; set; }
    }
}
