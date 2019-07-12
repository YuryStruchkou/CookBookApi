using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CookBook.CoreProject.Helpers;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.RecipeDtos;
using CookBook.Domain.ViewModels.RecipeViewModels;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Testing.Helpers;
using Testing.Mocking;
using Xunit;

namespace Testing.TestSuites
{
    public class RecipeControllerTesting
    {
        private readonly RecipeController _controller;
        private readonly ActionExecutingContext _context;

        public RecipeControllerTesting()
        {
            var mocker = new RecipeControllerMocking();
            _controller = mocker.Setup();
            _context = mocker.SetupContext(_controller);
        }

        [Fact]
        public async Task CreateRecipeOk()
        {
            var model = CreateDefaultCreateRecipeViewModel();
            var json = (OkObjectResult)await _controller.CreateRecipe(model);
            var result = (RecipeDto)json.Value;
            Assert.NotNull(result);
        }

        private CreateRecipeViewModel CreateDefaultCreateRecipeViewModel()
        {
            return new CreateRecipeViewModel
            {
                Name = "Name",
                Description = "A short description",
                Content = "A very very long description",
                Tags = new List<string> { "tag1", "tag2" }
            };
        }

        [Fact]
        public void CreateRecipeHasModelValidation()
        {
            Assert.True(AttributeHelper.IsModelValidationApplied(typeof(RecipeController), "CreateRecipe"));
        }

        [Fact]
        public void CreateRecipeInvalidModel()
        {
            var model = new CreateRecipeViewModel();
            _context.ModelState.Validate(model);

            var error = AttributeHelper.ExecuteModelValidation(_context);

            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.Contains("The Name field is required.", error.Errors);
            Assert.Contains("The Description field is required.", error.Errors);
            Assert.Contains("The Content field is required.", error.Errors);
        }

        [Fact]
        public async Task GetRecipeOk()
        {
            var json = (OkObjectResult)await _controller.GetRecipe(5);
            var result = (RecipeDto) json.Value;

            Assert.Equal(5, result.Id);
        }

        [Fact]
        public async Task GetRecipeNotFound()
        {
            var json = (NotFoundObjectResult)await _controller.GetRecipe(-5);
            var error = (ErrorDto)json.Value;

            Assert.Equal((int)HttpStatusCode.NotFound, error.Code);
            Assert.Contains("Recipe not found.", error.Errors);
        }
    }
}
