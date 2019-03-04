using System.Threading.Tasks;
using ControllerTesting.Mocking;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace ControllerTesting.TestSuites
{
    public class AccountTesting
    {
        [Fact]
        public async Task RegisterOk()
        {
            var controller = new AccountMocking().SetupController();
            var model = new RegistrationViewModel
            {
                Email = "user@mailinator.com",
                Username = "user",
                Password = "pass",
                ConfirmPassword = "pass"
            };
            var json = await controller.Register(model);
            var data = (ApplicationUser)json.Value;
            Assert.Equal(model.Email, data.Email);
            Assert.Equal(model.Username, data.UserName);
        }

        
    }
}
