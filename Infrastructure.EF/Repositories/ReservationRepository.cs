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
    public class ReservationRepository : IReservationRepository
    {
        private readonly AvansMealsDbContext _context;

        public ReservationRepository(AvansMealsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Reservation> GetList()
        {
            return _context.Reservations;
        }

        public Task<Reservation> GetSingle(int id)
        {
            return _context.Reservations
                .Include(r => r.Mealbox)
                .Include(r => r.Student)
                .SingleAsync(r => r.Id == id);
        }

        public async Task Create(Reservation item)
        {
            await _context.Reservations.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Reservation item)
        {
            Reservation oldReservation = await GetSingle(item.Id);

            oldReservation.IsPickedUp = item.IsPickedUp;

            oldReservation.Student = item.Student;
            oldReservation.Mealbox = item.Mealbox;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Reservations.Remove(await GetSingle(id));
            await _context.SaveChangesAsync();
        }
    }
}
