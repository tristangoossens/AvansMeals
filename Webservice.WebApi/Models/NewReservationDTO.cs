using System.ComponentModel.DataAnnotations;

namespace Webservice.WebApi.Models
{
    public class NewReservationDTO
    {
        [Required]
        public int StudentId { get; set; }
    }
}
