using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Interfaces;
using CookBook.DAL.Data;
using CookBook.Domain.ElasticSearch;
using CookBook.Domain.Enums;
using CookBook.Domain.Helpers;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.RecipeViewModels;
using CookBook.Queries.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace CookBook.BLL.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ISearchService _searchService;

        public RecipeService(IMapper mapper, ApplicationDbContext context, ISearchService searchService)
        {
            _mapper = mapper;
            _context = context;
            _searchService = searchService;
        }

        public async Task<Recipe> AddAsync(CreateUpdateRecipeViewModel model, int? userId)
        {
            var recipe = _mapper.Map<CreateUpdateRecipeViewModel, Recipe>(model);
            var tags = GetOrAddTags(model.Tags);
            recipe.RecipeTags = tags.Select(t => new RecipeTag { Tag = t }).ToHashSet();
            recipe.UserId = userId;
            recipe.CreationDate = DateTime.Now;
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            await _searchService.AddRecipeAsync(_mapper.Map<Recipe, RecipeDocument>(recipe));
            return recipe;
        }

        private IEnumerable<Tag> GetOrAddTags(List<string> tags)
        {
            var dbTags = _context.Tags.GetAttachableTags(tags).ToList();
            _context.Tags.AttachRange(dbTags);
            return dbTags;
        }

        public async Task<Recipe> GetAsync(int id)
        {
            var result = await _context.Recipes.FindAsync(id);
            return result?.RecipeStatus != RecipeStatus.Deleted ? result : null;
        }

        public async Task<Recipe> UpdateAsync(CreateUpdateRecipeViewModel model, int recipeId)
        {
            var recipe = _context.Recipes.Include(r => r.RecipeTags).SingleOrDefault(r => r.Id == recipeId);
            if (recipe == null)
            {
                return null;
            }
            var tags = GetOrAddTags(model.Tags);
            recipe.RecipeTags = tags.Select(t => new RecipeTag { Tag = t }).ToHashSet();
            recipe.Name = model.Name;
            recipe.Content = model.Content;
            recipe.Description = model.Description;
            recipe.EditDate = DateTime.Now;
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            await _searchService.UpdateRecipeAsync(_mapper.Map<Recipe, RecipeDocument>(recipe));
            return recipe;
        }

        public async Task<bool> MarkAsDeletedAsync(int id)
        {
            var recipe = _context.Recipes.Find(id);
            if (recipe == null) return false;
            recipe.RecipeStatus = RecipeStatus.Deleted;
            recipe.DeleteDate = DateTime.Now;
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            await _searchService.DeleteRecipeAsync(_mapper.Map<Recipe, RecipeDocument>(recipe));
            return true;
        }

        public async Task<Vote> AddVoteAsync(int recipeId, int userId, int voteValue)
        {
            var recipe = _context.Recipes.Find(recipeId);
            if (recipe == null) return null;
            var vote = recipe.AddOrUpdateVote(userId, voteValue);
            await _context.SaveChangesAsync();
            return vote;
        }

        public IQueryable<Recipe> GetPopularRecipesAsync(int count)
        {
            return _context.Recipes
                .Where(r => r.RecipeStatus == RecipeStatus.Active)
                .AsEnumerable() // TODO: Fix lazy loading issue
                .OrderByDescending(r => r.GetAverageVote())
                .ThenByDescending(r => r.Votes.Count)
                .Take(count).AsQueryable();
        }

        public IQueryable<Recipe> GetRecentRecipesAsync(int count)
        {
            return _context.Recipes
                .Where(r => r.RecipeStatus == RecipeStatus.Active)
                .OrderByDescending(r => r.CreationDate)
                .Take(count);
        }
    }
}
