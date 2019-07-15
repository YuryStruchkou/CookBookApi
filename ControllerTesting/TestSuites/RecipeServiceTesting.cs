﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBook.BLL.Services;
using CookBook.DAL.Data;
using CookBook.Domain.Enums;
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

            var result = await _service.AddAsync(model, 1);

            Assert.Equal(model.Name, result.Name);
            Assert.Equal(model.Description, result.Description);
            Assert.Equal(model.Content, result.Content);
            var resultTags = result.RecipeTags.Select(rt => rt.Tag.Content).ToList();
            model.Tags.Sort();
            resultTags.Sort();
            Assert.Equal(model.Tags, resultTags);
            Assert.NotEqual(default(DateTime), result.CreationDate);
            Assert.Equal(RecipeStatus.Active, result.RecipeStatus);
            Assert.Equal(1, result.UserId);
        }

        private CreateUpdateRecipeViewModel CreateDefaultCreateRecipeViewModel()
        {
            return new CreateUpdateRecipeViewModel
            {
                Name = "Name",
                Description = "A short description",
                Content = "A very very long description",
                Tags = new List<string>() {"tag1", "tag2"}
            };
        }

        [Fact]
        public async Task GetRecipeOk()
        {
            var model = CreateDefaultCreateRecipeViewModel();
            var addedRecipe = await _service.AddAsync(model, 1);

            var result = await _service.GetAsync(addedRecipe.Id);

            Assert.Equal(addedRecipe.Id, result.Id);
            Assert.Equal(addedRecipe.Name, result.Name);
            Assert.Equal(addedRecipe.Description, result.Description);
            Assert.Equal(addedRecipe.Content, result.Content);
            Assert.Equal(addedRecipe.RecipeTags.Select(rt => rt.Tag.Content).ToList(), result.RecipeTags.Select(rt => rt.Tag.Content).ToList());
            Assert.Equal(addedRecipe.CreationDate, result.CreationDate);
            Assert.Equal(addedRecipe.UserId, result.UserId);
        }

        [Fact]
        public async Task GetRecipeNotFound()
        {
            var result = await _service.GetAsync(5);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateRecipeOk()
        {
            var model = CreateDefaultCreateRecipeViewModel();
            var addedRecipe = await _service.AddAsync(model, 1);
            model.Name = "Another name";
            model.Description = "New description";
            model.Content = "Another content";
            model.Tags = new List<string> { "NewTag1", "tag2" };

            var result = await _service.UpdateAsync(model, addedRecipe.Id);

            Assert.Equal(model.Name, result.Name);
            Assert.Equal(model.Description, result.Description);
            Assert.Equal(model.Content, result.Content);
            var resultTags = result.RecipeTags.Select(rt => rt.Tag.Content).ToList();
            model.Tags.Sort();
            resultTags.Sort();
            Assert.Equal(model.Tags, resultTags);
            Assert.NotEqual(default(DateTime), result.CreationDate);
            Assert.NotNull(result.EditDate);
            Assert.Equal(RecipeStatus.Active, result.RecipeStatus);
            Assert.Equal(1, result.UserId);
        }
    }
}
