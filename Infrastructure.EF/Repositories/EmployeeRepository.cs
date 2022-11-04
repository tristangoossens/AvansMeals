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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AvansMealsDbContext _context;

        public EmployeeRepository(AvansMealsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Employee> GetList()
        {
            return _context.Employees;
        }

        public Task<Employee> GetSingle(int id)
        {
            return _context.Employees
                .SingleAsync(e => e.Id == id);
        }

        public async Task Create(Employee item)
        {
            await _context.Employees.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Employee item)
        {
            Employee oldEmployee = await GetSingle(item.Id);
            oldEmployee.EmployeeNr = item.EmployeeNr;
            oldEmployee.Name = item.Name;

            oldEmployee.Canteen = item.Canteen;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Employees.Remove(await GetSingle(id));
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> GetSingleByEmail(string email)
        {
            return await _context.Employees
                .Include(e => e.Canteen)
                .ThenInclude(c => c.Mealboxes)
                .ThenInclude(mb => mb.Category)
                .SingleAsync(e => e.Email == email);
        }
    }
}
