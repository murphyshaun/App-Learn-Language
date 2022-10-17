using Dapper;
using Repository.Connection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class BaseRepository
    {
        /// <summary>
        /// Queries the first or default asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using (var connection = ManagerConnection.CreateConnection)
            {
                return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
            }
        }

        /// <summary>
        /// Queries the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (var connection = ManagerConnection.CreateConnection)
            {
                return await connection.QueryAsync<T>(sql, parameters);
            }
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using (var connection = ManagerConnection.CreateConnection)
            {
                return await connection.ExecuteAsync(sql, parameters);
            }
        }

        /// <summary>
        /// Executes the scalar asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected async Task<T> ExecuteScalarAsync<T>(string sql, object parameters = null)
        {
            using (var connection = ManagerConnection.CreateConnection)
            {
                return await connection.ExecuteScalarAsync<T>(sql, parameters);
            }
        }
    }
}