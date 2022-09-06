using Common.Dto;
using Repository.IRepositories;
using Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Service
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;

        public WordService(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        /// <summary>
        /// Adds the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public async Task<int> AddWord(WordDto word)
        {
            return await _wordRepository.AddWord(word);
        }

        /// <summary>
        /// Gets the list word.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WordDto>> GetListWord(int categoryId)
        {
            return await _wordRepository.GetListWord(categoryId);
        }

        /// <summary>
        /// Updates the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public async Task<int> UpdateWord(WordDto word)
        {
            return await _wordRepository.UpdateWord(word);
        }

        /// <summary>
        /// Deletes the word.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<int> DeleteWord(int id)
        {
            return await _wordRepository.DeleteWord(id);
        }
    }
}