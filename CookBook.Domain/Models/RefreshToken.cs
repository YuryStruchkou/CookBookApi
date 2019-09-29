using System;

namespace CookBook.Domain.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
