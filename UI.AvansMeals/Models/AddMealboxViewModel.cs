using Core.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UI.AvansMeals.Models
{
    public class AddMealboxViewModel
    {
        [Required(ErrorMessage = "Vul een naam voor het pakket in (Verplicht)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vul een prijs voor het product in (Verplicht)")]
        public string Price_String { get; set; }

        [Required(ErrorMessage = "Vul een ophaaldatum in (Verplicht)")]
        public DateTime PickupFrom { get; set; }

        [Required(ErrorMessage = "Vul een deadline voor ophalen in (Verplicht)")]
        public DateTime PickupUntil { get; set; }

        [Required(ErrorMessage = "Selecteer een categorie (Verplicht)")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Selecteer een kantine (Verplicht)")]
        public int CanteenId { get; set; }
    }
}
