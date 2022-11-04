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
    public class ProductService : IProductService
    {
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public Product CreateNewProduct(string name, byte[] image, bool isAgeBound)
        {
            return new Product
            {
                Name = name,
                Image = image,
                AgeBound = isAgeBound,
                Categories = new List<Category>()
            };
        }
    }
}
