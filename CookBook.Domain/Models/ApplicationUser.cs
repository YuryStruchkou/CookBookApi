using Microsoft.AspNetCore.Identity;

namespace CookBook.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile UserProfile { get; set; }
    }
}
