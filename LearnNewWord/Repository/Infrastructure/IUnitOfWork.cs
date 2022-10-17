using System.Data.SQLite;

namespace Repository.Infrastructure
{
    public interface IUnitOfWork
    {
        SQLiteConnection DbContext { get; }
    }
}
