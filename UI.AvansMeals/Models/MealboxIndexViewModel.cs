using Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace UI.AvansMeals.Models
{
    public class MealboxIndexViewModel
    {
        [BindProperty]
        public string? Category { get; set; }
        public List<string> Categories { get; set; }

        public City? Location { get; set; }

        public string? SearchQuery { get; set; }
        public FilterMenuSortType? SortType { get; set; }
        public List<ListItemMealboxViewModel>? FilteredList { get; set; }
        public bool ReservedSelected { get; set; }
    }
}
