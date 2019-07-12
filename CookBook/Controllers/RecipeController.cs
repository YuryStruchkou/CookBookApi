using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.RecipeDtos;
using CookBook.Domain.ViewModels.RecipeViewModels;
using CookBook.Presentation.Filters;
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
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeViewModel model)
        {
            var username = _userManager.GetUserName(User);
            var userId = (await _userManager.FindByNameAsync(username))?.Id;
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
    }
}