using Microsoft.AspNetCore.Identity;

namespace CookBook.Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public virtual UserProfile UserProfile { get; set; }
    }
}
