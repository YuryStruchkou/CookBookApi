using System;
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

        public async Task<Recipe> AddAsync(CreateUpdateRecipeViewModel model, int? id)
        {
            var recipe = _mapper.Map<CreateUpdateRecipeViewModel, Recipe>(model);
            var tags = GetOrAddTags(model.Tags);
            recipe.RecipeTags = tags.Select(t => new RecipeTag { Tag = t }).ToHashSet();
            recipe.UserId = id;
            recipe.CreationDate = DateTime.Now;
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        private IEnumerable<Tag> GetOrAddTags(List<string> tags)
        {
            var dbTags = _context.Tags.Where(t => tags.Contains(t.Content)).ToList();
            var dbTagNames = dbTags.Select(t => t.Content).ToHashSet();
            var newTags = tags.Where(t => !dbTagNames.Contains(t)).Select(t => new Tag{Content = t}).ToList();
            dbTags.AddRange(newTags);
            _context.Tags.AddRange(newTags);
            return dbTags;
        }

        public async Task<Recipe> GetAsync(int id)
        {
            return await _context.Recipes.FindAsync(id);
        }

        public async Task<Recipe> UpdateAsync(CreateUpdateRecipeViewModel model, int recipeId)
        {
            var recipe = _context.Recipes.Find(recipeId);
            var tags = GetOrAddTags(model.Tags);
            recipe.RecipeTags = tags.Select(t => new RecipeTag { Tag = t }).ToHashSet();
            recipe.Name = model.Name;
            recipe.Content = model.Content;
            recipe.Description = model.Description;
            recipe.EditDate = DateTime.Now;
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }
    }
}
