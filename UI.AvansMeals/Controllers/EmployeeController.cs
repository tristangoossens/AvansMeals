using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.AvansMeals.Common;
using UI.AvansMeals.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace UI.AvansMeals.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICanteenRepository _canteenRepository;
        private readonly IMealboxRepository _mealboxRepository;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            UserManager<IdentityUser> userManager,
            ICanteenRepository canteenRepository,
            IMealboxRepository mealboxRepository)
        {
            _employeeRepository = employeeRepository;
            _userManager = userManager;
            _canteenRepository = canteenRepository;
            _mealboxRepository = mealboxRepository;
        }

        [HttpGet]
        [Authorize(Policy = "KM")]
        public async Task<IActionResult> Profile()
        {
            SingleEmployeeViewModel viewModel = await GetViewModelWithData();
            IQueryable<Mealbox> mealboxes = _canteenRepository.GetMealboxesInCanteen(viewModel.SelectedCanteenId);

            foreach (Mealbox mealbox in mealboxes)
            {
                viewModel.FilteredMealboxesList.Add(new ListItemMealboxViewModel
                {
                    Id = mealbox.Id,
                    Name = mealbox.Name,
                    Price = mealbox.Price,
                    PickupDate = mealbox.PickupFrom.ToString("dd-MM-yyyy"),
                    CategoryName = mealbox.Category.Name,
                    CategoryImageBase64 = mealbox.Category.Image.ToBase64String(),
                    Reserved = (mealbox.Reservation != null),
                    ReservedBy = mealbox.Reservation?.Student ?? null
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = "KM")]
        public async Task<IActionResult> Profile(SingleEmployeeViewModel viewModel)
        {
            SingleEmployeeViewModel newViewModel = await GetViewModelWithData();
            IQueryable<Mealbox> mealboxes = _canteenRepository.GetMealboxesInCanteen(viewModel.SelectedCanteenId);

            // Apply sort if 
            if (newViewModel.SortType != null)
            {
                mealboxes = newViewModel.SortType.Value.Sort(mealboxes);
            }

            foreach (Mealbox mealbox in mealboxes)
            {
                newViewModel.FilteredMealboxesList.Add(new ListItemMealboxViewModel
                {
                    Id = mealbox.Id,
                    Name = mealbox.Name,
                    Price = mealbox.Price,
                    PickupDate = mealbox.PickupFrom.ToString("dd-MM-yyyy"),
                    CategoryName = mealbox.Category.Name,
                    CategoryImageBase64 = mealbox.Category.Image.ToBase64String(),
                    Reserved = (mealbox.Reservation != null),
                    ReservedBy = mealbox.Reservation?.Student ?? null
                });
            }

            return View(newViewModel);
        }


        private async Task<SingleEmployeeViewModel> GetViewModelWithData()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            Employee employee = await _employeeRepository.GetSingleByEmail(currentUser.Email);
            IEnumerable<Mealbox> reservedMealboxes = _mealboxRepository.GetList().Include(mb => mb.Reservation)
                .Where(mb => mb.Canteen.Id == employee.CanteenId && mb.Reservation != null && mb.PickupUntil > DateTime.Now)
                .ToList();

            SingleEmployeeViewModel viewModel = new SingleEmployeeViewModel
            {
                EmployeeNr = employee.EmployeeNr,
                Email = employee.Email,
                Name = employee.Name,
                EmployeeCanteen = new SingleCanteenViewModel
                {
                    Name = employee.Canteen.Name,
                    City = employee.Canteen.City,
                    WarmMeals = employee.Canteen.WarmMeals,
                    Reservations = new List<ListItemReservationViewModel>(),
                    Products = new List<SingleProductViewModel>()
                },
                SelectedCanteenId = employee.Canteen.Id,
                CanteensInCity = _canteenRepository.GetAllInCity(employee.Canteen.City).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                FilteredMealboxesList = new List<ListItemMealboxViewModel>()
            };

            foreach (Mealbox reservedMealbox in reservedMealboxes)
            {
                viewModel.EmployeeCanteen.Reservations.Add(new ListItemReservationViewModel
                {
                    MealboxId = reservedMealbox.Id,
                    MealboxName = reservedMealbox.Name,
                    IsPickedUp = reservedMealbox.Reservation.IsPickedUp,
                    PlacedOn = reservedMealbox.CreatedAt.ToString("dd-MM-yyyy"),
                    PickupDate = reservedMealbox.PickupFrom.ToString("dd-MM-yyyy"),
                    PickupTimes = $"{reservedMealbox.PickupFrom.ToString("hh:mm")} - {reservedMealbox.PickupUntil.ToString("hh:mm")}"
                });
            }

            return viewModel;
        }
    }
}
