using Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IWordService
    {
        Task<IEnumerable<WordDto>> GetListWord(int categoryId);

        Task<int> AddWord(WordDto word);

        Task<int> UpdateWord(WordDto word);

        Task<int> DeleteWord(int id);
    }
}