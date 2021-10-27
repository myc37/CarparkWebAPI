using CarparkWebAPI.DbContext;
using CarparkWebAPI.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarparkWebAPI.Service
{
    public interface ITokenService
    {
        string BuildToken(string key, LoginViewModel user);
        bool ValidateToken(string key, string token);
    }
    public class TokenService : ITokenService
    {
        public string BuildToken(string key, LoginViewModel user)
        {
            var decoded = Base64UrlEncoder.DecodeBytes(key);
            var securityKey = new SymmetricSecurityKey(decoded);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Email", user.Email) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public bool ValidateToken(string key, string token)
        {
            var decoded = Base64UrlEncoder.DecodeBytes(key);
            var signingKey = new SymmetricSecurityKey(decoded);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
