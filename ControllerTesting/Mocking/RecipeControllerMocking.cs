using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Mappers;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.RecipeViewModels;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Testing.Mocking
{
    class RecipeControllerMocking : BaseMocking<RecipeController, RecipeProfile>
    {
        public override RecipeController Setup()
        {
            var mapper = SetupMapper();
            var recipeService = MockRecipeService().Object;
            var userManager = MockUserManager().Object;
            return new RecipeController(mapper, recipeService, userManager);
        }

        private Mock<IRecipeService> MockRecipeService()
        {
            var mock = new Mock<IRecipeService>();
            mock.Setup(s => s.AddAsync(It.IsAny<CreateRecipeViewModel>(), It.IsAny<int>())).ReturnsAsync((CreateRecipeViewModel m, int id) => new Recipe { UserId = id });
            mock.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => id > 0 ? new Recipe { Id = id } : null );
            return mock;
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser {Id = 1});
            return mock;
        }
    }
}
