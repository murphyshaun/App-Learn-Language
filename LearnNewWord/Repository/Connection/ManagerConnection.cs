using Common.Extension;

namespace Repository.Connection
{
    internal static class ManagerConnection
    {
        internal static readonly string ConnectionString = string.Format($"Data Source={EnvironmentPath.PathFileDatabase}");
    }
}