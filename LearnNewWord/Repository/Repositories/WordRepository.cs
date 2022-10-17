using Common.Dto;
using Repository.Infrastructure;
using Repository.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class WordRepository : BaseRepository, IWordRepository
    {
        public WordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Adds the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public async Task<int> AddWord(WordDto word)
        {
            var sql = $"INSERT INTO Word([English], [IpaText], [Vietnamese], [Example], [Image], [Noun], [Ajective], [Verb], [Adverb], [Known], [CategoryId]) " +
                      $"VALUES(@English, @IpaText, @Vietnamese, @Example, @Image, @Noun, @Ajective, @Verb, @Adverb, @Known, @CategoryId)";

            return await ExecuteAsync(sql, new { word.English, word.IpaText, word.Vietnamese, word.Example, word.Image, word.Noun, word.Ajective, word.Verb, word.Adverb, word.Known, word.CategoryId });
        }

        /// <summary>
        /// Gets the list word.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WordDto>> GetListWord(int categoryId)
        {
            var sql = $"SELECT * " +
                      $"FROM Word " +
                      $"WHERE CategoryId = @categoryId";

            return await QueryAsync<WordDto>(sql, new { categoryId });
        }

        /// <summary>
        /// Updates the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public async Task<int> UpdateWord(WordDto word)
        {
            var sql = $"UPDATE Word " +
                      $"SET [English] = @English, " +
                          $"[IpaText] = @IpaText, " +
                          $"[Vietnamese] = @Vietnamese, " +
                          $"[Example] = @Example, " +
                          $"[Image] = @Image, " +
                          $"[Noun] = @Noun, " +
                          $"[Ajective] = @Ajective, " +
                          $"[Verb] = @Verb, " +
                          $"[Adverb] = @Adverb, " +
                          $"[Known] = @Known, " +
                          $"[CategoryId] = @CategoryId " +
                      $"WHERE Id = @Id";

            return await ExecuteAsync(sql, new { word.Id, word.English, word.IpaText, word.Vietnamese, word.Example, word.Image, word.Noun, word.Ajective, word.Verb, word.Adverb, word.Known, word.CategoryId });
        }

        /// <summary>
        /// Deletes the word.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<int> DeleteWord(int id)
        {
            var sql = $"DELETE FROM Word " +
                      $"WHERE Id = @id";

            return await ExecuteAsync(sql, new { id });
        }
    }
}