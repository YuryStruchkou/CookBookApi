using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Interfaces;
using CookBook.DAL.Data;
using CookBook.Domain.ElasticSearch;
using CookBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace CookBook.BLL.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMapper _mapper;
        private readonly IElasticClient _elasticClient;
        private readonly ApplicationDbContext _context;

        public SearchService(IMapper mapper, IElasticClient elasticClient, ApplicationDbContext context)
        {
            _mapper = mapper;
            _elasticClient = elasticClient;
            _context = context;
        }


        public async Task AddRecipeAsync(RecipeDocument recipe)
        {
            await _elasticClient.IndexDocumentAsync(recipe);
        }

        public async Task UpdateRecipeAsync(RecipeDocument recipe)
        {
            await _elasticClient.UpdateAsync<RecipeDocument>(recipe, u => u.Doc(recipe));
        }

        public async Task DeleteRecipeAsync(RecipeDocument recipe)
        {
            await _elasticClient.DeleteAsync<RecipeDocument>(recipe);
        }

        public async Task<IEnumerable<RecipeDocument>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10)
        {
            var results = await _elasticClient.SearchAsync<RecipeDocument>(
                s => s.Query(q => q.QueryString(d => d.Query(searchTerm)))
                    .From((page - 1) * pageSize)
                    .Size(pageSize));
            return results.Documents;
        }

        public async Task Reindex()
        {
            await _elasticClient.DeleteByQueryAsync<RecipeDocument>(q => q.MatchAll());
            var recipes = _context.Recipes
                .Include(r => r.RecipeTags)
                .ThenInclude(rt => rt.Tag)
                .ToList()
                .Select(r => _mapper.Map<Recipe, RecipeDocument>(r));
            foreach (var recipe in recipes)
            {
                await _elasticClient.IndexDocumentAsync(recipe);
            }
        }
    }
}
