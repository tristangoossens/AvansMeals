using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task AddProductToCategory(Tuple<Product, Category> productCategory);
        Task DeleteProductFromCategory(Tuple<Product, Category> productCategory);
    }
}
