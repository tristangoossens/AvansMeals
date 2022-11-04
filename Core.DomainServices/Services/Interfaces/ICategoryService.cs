using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services.Interfaces
{
    public interface ICategoryService
    {
        Category CreateNewCategory(
            string name, 
            byte[] image
        );

        Task<Category> CreateModifiedCategory(
            int id,
            string name,
            byte[] image
        );

        // Intersection table 
        Task<Tuple<Product, Category>> CreateNewProductCategory(
            int productId,
            int categoryId
        );


        Task<Tuple<Product, Category>> CreateModifiedProductCategory(
            int productId,
            int categoryId
        );

        Task<bool> IsAgeBound(int categoryId);
        Task<bool> HasReservedPackage(int categoryId);
    }
}
