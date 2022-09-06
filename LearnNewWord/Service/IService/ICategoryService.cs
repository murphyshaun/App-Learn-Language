using Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetListCategory();

        Task<int> AddCategory(string name);
    }
}