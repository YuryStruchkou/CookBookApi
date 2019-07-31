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
        private static readonly int DefaultUserId = 1;

        public Mock<IRecipeService> RecipeServiceMock { get; } = new Mock<IRecipeService>();

        public Mock<UserManager<ApplicationUser>> UserManagerMock { get; } = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object,
            null, null, null, null, null, null, null, null);

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
            RecipeServiceMock.Setup(s => s.AddAsync(It.IsAny<CreateUpdateRecipeViewModel>(), It.IsAny<int>()))
                .ReturnsAsync((CreateUpdateRecipeViewModel m, int id) => new Recipe { UserId = id, Id = 1 });
            RecipeServiceMock.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => id > 0 ? new Recipe { Id = id, UserId = id } : null );
            RecipeServiceMock.Setup(s => s.UpdateAsync(It.IsAny<CreateUpdateRecipeViewModel>(), It.IsAny<int>()))
                .ReturnsAsync((CreateUpdateRecipeViewModel m, int id) => new Recipe { Id = id, Name = m.Name });
            RecipeServiceMock.Setup(s => s.MarkAsDeletedAsync(It.IsAny<int>())).ReturnsAsync((int id) => id > 0);
            RecipeServiceMock.Setup(s => s.AddVoteAsync(It.Is<int>(id => id > 0), It.Is<int>(id => id > 0), It.Is<int>(id => id > 0)))
                .ReturnsAsync((int recipeId, int userId, int voteValue) => new Vote{ RecipeId = recipeId, UserId = userId, Value = voteValue });
            return RecipeServiceMock;
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            UserManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser {Id = DefaultUserId});
            return UserManagerMock;
        }
    }
}
