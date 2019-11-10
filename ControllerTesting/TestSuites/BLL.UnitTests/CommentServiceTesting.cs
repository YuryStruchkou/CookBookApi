using System;
using System.Threading.Tasks;
using CookBook.BLL.Services;
using CookBook.DAL.Data;
using CookBook.Domain.ViewModels.CommentViewModels;
using CookBook.Domain.ViewModels.RecipeViewModels;
using Microsoft.EntityFrameworkCore;
using Testing.Mocking;
using Testing.Mocking.BLL;
using Xunit;

namespace Testing.TestSuites.BLL.UnitTests
{
    public class CommentServiceTesting : IDisposable, IAsyncLifetime
    {
        private CommentService _service;
        private CommentServiceMocking _mocker;
        private int _recipeId;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("testComments")
                .EnableSensitiveDataLogging()
                .Options;
            _mocker = new CommentServiceMocking(options);
            var recipeService = new RecipeServiceMocking(options).Setup();
            _recipeId = (await recipeService.AddAsync(new CreateUpdateRecipeViewModel
            {
                Name = "a",
                Content = "a",
                Description = "a"
            }, 1)).Id;
            _service = _mocker.Setup();
        }

        [Fact]
        public async Task CreateCommentOk()
        {
            var model = CreateDefaultCreateCommentViewModel();

            var result = await _service.AddAsync(model, 1);

            Assert.Equal(model.Content, result.Content);
            Assert.Equal(model.RecipeId, result.RecipeId);
            Assert.NotEqual(default, result.CreationDate);
            Assert.Equal(1, result.UserId);
        }

        private CreateCommentViewModel CreateDefaultCreateCommentViewModel()
        {
            return new CreateCommentViewModel
            {
                Content = MockConstants.DefaultComment.Content,
                RecipeId = _recipeId
            };
        }

        [Fact]
        public async Task GetCommentOk()
        {
            var model = CreateDefaultCreateCommentViewModel();
            var addedComment = await _service.AddAsync(model, 1);

            var result = await _service.GetAsync(addedComment.Id);

            Assert.Equal(addedComment.Id, result.Id);
            Assert.Equal(addedComment.Content, result.Content);
            Assert.Equal(addedComment.CreationDate, result.CreationDate);
            Assert.Equal(addedComment.UserId, result.UserId);
            Assert.Equal(addedComment.RecipeId, result.RecipeId);
        }

        [Fact]
        public async Task GetCommentNotFound()
        {
            var result = await _service.GetAsync(5);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCommentOk()
        {
            var model = CreateDefaultCreateCommentViewModel();
            var addedComment = await _service.AddAsync(model, 1);
            var updateModel = CreateDefaultUpdateCommentViewModel();

            var result = await _service.UpdateAsync(updateModel, addedComment.Id);

            Assert.Equal(updateModel.Content, result.Content);
            Assert.NotEqual(default, result.CreationDate);
            Assert.NotNull(result.EditDate);
            Assert.Equal(1, result.UserId);
            Assert.Equal(model.RecipeId, result.RecipeId);
        }

        private UpdateCommentViewModel CreateDefaultUpdateCommentViewModel()
        {
            return new UpdateCommentViewModel
            {
                Content = "Another content"
            };
        }

        [Fact]
        public async Task UpdateCommentNotFound()
        {
            var model = CreateDefaultCreateCommentViewModel();
            var addedComment = await _service.AddAsync(model, 1);
            var updateModel = CreateDefaultUpdateCommentViewModel();

            var result = await _service.UpdateAsync(updateModel, addedComment.Id + 10);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteCommentOk()
        {
            var model = CreateDefaultCreateCommentViewModel();
            var addedComment = await _service.AddAsync(model, 1);
            var id = addedComment.Id;

            bool result = await _service.DeleteAsync(id);

            Assert.True(result);
            Assert.Null(await _service.GetAsync(id));
        }

        [Fact]
        public async Task DeleteCommentNotFound()
        {
            var model = CreateDefaultCreateCommentViewModel();
            var addedRecipe = await _service.AddAsync(model, 1);
            var id = addedRecipe.Id;

            bool result = await _service.DeleteAsync(4000);

            Assert.False(result);
            Assert.NotNull(await _service.GetAsync(id));
        }

        public void Dispose()
        {
            _mocker.ClearContext();
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}
