using System.ComponentModel.DataAnnotations;

namespace UI.AvansMeals.Models
{
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = "Vul een naam in (verplicht)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Upload een foto (verplicht)")]
        public IFormFile Image { get; set; }
    }
}
