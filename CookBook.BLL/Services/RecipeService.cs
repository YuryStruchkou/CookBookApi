using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Interfaces;
using CookBook.DAL.Data;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.RecipeViewModels;

namespace CookBook.BLL.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RecipeService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Recipe> AddAsync(CreateRecipeViewModel model)
        {
            var recipe = _mapper.Map<CreateRecipeViewModel, Recipe>(model);
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }
    }
}
