using System;

namespace CookBook.Presentation.JWT
{
    public class JwtToken
    {
        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string UserRole { get; set; }
    }
}
