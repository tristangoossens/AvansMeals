using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetList();
        public Task<T> GetSingle(int id);

        public Task Create(T item);

        public Task Update(T item);
        public Task Delete(int id);
    }
}
