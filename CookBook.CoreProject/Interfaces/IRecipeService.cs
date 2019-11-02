using System.Linq;
using System.Threading.Tasks;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.RecipeViewModels;

namespace CookBook.CoreProject.Interfaces
{
    public interface IRecipeService
    {
        Task<Recipe> AddAsync(CreateUpdateRecipeViewModel model, int? userId);

        Task<Recipe> GetAsync(int id);

        Task<Recipe> UpdateAsync(CreateUpdateRecipeViewModel model, int recipeId);

        Task<bool> MarkAsDeletedAsync(int id);

        Task<Vote> AddVoteAsync(int recipeId, int userId, int voteValue);

        IQueryable<Recipe> GetPopularRecipesAsync(int count);

        IQueryable<Recipe> GetRecentRecipesAsync(int count);
    }
}
