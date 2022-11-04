using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AvansMealsDbContext _context;

        public CategoryRepository(AvansMealsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Category> GetList()
        {
            return _context.Categories;
        }

        public async Task<Category> GetSingle(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .Include(c => c.Mealboxes)
                .ThenInclude(mb => mb.Reservation)
                .SingleAsync(c => c.Id == id);
        }

        public async Task Create(Category item)
        {
            await _context.Categories.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Category item)
        {
            Category oldCategory = await GetSingle(item.Id);

            oldCategory.Name = item.Name;
            oldCategory.Image = item.Image;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {   
            _context.Categories.Remove(await GetSingle(id));
            await _context.SaveChangesAsync();
        }

        public async Task AddProductToCategory(Tuple<Product, Category> productCategory)
        {
            productCategory.Item2.Products = productCategory.Item2.Products.Append(productCategory.Item1);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductFromCategory(Tuple<Product, Category> productCategory)
        {
            productCategory.Item2.Products = productCategory.Item2.Products.Where(p => p.Id != productCategory.Item1.Id).ToList();
            await _context.SaveChangesAsync();
        }
    }
}
