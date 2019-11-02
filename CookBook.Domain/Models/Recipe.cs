using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual ICollection<RecipeTag> RecipeTags { get; set; } = new HashSet<RecipeTag>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();

        public int? UserId { get; set; }

        public virtual UserProfile User { get; set; }

        public Vote AddOrUpdateVote(int userId, int voteValue)
        {
            var vote = Votes.SingleOrDefault(v => v.UserId == userId);
            if (vote == null)
            {
                vote = new Vote { UserId = userId };
                Votes.Add(vote);
            }
            vote.Value = voteValue;
            return vote;
        }
    }
}
