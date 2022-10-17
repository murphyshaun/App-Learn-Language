using System.Data.SQLite;
using Repository.Connection;

namespace Repository.Infrastructure
{
    public class DbFactory : IDbFactory
    {
        private SQLiteConnection _dbContext;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection Init()
        {
            return _dbContext ?? (_dbContext = new SQLiteConnection(ManagerConnection.ConnectionString));
        }
    }
}
