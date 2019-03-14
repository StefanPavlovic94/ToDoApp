using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;

namespace UserManagement.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly string _secret;

        public JwtService()
        {
            this._secret = ConfigurationManager.AppSettings["JwtSecret"];
        }

        public string GenerateJwt(int userId)
        {
            byte[] key = Convert.FromBase64String(this._secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {            
                new Claim("UserId", userId.ToString())
            });

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                      Subject = claimsIdentity,                      
                      Expires = DateTime.UtcNow.AddMinutes(30),
                      SigningCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
