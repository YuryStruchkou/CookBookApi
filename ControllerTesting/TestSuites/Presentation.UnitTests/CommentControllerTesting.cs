using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CookBook.CoreProject.Helpers;
using CookBook.Domain.ResultDtos;
using CookBook.Domain.ResultDtos.CommentDtos;
using CookBook.Domain.ResultDtos.RecipeDtos;
using CookBook.Domain.ViewModels.CommentViewModels;
using CookBook.Domain.ViewModels.RecipeViewModels;
using CookBook.Presentation.Controllers;
using CookBook.Presentation.ObjectResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using Testing.Helpers;
using Testing.Mocking;
using Testing.Mocking.Presentation;
using Xunit;

namespace Testing.TestSuites.Presentation.UnitTests
{
    public class CommentControllerTesting
    {
        private readonly CommentController _controller;
        private readonly ActionExecutingContext _context;
        private readonly CommentControllerMocking _mocker;

        public CommentControllerTesting()
        {
            _mocker = new CommentControllerMocking();
            _controller = _mocker.Setup();
            _context = _mocker.SetupContext(_controller);
        }

        [Fact]
        public async Task CreateCommentOk()
        {
            var model = CreateDefaultCreateCommentViewModel();
            var json = (OkObjectResult)await _controller.CreateComment(model);
            var result = (CommentDetailsDto)json.Value;
            Assert.NotNull(result);
        }

        private CreateUpdateCommentViewModel CreateDefaultCreateCommentViewModel()
        {
            return new CreateUpdateCommentViewModel
            {
                Content = MockConstants.DefaultComment.Content
            };
        }

        [Fact]
        public void CreateCommentHasModelValidation()
        {
            Assert.True(AttributeHelper.IsModelValidationApplied(typeof(CommentController), "CreateComment"));
        }

        [Fact]
        public void CreateCommentInvalidModel()
        {
            var model = new CreateUpdateCommentViewModel();
            _context.ModelState.Validate(model);

            var error = AttributeHelper.ExecuteModelValidation(_context);

            Assert.Equal((int)HttpStatusCode.BadRequest, error.Code);
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public async Task GetCommentOk()
        {
            var json = (OkObjectResult)await _controller.GetComment(5);
            var result = (CommentDetailsDto)json.Value;

            Assert.Equal(5, result.Id);
        }

        [Fact]
        public async Task GetCommentNotFound()
        {
            var json = (NotFoundObjectResult)await _controller.GetComment(-5);
            var error = (ErrorDto)json.Value;

            Assert.Equal((int)HttpStatusCode.NotFound, error.Code);
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public async Task UpdateCommentOk()
        {
            var model = CreateDefaultCreateCommentViewModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = SetupClaimsPrincipal() }
            };

            var json = (OkObjectResult)await _controller.UpdateComment(model, 1);
            var result = (CommentDetailsDto)json.Value;

            Assert.NotNull(result);
        }

        private ClaimsPrincipal SetupClaimsPrincipal(string role = "User")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task UpdateCommentWrongUserId()
        {
            var model = CreateDefaultCreateCommentViewModel();
            var user = SetupClaimsPrincipal();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var json = (ForbiddenObjectResult)await _controller.UpdateComment(model, 2);
            var error = (ErrorDto)json.Value;

            Assert.Equal((int)HttpStatusCode.Forbidden, error.Code);
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public async Task UpdateCommentNotFound()
        {
            var model = CreateDefaultCreateCommentViewModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = SetupClaimsPrincipal() }
            };

            var json = (NotFoundObjectResult)await _controller.UpdateComment(model, 0);
            var error = (ErrorDto)json.Value;

            Assert.Equal((int)HttpStatusCode.NotFound, error.Code);
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public void UpdateRecipeHasModelValidation()
        {
            Assert.True(AttributeHelper.IsModelValidationApplied(typeof(CommentController), "UpdateComment"));
        }

        [Fact]
        public void DeleteCommentHasModelValidation()
        {
            Assert.True(AttributeHelper.IsModelValidationApplied(typeof(CommentController), "DeleteComment"));
        }

        [Fact]
        public async Task DeleteCommentOk()
        {
            var user = SetupClaimsPrincipal();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var actionResult = (NoContentResult)await _controller.DeleteComment(1);

            _mocker.CommentServiceMock.Verify(m => m.DeleteAsync(1), Times.Once);
            Assert.NotNull(actionResult);
        }

        [Fact]
        public async Task DeleteCommentNotFound()
        {
            var json = (NotFoundObjectResult)await _controller.DeleteComment(-1);
            var error = (ErrorDto)json.Value;

            Assert.Equal((int)HttpStatusCode.NotFound, error.Code);
            Assert.NotEmpty(error.Errors);
        }

        [Fact]
        public async Task DeleteCommentWrongUserId()
        {
            var user = SetupClaimsPrincipal();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var json = (ForbiddenObjectResult)await _controller.DeleteComment(2);
            var error = (ErrorDto)json.Value;

            Assert.Equal((int)HttpStatusCode.Forbidden, error.Code);
            Assert.NotEmpty(error.Errors);
        }
    }
}
