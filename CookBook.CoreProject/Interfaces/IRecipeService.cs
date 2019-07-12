using System.Threading.Tasks;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.RecipeViewModels;

namespace CookBook.CoreProject.Interfaces
{
    public interface IRecipeService
    {
        Task<Recipe> AddAsync(CreateRecipeViewModel model, int? userId);

        Task<Recipe> GetAsync(int id);
    }
}
