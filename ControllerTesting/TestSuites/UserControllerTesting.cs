using System.Threading.Tasks;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.UserDetailsDtos;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Testing.Mocking;
using Xunit;

namespace Testing.TestSuites
{
    public class UserControllerTesting
    {
        private readonly UserController _controller;
        private readonly UserControllerMocking _mocker;

        public UserControllerTesting()
        {
            _mocker = new UserControllerMocking();
            _controller = _mocker.Setup();
        }

        [Fact]
        public async Task GetUserOk()
        {
            var json = (OkObjectResult)await _controller.GetUser(1);
            var data = (UserDetailsDto) json.Value;

            Assert.Equal(UserControllerMocking.DefaultName, data.UserName);
        }

        [Fact]
        public async Task GetUserNotFound()
        {
            var json = (NotFoundObjectResult)await _controller.GetUser(0);
            var error = (ErrorDto)json.Value;

            Assert.Equal("User does not exist.", error.Errors[0]);
        }
    }
}
