
using System.Data.SQLite;

namespace Repository.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private readonly SQLiteConnection _dbContext;

        public SQLiteConnection DbContext => _dbContext ?? (_dbFactory.Init());

        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }


    }
}
