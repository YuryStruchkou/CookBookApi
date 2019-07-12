using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Mappers;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.RecipeViewModels;
using CookBook.Presentation.Controllers;
using Moq;

namespace Testing.Mocking
{
    class RecipeControllerMocking : BaseMocking<RecipeController, RecipeProfile>
    {
        public override RecipeController Setup()
        {
            var mapper = SetupMapper();
            var recipeService = MockRecipeService().Object;
            return new RecipeController(mapper, recipeService);
        }

        private Mock<IRecipeService> MockRecipeService()
        {
            var mock = new Mock<IRecipeService>();
            mock.Setup(s => s.AddAsync(It.IsAny<CreateRecipeViewModel>())).ReturnsAsync(new Recipe());
            mock.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => id > 0 ? new Recipe { Id = id } : null );
            return mock;
        }
    }
}
