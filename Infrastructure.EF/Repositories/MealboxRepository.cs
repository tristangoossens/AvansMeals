using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Repositories
{
    public class MealboxRepository : IMealboxRepository
    {
        // Context
        private readonly AvansMealsDbContext _context;

        public MealboxRepository(AvansMealsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Mealbox> GetList()
        {
            return _context.Mealboxes;
        }

        public async Task<Mealbox> GetSingle(int id)
        {
            return await _context.Mealboxes
                .Include(mb => mb.Canteen)
                .Include(mb => mb.Reservation)
                .Include(mb => mb.Category)
                .ThenInclude(c => c.Products)
                .SingleAsync(mealbox => mealbox.Id == id);
        }

        public async Task Create(Mealbox item)
        {
            await _context.Mealboxes.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Mealbox item)
        {
            var mealbox = await GetSingle(item.Id);
            
            mealbox.Name = item.Name;
            mealbox.Price = item.Price;
            mealbox.PickupFrom = item.PickupFrom;
            mealbox.PickupUntil = item.PickupUntil;

            mealbox.Category = item.Category;
            mealbox.Canteen = item.Canteen;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Mealboxes.Remove(await GetSingle(id));
            await _context.SaveChangesAsync();
        }

        public IQueryable<Mealbox> GetUnreservedPackages()
        {
            return _context.Mealboxes
                .Include(mb => mb.Canteen)
                .Include(mb => mb.Category)
                .ThenInclude(c => c.Products)
                .Include(mb => mb.Reservation)
                .Where(mb => mb.Reservation == null);
        }
    }
}
