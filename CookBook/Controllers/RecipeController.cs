using System.Threading.Tasks;
using AutoMapper;
using CookBook.Domain.Models;
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

        public RecipeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeViewModel model)
        {
            var recipe = _mapper.Map<CreateRecipeViewModel, Recipe>(model);
            var result = _mapper.Map<Recipe, RecipeDto>(recipe);
            return new OkObjectResult(result);
        }
    }
}