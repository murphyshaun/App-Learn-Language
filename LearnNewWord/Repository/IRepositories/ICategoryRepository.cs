using Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetListCategory();

        Task<int> AddCategory(string name);
    }
}