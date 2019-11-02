using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CookBook.CoreProject.Helpers;
using CookBook.Domain.Enums;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.AccountDtos;
using CookBook.Domain.ViewModels.AccountViewModels;
using CookBook.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using Testing.Helpers;
using Testing.Mocking;
using Testing.Mocking.Presentation;
using Xunit;

namespace Testing.TestSuites.Presentation.UnitTests
{
    public class AccountTesting
    {
        private readonly AccountController _controller;
        private readonly ActionExecutingContext _context;
        private readonly AccountMocking _mocker;

        public AccountTesting()
        {
            _mocker = new AccountMocking();
            _controller = _mocker.Setup();
            _context = _mocker.SetupContext(_controller);
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
            Assert.Equal(UserStatus.Active.ToString(), data.UserStatus);
        }

        private RegistrationViewModel CreateDefaultRegisterViewModel()
        {
            return new RegistrationViewModel
            {
                Email = MockConstants.DefaultUser.Email,
                UserName = MockConstants.DefaultUser.UserName,
                Password = MockConstants.DefaultPassword,
                ConfirmPassword = MockConstants.DefaultPassword
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
            Assert.NotEmpty(error.Errors);
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
            model.ConfirmPassword += "1";
            _context.ModelState.Validate(model);

            var error = AttributeHelper.ExecuteModelValidation(_context);

            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public void ValidateInvalidEmail()
        {
            var model = CreateDefaultRegisterViewModel();
            model.Email = "Invalid email";
            _context.ModelState.Validate(model);

            var error = AttributeHelper.ExecuteModelValidation(_context);

            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public void ValidateRegisterEmptyRequiredFields()
        {
            var model = new RegistrationViewModel();
            _context.ModelState.Validate(model);

            var error = AttributeHelper.ExecuteModelValidation(_context);

            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.NotEmpty(error.Errors);
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
                UserNameOrEmail = MockConstants.DefaultUser.UserName,
                Password = MockConstants.DefaultPassword
            };
        }

        private void AssertLoggedInSuccessfully(LoginResultDto data)
        {
            Assert.NotNull(data.JwtToken);
            Assert.NotEqual(default, data.ExpiryDate);
            Assert.NotNull(data.UserName);
        }

        [Fact]
        public async Task TestLoginWithEmailOk()
        {
            var model = CreateDefaultLoginViewModel();
            model.UserNameOrEmail = MockConstants.DefaultUser.Email;
            var json = (OkObjectResult)await _controller.Login(model);
            var data = (LoginResultDto)json.Value;
            AssertLoggedInSuccessfully(data);
        }

        [Fact]
        public async Task TestLoginWithIncorrectPassword()
        {
            var model = CreateDefaultLoginViewModel();
            model.Password = "Incorrect password";
            var json = (BadRequestObjectResult)await _controller.Login(model);
            var error = (ErrorDto)json.Value;
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public async Task TestLoginWithIncorrectUserName()
        {
            var model = CreateDefaultLoginViewModel();
            model.UserNameOrEmail = "Incorrect username";
            var json = (BadRequestObjectResult)await _controller.Login(model);
            var error = (ErrorDto)json.Value;
            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public async Task RefreshTokenOk()
        {
            var model = CreateDefaultRefreshTokenViewModel();
            var token = MockConstants.JwtToken;
            _mocker.MockedUserManager.Setup(m => m.FindByNameAsync("user1")).ReturnsAsync(new ApplicationUser
            {
                UserName = MockConstants.DefaultUser.UserName,
                RefreshTokens = new List<RefreshToken> { new RefreshToken { Token = token, ExpiryDate = DateTime.Now.AddDays(1) } }
            });

            var json = (OkObjectResult)await _controller.Refresh(model);
            var data = (LoginResultDto)json.Value;

            AssertLoggedInSuccessfully(data);
        }

        private RefreshTokenLogoutViewModel CreateDefaultRefreshTokenViewModel()
        {
            return new RefreshTokenLogoutViewModel
            {
                UserName = MockConstants.DefaultUser.UserName
            };
        }

        [Fact]
        public async Task RefreshTokenIncorrectToken()
        {
            var model = CreateDefaultRefreshTokenViewModel();
            var json = (UnauthorizedObjectResult)await _controller.Refresh(model);
            var error = (ErrorDto)json.Value;
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public async Task RefreshTokenExpiredToken()
        {
            var model = CreateDefaultRefreshTokenViewModel();
            _mocker.MockedUserManager.Setup(m => m.FindByNameAsync(MockConstants.DefaultUser.UserName)).ReturnsAsync(new ApplicationUser
            {
                RefreshTokens = new List<RefreshToken>
                {
                    new RefreshToken { ExpiryDate = DateTime.Now.AddDays(-1), Token = MockConstants.JwtToken }
                }
            });

            var json = (UnauthorizedObjectResult)await _controller.Refresh(model);
            var error = (ErrorDto)json.Value;
            Assert.NotEmpty(error.Errors);
            _mocker.MockedUserManager.Verify(u => u.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [Fact]
        public async Task LogoutOk()
        {
            var model = CreateDefaultRefreshTokenViewModel();
            var token = MockConstants.JwtToken;
            _mocker.MockedUserManager.Setup(m => m.FindByNameAsync(MockConstants.DefaultUser.UserName)).ReturnsAsync(new ApplicationUser
            {
                UserName = MockConstants.DefaultUser.UserName,
                RefreshTokens = new List<RefreshToken> { new RefreshToken { Token = token, ExpiryDate = DateTime.Now.AddDays(1) } }
            });

            var result = (NoContentResult)await _controller.Logout(model);

            Assert.NotNull(result);
        }
    }
}
