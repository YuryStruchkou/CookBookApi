using System.Collections.Generic;

namespace CookBook.Domain.Models
{
    public class UserStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UserProfile> Users { get; set; }
    }
}
