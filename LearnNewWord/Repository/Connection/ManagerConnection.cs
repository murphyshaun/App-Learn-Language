using Common.Extension;
using System.Data.SQLite;

namespace Repository.Connection
{
    internal class ManagerConnection
    {
        //private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private static readonly string connectionString = string.Format($"Data Source={EnvironmentPath.PathFileDatabase}");

        public static SQLiteConnection CreateConnection
        {
            get => new SQLiteConnection(connectionString);
        }
    }
}