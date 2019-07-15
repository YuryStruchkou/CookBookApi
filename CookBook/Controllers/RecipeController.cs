using System.Net;
using System.Security.Claims;
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
            var result = _mapper.Map<Recipe, RecipeDto>(recipe);
            return new OkObjectResult(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetRecipe([FromRoute] int id)
        {
            var recipe = await _recipeService.GetAsync(id);
            var result = _mapper.Map<Recipe, RecipeDto>(recipe);
            if (result == null)
            {
                return new NotFoundObjectResult(new ErrorDto((int) HttpStatusCode.NotFound, "Recipe not found."));
            }
            return new OkObjectResult(result);
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
            var result = _mapper.Map<Recipe, RecipeDto>(recipe);
            return new OkObjectResult(result);
        }
    }
}