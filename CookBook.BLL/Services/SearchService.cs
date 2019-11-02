using System.Collections.Generic;
using System.Threading.Tasks;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.ElasticSearch;
using Nest;

namespace CookBook.BLL.Services
{
    public class SearchService : ISearchService
    {
        private readonly IElasticClient _elasticClient;

        public SearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
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
    }
}
