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
        public static readonly int DefaultUserId = 1;

        public override RecipeController Setup()
        {
            var mapper = SetupMapper();
            var recipeService = MockRecipeService().Object;
            var userManager = MockUserManager().Object;
            var controller = new RecipeController(mapper, recipeService, userManager);
            return controller;
        }

        private Mock<IRecipeService> MockRecipeService()
        {
            var mock = new Mock<IRecipeService>();
            mock.Setup(s => s.AddAsync(It.IsAny<CreateUpdateRecipeViewModel>(), It.IsAny<int>()))
                .ReturnsAsync((CreateUpdateRecipeViewModel m, int id) => new Recipe { UserId = id, Id = 1 });
            mock.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => id > 0 ? new Recipe { Id = id, UserId = id } : null );
            mock.Setup(s => s.UpdateAsync(It.IsAny<CreateUpdateRecipeViewModel>(), It.IsAny<int>()))
                .ReturnsAsync((CreateUpdateRecipeViewModel m, int id) => new Recipe { Id = id, Name = m.Name });
            return mock;
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser {Id = DefaultUserId});
            return mock;
        }
    }
}
