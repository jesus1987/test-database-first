
using test_database_first.DBContext;
using Microsoft.EntityFrameworkCore;

namespace test_database_first.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PruebaContext pruebaContext;
        public Repository(PruebaContext pruebaContext)
        {
            this.pruebaContext = pruebaContext;
        }

        public IQueryable<T> Query => pruebaContext.Set<T>().AsNoTracking().AsQueryable();
    }
}
