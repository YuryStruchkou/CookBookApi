using System;
using System.Security.Cryptography;

namespace CookBook.Presentation.JWT
{
    public class RefreshTokenFactory
    {
        private readonly int _size;

        public RefreshTokenFactory(int size)
        {
            _size = size;
        }

        public string GenerateToken()
        {
            var token = new byte[_size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(token);
                return Convert.ToBase64String(token);
            }
        }
    }
}
