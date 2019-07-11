using System;
using System.Collections.Generic;
using CookBook.Domain.Enums;

namespace CookBook.Domain.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? EditDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }

        public RecipeStatus RecipeStatus { get; set; }

        public virtual ICollection<RecipeTag> RecipeTags { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public int? UserId { get; set; }

        public virtual UserProfile User { get; set; }
    }
}
