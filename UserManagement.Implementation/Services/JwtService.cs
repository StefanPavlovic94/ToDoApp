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
using UserManagement.Core.Model;

namespace UserManagement.Implementation.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _accessTokenSecret;
        private readonly string _refreshTokenSecret;

        public JwtService()
        {
            this._accessTokenSecret = ConfigurationManager.AppSettings["AccessTokenSecret"];
            this._refreshTokenSecret = ConfigurationManager.AppSettings["RefreshTokenSecret"];
        }

        public AuthenticationResponse GenerateJwtTokens(int userId)
        {
            byte[] accessTokenKey = Convert.FromBase64String(this._accessTokenSecret);
            SymmetricSecurityKey accessTokenSecurityKey = new SymmetricSecurityKey(accessTokenKey);

            byte[] refreshTokenKey = Convert.FromBase64String(this._refreshTokenSecret);
            SymmetricSecurityKey refreshTokenSecurityKey = new SymmetricSecurityKey(refreshTokenKey);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {            
                new Claim("UserId", userId.ToString())
            });

            SecurityTokenDescriptor accessTokenDescriptor = new SecurityTokenDescriptor
            {
                      Subject = claimsIdentity,                      
                      Expires = DateTime.UtcNow.AddMinutes(1),
                      SigningCredentials = new SigningCredentials(accessTokenSecurityKey,SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityTokenDescriptor refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(refreshTokenSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken accessSecurityToken = handler.CreateJwtSecurityToken(accessTokenDescriptor);
            JwtSecurityToken refreshSecurityToken = handler.CreateJwtSecurityToken(refreshTokenDescriptor);

            return new AuthenticationResponse()
            {
                IsAuthenticated = true,
                AccessToken = handler.WriteToken(accessSecurityToken),
                RefreshToken = handler.WriteToken(refreshSecurityToken)
            };
        }
    }
}
