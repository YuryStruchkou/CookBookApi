using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Constants;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.ElasticSearch;
using CookBook.Domain.Helpers;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.RecipeDtos;
using CookBook.Presentation.ObjectResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Presentation.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISearchService _searchService;
        private readonly IRecipeService _recipeService;

        public SearchController(IMapper mapper, ISearchService searchService, IRecipeService recipeService)
        {
            _mapper = mapper;
            _searchService = searchService;
            _recipeService = recipeService;
        }

        [HttpGet, Route(""), AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new OkObjectResult(new List<RecipeBriefDto>());
            }
            var results = await _searchService.SearchAsync(query, page, pageSize);
            var dtos = results.Select(r => _mapper.Map<RecipeDocument, RecipeBriefDto>(r)).ToList();
            foreach (var dto in dtos)
            {
                dto.AverageVote = (await _recipeService.GetAsync(dto.Id)).GetAverageVote();
            }
            return new OkObjectResult(dtos.ToList());
        }

        [HttpPost, Route("reindex"), Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Reindex()
        {
            if (!User.IsInRole(UserRoleNames.Admin))
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "User must be an administrator."));
            }
            await _searchService.Reindex();
            return NoContent();
        }
    }
}