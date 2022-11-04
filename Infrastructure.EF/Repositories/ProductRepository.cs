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
    public class ProductRepository : IProductRepository
    {
        private readonly AvansMealsDbContext _context;

        public ProductRepository(AvansMealsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Product> GetList()
        {
            return _context.Products;
        }

        public async Task<Product> GetSingle(int id)
        {
            return await _context.Products
                .SingleAsync(p => p.Id == id);
        }

        public async Task Create(Product item)
        {
            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product item)
        {
            Product oldProduct = await GetSingle(item.Id);

            oldProduct.Name = item.Name;
            oldProduct.Image = item.Image;
            oldProduct.AgeBound = item.AgeBound;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Products.Remove(await GetSingle(id));
            await _context.SaveChangesAsync();
        }
    }
}
