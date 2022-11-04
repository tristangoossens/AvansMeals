using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UI.AvansMeals.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        public StudentController(UserManager<IdentityUser> userManager, IStudentRepository studentRepository)
        {
            _userManager = userManager;
            _studentRepository = studentRepository;
        }

        [Authorize(Policy = "S")]
        public async Task<IActionResult> Profile()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            return View(await _studentRepository.GetSingleByEmail(loggedInUser!.Email));
        }
    }
}
