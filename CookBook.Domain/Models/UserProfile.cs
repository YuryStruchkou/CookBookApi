using System.Collections.Generic;

namespace CookBook.Domain.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsMuted { get; set; }

        public int UserStatusId { get; set; }

        public virtual UserStatus UserStatus { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
