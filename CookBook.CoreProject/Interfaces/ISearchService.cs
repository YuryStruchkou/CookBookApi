using System.Threading.Tasks;
using CookBook.Domain.ElasticSearch;

namespace CookBook.CoreProject.Interfaces
{
    public interface ISearchService
    {
        Task AddRecipeAsync(RecipeDocument recipe);

        Task UpdateRecipeAsync(RecipeDocument recipe);

        Task DeleteRecipeAsync(RecipeDocument recipe);

        
    }
}
