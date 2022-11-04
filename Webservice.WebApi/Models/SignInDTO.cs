using System.ComponentModel.DataAnnotations;

namespace Webservice.WebApi.Models
{
    public class SignInDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
