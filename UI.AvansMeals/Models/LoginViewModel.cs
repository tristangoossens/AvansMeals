using System.ComponentModel.DataAnnotations;

namespace UI.AvansMeals.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vul je e-mail adres in (verplicht)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vul je wachtwoord in (verplicht)")]
        public string Password { get; set; }
    }
}
