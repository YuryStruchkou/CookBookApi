using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CookBook.Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
