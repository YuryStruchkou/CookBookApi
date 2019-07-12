using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBook.BLL.Services;
using CookBook.DAL.Data;
using CookBook.Domain.ViewModels.RecipeViewModels;
using Microsoft.EntityFrameworkCore;
using Testing.Mocking;
using Xunit;

namespace Testing.TestSuites
{
    public class RecipeServiceTesting
    {
        private readonly RecipeService _service;

        public RecipeServiceTesting()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test")
                .EnableSensitiveDataLogging()
                .Options;
            var mocker = new RecipeServiceMocking(options);
            _service = mocker.Setup();
        }

        [Fact]
        public async Task CreateRecipeOk()
        {
            var model = CreateDefaultCreateRecipeViewModel();

            var result = await _service.AddAsync(model);

            Assert.Equal(model.Name, result.Name);
            Assert.Equal(model.Description, result.Description);
            Assert.Equal(model.Content, result.Content);
            Assert.Equal(model.Tags, result.RecipeTags.Select(rt => rt.Tag.Content).ToList());
        }

        private CreateRecipeViewModel CreateDefaultCreateRecipeViewModel()
        {
            return new CreateRecipeViewModel
            {
                Name = "Name",
                Description = "A short description",
                Content = "A very very long description",
                Tags = new List<string>() {"tag1", "tag2"}
            };
        }
    }
}
