using Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IWordRepository
    {
        /// <summary>
        /// Gets the list word.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WordDto>> GetListWord(int categoryId);

        /// <summary>
        /// Adds the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        Task<int> AddWord(WordDto word);

        /// <summary>
        /// Updates the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        Task<int> UpdateWord(WordDto word);

        /// <summary>
        /// Deletes the word.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<int> DeleteWord(int id);
    }
}