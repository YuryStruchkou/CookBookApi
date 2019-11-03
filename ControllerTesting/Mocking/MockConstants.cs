using System.Collections.Generic;
using CookBook.Domain.Models;

namespace Testing.Mocking
{
    static class MockConstants
    {
        public const string JwtToken = "token";
        public const string JwtIssuer = "https://localhost:44342/";
        public const string JwtKey = "gyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy";
        public const int JwtValidForMinutes = 60;
        public const int JwtRefreshTokenBytes = 128;
        public const int JwtRefreshTokenValidForDays = 60;
        public const string DefaultPassword = "pass";
        public const int TotalNumberOfRecipes = 8;
        
        public static readonly ApplicationUser DefaultUser = new ApplicationUser
        {
            UserName = "user1",
            Email = "user1@mailinator.com",
            PasswordHash = "pass"
        };

        public static readonly Recipe DefaultRecipe = new Recipe
        {
            Name = "Name",
            Description = "A short description",
            Content = "A very very long description"
        };

        public static readonly Comment DefaultComment = new Comment
        {
            Content = "Good stuff"
        };

        public static readonly List<string> DefaultTags = new List<string> {"tag1", "tag2"};
    }
}
