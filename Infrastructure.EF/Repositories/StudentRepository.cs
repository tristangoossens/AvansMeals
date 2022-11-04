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
    public class StudentRepository : IStudentRepository
    {
        private readonly AvansMealsDbContext _context;

        public StudentRepository(AvansMealsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Student> GetList()
        {
            return _context.Students;
        }

        public Task<Student> GetSingle(int id)
        {
            return GetList()
                .Include(s => s.Reservations)
                .SingleAsync(s => s.Id == id);
        }

        public Task Create(Student item)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Student item)
        {
            Student oldStudent = await GetSingle(item.Id);

            oldStudent.StudentNr = item.StudentNr;
            oldStudent.Name = item.Name;
            oldStudent.Birthdate = item.Birthdate;
            oldStudent.Phonenumber = item.Phonenumber;
            oldStudent.City = item.City;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Students.Remove(await GetSingle(id));
            await _context.SaveChangesAsync();
        }

        public async Task<Student> GetSingleByEmail(string email)
        {
            return await _context.Students
                .Where(s => s.Email == email)
                .Include(s => s.Reservations)
                .ThenInclude(r => r.Mealbox)
                .SingleAsync();
        }
    }
}
