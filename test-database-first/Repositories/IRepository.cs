using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_database_first.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Query { get; }
    }
}
