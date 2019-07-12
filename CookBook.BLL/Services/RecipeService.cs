using System.Collections.Generic;
using System.Linq;
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
            var tags = await FindOrAddTags(model.Tags);
            recipe.RecipeTags = tags.Select(t => new RecipeTag { Tag = t }).ToHashSet();
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        private async Task<IEnumerable<Tag>> FindOrAddTags(List<string> tags)
        {
            var dbTags = _context.Tags.Where(t => tags.Contains(t.Content)).ToList();
            var dbTagNames = dbTags.Select(t => t.Content).ToHashSet();
            var newTags = tags.Where(t => !dbTagNames.Contains(t)).Select(t => new Tag{Content = t}).ToList();
            dbTags.AddRange(newTags);
            await _context.Tags.AddRangeAsync(newTags);
            return dbTags;
        }
    }
}
