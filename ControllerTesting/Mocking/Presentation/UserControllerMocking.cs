using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Mappers;
using CookBook.Domain.Models;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Testing.Mocking.Presentation
{
    class UserControllerMocking : BaseMocking<UserController, UserDetailsProfile>
    {
        private static readonly int DefaultUserId = 1;

        public static readonly string DefaultName = "User";

        public Mock<IUserService> UserServiceMock { get; } = new Mock<IUserService>();

        public Mock<UserManager<ApplicationUser>> UserManagerMock { get; } = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object,
            null, null, null, null, null, null, null, null);

        public override UserController Setup()
        {
            var mapper = SetupMapper();
            var userService = MockUserService().Object;
            var userManager = MockUserManager().Object;
            return new UserController(userService, mapper, userManager);
        }

        private Mock<IUserService> MockUserService()
        {
            UserServiceMock.Setup(u => u.GetAsync(It.Is<int>(id => id > 0)))
                .ReturnsAsync((int id) => new UserProfile { UserId = id, ApplicationUser = new ApplicationUser { Id = id, UserName = DefaultName }});
            UserServiceMock.Setup(u => u.GetAsync(It.Is<int>(id => id <= 0)))
                .ReturnsAsync((int id) => null);
            return UserServiceMock;
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            UserManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser { Id = DefaultUserId });
            return UserManagerMock;
        }
    }
}
