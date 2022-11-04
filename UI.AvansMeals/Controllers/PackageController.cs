using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Interfaces;
using Infrastructure.EF.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UI.AvansMeals.Common;
using UI.AvansMeals.Models;

namespace AvansMeals.Controllers
{
    public class PackageController : Controller
    {
        // Repositories
        private readonly IMealboxRepository _mealboxRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ICanteenRepository _canteenRepository;

        // Services
        private readonly IMealboxService _mealboxService;
        private readonly IReservationService _reservationService;

        // Managers
        private readonly UserManager<IdentityUser> _userManager;

        public PackageController(
            IMealboxRepository mealboxRepository, 
            ICategoryRepository categoryRepository,
            IEmployeeRepository employeeRepository,
            IStudentRepository studentRepository,
            IReservationRepository reservationRepository,
            ICanteenRepository canteenRepository,
            IMealboxService mealboxService,
            IReservationService reservationService,
            UserManager<IdentityUser> userManager
        )
        {
            _mealboxRepository = mealboxRepository ?? throw new ArgumentNullException(nameof(mealboxRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _canteenRepository = canteenRepository ?? throw new ArgumentNullException(nameof(canteenRepository));

            _mealboxService = mealboxService ?? throw new ArgumentNullException(nameof(mealboxService));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));

            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            MealboxIndexViewModel viewModel = new()
            {
                
                FilteredList = new List<ListItemMealboxViewModel>()
            };

            var mealboxes = _mealboxRepository.GetUnreservedPackages();
            viewModel = await SetDataFieldsIndexModel(viewModel, mealboxes);

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(MealboxIndexViewModel viewModel)
        {
            // Get initial boxes
            IQueryable<Mealbox> mealboxes;
            if (viewModel.ReservedSelected)
            {
                mealboxes = _mealboxRepository
                    .GetList()
                    .Include(mb => mb.Category)
                    .Include(mb => mb.Reservation);
            }
            else
            {
                mealboxes = _mealboxRepository.GetUnreservedPackages();
            }

            // Apply location filter if set
            if(viewModel.Location != null)
            {
                mealboxes = mealboxes.Where(mb => mb.Canteen.City == viewModel.Location).AsQueryable();
            }

            // Apply category filter if set
            if (viewModel.Category != null)
            {
                mealboxes = mealboxes.Where(mb => mb.Category.Name == viewModel.Category).AsQueryable();
            }

            // Apply sort filter if set
            if (viewModel.SortType != null)
            {
                mealboxes = viewModel.SortType.Value.Sort(mealboxes);
            }

            // Apply search query if set
            if(viewModel.SearchQuery != null)
            {
                mealboxes = mealboxes.Where(mb => mb.Name.Contains(viewModel.SearchQuery));
            }

            viewModel = await SetDataFieldsIndexModel(viewModel, mealboxes);

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            Mealbox mealbox = await _mealboxRepository.GetSingle(id);

            return View(new SingleMealboxViewModel
            {
                Id = mealbox.Id,
                Name = mealbox.Name,
                Price = mealbox.Price,
                City = mealbox.Canteen.City.ToString(),
                PickupFrom = mealbox.PickupFrom.ToString("dd-MM-yyyy"),
                PickupUntil = mealbox.PickupUntil.ToString("dd-MM-yyyy"),
                Canteen = mealbox.Canteen,
                Category = new SingleCategoryViewModel
                {
                    Id = mealbox.Category.Id,
                    Name = mealbox.Category.Name,
                    ImageBase64 = $"data:image/gif;base64,{Convert.ToBase64String(mealbox.Category.Image)}",
                    ProductCount = mealbox.Category.Products.Count(),
                    Products = mealbox.Category.Products.Select(p => new SingleProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ImageBase64 = $"data:image/gif;base64,{Convert.ToBase64String(p.Image)}",
                        IsAgeBound = p.AgeBound,
                    }).ToList()
                }
            });
        }

