using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Domain;

namespace Core.DomainServices.Repositories
{
    public interface IMealboxRepository : IGenericRepository<Mealbox>
    {
        IQueryable<Mealbox> GetUnreservedPackages();
    }
}
