using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace CookBook.Presentation.JWT
{
    public class JwtFactory
    {
        private readonly string _issuer;
        private readonly string _key;

        public JwtFactory(string issuer, string key)
        {
            _issuer = issuer;
            _key = key;
        }

        public string GenerateEncodedToken(ClaimsIdentity claimsIdentity, int validForMinutes)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(validForMinutes),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
