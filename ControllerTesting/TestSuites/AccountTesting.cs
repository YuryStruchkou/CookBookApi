using System.Net;
using System.Threading.Tasks;
using ControllerTesting.Helpers;
using ControllerTesting.Mocking;
using CookBook.CoreProject.Helpers;
using CookBook.Domain.Enums;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.AccountDtos;
using CookBook.Domain.ViewModels.AccountViewModels;
using CookBook.Presentation.Controllers;
using CookBook.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;

namespace ControllerTesting.TestSuites
{
    public class AccountTesting
    {
        private readonly AccountController _controller;
        private readonly ActionExecutingContext _context;

        public AccountTesting()
        {
            var mocker = new AccountMocking();
            _controller = mocker.SetupController();
            _context = mocker.SetupContext(_controller);
        }

        [Fact]
        public async Task RegisterOk()
        {
            var model = CreateDefaultRegisterViewModel();

            var json = (OkObjectResult)await _controller.Register(model);
            var data = (RegistrationResultDto)json.Value;

            Assert.Equal(model.Email, data.Email);
            Assert.Equal(model.UserName, data.UserName);
            Assert.False(data.IsMuted);
            Assert.Equal((int) UserStatuses.Active, data.UserStatusId);
        }

        private RegistrationViewModel CreateDefaultRegisterViewModel()
        {
            return new RegistrationViewModel
            {
                Email = "user@mailinator.com",
                UserName = "user",
                Password = "pass",
                ConfirmPassword = "pass"
            };
        }

        [Fact]
        public async Task RegisterUserExists()
        {
            var model = CreateDefaultRegisterViewModel();

            await _controller.Register(model);
            var json = (ConflictObjectResult)await _controller.Register(model);
            var error = (ErrorDto)json.Value;

            Assert.Equal((int)HttpStatusCode.Conflict, error.Code);
            Assert.Equal("User already exists.", error.Errors[0]);
        }

        [Fact]
        public void RegisterHasModelValidation()
        {
            Assert.True(AttributeHelper.IsModelValidationApplied(typeof(AccountController), "Register"));
        }

        [Fact]
        public void LoginHasModelValidation()
        {
            Assert.True(AttributeHelper.IsModelValidationApplied(typeof(AccountController), "Login"));
        }

        [Fact]
        public void ValidateUserPasswordsDontMatch()
        {
            var model = CreateDefaultRegisterViewModel();
            model.ConfirmPassword = "pass1";
            _context.ModelState.Validate(model);

            var error = ExecuteModelValidation();

            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.Equal("Passwords do not match.", error.Errors[0]);
        }

        private ErrorDto ExecuteModelValidation()
        {
            new ModelValidationAttribute().OnActionExecuting(_context);
            var json = (BadRequestObjectResult)_context.Result;
            return (ErrorDto)json.Value;
        }

        [Fact]
        public void ValidateInvalidEmail()
        {
            var model = CreateDefaultRegisterViewModel();
            model.Email = "user";
            _context.ModelState.Validate(model);

            var error = ExecuteModelValidation();

            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.Equal("Invalid email format.", error.Errors[0]);
        }

        [Fact]
        public void ValidateRegisterEmptyRequiredFields()
        {
            var model = new RegistrationViewModel();
            _context.ModelState.Validate(model);

            var error = ExecuteModelValidation();

            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.Contains("The Email field is required.", error.Errors);
            Assert.Contains("The Username field is required.", error.Errors);
            Assert.Contains("The Password field is required.", error.Errors);
            Assert.Contains("The Confirm password field is required.", error.Errors);
        }

        [Fact]
        public async Task TestLoginWithUserNameOk()
        {
            var model = CreateDefaultLoginViewModel();
            var json = (OkObjectResult)await _controller.Login(model);
            var data = (LoginResultDto) json.Value;
            AssertLoggedInSuccessfully(data);
        }

        private LoginViewModel CreateDefaultLoginViewModel()
        {
            return new LoginViewModel
            {
                UserNameOrEmail = "user1",
                Password = "pass"
            };
        }

        private void AssertLoggedInSuccessfully(LoginResultDto data)
        {
            Assert.NotNull(data.JwtToken);
            Assert.NotNull(data.UserName);
        }

        [Fact]
        public async Task TestLoginWithEmailOk()
        {
            var model = CreateDefaultLoginViewModel();
            model.UserNameOrEmail = "user1@mailinator.com";
            var json = (OkObjectResult)await _controller.Login(model);
            var data = (LoginResultDto)json.Value;
            AssertLoggedInSuccessfully(data);
        }

        [Fact]
        public async Task TestLoginWithIncorrectPassword()
        {
            var model = CreateDefaultLoginViewModel();
            model.Password = "*****************";
            var json = (BadRequestObjectResult)await _controller.Login(model);
            var error = (ErrorDto)json.Value;
            Assert.Equal("Incorrect username and/or password.", error.Errors[0]);
        }

        [Fact]
        public async Task TestLoginWithIncorrectUserName()
        {
            var model = CreateDefaultLoginViewModel();
            model.UserNameOrEmail = "Incorrect username";
            var json = (BadRequestObjectResult)await _controller.Login(model);
            var error = (ErrorDto)json.Value;
            Assert.Equal("Incorrect username and/or password.", error.Errors[0]);
        }
    }
}
