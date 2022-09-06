using Common.Dto;
using Repository.IRepositories;
using Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Task<int> AddCategory(string name)
        {
            return _categoryRepository.AddCategory(name);
        }

        /// <summary>
        /// Gets the list category.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryDto>> GetListCategory()
        {
            return await _categoryRepository.GetListCategory();
        }
    }
}