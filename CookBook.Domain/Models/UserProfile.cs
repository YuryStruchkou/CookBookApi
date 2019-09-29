using System.Collections.Generic;
using CookBook.Domain.Enums;

namespace CookBook.Domain.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsMuted { get; set; }

        public UserStatus UserStatus { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();

        public virtual ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();
    }
}
