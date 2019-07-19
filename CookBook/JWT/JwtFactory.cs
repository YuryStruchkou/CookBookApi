using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CookBook.Presentation.JWT
{
    public class JwtFactory
    {
        private readonly string _issuer;
        private readonly string _key;
        private readonly int _validForMinutes;

        public JwtFactory(string issuer, string key, int validForMinutes)
        {
            _issuer = issuer;
            _key = key;
            _validForMinutes = validForMinutes;
        }

        public string GenerateEncodedToken(ClaimsIdentity claimsIdentity)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(_validForMinutes),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
