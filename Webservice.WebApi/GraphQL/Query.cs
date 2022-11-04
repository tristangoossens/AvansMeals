using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Webservice.WebApi.GraphQL
{
    public class Query
    {
        private readonly IMealboxRepository _mealboxRepository;

        public Query(IMealboxRepository mealboxRepository)
        {
            _mealboxRepository = mealboxRepository;
        }

        public IQueryable<Mealbox> Mealboxes()
        {
            return _mealboxRepository.GetList()
                .Include(mb => mb.Reservation)
                .ThenInclude(r => r.Student)
                .Include(mb => mb.Category)
                .ThenInclude(c => c.Products)
                .Include(mb => mb.Canteen)
                .ThenInclude(c => c.Employees);
        }
    }
}
