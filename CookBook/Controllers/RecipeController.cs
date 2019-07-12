using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.RecipeDtos;
using CookBook.Domain.ViewModels.RecipeViewModels;
using CookBook.Presentation.Filters;
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

        public RecipeController(IMapper mapper, IRecipeService recipeService)
        {
            _mapper = mapper;
            _recipeService = recipeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeViewModel model)
        {
            var recipe = await _recipeService.AddAsync(model);
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