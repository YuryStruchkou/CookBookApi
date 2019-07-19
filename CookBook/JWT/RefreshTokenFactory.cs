using System;
using System.Security.Cryptography;
using CookBook.Domain.Models;

namespace CookBook.Presentation.JWT
{
    public class RefreshTokenFactory
    {
        private readonly int _size;
        private readonly int _validForDays;

        public RefreshTokenFactory(int size, int validForDays)
        {
            _size = size;
            _validForDays = validForDays;
        }

        public RefreshToken GenerateToken()
        {
            var token = new byte[_size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(token);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(token),
                    ExpiryDate = DateTime.Now.AddDays(_validForDays)
                };
            }
        }
    }
}
