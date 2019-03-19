using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Enums;
using UserManagement.Core.Model;

namespace UserManagement.Implementation.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _accessTokenSecret;
        private readonly string _refreshTokenSecret;

        public JwtService(string accessTokenSecret, string refreshTokenSecret)
        {
            this._accessTokenSecret = accessTokenSecret;
            this._refreshTokenSecret = refreshTokenSecret;
        }

        public AuthenticationResponse GenerateJwtTokens(int userId)
        {
            var accessToken = GenerateJwtToken(userId, JwtTokenType.AccessToken, DateTime.UtcNow.AddMinutes(1));
            var refreshToken = GenerateJwtToken(userId, JwtTokenType.RefreshToken, DateTime.UtcNow.AddMinutes(20));

            return new AuthenticationResponse()
            {
                IsAuthenticated = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateJwtToken(int userId, JwtTokenType tokenType, DateTime expireDate)
        {
            var secretKey = GetSecretKey(tokenType);

            var secretKeyBase64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(secretKey));

            byte[] tokenKey = Convert.FromBase64String(secretKeyBase64String);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(tokenKey);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(CustomClaimType.UserId.ToString(), userId.ToString())
            });

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken accessSecurityToken = handler.CreateJwtSecurityToken(tokenDescriptor);

            return handler.WriteToken(accessSecurityToken);
        }

        public bool ValidateJwtToken(string token, JwtTokenType tokenType)
        {
            try
            {
                var secretKey = GetSecretKey(tokenType);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public JwtInfo ReadJwtToken(string token, JwtTokenType tokenType)
        {
            try
            {
                var secretKey = GetSecretKey(tokenType);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.ReadToken(token) as JwtSecurityToken;

                var userId = securityToken.Claims
                    .First(c => c.Type == CustomClaimType.UserId.ToString())
                    .Value;

                return new JwtInfo()
                    .WithClaim(CustomClaimType.UserId, userId.ToString(), validJwt: true);
            }
            catch (Exception)
            {
                return new JwtInfo() { IsValidJwt = false };
            }
        }

        private string GetSecretKey(JwtTokenType tokenType)
        {
            switch (tokenType)
            {
                case JwtTokenType.AccessToken:
                    return _accessTokenSecret;

                case JwtTokenType.RefreshToken:
                    return _refreshTokenSecret;

                default:
                    throw new Exception($"Token type {tokenType} dont have secret key");
            }
        }
    }
}
