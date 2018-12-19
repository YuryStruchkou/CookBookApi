using CookBook.Domain.Helpers;
using CookBook.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Domain.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeStatus> RecipeStatuses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.SetupPrimaryKeys();
            builder.SetupAlternateKeys();
            builder.SetupRequiredColumns();
            builder.SetupRelations();
        }
    }
}