        [HttpGet]
        [Authorize(Policy = "KM")]
        public async Task<IActionResult> Add()
        {
            await SetViewbagCreateForm();
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "KM")]
        public async Task<IActionResult> Add(AddMealboxViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    Employee employee = await _employeeRepository.GetSingleByEmail(currentUser.Email);

                    Mealbox newMealBox = await _mealboxService.CreateNewMealbox(
                        viewModel.Name,
                        viewModel.PickupFrom,
                        viewModel.PickupUntil,
                        decimal.Parse(viewModel.Price_String.Replace(".", ",")),
                        viewModel.CategoryId,
                        employee.Canteen.Id
                    );

                    await _mealboxRepository.Create(newMealBox);
                    TempData.SetSuccessData($"Het pakket met naam '{newMealBox.Name}' is successvol toegevoegd");
                    return RedirectToAction("Index", "Package");
                }
                catch (Exception ex)
                {
                    TempData.SetErrorData(ex.Message);
                }
            }

            await SetViewbagCreateForm();
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Policy = "KM")]
        public async Task<IActionResult> Edit(int id)
        {
            var mealbox = await _mealboxRepository.GetSingle(id);

            await SetViewbagUpdateForm(id);

            return View(new EditMealboxViewModel
            {
                Id = id,
                Name = mealbox.Name,
                Price_String = mealbox.Price.ToString().Replace(",", "."),
                PickupFrom = mealbox.PickupFrom.ToLocalTime(),
                PickupUntil = mealbox.PickupUntil.ToLocalTime(),
                CanteenId = mealbox.Canteen.Id,
                CategoryId = mealbox.CategoryId,
            });
        }

        [HttpPost]
        [Authorize(Policy = "KM")]
        public async Task<IActionResult> Edit(EditMealboxViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Mealbox updatedMealbox = await _mealboxService.CreateModifiedMealbox(
                           viewModel.Id,
                           viewModel.Name,
                           viewModel.PickupFrom,
                           viewModel.PickupUntil,
                           decimal.Parse(viewModel.Price_String.Replace(".", ",")),
                           viewModel.CategoryId,
                           viewModel.CanteenId
                    );

                    await _mealboxRepository.Update(updatedMealbox);
                    TempData.SetSuccessData($"Het pakket met naam '{updatedMealbox.Name}' is successvol gewijzigd");
                    return RedirectToAction("Index", "Package");
                }
                catch (Exception ex)
                {
                    TempData.SetErrorData(ex.Message);
                }
            }

            await SetViewbagUpdateForm(viewModel.Id);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Policy = "KM")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Mealbox mealbox = await _mealboxRepository.GetSingle(id);
                Mealbox validatedMealbox = await _mealboxService.CreateModifiedMealbox(
                    mealbox.Id,
                    mealbox.Name,
                    mealbox.PickupFrom,
                    mealbox.PickupUntil,
                    mealbox.Price,
                    mealbox.CategoryId,
                    mealbox.CanteenId
                );
                await _mealboxRepository.Delete(validatedMealbox.Id);


                TempData.SetSuccessData($"Het pakket met ID '{id}' is successvol verwijderd");
            }
            catch(Exception ex)
            {
                TempData.SetErrorData(ex.Message);
            }

            return RedirectToAction("Index", "Package");
        }

        [HttpGet]
        [Authorize(Policy = "S")]
        public async Task<IActionResult> Reserve(int id)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                Student student = await _studentRepository.GetSingleByEmail(currentUser.Email);

                await _reservationRepository.Create(await _reservationService.CreateReservation(id, student.Id));
                TempData.SetSuccessData("Je hebt het pakket successvol gereserveerd!");
                return RedirectToAction("Profile", "Student");
            }
            catch (Exception ex)
            {
                TempData.SetErrorData(ex.Message);
            }

            return RedirectToAction("Details", "Package", new { id = id });
        }

        // Helper function (Controller bound)
        private async Task<MealboxIndexViewModel> SetDataFieldsIndexModel(MealboxIndexViewModel viewModel, IQueryable<Mealbox> currentList)
        {
            viewModel.Categories = _categoryRepository.GetList().Select(c => c.Name).ToList();

            List<ListItemMealboxViewModel> viewModels = new();
            
            foreach (var mealbox in currentList)
            {
                viewModels.Add(new ListItemMealboxViewModel
                {
                    Id = mealbox.Id,
                    Name = mealbox.Name,
                    Price = mealbox.Price,
                    IsAgeBound = await _mealboxService.IsAgeBound(mealbox.Id),
                    CategoryName = mealbox!.Category!.Name,
                    CategoryImageBase64 = $"data:image/gif;base64,{Convert.ToBase64String(mealbox!.Category!.Image)}",
                    Reserved = (mealbox.Reservation != null)
                });
            }

            viewModel.FilteredList = viewModels;

            return viewModel;
        }

        // Helper function (Controller bound)
        private async Task SetViewbagCreateForm()
        {
            IQueryable<Category> categories = _categoryRepository.GetList();
            var currentUser = await _userManager.GetUserAsync(User);
            Employee employee = await _employeeRepository.GetSingleByEmail(currentUser.Email);

            ViewBag.DataModel = new AddMealboxDataViewModel
            {
                Canteen = employee.Canteen,
                Categories = categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
            };
        }

        // Helper function (Controller bound)
        private async Task SetViewbagUpdateForm(int id)
        {
            var mealbox = await _mealboxRepository.GetSingle(id);
            IQueryable<Category> categories = _categoryRepository.GetList();
            var currentUser = await _userManager.GetUserAsync(User);
            Employee employee = await _employeeRepository.GetSingleByEmail(currentUser.Email);

            // Edit data viewmodel is the same as addviewmodel so it is reused
            ViewBag.DataModel = new AddMealboxDataViewModel
            {
                Canteen = employee.Canteen,
                Categories = categories.Select(c =>
                   new SelectListItem
                   {
                       Text = c.Name,
                       Value = c.Id.ToString(),
                       Selected = (c.Id == mealbox.CategoryId)
                   })
            };
            
        }
    }
}
