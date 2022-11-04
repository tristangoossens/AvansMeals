using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Interfaces;
using Infrastructure.EF.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.AvansMeals.Common;
using UI.AvansMeals.Models;

namespace UI.AvansMeals.Controllers
{
    [Authorize(Policy = "KM")]
    public class CategoryController : Controller
    {
        // Repositories
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        // Services
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CategoryController(
            ICategoryRepository categoryRepository, 
            IProductRepository productRepository, 
            IProductService productService,
            ICategoryService categoryService)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _productRepository = productRepository;
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IQueryable<Category> categories = _categoryRepository.GetList();
            List<SingleCategoryViewModel> viewModels = new List<SingleCategoryViewModel>();

            foreach(Category category in categories)
            {
                viewModels.Add(new SingleCategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageBase64 = $"data:image/gif;base64,{Convert.ToBase64String(category.Image)}",
                    IsAgeBound = await _categoryService.IsAgeBound(category.Id),
                    ProductCount = category.Products.Count(),
                });
            }

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Category category = await _categoryRepository.GetSingle(id);
            SingleCategoryViewModel categoryViewModel = new()
            {
                Id = category.Id,
                Name = category.Name,
                ImageBase64 = $"data:image/gif;base64,{Convert.ToBase64String(category.Image)}",
                IsAgeBound = await _categoryService.IsAgeBound(category.Id),
                ProductCount = category.Products.Count(),
                Products = new List<SingleProductViewModel>()
            };

            foreach(Product product in category.Products)
            {
                categoryViewModel.Products.Add(new SingleProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    ImageBase64 = $"data:image/gif;base64,{Convert.ToBase64String(product.Image)}",
                    IsAgeBound = product.AgeBound
                });
            }

            return View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category category = _categoryService.CreateNewCategory(
                       viewModel.Name,
                       await viewModel.Image.GetBytes()
                    );

                    await _categoryRepository.Create(category);
                    TempData.SetSuccessData($"ProductCategorie met naam '{category.Name}' is succesvol toegevoegd");
                    return RedirectToAction("Index", "Category");
                }
                catch(Exception ex)
                {
                    TempData.SetErrorData(ex.Message);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Create model using service so the buisness logic is checked
                Category category = await _categoryRepository.GetSingle(id);
                Category validatedCategory = await _categoryService.CreateModifiedCategory(
                    category.Id,
                    category.Name,
                    category.Image
                );

                await _categoryRepository.Delete(validatedCategory.Id);

                TempData.SetSuccessData($"ProductCategorie met ID '{id}' is succesvol verwijderd");
            }
            catch(Exception ex)
            {
                TempData.SetErrorData(ex.Message);
            }

            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult AddProduct(int id)
        {
            SetViewbagCreateData(id);

            return View(new AddProductViewModel
            {
                CategoryId = id,
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel viewModel)
        {
            try
            {
                Product product;


                if (viewModel.IsNewProduct)
                {
                    // Validate product viewmodel when a new product is made
                    if (ModelState.IsValid)
                    {
                        product = _productService.CreateNewProduct(
                            viewModel.Name, 
                            await viewModel.Image.GetBytes(), 
                            viewModel.IsAgeBound
                        );

                        await _productRepository.Create(product);
                    }
                    else
                    {
                        SetViewbagCreateData(viewModel.CategoryId);
                        return View(viewModel);
                    }
                }
                else
                {
                    product = await _productRepository.GetSingle(viewModel.ProductId ?? 0);
                }

                // Add intersection record
                await _categoryRepository.AddProductToCategory(await _categoryService.CreateNewProductCategory(
                    product.Id,
                    viewModel.CategoryId
                ));

                TempData.SetSuccessData($"Product met naam '{product.Name}' is succesvol toegevoegd aan categorie met ID '{viewModel.CategoryId}'");
                return RedirectToAction("Details", "Category", new { id = viewModel.CategoryId });
                
            }
            catch(Exception ex)
            {
                TempData.SetErrorData(ex.Message);
                SetViewbagCreateData(viewModel.CategoryId);
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int categoryId, int productId)
        {
            try
            {
                await _categoryRepository.DeleteProductFromCategory(await _categoryService.CreateModifiedProductCategory(
                    productId,
                    categoryId
                ));
                TempData.SetSuccessData($"Product met ID '{productId}' is successfully deleted from category '{categoryId}'");
            }
            catch(Exception ex)
            {
                TempData.SetErrorData(ex.Message);
            }
            
            return RedirectToAction("Details", "Category", new { id = categoryId });
        }

        // Helper function (controller bound)
        private void SetViewbagCreateData(int categoryId)
        {
            // Select only products that are not in the category already
            IQueryable<Product> products = _productRepository.GetList().Where(p => !p.Categories.Any(c => c.Id == categoryId));

            ViewBag.DataModel = new AddProductDataViewModel
            {
                Products = products.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }),
            };
        }
    }
}
