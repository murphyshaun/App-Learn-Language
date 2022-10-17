using System;
using System.Data.SQLite;

namespace Repository.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        SQLiteConnection Init();
    }
}
