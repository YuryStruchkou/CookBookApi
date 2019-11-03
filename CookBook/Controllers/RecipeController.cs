using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Constants;
using CookBook.CoreProject.Helpers;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.RecipeDtos;
using CookBook.Domain.ViewModels.RecipeViewModels;
using CookBook.Presentation.Filters;
using CookBook.Presentation.ObjectResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Presentation.Controllers
{
    [Route("api/recipe")]
    [ApiController]
    [ModelValidation]
    public class RecipeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipeController(IMapper mapper, IRecipeService recipeService, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _recipeService = recipeService;
            _userManager = userManager;
        }

        [HttpPost, Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateUpdateRecipeViewModel model)
        {
            var userId = await _userManager.GetCurrentUserIdAsync(User);
            var recipe = await _recipeService.AddAsync(model, userId);
            var result = _mapper.Map<Recipe, RecipeDetailsDto>(recipe);
            return new OkObjectResult(result);
        }

        [HttpGet, Route("{id}"), Authorize(AuthenticationSchemes = "Bearer"), AllowAnonymous]
        public async Task<IActionResult> GetRecipe([FromRoute] int id)
        { 
            var recipe = await _recipeService.GetAsync(id);
            var result = _mapper.Map<Recipe, RecipeDetailsDto>(recipe);
            if (result != null)
            {
                result.Comments = result.Comments?.OrderByDescending(c => c.CreationDate).ToList();
            }
            return result != null
                ? (IActionResult) new OkObjectResult(result)
                : new NotFoundObjectResult(new ErrorDto((int) HttpStatusCode.NotFound, "Recipe not found."));
        }

        [HttpPut, Route("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateRecipe([FromBody] CreateUpdateRecipeViewModel model, [FromRoute] int id)
        {
            var recipe = await _recipeService.GetAsync(id);
            if (recipe == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int)HttpStatusCode.NotFound, "Recipe not found."));
            }
            if (!User.IsInRole(UserRoleNames.Admin) && recipe.UserId != await _userManager.GetCurrentUserIdAsync(User))
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "User id does not match."));
            }
            recipe = await _recipeService.UpdateAsync(model, recipe.Id);
            var result = _mapper.Map<Recipe, RecipeDetailsDto>(recipe);
            return new OkObjectResult(result);
        }

        [HttpDelete, Route("{id}"), Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> MarkRecipeAsDeleted([FromRoute] int id)
        {
            var recipe = await _recipeService.GetAsync(id);
            if (recipe == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int) HttpStatusCode.NotFound, "Recipe not found."));
            }
            if (!User.IsInRole(UserRoleNames.Admin) && recipe.UserId != await _userManager.GetCurrentUserIdAsync(User))
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "User id does not match."));
            }
            await _recipeService.MarkAsDeletedAsync(id);
            return new NoContentResult();
        }

        [HttpPost, Route("{id}/vote"), Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddVote([FromRoute] int id, [FromQuery] int value)
        {
            var userId = await _userManager.GetCurrentUserIdAsync(User);
            if (!userId.HasValue)
            {
                return new ForbiddenObjectResult(new ErrorDto((int)HttpStatusCode.Forbidden, "User does not exist."));
            }
            var vote = await _recipeService.AddVoteAsync(id, userId.Value, value);
            var result = _mapper.Map<Recipe, RecipeVoteDto>(vote.Recipe);
            return new OkObjectResult(result);
        }

        [HttpGet, Route("{id}/vote"), Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCurrentUserVote([FromRoute] int id)
        {
            var recipe = await _recipeService.GetAsync(id);
            var userId = await _userManager.GetCurrentUserIdAsync(User);
            var voteValue = recipe != null && userId.HasValue ? recipe.Votes.FirstOrDefault(v => v.UserId == userId)?.Value : null;
            return new OkObjectResult(new CurrentUserVoteDto(voteValue));
        }

        [HttpGet, Route("popular"), AllowAnonymous]
        public async Task<IActionResult> GetPopularRecipes([FromQuery] int count)
        {
            var recipes = _recipeService.GetPopularRecipesAsync(count).ToList();
            var resultDtos = recipes.Select(r => _mapper.Map<Recipe, RecipeBriefDto>(r));
            return new OkObjectResult(resultDtos.ToList());
        }

        [HttpGet, Route("recent"), AllowAnonymous]
        public async Task<IActionResult> GetRecentRecipes([FromQuery] int count)
        {
            var recipes = _recipeService.GetRecentRecipesAsync(count).ToList();
            var resultDtos = recipes.Select(r => _mapper.Map<Recipe, RecipeBriefDto>(r));
            return new OkObjectResult(resultDtos.ToList());
        }
    }
}