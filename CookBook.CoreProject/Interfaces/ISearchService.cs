using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CookBook.Domain.ElasticSearch;

namespace CookBook.CoreProject.Interfaces
{
    public interface ISearchService
    {
        Task AddRecipeAsync(RecipeDocument recipe);

        Task UpdateRecipeAsync(RecipeDocument recipe);

        Task DeleteRecipeAsync(RecipeDocument recipe);

        Task<IEnumerable<RecipeDocument>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10);
    }
}
