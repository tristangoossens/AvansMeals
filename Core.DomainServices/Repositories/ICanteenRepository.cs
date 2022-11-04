using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Repositories
{
    public interface ICanteenRepository : IGenericRepository<Canteen>
    {
        IEnumerable<Canteen> GetAllInCity(City city);
        IQueryable<Mealbox> GetMealboxesInCanteen(int canteenId);
    }
}
