using Common.Dto;
using Repository.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CategoryReposity : BaseRepository, ICategoryRepository
    {
        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<int> AddCategory(string name)
        {
            var sql = $"INSERT INTO Category(Name) " +
                $"VALUES(@name)";

            return await ExecuteAsync(sql, new { name });
        }

        /// <summary>
        /// Gets the list category.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryDto>> GetListCategory()
        {
            var sql = $"SELECT c.Id, c.Name, IFNULL(w.Total, 0) as Total " +
                      $"FROM Category c " +
                      $"LEFT JOIN (SELECT CategoryId, COUNT(*) as Total " +
                                   $"FROM Word " +
                                   $"GROUP BY CategoryId) w " +
                      $"ON c.Id = w.CategoryId";

            return await QueryAsync<CategoryDto>(sql);
        }
    }
}