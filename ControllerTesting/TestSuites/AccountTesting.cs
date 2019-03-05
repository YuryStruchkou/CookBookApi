using System.Threading.Tasks;
using ControllerTesting.Mocking;
using CookBook.Domain.Enums;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ViewModels;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ControllerTesting.TestSuites
{
    public class AccountTesting
    {
        private readonly AccountController _controller;

        public AccountTesting()
        {
            _controller = new AccountMocking().SetupController();
        }

        [Fact]
        public async Task RegisterOk()
        {
            var model = new RegistrationViewModel
            {
                Email = "user@mailinator.com",
                UserName = "user",
                Password = "pass",
                ConfirmPassword = "pass"
            };

            var json = (OkObjectResult)await _controller.Register(model);
            var data = (RegistrationResultDto)json.Value;

            Assert.Equal(model.Email, data.Email);
            Assert.Equal(model.UserName, data.UserName);
            Assert.False(data.IsMuted);
            Assert.Equal((int) UserStatuses.Active, data.UserStatusId);
        }

        
    }
}
