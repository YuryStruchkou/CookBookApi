using ControllerTesting.Mocking;
using CookBook.Domain.Models;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace ControllerTesting.TestSuites
{
    public class AccountTesting
    {
        [Fact]
        public void RegisterOk()
        {
            var controller = new AccountMocking().SetupController();
            var json = controller.Register();
            Assert.Null(json);
        }

        
    }
}
