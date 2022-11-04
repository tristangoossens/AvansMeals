using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository
        )
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public Category CreateNewCategory(string name, byte[] image)
        {
            return new Category
            {
                Name = name,
                Image = image
            };
        }

        public async Task<Category> CreateModifiedCategory(int id, string name, byte[] image)
        {
            if (await HasReservedPackage(id))
                throw new InvalidOperationException("Deze categorie bevat een of meerdere pakketten die reeds gereserveerd zijn door gebruikers");

            return new Category
            {
                Id = id,
                Name = name,
                Image = image
            };
        }

        public async Task<Tuple<Product, Category>> CreateNewProductCategory(int productId, int categoryId)
        {
            return Tuple.Create(
                await _productRepository.GetSingle(productId),
                await _categoryRepository.GetSingle(categoryId)  
            );
        }

        public async Task<Tuple<Product, Category>> CreateModifiedProductCategory(int productId, int categoryId)
        {
            if(await HasReservedPackage(categoryId))
                throw new InvalidOperationException("Dit product uit de gekozen categorie is inbegrepen in een gereserveerd pakket");

            return Tuple.Create(
                await _productRepository.GetSingle(productId),
                await _categoryRepository.GetSingle(categoryId)
            );
        }

        public async Task<bool> IsAgeBound(int categoryId)
        {
            Category category = await _categoryRepository.GetSingle(categoryId);
            return category.Products.Any(c => c.AgeBound);
        }

        public async Task<bool> HasReservedPackage(int categoryId)
        {
            Category category = await _categoryRepository.GetSingle(categoryId);
            return category.Mealboxes.Any(c => c.Reservation != null);
        }
    }
}
