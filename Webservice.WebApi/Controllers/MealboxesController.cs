using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webservice.WebApi.Models;

namespace Webservice.WebApi.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class MealboxesController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMealboxRepository _mealboxRepository;

        public MealboxesController(
            IReservationService reservationService,
            IReservationRepository reservationRepository,
            IMealboxRepository mealboxRepository)
        {
            _reservationService = reservationService;
            _reservationRepository = reservationRepository;
            _mealboxRepository = mealboxRepository;
        }

        [HttpPost("{id}/Reserve"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Reserve(int id, [FromBody] NewReservationDTO newReservation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var reservation = await _reservationService.CreateReservation(id, newReservation.StudentId);
                    await _reservationRepository.Create(reservation);
                    return Ok(new {Message = $"Nieuwe reservering voor pakket '{reservation.Mealbox.Name}', op de naam van '{reservation.Student.Name}'" });
                }
                catch (ArgumentException ex)
                {
                    // Bad request
                    return BadRequest(new { Message = ex.Message });
                }
                catch (Exception ex)
                {
                    // An error has occurred
                    return Problem(ex.Message);
                }
            }

            return BadRequest(new { Message = "Vul alle verplichte velden in (MealboxId, StudentId)" });
        }
    }
}
