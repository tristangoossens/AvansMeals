using Core.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UI.AvansMeals.Models
{
    public class AddProductViewModel
    {
        // Form fields (will only validate if a new product is made)
        [Required(ErrorMessage = "Vul een naam voor het product in (verplicht)")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Upload een foto voor het product (verplicht)")]
        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "Vul in of een product leeftijdsgebonden is (verplicht)")]
        public bool IsAgeBound { get; set; }
        [Required(ErrorMessage = "Kies een categorie (verplicht)")]
        public int CategoryId { get; set; }

        // Check whether a new product is added or a existing product is added (always set)
        public bool IsNewProduct { get; set; }

        // Optional field, a existed product is not bound when creating a new product
        public int? ProductId { get; set; }
    }
}
