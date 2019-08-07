using System;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.Mappers;
using CookBook.Domain.Models;
using CookBook.Presentation.Controllers;
using Moq;

namespace Testing.Mocking
{
    class UserControllerMocking : BaseMocking<UserController, UserDetailsProfile>
    {
        public static readonly string DefaultName = "User";

        public Mock<IUserService> UserServiceMock { get; } = new Mock<IUserService>();

        public override UserController Setup()
        {
            var mapper = SetupMapper();
            var userService = MockUserService().Object;
            return new UserController(userService, mapper);
        }

        private Mock<IUserService> MockUserService()
        {
            UserServiceMock.Setup(u => u.GetAsync(It.Is<int>(id => id > 0)))
                .ReturnsAsync((int id) => new UserProfile { UserId = id, ApplicationUser = new ApplicationUser { Id = id, UserName = DefaultName }});
            UserServiceMock.Setup(u => u.GetAsync(It.Is<int>(id => id <= 0)))
                .ReturnsAsync((int id) => null);
            return UserServiceMock;
        }
    }
}
