using CookBook.CoreProject.Constants;
using CookBook.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Domain.Helpers
{
    static class ModelBuilderHelper
    {
        public static void SetupPrimaryKeys(this ModelBuilder builder)
        {
            builder.Entity<UserProfile>()
                .HasKey(u => u.UserId);

            builder.Entity<Recipe>()
                .HasKey(r => r.Id);
            builder.Entity<Recipe>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Comment>()
                .HasKey(c => c.Id);
            builder.Entity<Comment>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Tag>()
                .HasKey(t => t.Id);
            builder.Entity<Tag>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Vote>()
                .HasKey(v => v.Id);
            builder.Entity<Vote>()
                .Property(v => v.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<UserStatus>()
                .HasKey(s => s.Id);
            builder.Entity<UserStatus>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<RecipeStatus>()
                .HasKey(s => s.Id);
            builder.Entity<RecipeStatus>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<RecipeTag>()
                .HasKey(rt => new { rt.RecipeId, rt.TagId });
        }

        public static void SetupAlternateKeys(this ModelBuilder builder)
        {
            builder.Entity<Tag>()
                .HasAlternateKey(t => t.Content);
            builder.Entity<Vote>()
                .HasAlternateKey(v => new {v.RecipeId, v.UserId});
            builder.Entity<UserStatus>()
                .HasAlternateKey(s => s.Name);
            builder.Entity<RecipeStatus>()
                .HasAlternateKey(s => s.Name);
        }

        public static void SetupRequiredColumns(this ModelBuilder builder)
        {
            builder.Entity<UserProfile>()
                .Property(u => u.IsMuted)
                .IsRequired();
            builder.Entity<UserProfile>()
                .Property(u => u.UserStatusId)
                .IsRequired();

            builder.Entity<Recipe>()
                .Property(r => r.Name)
                .IsRequired();
            builder.Entity<Recipe>()
                .Property(r => r.Description)
                .IsRequired();
            builder.Entity<Recipe>()
                .Property(r => r.Content)
                .IsRequired();
            builder.Entity<Recipe>()
                .Property(r => r.CreationDate)
                .IsRequired();
            builder.Entity<Recipe>()
                .Property(r => r.RecipeStatusId)
                .IsRequired();

            builder.Entity<Comment>()
                .Property(c => c.Content)
                .IsRequired();
            builder.Entity<Comment>()
                .Property(c => c.CreationDate)
                .IsRequired();
            builder.Entity<Comment>()
                .Property(c => c.RecipeId)
                .IsRequired();

            builder.Entity<Vote>()
                .Property(v => v.Value)
                .IsRequired();
        }

        public static void SetupRelations(this ModelBuilder builder)
        {
            builder.Entity<UserProfile>()
                .HasOne(up => up.ApplicationUser)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserProfile>()
                .HasOne(u => u.UserStatus)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.UserStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Recipe>()
                .HasOne(r => r.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Recipe>()
                .HasOne(r => r.RecipeStatus)
                .WithMany(s => s.Recipes)
                .HasForeignKey(r => r.RecipeStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.Recipe)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Vote>()
                .HasOne(v => v.Recipe)
                .WithMany(r => r.Votes)
                .HasForeignKey(v => v.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RecipeTag>()
                .HasOne(rt => rt.Recipe)
                .WithMany(r => r.RecipeTags)
                .HasForeignKey(rt => rt.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RecipeTag>()
                .HasOne(rt => rt.Tag)
                .WithMany(t => t.RecipeTags)
                .HasForeignKey(rt => rt.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static void SeedData(this ModelBuilder builder)
        {
            builder.SeedUserStatuses();
            builder.SeedRecipeStatuses();
            builder.SeedUserRoles();
        }

        public static void SeedUserStatuses(this ModelBuilder builder)
        {
            builder.Entity<UserStatus>()
                .HasData(new UserStatus
                {
                    Id = 1,
                    Name = DataConstants.UserStatusActive
                });
            builder.Entity<UserStatus>()
                .HasData(new UserStatus
                {
                    Id = 2,
                    Name = DataConstants.UserStatusPending
                });
            builder.Entity<UserStatus>()
                .HasData(new UserStatus
                {
                    Id = 3,
                    Name = DataConstants.UserStatusBlocked
                });
            builder.Entity<UserStatus>()
                .HasData(new UserStatus
                {
                    Id = 4,
                    Name = DataConstants.UserStatusDeleted
                });
        }

        public static void SeedRecipeStatuses(this ModelBuilder builder)
        {
            builder.Entity<RecipeStatus>()
                .HasData(new RecipeStatus
                {
                    Id = 1,
                    Name = DataConstants.RecipeStatusActive
                });
            builder.Entity<RecipeStatus>()
                .HasData(new RecipeStatus
                {
                    Id = 2,
                    Name = DataConstants.RecipeStatusDeleted
                });
        }

        public static void SeedUserRoles(this ModelBuilder builder)
        {
            builder.Entity<IdentityRole>()
                .HasData(new IdentityRole
                {
                    Name = DataConstants.UserRoleAdmin,
                    NormalizedName = DataConstants.UserRoleAdmin.ToUpper()
                });
            builder.Entity<IdentityRole>()
                .HasData(new IdentityRole
                {
                    Name = DataConstants.UserRoleUser,
                    NormalizedName = DataConstants.UserRoleUser.ToUpper()
                });
        }
    }
}
