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
    public class CanteenRepository : ICanteenRepository
    {
        private readonly AvansMealsDbContext _context;

        public CanteenRepository(AvansMealsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Canteen> GetList()
        {
            return _context.Canteens;
        }

        public async Task<Canteen> GetSingle(int id)
        {
            return await _context.Canteens
                .SingleAsync(c => c.Id == id);
        }

        public async Task Create(Canteen item)
        {
            await _context.Canteens.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Canteen item)
        {
            Canteen oldCanteen = await GetSingle(item.Id);

            oldCanteen.Name = item.Name;
            oldCanteen.City = item.City;
            oldCanteen.WarmMeals = item.WarmMeals;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Canteens.Remove(await GetSingle(id));
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Canteen> GetAllInCity(City city)
        {
            return _context.Canteens.Where(c => c.City == city);
        }

        public IQueryable<Mealbox> GetMealboxesInCanteen(int canteenId)
        {
            return _context.Mealboxes
                .Include(mb => mb.Category)
                .Include(mb => mb.Reservation)
                .ThenInclude(r => r.Student)
                .Where(c => c.CanteenId == canteenId);
        }
    }
}
